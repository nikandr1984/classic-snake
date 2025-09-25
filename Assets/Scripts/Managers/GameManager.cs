using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{          
    public static GameManager Instance;              // Синглтон для доступа к GameManager
                                                      
    public static event System.Action OnGameOver;    // Событие для уведомления об окончании игры
    public static event System.Action OnWallsFlash;  // Событие для уведомления о мигании стен

    public UIManager uiManager;          // Ссылка на UIManager для обновления интерфейса
    public GameOverUI gameOverUI;        // Ссылка на GameOverUI для отображения экрана окончания игры
    public float gameOverDelay = 1.5f;   // Задержка перед показом экрана окончания игры

    public bool IsGameOver { get; private set; } = false; // Флаг окончания игры


    public int score { get; private set; }      // Текущий счет
    public int highScore { get; private set; }  // Рекордный счет

    

    // ========================================

    private void Awake()
    {
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


    private void OnEnable() 
    {
        if (Instance == this) 
        {
            // Обновляем UI при загрузке сцены
            if (uiManager != null)
            {
                uiManager.Initialize(score, highScore);
            }
        }
    } 


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


    // Метод для сброса счета
    public void ResetScore() 
    {
        score = 0; 
    }   
    

    // Метод для обработки окончания игры
    public void GameOver() 
    {       
       if (IsGameOver) return;      // Если игра уже окончена, выходим

       IsGameOver = true;            // Устанавливаем флаг окончания игры
       PauseManager.SetGameOver();   // Устанавливаем флаг окончания игры в PauseManager       
       OnWallsFlash?.Invoke();       // Уведомляем рамку о необходимости мигнуть       
       
       StartCoroutine(DelayedGameOver()); // Запускаем корутину с задержкой перед показом экрана окончания игры      
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSecondsRealtime(gameOverDelay);
        OnGameOver?.Invoke();
    }
    

    // Метод для сброса игры через перезагрузку сцены
    public void RestartGame()  
    {        
        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);                 
    }


    // Метод для сброса состояния игры (без перезагрузки сцены)
    public void ResetGameState()
    {
        IsGameOver = false;
        score = 0;
    }
}
