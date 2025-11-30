using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI loserText;
    
    void Start()
    {
        gameOverPanel.SetActive(false);
    }
    
    public void ShowGameOver(string loserName)
    {
        gameOverPanel.SetActive(true);
        loserText.text = loserName + " LOST!";
        Time.timeScale = 0f; // Pause the game
    }
    
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(0); // Reload scene
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Change to your menu scene name
    }
}