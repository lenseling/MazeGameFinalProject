using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int maxLives = 5; // Set the maximum number of lives

    private int currentLives;
    private TMP_Text lifeText; // Reference to the Text component


    void Start()
    {
        lifeText = GetComponent<TMP_Text>();
        // Initialize the number of lives
        currentLives = maxLives;
        UpdateLifeText();
        Debug.Log("Lives: " + currentLives);
    }

    public void reduceLife()
    {
        if (currentLives > 0)
        {
            currentLives--; // Reduce one life
            UpdateLifeText();
            Debug.Log("Life lost! Remaining lives: " + currentLives);

            if (currentLives <= 0)
            {
                OnPlayerDeath(); // Call death logic if no lives are left
            }
        }
    }

    public void ResetLives()
    {
        // Reset lives to the maximum number
        currentLives = maxLives;
        UpdateLifeText();
        Debug.Log("Lives reset! Lives: " + currentLives);
    }

    private void UpdateLifeText()
    {
        // Update the Text component with the current number of lives
        lifeText.text = "Remaining Lives: " + currentLives;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has no lives left. Game Over!");
        SceneManager.LoadScene("DeathScene");
    }
}
