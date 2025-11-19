using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarJuego : MonoBehaviour
{
    public float tiempoEspera = 3.93f;
    public GameObject jugador;
    
    void Start()
    {
        jugador.SetActive(false); // Desactiva al inicio
        Invoke("ActivarJugador", tiempoEspera);
    }
    
    void ActivarJugador()
    {
        jugador.SetActive(true); // Activa después de la cinemática
    }
}