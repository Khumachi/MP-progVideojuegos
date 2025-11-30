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
    private float tiempoIA = 0.05f;
    private float contadorIA;
    
    public float velocidadBomba = 5f; // Velocidad del movimiento
    private bool bombaEnMovimiento = false;

    //explosion bomba

    //sonido
    public AudioSource audioSource;
    public AudioClip sonidoExplosion;   

    public float tiempoExplosion = 10f; // Tiempo hasta que explote
    private float tiempoRestante;
    private Vector3 escalaInicial;
    private Vector3 escalaFinal = new Vector3(0.5f, 0.5f, 0.5f); // Tamaño máximo antes de explotar
    
    public bool juegoIniciado = false; 
    
    void Start()
{
    quienTieneBomba = jugador;
        
        Vector3 offsetInicial = ObtenerOffset(jugador);
        bomba.transform.localPosition = new Vector3(0.033f, 0.587f, 0.6029f);
        bomba.transform.SetParent(jugador.transform, true);
        
        bomba.transform.localRotation = Quaternion.Euler(0, 0, 0);
        escalaInicial = new Vector3(0.43f, 0.43f, 0.43f);
        bomba.transform.localScale = escalaInicial;
        
        contadorIA = tiempoIA;
        tiempoRestante = tiempoExplosion;
}
    
    void Update()
    {   
        //iniciar script
        if (!juegoIniciado) return;
         // Actualizar tiempo de la bomba
        tiempoRestante -= Time.deltaTime;
        
        // Solo inflar si NO está en movimiento
    if (!bombaEnMovimiento)
    {
        // Inflar la bomba según el tiempo restante
        float progreso = 1 - (tiempoRestante / tiempoExplosion); // 0 a 1
        bomba.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, progreso);
    }
    
    // Explotar cuando llegue a 0
    if (tiempoRestante <= 0)
    {
        ExplotarBomba();
        return;
    }
        
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
                contadorIA = Random.Range(0.1f, 0.5f);
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
    
    // Guardar la escala de mundo (no local) antes de desparentar
    Vector3 escalaGlobal = bomba.transform.lossyScale;
    
    // Desparentar la bomba para que se mueva libremente
    bomba.transform.SetParent(null);
    bomba.transform.localScale = escalaGlobal; // Asignar escala global como local
    
    // Posición inicial
    Vector3 posInicial = bomba.transform.position;
    
    // Calcular posición de destino
    Vector3 offsetDestino = ObtenerOffset(nuevoDestino);
    Vector3 posFinal = nuevoDestino.transform.position + nuevoDestino.transform.TransformDirection(offsetDestino);
    
    // Animar el movimiento con parábola
    float tiempoTranscurrido = 0f;
    float duracion = 0.5f;
    float alturaArco = 3f;
    
    while(tiempoTranscurrido < duracion)
    {
        tiempoTranscurrido += Time.deltaTime;
        float porcentaje = tiempoTranscurrido / duracion;
        
        // Aplicar aceleración
        float porcentajeAcelerado = porcentaje * porcentaje * porcentaje;
        
        // Movimiento horizontal con aceleración
        Vector3 posicionHorizontal = Vector3.Lerp(posInicial, posFinal, porcentajeAcelerado);
        
        // Movimiento vertical (parábola)
        float alturaParabola = alturaArco * (porcentaje * (1 - porcentaje) * 4);
        
        // Posición final con arco
        bomba.transform.position = posicionHorizontal + Vector3.up * alturaParabola;
        
        yield return null;
    }
    
    // Hacer hija del nuevo destino
    quienTieneBomba = nuevoDestino;
    bomba.transform.SetParent(nuevoDestino.transform);
    
    bomba.transform.localPosition = ObtenerOffset(nuevoDestino);
    bomba.transform.localRotation = Quaternion.identity;
    
    Animator animator = bomba.GetComponent<Animator>();
    if (animator != null)
    {
        animator.enabled = false;
    }
    
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
        return new Vector3(0.37f, 0.42f, 0.66f);
    }
    else if(animal == personaje3) // Cat
    {
        return new Vector3(0.30f, 0.42f, 0.71f);
    }
    else if(animal == personaje4) // Llama
    {
        return new Vector3(0.33f, 0.56f, 0.42f);
    }
    
    return Vector3.up * 1.5f; // Default
}

    void ExplotarBomba()
{
    Debug.Log($"¡EXPLOSIÓN! {quienTieneBomba.name} perdió!");
    
    // Reproducir con AudioSource
    if (audioSource != null && sonidoExplosion != null)
    {
        audioSource.PlayOneShot(sonidoExplosion);
    }
    
    Destroy(bomba);
    Destroy(quienTieneBomba);
    this.enabled = false;
}

//iniciar juego
    public void IniciarJuego()
{
    juegoIniciado = true;
}
}