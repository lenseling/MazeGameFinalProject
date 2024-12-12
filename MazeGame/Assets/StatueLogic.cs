using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueLogic : MonoBehaviour
{
    private GameObject key; // Reference to the key hidden inside the statue
    private bool keyActivated = false;

    void Start()
    {
        // Find the key attached to this statue
        key = GetComponentInChildren<Key>()?.gameObject;

        if (key != null)
        {
            key.SetActive(false); // Ensure the key is inactive at the start
        }
    }

    // Called when the statue is moved
    public void OnStatueMoved()
    {
        if (!keyActivated && key != null)
        {
            key.SetActive(true); // Activate the key
            keyActivated = true; // Prevent reactivation
            Debug.Log("Statue moved! Key activated.");
        }
    }

    // Optional: Listen for grabbing/releasing events
    public void OnStatueGrabbed()
    {
        Debug.Log("Statue grabbed!");
        // You can add logic here if needed
    }

    public void OnStatueReleased()
    {
        Debug.Log("Statue released!");
        // Check if moved enough to activate key
        CheckIfStatueMoved();
    }

    private void CheckIfStatueMoved()
    {
        if (transform.position.magnitude > 1f && !keyActivated) // Example condition for "moved"
        {
            OnStatueMoved();
        }
    }
}

