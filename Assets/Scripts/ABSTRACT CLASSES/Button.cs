using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    public Material onHoverMat, offHoverMat;
    private GameObject buttonObject;
    private Renderer buttonRenderer;
    public PlayerController playerController;
    private bool isBeingLookedAt;

    // Start is called before the first frame update
    public void Start()
    {
        onHoverMat = Resources.Load<Material>("green");
        offHoverMat = Resources.Load<Material>("default");

        buttonObject = this.gameObject;
        buttonRenderer = this.gameObject.GetComponent<Renderer>();
        playerController = GameObject.Find("playerCam").GetComponent<PlayerController>();
        buttonRenderer.material = offHoverMat;
    }

    public void Update()
    {
        if(playerController.getLookingAt() != null)
        {
            if (playerController.getLookingAt().Equals(this.gameObject) 
                && playerController.getLookingAt().name.Equals(this.gameObject.name))
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    buttonInteraction();
                }
                onUserHover();
            }
            else
            {
                offUserHover();
            }
        }
        else
        {
            offUserHover();
        }

    }
    /// <summary>
    /// if we ever have another button you can extend this button class and replace button interaction
    /// that way it was have the same functionality we just change the function
    /// </summary>
    public abstract void buttonInteraction();

    public bool returnLookingAtStatus()
    {
        return isBeingLookedAt;
    }

    //changes object color when user looks at it
    public void onUserHover()
    {
        buttonRenderer.material = onHoverMat;
        isBeingLookedAt = true;
    }

    //changes object color when user stops looking at it
    public void offUserHover()
    {
        buttonRenderer.material = offHoverMat;
        isBeingLookedAt = false;
    }
}
