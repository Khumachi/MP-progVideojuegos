using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownController : MonoBehaviour
{
    public int countdownTime;
    public TextMeshProUGUI Numero;
    public JuegoBomba juegoBomba; // Agregar esta línea

    IEnumerator countdownToStart()
    {
        while(countdownTime > 0)
        {
            Numero.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        Numero.text = "GO!";
        
        // Iniciar el juego de la bomba
        if (juegoBomba != null)
        {
            juegoBomba.IniciarJuego();
        }
        
        yield return new WaitForSeconds(1f);
        Numero.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(countdownToStart());
    }
}
