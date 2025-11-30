using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject rulesText;
    public GameObject howToPlayText;
    
    
    void Start()
    {
        if (rulesText != null) rulesText.SetActive(true);
        if (howToPlayText != null) howToPlayText.SetActive(false);
    }
    
    // Button GAME RULES
    public void ShowRules()
    {
        if (rulesText != null) rulesText.SetActive(true);
        if (howToPlayText != null) howToPlayText.SetActive(false);
    }
    
    // Button HOW TO PLAY
    public void ShowHowToPlay()
    {
        if (howToPlayText != null) howToPlayText.SetActive(true);
        if (rulesText != null) rulesText.SetActive(false);
    }
    
    // Button PLAY
    public void Play()
    {
        SceneManager.LoadScene(0); // Change to your scene name
    }
    
    // Button EXIT
    public void Exit()
    {
        Application.Quit();
    }
}