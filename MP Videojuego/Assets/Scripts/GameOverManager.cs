using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject panelGameOver;
    public TextMeshProUGUI textoPerdedor;
    
    void Start()
    {
        panelGameOver.SetActive(false);
    }
    
    public void MostrarGameOver(string nombrePerdedor)
    {
        panelGameOver.SetActive(true);
        textoPerdedor.text = nombrePerdedor + " PERDIO!";
        Time.timeScale = 0f; // Pausar el juego
    }
    
    public void VolverAJugar()
    {
        Time.timeScale = 1f; // Reanudar tiempo
        SceneManager.LoadScene(1); // Recargar escena
    }
    
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Cambiar por el nombre de tu escena de menú
    }
}