using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;   // Ссылка на панель Game Over
    public TextMeshProUGUI scoreText;  // Ссылка на текст для отображения счета
    public TextMeshProUGUI bestText;   // Ссылка на текст для отображения рекорда

    private void Awake()
    {
        if (gameOverPanel != null) 
        {
            gameOverPanel.SetActive(false); // Скрыем панель Game Over при старте
        }

        GameManager.OnGameOver += Show; // Подписываемся на событие окончания игры
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= Show; // Отписываемся от события при уничтожении объекта
    }

    private void Show()
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
