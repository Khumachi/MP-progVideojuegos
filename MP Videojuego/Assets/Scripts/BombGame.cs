using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame : MonoBehaviour
{
    public GameObject bomb;
    public GameObject player;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;
    
    private GameObject whoHasBomb;
    private float aiTime = 0.05f;
    private float aiCounter;
    
    public float bombSpeed = 5f; // Movement speed
    private bool bombMoving = false;

    // Bomb explosion
    
    // Sound
    public AudioSource audioSource;
    public AudioClip explosionSound;   

    public float explosionTime = 10f; // Time until explosion
    private float remainingTime;
    private Vector3 initialScale;
    private Vector3 finalScale = new Vector3(0.5f, 0.5f, 0.5f); // Maximum size before exploding
    
    public bool gameStarted = false; 

    public GameOverManager gameOverManager; // Game over
    
    void Start()
    {
        whoHasBomb = player;
        
        Vector3 initialOffset = GetOffset(player);
        bomb.transform.localPosition = new Vector3(0.033f, 0.587f, 0.6029f);
        bomb.transform.SetParent(player.transform, true);
        
        bomb.transform.localRotation = Quaternion.Euler(0, 0, 0);
        initialScale = new Vector3(0.43f, 0.43f, 0.43f);
        bomb.transform.localScale = initialScale;
        
        aiCounter = aiTime;
        remainingTime = explosionTime;
    }
    
    void Update()
    {   
        // Start script
        if (!gameStarted) return;
        
        // Update bomb time
        remainingTime -= Time.deltaTime;
        
        // Only inflate if NOT moving
        if (!bombMoving)
        {
            // Inflate bomb according to remaining time
            float progress = 1 - (remainingTime / explosionTime); // 0 to 1
            bomb.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
        }
        
        // Explode when it reaches 0
        if (remainingTime <= 0)
        {
            ExplodeBomb();
            return;
        }
        
        // Don't allow actions while bomb is moving
        if(bombMoving) return;
        
        if(whoHasBomb == player)
        {
            PlayerControl();
        }
        else
        {
            aiCounter -= Time.deltaTime;
            if(aiCounter <= 0)
            {
                PassBombAI();
                aiCounter = Random.Range(0.1f, 0.5f);
            }
        }
    }
    
    void PlayerControl()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PassBomb(character3); // Cat
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            PassBomb(character4); // Llama
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            PassBomb(character2); // Elephant
        }
    }
    
    void PassBomb(GameObject newTarget)
    {
        if(!bombMoving)
        {
            StartCoroutine(AnimateBombPass(newTarget));
        }
    }
    
    void PassBombAI()
    {
        GameObject[] all = { player, character2, character3, character4 };
        GameObject chosen;
        
        do
        {
            chosen = all[Random.Range(0, all.Length)];
        }
        while(chosen == whoHasBomb);
        
        PassBomb(chosen);
    }
    
    IEnumerator AnimateBombPass(GameObject newDestination)
    {
        bombMoving = true;
        
        // Save world scale (not local) before unparenting
        Vector3 globalScale = bomb.transform.lossyScale;
        
        // Unparent bomb so it moves freely
        bomb.transform.SetParent(null);
        bomb.transform.localScale = globalScale; // Assign global scale as local
        
        // Initial position
        Vector3 initialPos = bomb.transform.position;
        
        // Calculate destination position
        Vector3 destinationOffset = GetOffset(newDestination);
        Vector3 finalPos = newDestination.transform.position + newDestination.transform.TransformDirection(destinationOffset);
        
        // Animate movement with parabola
        float elapsedTime = 0f;
        float duration = 0.5f;
        float arcHeight = 3f;
        
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / duration;
            
            // Apply acceleration
            float acceleratedPercentage = percentage * percentage * percentage;
            
            // Horizontal movement with acceleration
            Vector3 horizontalPosition = Vector3.Lerp(initialPos, finalPos, acceleratedPercentage);
            
            // Vertical movement (parabola)
            float parabolaHeight = arcHeight * (percentage * (1 - percentage) * 4);
            
            // Final position with arc
            bomb.transform.position = horizontalPosition + Vector3.up * parabolaHeight;
            
            yield return null;
        }
        
        // Make child of new destination
        whoHasBomb = newDestination;
        bomb.transform.SetParent(newDestination.transform);
        
        bomb.transform.localPosition = GetOffset(newDestination);
        bomb.transform.localRotation = Quaternion.identity;
        
        Animator animator = bomb.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        
        bombMoving = false;
        
        Debug.Log("Bomb passed to: " + newDestination.name);
    }
    
    Vector3 GetOffset(GameObject animal)
    {
        if(animal == player) // Cow
        {
            return new Vector3(0.033f, 0.587f, 0.929f);
        }
        else if(animal == character2) // Elephant
        {
            return new Vector3(0.37f, 0.42f, 0.66f);
        }
        else if(animal == character3) // Cat
        {
            return new Vector3(0.30f, 0.42f, 0.71f);
        }
        else if(animal == character4) // Llama
        {
            return new Vector3(0.33f, 0.56f, 0.42f);
        }
        
        return Vector3.up * 1.5f; // Default
    }

    void ExplodeBomb()
    {
        Debug.Log($"EXPLOSION! {whoHasBomb.name} lost!");
        
        // Play with AudioSource
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(whoHasBomb.name);
        }
        
        // Destroy after delay to see explosion
        Destroy(bomb);
        Destroy(whoHasBomb);
        
        this.enabled = false;
    }

    // Start game
    public void StartGame()
    {
        gameStarted = true;
    }
}