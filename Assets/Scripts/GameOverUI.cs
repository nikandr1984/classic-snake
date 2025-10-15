using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;   // Ссылка на панель Game Over
    public TextMeshProUGUI scoreText;  // Ссылка на текст для отображения счета
    public TextMeshProUGUI bestText;   // Ссылка на текст для отображения рекорда
        

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Скрываем панель Game Over в начале игры
        }
    }

    private void OnEnable()
    {
        if (gameOverPanel != null)
        {
            if (GameManager.Instance != null)
            {
                GameManager.OnGameOver += OnGameOverHandler; // Подписываемся на событие Game Over             
            }                      
        }
    }

    private void OnDisable()
    {
        if (gameOverPanel != null)
        {
            GameManager.OnGameOver -= OnGameOverHandler; // Отписываемся от события при уничтожении объекта  
        }
    }


    // Обработчик события Game Over
    private void OnGameOverHandler()
    {
        if (gameOverPanel != null && GameManager.Instance != null) // Проверяем, что ссылки не null
        {
            // Обновляем текст счета и рекорда
            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameManager.Instance.score;
            }
            if (bestText != null)
            {
                bestText.text = "Best: " + GameManager.Instance.highScore;
            }           

            gameOverPanel.SetActive(true); // Показываем панель Game Over
        }
    }
}
    
