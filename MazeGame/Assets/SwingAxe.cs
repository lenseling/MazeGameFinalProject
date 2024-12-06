using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    public GameObject axe; // Reference to the axe GameObject
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

        // Rotate the axe around the swing point
        RotateAxe();
    }

    private void RotateAxe()
    {
        if (axe != null)
        {
            // Rotate the axe around the SwingPoint's position
            axe.transform.RotateAround(transform.position, Vector3.forward, swingingForward ? swingSpeed * Time.deltaTime : -swingSpeed * Time.deltaTime);
        }
    }
}