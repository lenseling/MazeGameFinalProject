using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset; // Offset from the camera


    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.renderQueue = 3001; // Renders on top
    }
    void Update()
    {
        if (cameraTransform != null)
        {
            transform.position = cameraTransform.position + cameraTransform.TransformDirection(offset);
            transform.rotation = cameraTransform.rotation;
        }
    }
}