using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public Transform vrRig; // Reference to the OVRCameraRig (e.g., CenterEyeAnchor)
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock rotation to prevent tipping over
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Get joystick input
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Calculate movement direction based on where the player is looking
        Vector3 forward = vrRig.forward;
        forward.y = 0; // Ignore vertical movement
        forward.Normalize();

        Vector3 right = vrRig.right;
        right.y = 0; // Ignore vertical movement
        right.Normalize();

        // Reverse the input to correct the direction
        Vector3 moveDirection = forward * input.y + right * input.x;


        // Apply movement with Rigidbody
        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}
