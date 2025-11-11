using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Basic elements")]
    public GameObject gameOverPanel;   // Ссылка на панель Game Over
    public TextMeshProUGUI scoreText;  // Ссылка на текст для отображения счета
    public TextMeshProUGUI timeText;   // Ссылка на время игры

    [Header("Entering name")]
    public GameObject enterNamePanel;      // Ссылка на панель ввода имени
    public TMP_InputField playerNameInput; // Ссылка на поле ввода имени игрока
    public Button submitButton;            // Ссылка на кнопку подтверждения имени
    public Button closeButton;             // Ссылка на кнопку отмены ввода имени    


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
            // Обновляем текст счета
            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameManager.Instance.Score;
            }

            if (timeText != null)
            {
                float gameTime = GameManager.Instance.GetGameTime();
                int minutes = (int)(gameTime / 60f);
                int seconds = (int)(gameTime % 60f);
                timeText.text = $"Time: {minutes:D2}:{seconds:D2}";
            }

            gameOverPanel.SetActive(true); // Показываем панель Game Over


            // Проверяем, попал ли игрок в таблицу лидеров
            int currentScore = GameManager.Instance.Score;
            if (LeaderboardManager.Instance != null && LeaderboardManager.Instance.IsTopScore(currentScore))
            {
                enterNamePanel?.SetActive(true);           // Показываем панель ввода имени
                playerNameInput?.SetTextWithoutNotify(""); // Очищаем поле ввода имени
                playerNameInput?.Select();                 // Фокусируемся на поле ввода имени
                playerNameInput?.ActivateInputField();     // Активируем поле ввода имени
            }
            else
            {
                enterNamePanel?.SetActive(false);          // Скрываем панель ввода имени, если не попал в топ
            }
        }
    }

    public void OnSubmitName()
    {
        if (playerNameInput == null || GameManager.Instance == null || LeaderboardManager.Instance == null)
        {
            return; // Проверяем, что ссылки не null
        }

        string name = playerNameInput.text.Trim();  // Получаем введенное имя и удаляем пробелы

        if (string.IsNullOrEmpty(name))
        {
            name = "ANON";                          // Если имя пустое, устанавливаем значение по умолчанию
        }
        else if (name.Length > 10)
        {
            name = name.Substring(0, 10);          // Ограничиваем длину имени до 10 символов
        }

        // Добавляем запись в таблицу лидеров
        LeaderboardManager.Instance.AddEntry(
            name,
            GameManager.Instance.Score,
            GameManager.Instance.GetGameTime()
        );

        enterNamePanel?.SetActive(false);          // Скрываем панель ввода имени после отправки

        Debug.Log($"GameOverUI: Result {name}: {GameManager.Instance.Score} added to the leaderboard!");
    }

    public void CloseEnterNamePanel()
    {
        enterNamePanel?.SetActive(false);          // Скрываем панель ввода имени при нажатии кнопки закрытия
    }
}
    
