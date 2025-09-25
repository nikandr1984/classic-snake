using UnityEngine;
using System;

public class InputManager : MonoBehaviour  // Скрипт для обработки глобального ввода
{
    public static InputManager Instance { get; private set; }     // Синглтон для доступа к InputManager
    
    public static event Action OnPausePressed;                    // Событие для уведомления об нажатии паузы

    public static event Action OnMoveUpPressed;                   // Событие для уведомления об нажатии вверх
    public static event Action OnMoveDownPressed;                 // Событие для уведомления об нажатии вниз
    public static event Action OnMoveLeftPressed;                 // Событие для уведомления об нажатии влево
    public static event Action OnMoveRightPressed;                // Событие для уведомления об нажатии вправо


    private void Awake()
    {
        // Реализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    void Update()
    {
        HandleSystemInput();    // Обработка системного ввода (пауза)
        HandleGameplayInput();  // Обработка игрового ввода (движение)
    }


    //===============================================

    private void HandleSystemInput() // Обработка системного ввода
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePressed?.Invoke(); 
        }
    }


    private void HandleGameplayInput() // Обработка геймплейного ввода
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            OnMoveUpPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            OnMoveDownPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            OnMoveLeftPressed?.Invoke(); 
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            OnMoveRightPressed?.Invoke(); 
        }
    }
}


