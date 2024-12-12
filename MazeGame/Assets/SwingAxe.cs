using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    public float swingSpeed = 50f; // Speed of the swing
    public float maxAngle = 45f;  // Maximum swing angle

    private float currentAngle = 0f;
    private bool swingingForward = true;

    void Update()
    {
        // Calculate the angle change based on the swing speed
        float angleChange = swingSpeed * Time.deltaTime;

        if (swingingForward)
        {
            currentAngle += angleChange;
            if (currentAngle >= maxAngle)
            {
                swingingForward = false;
            }
        }
        else
        {
            currentAngle -= angleChange;
            if (currentAngle <= -maxAngle)
            {
                swingingForward = true;
            }
        }

        // Apply the rotation to the axe
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f); // Adjust axis if needed
    }
}