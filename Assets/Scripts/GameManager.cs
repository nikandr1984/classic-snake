using UnityEngine;


public class GameManager : MonoBehaviour
{          
    public static GameManager Instance;                     // Синглтон для доступа к GameManager
    public static event System.Action<bool> OnPauseToggled; // Событие для уведомления об изменении состояния паузы
    public static event System.Action OnGameOver;           // Событие для уведомления об окончании игры

    public UIManager uiManager;               // Ссылка на UIManager для обновления интерфейса
    public GameOverUI gameOverUI;             // Ссылка на GameOverUI для отображения экрана окончания игры
    public UnityEngine.UI.Button pauseButton; // Кнопка паузы, если используется

    private bool isPaused = false; // Флаг паузы игры

    public int score { get; private set; }      // Текущий счет
    public int highScore { get; private set; }  // Рекордный счет

    public bool IsPaused => isPaused;  // Свойство для проверки состояния паузы

    // ========================================

    private void Awake()
    {
        // Паттерн Singleton - гарантируем, что GameManager будет единственным экземпляром
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняется между сценами
            Debug.Log("GameManager создан инициализирован.");
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0); // Загружаем рекорд из памяти

        // Проверяем, что uiManager назначен
        if (uiManager != null) 
        {
            uiManager.Initialize(score, highScore); // Инициализируем UI с текущим счетом и рекордом
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause); // Подписываемся на событие клика по кнопке паузы
            OnPauseToggled += UpdatePauseButtonText;
            UpdatePauseButtonText(isPaused);              // Обновляем текст кнопки паузы при инициализации

        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Проверяем нажатие клавиши Spase
        {
            TogglePause(); // Переключаем состояние паузы
        }
    }

    // ========================================

    public void AddScore() // Метод вызывается, когда змейка съедает еду
    {
        score += 10; // Увеличиваем счет на 10

        // Проверяем побит ли рекорд
        if (score > highScore)
        {
            highScore = score; // Обновляем рекорд
            PlayerPrefs.SetInt("HighScore", highScore); // Сохраняем рекорд в памяти
        } 
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(score); // Обновляем счет в UI
        }
    }


    public void ResetScore() // Метод для сброса счета
    {
        score = 0; 
    }


    public void TogglePause()  // Метод для переключения паузы
    {
        isPaused = !isPaused;
        Debug.Log("Game " + (isPaused ? "Paused" : "Resumed"));

        // Уведомляем подписчиков об изменении состояния паузы
        OnPauseToggled?.Invoke(isPaused);
    }


    private void UpdatePauseButtonText(bool paused) // Метод для обновления текста кнопки паузы
    {
        if (pauseButton != null)
        {
            var text = pauseButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.text = paused ? "Resume" : "Pause"; // Меняем текст кнопки в зависимости от состояния паузы
            }            
        }
    }
    

    // Метод для обработки окончания игры
    public void GameOver() 
    {
       isPaused = true;           // Устанавливаем паузу
       Debug.Log("Game Over!");   // Выводим сообщение в консоль
       
       DisablePauseButton(); // Отключаем кнопку паузы

       OnGameOver?.Invoke();      // Уведомляем подписчиков об окончании игры
    }


    // Метод для отключения кнопки паузы
    private void DisablePauseButton() 
    {
        if (pauseButton != null)
        {
            pauseButton.interactable = false; 
        }
    }

    // 
    public void ResetGame() 
    {
        ResetScore();     // Сбрасываем счет
        isPaused = false; // Снимаем паузу

        if (pauseButton != null)
        {
            pauseButton.interactable = true; // Включаем кнопку паузы            
        }

        if (gameOverUI != null && gameOverUI.gameOverPanel != null) // Проверяем, что ссылки не null
        {
            gameOverUI.gameOverPanel.SetActive(false); // Скрываем панель Game Over
        }

        Debug.Log("New Game started!");    // Выводим сообщение в консоль
    }

}
