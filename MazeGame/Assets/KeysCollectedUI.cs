using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeysCollectedUI : MonoBehaviour
{
    public TextMeshPro keyCountText; // Reference to the Text component in your UI Canvas
    public Transform playerTransform; // Reference to the player's position (VR camera rig or head position)
    private int currentKeys = 0; // Tracks collected keys
    public MazeSpawner mazeSpawner;

    void Start()
    {
        if (!playerTransform)
        {
            // Default to the VR camera's position if no playerTransform is assigned
            playerTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Rotate the UI to always face the player
        FacePlayer();

        // Update the text UI to reflect the current key count
        UpdateUI();
    }

    private void FacePlayer()
    {
        if (playerTransform)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = transform.position - playerTransform.position;
            directionToPlayer.y = 0; // Keep only horizontal rotation (no unnecessary vertical tilting)
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Apply the calculated rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void UpdateUI()
    {
        currentKeys = mazeSpawner.keysCollected;
        keyCountText.text = $"Keys Collected: {currentKeys}";
    }

}
