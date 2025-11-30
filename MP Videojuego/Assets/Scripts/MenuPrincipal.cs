using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject textoReglas;
    public GameObject textoComoJugar;
    
    
    void Start()
    {
        
        if (textoReglas != null) textoReglas.SetActive(true);
        if (textoComoJugar != null) textoComoJugar.SetActive(false);
        
    }
    
    // Botón REGLAS DEL JUEGO
    public void MostrarReglas()
    {
        if (textoReglas != null) textoReglas.SetActive(true);
        if (textoComoJugar != null) textoComoJugar.SetActive(false);
        
    }
    
    // Botón CÓMO JUGAR
    public void MostrarComoJugar()
    {
        if (textoComoJugar != null) textoComoJugar.SetActive(true);
        if (textoReglas != null) textoReglas.SetActive(false);
        
    }
    
    // Botón JUGAR
    public void Jugar()
    {
        SceneManager.LoadScene(0); // Cambia por el nombre de tu escena
    }
    
    // Botón SALIR 
    public void Salir()
    {
        Application.Quit();
    }
}