using UnityEngine;
using System;

public class InputManager : MonoBehaviour  // Скрипт для обработки глобального ввода
{
    public static InputManager Instance { get; private set; }  // Синглтон для доступа к InputManager
    
    public static event Action OnPausePressed;      // Событие для уведомления об нажатии паузы
    public static event Action OnClosePressed;      // Событие для уведомления об нажатии закрытия окна

    public static event Action OnMoveUpPressed;     // Событие для уведомления об нажатии вверх
    public static event Action OnMoveDownPressed;   // Событие для уведомления об нажатии вниз
    public static event Action OnMoveLeftPressed;   // Событие для уведомления об нажатии влево
    public static event Action OnMoveRightPressed;  // Событие для уведомления об нажатии вправо

    public bool IsIgnoreSystemInput { get; private set; } = true;    // Флаг для игнорирования системного ввода (пауза)


    private void Awake()
    {
        // Реализация синглтона
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += AllowSystemInput;                      // Разрешаем системный ввод при старте игры
        RulesPanelUI.OnRulesPanelShown  += IgnoreSystemInput;               // Игнорируем системный ввод при показе панели с правилами
        RulesPanelUI.OnRulesPanelHidden += AllowSystemInput;                // Разрешаем системный ввод при скрытии панели с правилами
        LeaderboardPanelUI.OnLeaderboardPanelShown  += IgnoreSystemInput;   // Игнорируем системный ввод при показе панели с таблицей лидеров
        LeaderboardPanelUI.OnLeaderboardPanelHidden += AllowSystemInput;    // Разрешаем системный ввод при скрытии панели с таблицей лидеров
        CreditsPanelUI.OnCreditsPanelShown += IgnoreSystemInput;            // Игнорируем системный ввод при показе панели с создателями игры
        CreditsPanelUI.OnCreditsPanelHidden += AllowSystemInput;            // Разрешаем системный ввод при скрытии панели с создателями игры                                                                   
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= AllowSystemInput;                      // Отписываемся от события при уничтожении объекта
        RulesPanelUI.OnRulesPanelShown  -= IgnoreSystemInput;               // Отписываемся от события при уничтожении объекта
        RulesPanelUI.OnRulesPanelHidden -= AllowSystemInput;                // Отписываемся от события при уничтожении объекта
        LeaderboardPanelUI.OnLeaderboardPanelShown  -= IgnoreSystemInput;   // Отписываемся от события при уничтожении объекта
        LeaderboardPanelUI.OnLeaderboardPanelHidden -= AllowSystemInput;    // Отписываемся от события при уничтожении объекта
        CreditsPanelUI.OnCreditsPanelShown -= IgnoreSystemInput;            // Отписываемся от события при уничтожении объекта
        CreditsPanelUI.OnCreditsPanelHidden -= AllowSystemInput;            // Отписываемся от события при уничтожении объекта
    }



    void Update()
    {           
        HandleSystemInput();    // Обработка системного ввода (старт, пауза)
        HandleGameplayInput();  // Обработка игрового ввода (движение)
        HandleCloseInput();     // Обработка ввода закрытия окна
    }


    
    private void HandleSystemInput() // Метод обработки системного ввода
    {
        if (IsIgnoreSystemInput) return; // Если установлен флаг игнорирования, не обрабатываем ввод

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPausePressed?.Invoke(); 
        }    
    }

    private void HandleCloseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClosePressed?.Invoke();
        }
    }


    private void HandleGameplayInput() // Обработка геймплейного ввода
    {
        if (GameManager.Instance == null || !GameManager.Instance.CanPlay) return; // Если игра на паузе или окончена, не обрабатываем ввод

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnMoveUpPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnMoveDownPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnMoveLeftPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnMoveRightPressed?.Invoke(); 
        }
    }

    private void IgnoreSystemInput()
    {
        IsIgnoreSystemInput = true;
    }

    private void AllowSystemInput()
    {
        IsIgnoreSystemInput = false;
    }
}


