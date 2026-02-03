using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text healthText;
    public Text scoreText;

    private int score = 0;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    void Awake()
    {
        instance = this;
    }

    public void UpdateHealthUI(int currentHealth)
    {
        healthText.text = "VIDAS: " + currentHealth;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "KILLS: " + score;
    }
}