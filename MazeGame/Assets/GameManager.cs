using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject statue; // Reference to the statue
    public GameObject statueSpot; // Reference to the statue's placement spot
    public GameObject statueKey; // Key unlocked by placing the statue

    public GameObject obstacleKey; // Key behind an obstacle
    public GameObject enemyKey; // Key guarded by an enemy

    private int keysCollected = 0; // Track collected keys
    private int totalKeys = 3; // Total number of keys to collect

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaceStatue()
    {
        Debug.Log("Statue placed! Unlocking the statue key.");
        if (statueKey != null)
        {
            statueKey.SetActive(true); // Activate the statue key
        }
    }

    public void CollectKey(GameObject key)
    {
        if (key != null)
        {
            keysCollected++;
            key.SetActive(false); // Deactivate the key when collected
            Debug.Log($"Key collected! {keysCollected}/{totalKeys} keys collected.");

            // Check if all keys are collected
            if (keysCollected >= totalKeys)
            {
                WinGame();
            }
        }
    }

    private void WinGame()
    {
        Debug.Log("All keys collected! You win!");
        SceneManager.LoadScene("Success");
    }
}
