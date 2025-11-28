using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuegoBomba : MonoBehaviour
{
    public GameObject bomba;
    public GameObject jugador;
    public GameObject personaje2;
    public GameObject personaje3;
    public GameObject personaje4;
    
    private GameObject quienTieneBomba;
    private float tiempoIA = 2f;
    private float contadorIA;
    
    public float velocidadBomba = 5f; // Velocidad del movimiento
    private bool bombaEnMovimiento = false;
    
    void Start()
    {
        quienTieneBomba = jugador;
        bomba.transform.SetParent(jugador.transform, false);
        bomba.transform.localPosition = new Vector3(0.033f, 0.587f, 0.929f);
        bomba.transform.localRotation = Quaternion.Euler(0, 0, 0);
        bomba.transform.localScale = new Vector3(0.43f, 0.43f, 0.43f);
        contadorIA = tiempoIA;
    }
    
    void Update()
    {
        // No permitar acciones mientras la bomba se mueve
        if(bombaEnMovimiento) return;
        
        if(quienTieneBomba == jugador)
        {
            ControlJugador();
        }
        else
        {
            contadorIA -= Time.deltaTime;
            if(contadorIA <= 0)
            {
                PasarBombaIA();
                contadorIA = Random.Range(1f, 2.5f);
            }
        }
    }
    
    void ControlJugador()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PasarBomba(personaje3); // Cat
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            PasarBomba(personaje4); // Llama
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            PasarBomba(personaje2); // Elephant
        }
    }
    
    void PasarBomba(GameObject nuevo)
    {
        if(!bombaEnMovimiento)
        {
            StartCoroutine(AnimarPaseBomba(nuevo));
        }
    }
    
    void PasarBombaIA()
    {
        GameObject[] todos = { jugador, personaje2, personaje3, personaje4 };
        GameObject elegido;
        
        do
        {
            elegido = todos[Random.Range(0, todos.Length)];
        }
        while(elegido == quienTieneBomba);
        
        PasarBomba(elegido);
    }
    
    IEnumerator AnimarPaseBomba(GameObject nuevoDestino)
    {
        bombaEnMovimiento = true;
        
        // Desparentar la bomba para que se mueva libremente
        bomba.transform.SetParent(null);
        
        // Posición inicial
        Vector3 posInicial = bomba.transform.position;
        
        // Calcular posición de destino
        Vector3 offsetDestino = ObtenerOffset(nuevoDestino);
       Vector3 posFinal = nuevoDestino.transform.position + nuevoDestino.transform.TransformDirection(offsetDestino);
        
        // Animar el movimiento
        float tiempoTranscurrido = 0f;
        float duracion = 0.5f; // Duración del viaje (ajústalo)
        
        while(tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / duracion;
            
            // Movimiento suave con curva
            bomba.transform.position = Vector3.Lerp(posInicial, posFinal, porcentaje);
            
            yield return null;
        }
        
        // Asegurar posición final
        bomba.transform.position = posFinal;
        
        // Hacer hija del nuevo destino
        quienTieneBomba = nuevoDestino;
        bomba.transform.SetParent(nuevoDestino.transform, true);
        bomba.transform.localRotation = Quaternion.identity;
        
        bombaEnMovimiento = false;
        
        Debug.Log("Bomba pasada a: " + nuevoDestino.name);
    }
    
    Vector3 ObtenerOffset(GameObject animal)
    {
        if(animal == jugador) // Cow
        {
            return new Vector3(0.033f, 0.587f, 0.929f);
        }
        else if(animal == personaje2) // Elephant
        {
            return new Vector3(0f, 2.5f, 0f);
        }
        else if(animal == personaje3) // Cat
        {
            return new Vector3(0f, 1.2f, 0f);
        }
        else if(animal == personaje4) // Llama
        {
            return new Vector3(0f, 2f, 0.3f);
        }
        
        return Vector3.up * 1.5f; // Default
    }
}