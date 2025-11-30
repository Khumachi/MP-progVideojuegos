using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public TextMeshProUGUI numberText;
    public BombGame bombGame;

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            numberText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        numberText.text = "GO!";
        
        // Start the bomb game
        if (bombGame != null)
        {
            bombGame.StartGame();
        }
        
        yield return new WaitForSeconds(1f);
        numberText.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(CountdownToStart());
    }
}