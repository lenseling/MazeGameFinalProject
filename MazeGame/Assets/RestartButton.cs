using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Transform rightHandAnchor; // VR Right Hand anchor
    public Transform leftHandAnchor;  // VR Left Hand anchor
    private Transform activeHand;

    private Material originalMaterial; // To store the ring's original material
    public Material highlightMaterial;
    

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Right hand trigger pressed");
            activeHand = rightHandAnchor;
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            Debug.Log("Left hand trigger pressed");
            activeHand = leftHandAnchor;
        }

        if (activeHand != null)
        {
            Ray ray = new Ray(activeHand.position, activeHand.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) // Perform raycast to detect the ring
            {
                if (hit.transform.root == transform.root) // If the ray hits the ring
                {
                    
                        // Restart game
                        SceneManager.LoadScene("MazeGeneratorScene");
                }
                
            }
            
        }
    }
}
