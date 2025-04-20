using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerOfGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText, healthText;
    public int score, health = 10;


    private void Update()
    {
        scoreText.SetText(score.ToString());
        healthText.SetText(health.ToString());
        if(health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
