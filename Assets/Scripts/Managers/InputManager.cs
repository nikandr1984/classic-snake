using UnityEngine;
using System;

public class InputManager : MonoBehaviour  // Скрипт для обработки глобального ввода
{
    public static InputManager Instance { get; private set; }  // Синглтон для доступа к InputManager
    
    public static event Action OnPausePressed;                 // Событие для уведомления об нажатии паузы

    public static event Action OnMoveUpPressed;                // Событие для уведомления об нажатии вверх
    public static event Action OnMoveDownPressed;              // Событие для уведомления об нажатии вниз
    public static event Action OnMoveLeftPressed;              // Событие для уведомления об нажатии влево
    public static event Action OnMoveRightPressed;             // Событие для уведомления об нажатии вправо

    public bool IsIgnoreSystemInput { get; private set; } = false;    // Флаг для игнорирования системного ввода (пауза)


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
        RulesPanelUI.OnRulesPanelShown  += IgnoreSystemInput;               // Игнорируем системный ввод при показе панели с правилами
        RulesPanelUI.OnRulesPanelHidden += AllowSystemInput;                // Разрешаем системный ввод при скрытии панели с правилами
        LeaderboardPanelUI.OnLeaderboardPanelShown  += IgnoreSystemInput;   // Игнорируем системный ввод при показе панели с таблицей лидеров
        LeaderboardPanelUI.OnLeaderboardPanelHidden += AllowSystemInput;    // Разрешаем системный ввод при скрытии панели с таблицей лидеров     
    }

    private void OnDisable()
    {
        RulesPanelUI.OnRulesPanelShown  -= IgnoreSystemInput;               // Отписываемся от события при уничтожении объекта
        RulesPanelUI.OnRulesPanelHidden -= AllowSystemInput;                // Отписываемся от события при уничтожении объекта
        LeaderboardPanelUI.OnLeaderboardPanelShown  -= IgnoreSystemInput;   // Отписываемся от события при уничтожении объекта
        LeaderboardPanelUI.OnLeaderboardPanelHidden -= AllowSystemInput;    // Отписываемся от события при уничтожении объекта        
    }



    void Update()
    {             
        HandleSystemInput();    // Обработка системного ввода (пауза)
        HandleGameplayInput();  // Обработка игрового ввода (движение)
    }


    
    private void HandleSystemInput() // Метод обработки системного ввода
    {
        if (IsIgnoreSystemInput) return; // Если установлен флаг игнорирования, не обрабатываем ввод

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePressed?.Invoke(); 
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


