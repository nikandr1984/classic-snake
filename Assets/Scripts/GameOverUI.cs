using System.Collections;
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

        GameManager.OnGameOver += OnGameOverHandler; // Подписываемся на событие Game Over
    }    


    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOverHandler; // Отписываемся от события при уничтожении объекта
    }


    // Метод для показа панели Game Over с задержкой
    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(0.7f); // Ждем немного перед показом панели

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

    // Обработчик события Game Over
    private void OnGameOverHandler()
    {
        Debug.Log("GameOverUI: Событие получено, запускаем корутину");
        StartCoroutine(ShowWithDelay());
    }
}
