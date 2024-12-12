using UnityEngine;

public class AxeSwing : MonoBehaviour
{
    public float swingSpeed = 50f; // Speed of swinging
    public float swingAngle = 30f; // Maximum swing angle

    private float swingDirection = 1f; // Direction of swing
    private float currentAngle = 0f; // Current angle of swing

    void Update()
    {
        // Update the swing angle
        currentAngle += swingDirection * swingSpeed * Time.deltaTime;

        // Reverse direction if the angle exceeds the limits
        if (Mathf.Abs(currentAngle) > swingAngle)
        {
            swingDirection *= -1f;
            currentAngle = Mathf.Clamp(currentAngle, -swingAngle, swingAngle);
        }

        // Apply rotation around the handle attachment point
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
