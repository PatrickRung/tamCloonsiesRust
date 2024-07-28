using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class movement : CharacterTemplate
{
    //Assingables
    [Header("MUST BE ASSIGNED")]
    public Transform playerCam;
    public UserData playerData;
    private Transform orientation;

    //wallrunning
    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallSpeed;
    private bool isWallRight, isWallLeft, isWallRunning;
    [HideInInspector] public float maxWallRunCameraTilt, wallRunCameraTilt;

    //Other
    [HideInInspector] public Rigidbody rb;

    //Rotation and look
    [Header("Rotation and look")]
    private float xRotation;
    public float sensitivity = 100f;
    private float sensMultiplier = 1f;
    private GameObject sensitivitySlider;


    //Movement
    [Header("Movement")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public LayerMask whatIsGround;
    public float counterMovement = 0.175f;
    public float maxSlopeAngle = 35f;
    private float threshold = 0.01f;
    [HideInInspector] public bool grounded, multiplayerEnabled;
    //network variable is a variable that is sinced over the server
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(new Vector3(0,0,0), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //Crouch & Slide
    [Header("Crouch and Slide")]
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    [HideInInspector] public Vector3 playerScale;

    //Jumping
    [Header("Jump")]
    public float jumpCooldown = 0.25f;
    public float jumpForce = 550f;
    public float movementSnappiness = 2;
    public float gravityAddedForce;
    [HideInInspector] public float timeOffGround = 0;
    [HideInInspector] public float jumpCount = 0;
    [HideInInspector] public bool readyToJump = true;

    //Input
    float x, y;
    private bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Display
    private GameObject Objective;

    public new void Awake() {


        //adds the method call changedActiveScene to be called when scene is changed
        
        //if the player has the awake funciton called in the menu it will try to grab objects that do not exist thus creating errors
        //MUST RECALL AWAKE FUNCTION IN OTHER SCENES
        if(SceneManager.GetActiveScene().name.Equals("menuScene")) {
            enabled = false;
            SceneManager.sceneLoaded += ChangedActiveScene;
            return;
        }
        else {
            enabled = true;
        }

        //if the the player is not the owner then it will not assign variables however base.awake still needs to be called to calculate health
        base.Awake();
        if(!IsOwner) {
            SceneManager.sceneLoaded -= ChangedActiveScene;
            return;
        } 
        //player controller which is held on the camera will keep on trying to acces the player which is still being loaded by the server
        //activiting the player controller will give time for the server to load and then the player controller can call Awake and Update with 
        //all its needed properties
        worldStorage = GameObject.Find("World Items");
        playerCam = worldStorage.GetComponent<WorldItemStorage>().PlayerCamera.transform;
        playerCam.GetComponent<PlayerController>().enabled = true;


        //LATER IMPLEMENT CHECK FOR WHEN WE ARE IN MULTIPLAYER MODE
        multiplayerEnabled = true;

        //more assigning
        
        rb = GetComponent<Rigidbody>();
        orientation = gameObject.transform;
        playerData = Resources.Load<UserData>("Data/UserData");
        playerCam.GetComponent<PlayerController>().player = gameObject.transform;
        playerCam.GetComponent<PlayerController>().playerMovement = gameObject.GetComponent<movement>();

        //will find any game object named "SpawnFloor" and will use it as the spawm point
        spawnPoint =  GameObject.Find("SpawnPoint").transform;

        setSensitivity(playerData.playerSensitivity);
        if(worldStorage.GetComponent<WorldItemStorage>().player == null) {
            worldStorage.GetComponent<WorldItemStorage>().player = gameObject;
        }
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.position = spawnPoint.transform.position;
        Objective = GameObject.Find("Objective");
        SceneManager.sceneLoaded -= ChangedActiveScene;
        
    }
    public void DisableListening()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, LoadSceneMode next)
    {
        Awake();
    }


    public virtual void WallRunInput()
    {
        //starts the wallrun
        if (Input.GetKey(KeyCode.D) && isWallRight) StartWallRun();
        if (Input.GetKey(KeyCode.A) && isWallLeft) StartWallRun();
    }

    private void StartWallRun()
    {
        rb.useGravity = false;
        isWallRunning = true;

        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime);

            //make sure stick to wall
            if (isWallRight)
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
        }
        if ((isWallLeft || isWallRight) && !isWallRunning)
        {
            jumpCount = 0;
            ResetJump();
        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();
        // reset double jump

    }


    private new void FixedUpdate() {
        if(!inMenu & IsOwner)
        {
            Movement();
            Position.Value = transform.position;
        }
        else {
            transform.position = Position.Value;
        }
        base.FixedUpdate();
        //checks if player fell to far
        if (gameObject.transform.position.y < -100)
        {
            changeHealth(-1000000);
        }

    }

    public void setSensitivity(float sense){this.sensitivity = sense;}
    public float getSensitivity() { return sensitivity; }

    //if the user is in the settings or menu thing then we disable looking around
    public bool inMenu;
    private void Update() {
        if(IsOwner) {
            if(!inMenu)
            {
                MyInput();
                Look();
                CheckForWall();
                WallRunInput();
            }
            if(!grounded)
            {
                smootherJump();
            }
            else
            {
                timeOffGround = 0;
            }
        }
    }

    //provides better jumping because the  character will start to experience more force pushing down as they are in the air for longer
    //why because realistic and also makes jumping better

    public void smootherJump()
    {
        if(!isWallRunning)
        {
            timeOffGround += Time.deltaTime;
        }
        rb.AddForce(new Vector3(0, timeOffGround * timeOffGround * -gravityAddedForce, 0));
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    public void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);
      
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (grounded) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }
    
    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public void Movement() {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);
        
        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;
        
        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        // Movement in air
        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);


    }

    public virtual void Jump() {
        if (grounded && readyToJump) {
            jumpCount= 0;
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }else if(jumpCount < 1 && !isWallRunning)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            jumpCount ++;

            ResetJump();
        }
        //Walljump
        if (isWallRunning)
        {
            readyToJump = false;  
                   
                if (isWallRight && Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-orientation.right * jumpForce * 5f);
                    rb.AddForce(orientation.up * jumpForce * 1.5f);
                    
                }
                else if (isWallLeft && Input.GetKey(KeyCode.D))
                {

                    rb.AddForce(orientation.right * jumpForce * 5f);
                    rb.AddForce(orientation.up * jumpForce * 1.5f);
                }
                else if(isWallLeft)
                {
                    rb.AddForce(orientation.right * jumpForce * 5f);
                    rb.AddForce(orientation.up * jumpForce * 1.5f);
                }
                else if (isWallRight)
                {
                    rb.AddForce(-orientation.right * jumpForce * 5f);
                    rb.AddForce(orientation.up * jumpForce * 1.5f);
                }



            //forward force
            rb.AddForce(orientation.forward * jumpForce * 1f);

                //Reset velocity
                rb.velocity = Vector3.zero;

                Invoke(nameof(ResetJump), jumpCooldown);
            
        }
    }
    
    public void ResetJump() {
            readyToJump = true;
    }
    
    private float desiredX;
    public float addedRecoil;
    public void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        xRotation += (addedRecoil/900);

        //move the camera to the position of the players head
        playerCam.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        //wallrun cam tilt

        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        //tilt back after wallrun
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }
    [Rpc(SendTo.Everyone)]
    public void PlayerDiedRPC(ulong ID) {
        if(playerCam == null) return;
        inMenu = true;
        playerCam.GetComponent<PlayerController>().openUI("DeathScreen");
        Debug.Log(ID);
        Debug.Log(Objective.transform.position);
        Objective.SetActive(true);
        Objective.GetComponent<RocketLauncherGameManager>().UpdateScoreBoardRPC(ID);
    }

}
