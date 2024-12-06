using UnityEngine;

public class StatueSpot : MonoBehaviour
{
    private bool isOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isOccupied && other.CompareTag("Statue"))
        {
            // Snap the statue into place
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            // Disable further interaction
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Notify the GameManager
            isOccupied = true;
            GameManager.Instance.PlaceStatue();
        }
    }
}
