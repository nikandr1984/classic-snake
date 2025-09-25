using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }   // Синглтон

    public static event Action<bool> OnPauseToggle;  // Событие: пауза включена/выключена

    private bool _isPaused = false;                  // Флаг: игра на паузе?
    private bool _isGameOver = false;                // Флаг: игра окончена?

    public static bool IsPaused => Instance != null ? Instance._isPaused : false;     // Статус паузы
    public static bool IsGameOver => Instance != null ? Instance._isGameOver : false; // Статус окончания игры
    public static bool CanPlay => Instance != null && !Instance._isPaused && !Instance._isGameOver; // Можно ли играть?

    [SerializeField] private bool pauseOnStart = false; // Пауза при старте игры?  
    
    
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
                
        _isGameOver = false;
        _isPaused = pauseOnStart;

        Time.timeScale = IsPaused ? 0f : 1f;  
    }

    

    private void OnEnable()
    {
        InputManager.OnPausePressed += TogglePause; // Подписываемся на событие нажатия паузы
    }
   
    private void OnDisable()
    {
        InputManager.OnPausePressed -= TogglePause; // Отписываемся от события при отключении
    }



    public static void TogglePause()  // Метод переключения паузы
    {
        if (Instance == null || Instance._isGameOver) return; // Если нет PauseManager или игра окончена, выходим
        SetPause(!Instance._isPaused);                        // Переключаем состояние паузы
    }


    public static void SetPause(bool paused) // Метод установки паузы
    {
        if (Instance == null) return;
        if (Instance._isGameOver && !paused) return; // Если игра окончена, нельзя снять паузу

        Instance._isPaused = paused;       // Устанавливаем состояние паузы
        Time.timeScale = paused ? 0f : 1f; // Останавливаем или возобновляем время
        OnPauseToggle?.Invoke(paused);     // Вызываем событие изменения паузы

        SnakeMovement[] movements = FindObjectsByType<SnakeMovement>(FindObjectsSortMode.None);
        foreach (var m in movements)
        {
            m.SetInputEnabled(!paused);
        }
    }
    

    public static void SetGameOver()     // Метод установки состояния "игра окончена"
    {
        if (Instance == null) return;
        Instance._isGameOver = true;     // Устанавливаем флаг окончания игры
        SetPause(true);                  // Ставим игру на паузу
    }    
}

