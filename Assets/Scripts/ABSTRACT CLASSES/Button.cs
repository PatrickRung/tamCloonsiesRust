using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    public Material onHoverMat, offHoverMat;
    private GameObject buttonObject;
    private Renderer buttonRenderer;
    private PlayerController playerController;

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
                if(Input.GetMouseButtonDown(0))
                {
                    buttonInteraction();
                }
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

    //if we ever have another button you can extend this button class and replace button interaction
    //that way it was have the same functionality we just change the function
    public abstract void buttonInteraction();


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
