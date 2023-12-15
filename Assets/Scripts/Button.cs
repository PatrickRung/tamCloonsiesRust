using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Material onHoverMat, offHoverMat;
    private GameObject buttonObject;
    private Renderer buttonRenderer;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        buttonObject = this.gameObject;
        buttonRenderer = this.gameObject.GetComponent<Renderer>();
        playerController = GameObject.Find("playerCam").GetComponent<PlayerController>();
    }

    public void FixedUpdate()
    {
        if(playerController.getLookingAt() != null)
        {
            if (playerController.getLookingAt().Equals(this.gameObject))
            {
                onUserHover();
            }
            else
            {
                offUserHover();
            }
        }

    }

    //changes object color when user looks at it
    public void onUserHover()
    {
        buttonRenderer.material = onHoverMat;
    }

    //changes object color when user stops looking at it
    public void offUserHover()
    {
        buttonRenderer.material = offHoverMat;
    }
}
