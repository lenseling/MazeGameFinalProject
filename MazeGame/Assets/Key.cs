using UnityEngine;

public class Key : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of spinning
    public float bounceSpeed = 2f; // Speed of vertical bounce
    public float bounceHeight = 0.5f; // Height of vertical bounce

    private Vector3 startPosition;

    void Start()
    {
        // Record the initial position of the key
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate the key around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Add a smooth up-and-down bouncing effect
        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
