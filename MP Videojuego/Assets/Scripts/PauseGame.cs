using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{   
    public GameObject pauseMenu;
    public bool gamePaused = false;
    
    public AudioSource audioSource; // AudioSource for UI sounds
    public AudioClip pauseSound;
    public AudioClip resumeSound;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        
        // Make AudioSource ignore pause
        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // Play sound BEFORE resuming
        if (audioSource != null && resumeSound != null)
        {
            audioSource.PlayOneShot(resumeSound);
        }
        
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        gamePaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        gamePaused = true;
        
        // Play sound AFTER pausing
        if (audioSource != null && pauseSound != null)
        {
            audioSource.PlayOneShot(pauseSound);
        }
    }
}