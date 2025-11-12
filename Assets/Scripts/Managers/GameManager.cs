using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{          
    public static GameManager Instance;  // Синглтон для доступа к GameManager


    // Статические события
    public static event System.Action OnGameOver;    // Событие для уведомления об окончании игры
    public static event System.Action OnWallsFlash;  // Событие для уведомления о мигании стен
    public static event System.Action OnLevelUp;     // Событие для уведомления о повышении уровня (каждые n яблок)


    // Ссылки на компоненты
    public UIManager uiManager;    // Ссылка на UIManager для обновления интерфейса
    public GameOverUI gameOverUI;  // Ссылка на GameOverUI для отображения экрана окончания игры
        

    // Свойства состояния игры
    public bool IsGameOver { get; private set; } = false; // Флаг: игра окончена? 
    public bool IsPaused { get; private set; } = false;   // Флаг: игра на паузе?
    public bool CanPlay => !IsPaused && !IsGameOver;      // Можно ли играть?
    public int Score { get; private set; }                // Текущий счет    
    public int LevelCount { get; private set; } = 1;      // Текущий уровень


    // Приватные поля
    private float _gameTime = 0f;         // Время, прошедшее с начала игры 
    private int _eatenNormalFood = 0;     // Счетчик съеденных яблок
    private float _gameOverDelay = 1.5f;  // Задержка перед показом экрана окончания игры                               


    // Настройки через инспектор
    [SerializeField] private int _normalFoodToLevelUp = 3;  // Количество яблок для повышения уровня
    public int NormalFoodToLevelUp => _normalFoodToLevelUp;  // Геттер для количества яблок для повышения уровня                                                        




    private void Awake()
    {       
        Time.timeScale = 1f; // Устанавливаем нормальное время при старте игры

        // Инициализация синглтона
        if (Instance == null) 
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }                    
    }


    private void Start()
    {
        uiManager.SetStartButtonToNewGame(); // Обновляем текст кнопки начала игры
    }


    private void Update()
    {
        if (CanPlay)
        {
            _gameTime += Time.deltaTime; // Обновляем время игры, если игра не на паузе и не окончена
        }        
    }


    private void OnEnable() 
    {
        if (Instance == this) 
        {
            // Обновляем UI при загрузке сцены
            if (uiManager != null)
            {
                uiManager.Initialize(Score);
            }

            InputManager.OnPausePressed += TogglePause;                            // Подписываемся на событие нажатия паузы
            Snake.OnFoodEaten += ScoringPoints;                                    // Подписываемся на событие съедания еды
            Snake.OnFoodEaten += ScoringEatenNormalFood;                           // Подписываемся на событие съедания еды для подсчета обычной еды
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
            Snake.OnFoodEaten -= ScoringEatenNormalFood;                          // Отписываемся от события съедания еды для подсчета обычной еды
            RulesPanelUI.OnRulesPanelShown -= OnSomePanelShowHandler;             // Отписываемся от события показа панели с правилами
            LeaderboardPanelUI.OnLeaderboardPanelShown -= OnSomePanelShowHandler; // Отписываемся от события показа панели с таблицей лидеров
        }
    }
    

    public float GetGameTime() // Метод для получения времени игры
    {
        return _gameTime;
    }


    private void LevelGameUp() // Метод увеличения уровня игры
    {
        
        if (_eatenNormalFood % _normalFoodToLevelUp == 0)
        {
            LevelCount++; // Увеличиваем уровень                     
            OnLevelUp?.Invoke(); 
        }

        if (uiManager != null)
        {
           uiManager.UpdateLevel(LevelCount); // Обновляем уровень в UI
        }
    }   
    

    // Метод для обработки окончания игры
    public void GameOver() 
    {       
       if (IsGameOver) return;            // Если игра уже окончена, выходим

       IsGameOver = true;                 // Устанавливаем флаг окончания игры
       SetPause(true);                    // Ставим игру на паузу при GameOver       
       OnWallsFlash?.Invoke();            // Уведомляем рамку о необходимости мигнуть       
       StartCoroutine(DelayedGameOver()); // Запускаем корутину с задержкой перед показом экрана окончания игры      
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSecondsRealtime(_gameOverDelay);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагружаем сцену                
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
            case FoodType.Slow:
                points = +5;
                break;
            default:
                points = 0;
                break;
        }
        
        Score += points;    // Увеличиваем счет на соответствующее количество очков        
        
        if (uiManager != null) uiManager.UpdateScore(Score); // Обновляем счет в UI                     
    }


    private void ScoringEatenNormalFood(FoodType foodType)
    {
        if (foodType == FoodType.Normal) // Если съедена обычная еда, увеличиваем счетчик
        {
            _eatenNormalFood++;            
            LevelGameUp();    // Проверяем, нужно ли повысить уровень
        }                 
    }
}
