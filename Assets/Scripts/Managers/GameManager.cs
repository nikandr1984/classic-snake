using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{          
    public static GameManager Instance;  // Синглтон для доступа к GameManager
                                                      
    public static event System.Action OnGameOver;          // Событие для уведомления об окончании игры
    public static event System.Action OnWallsFlash;        // Событие для уведомления о мигании стен
    public static event System.Action OnLevelUp;           // Событие для уведомления о повышении уровня (каждые n яблок)
    
    public UIManager uiManager;          // Ссылка на UIManager для обновления интерфейса
    public GameOverUI gameOverUI;        // Ссылка на GameOverUI для отображения экрана окончания игры
    public float gameOverDelay = 1.5f;   // Задержка перед показом экрана окончания игры

    public bool IsGameOver { get; private set; } = false;  // Флаг: игра окончена? 
    public bool IsPaused { get; private set; } = false;    // Флаг: игра на паузе?
    public bool CanPlay => !IsPaused && !IsGameOver;       // Можно ли играть?


    public int score { get; private set; }      // Текущий счет
    public int highScore { get; private set; }  // Рекордный счет
    public int levelCount { get; private set; } = 1; // Текущий уровень


    private int _eatenApples = 0;           // Счетчик съеденных яблок    

    [SerializeField] private int _appleToLevelUp = 15; // Количество яблок для повышения уровня    




    private void Awake()
    {
        Time.timeScale = 1f; // Устанавливаем нормальное время при старте игры

        // Инициализация синглтона
        if (Instance == null) 
        {
            Instance = this;  
            Debug.Log("GameManager: синглтон создан и инициализирован.");
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0); // Загружаем рекорд из памяти               
    }


    private void Start()
    {
        uiManager.SetStartButtonToNewGame(); // Обновляем текст кнопки начала игры
    }


    private void Update()
    {
        // Обновляем таймер в UI, если игра не на паузе и не окончена
        if (CanPlay && uiManager != null)
        {
            uiManager.UpdateTimer(Time.deltaTime);
        }
    }


    private void OnEnable() 
    {
        if (Instance == this) 
        {
            // Обновляем UI при загрузке сцены
            if (uiManager != null)
            {
                uiManager.Initialize(score, highScore);
            }

            InputManager.OnPausePressed += TogglePause;                            // Подписываемся на событие нажатия паузы
            Snake.OnFoodEaten += ScoringPoints;                                    // Подписываемся на событие съедания еды
            RulesPanelUI.OnRulesPanelShown += OnSomePanelShowHandler;              // Подписываемся на событие показа панели с правилами
            LeaderboardPanelUI.OnLeaderboardPanelShown += OnSomePanelShowHandler;  // Подписываемся на событие показа панели с таблицей лидеров
        }
    }
    
    private void OnDisable() 
    {
        if (Instance == this) 
        {
            InputManager.OnPausePressed -= TogglePause;                           // Отписываемся от события нажатия паузы
            Snake.OnFoodEaten -= ScoringPoints;                                   // Отписываемся от события съедания еды
            RulesPanelUI.OnRulesPanelShown -= OnSomePanelShowHandler;             // Отписываемся от события показа панели с правилами
            LeaderboardPanelUI.OnLeaderboardPanelShown -= OnSomePanelShowHandler; // Отписываемся от события показа панели с таблицей лидеров

        }
    }
    

    private void LevelGameUp() // Метод увеличения уровня игры
    {
        
        if (_eatenApples % _appleToLevelUp == 0)
        {
            levelCount++; // Увеличиваем уровень
            
            Debug.Log("Level Up! Eaten " + _eatenApples + " apples.");
           
            OnLevelUp?.Invoke(); 
        }

        if (uiManager != null)
        {
           uiManager.UpdateLevel(levelCount); // Обновляем уровень в UI
        }

    }   
    

    // Метод для обработки окончания игры
    public void GameOver() 
    {       
       if (IsGameOver) return;   // Если игра уже окончена, выходим

       IsGameOver = true;        // Устанавливаем флаг окончания игры
       SetPause(true);           // Ставим игру на паузу при GameOver       
       OnWallsFlash?.Invoke();   // Уведомляем рамку о необходимости мигнуть       
       StartCoroutine(DelayedGameOver()); // Запускаем корутину с задержкой перед показом экрана окончания игры      
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSecondsRealtime(gameOverDelay);
        OnGameOver?.Invoke();
    }
    

    private void SetPause(bool paused)
    {
        if (IsGameOver && !paused) return;  // Заапрещаем снимать паузу, если GameOver

        IsPaused = paused;                  // Устанавливаем флаг паузы
        Time.timeScale = paused ? 0f : 1f;  // Останавливаем или возобновляем время       
        
    }

    private void TogglePause()
    {
        SetPause(!IsPaused); // Переключаем состояние паузы
    }


    private void OnSomePanelShowHandler()  // Обработчик события показа панели с правилами
    {
        SetPause(true);                     // Ставим игру на паузу при показе панели с правилами
    }




    // Метод для сброса игры через перезагрузку сцены
    public void RestartGame()  
    {        
        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);                 
    }

    private void ScoringPoints(FoodType foodType)
    {
        int points = 0;
        switch (foodType)
        {
            case FoodType.Normal:
                points = 10;
                break;
            case FoodType.Golden:
                points = 100;
                break;
            case FoodType.Poison:
                points = -30;
                break;
            default:
                points = 0;
                break;

        }
        score += points;    // Увеличиваем счет на соответствующее количество очков

        Debug.Log($"GameManager: Eaten {foodType} food. Score: {score}");
        
        if (uiManager != null) uiManager.UpdateScore(score); // Обновляем счет в UI        

        if (score > highScore)
        {
            highScore = score;                           // Обновляем рекорд
            PlayerPrefs.SetInt("HighScore", highScore);  // Сохраняем рекорд в памяти
        }

        _eatenApples++;                                          // Увеличиваем счетчик съеденных яблок
        Debug.Log("GameManger: Eaten Apples: " + _eatenApples);
        LevelGameUp();                                           // Проверяем, нужно ли повысить уровень
    }

}
