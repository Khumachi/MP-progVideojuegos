using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausarJuego : MonoBehaviour
{   
    public GameObject menuPausa;
    public bool juegoPausado = false;
    
    public AudioSource audioSource; // AudioSource para sonidos de UI
    public AudioClip sonidoPausa;
    public AudioClip sonidoReanudar;
    
    void Start()
    {
        menuPausa.SetActive(false);
        
        // Hacer que el AudioSource ignore la pausa
        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        // Reproducir sonido ANTES de reanudar
        if (audioSource != null && sonidoReanudar != null)
        {
            audioSource.PlayOneShot(sonidoReanudar);
        }
        
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        juegoPausado = false;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        juegoPausado = true;
        
        // Reproducir sonido DESPUÉS de pausar
        if (audioSource != null && sonidoPausa != null)
        {
            audioSource.PlayOneShot(sonidoPausa);
        }
    }
}