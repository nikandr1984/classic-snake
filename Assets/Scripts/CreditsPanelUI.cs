using System;
using UnityEngine;

public class CreditsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _creditsPanel; // Ссылка на панель с создателями игры

    public static event Action OnCreditsPanelShown;    // Событие для уведомления о показе панели с создателями игры
    public static event Action OnCreditsPanelHidden;   // Событие для уведомления о скрытии панели с создателями игры

    
    
    private void OnEnable()
    {
        InputManager.OnClosePressed += HideCreditsPanel; // Подписываемся на событие нажатия закрытия окна
    }

    private void OnDisable()
    {
        InputManager.OnClosePressed -= HideCreditsPanel; // Отписываемся от события при уничтожении объекта
    }




    public void ShowCreditsPanel()      // Метод для показа панели с создателями игры
    {
        _creditsPanel.SetActive(true);
        OnCreditsPanelShown?.Invoke();
    }

    public void HideCreditsPanel()      // Метод для скрытия панели с создателями игры
    {
        _creditsPanel.SetActive(false);
        OnCreditsPanelHidden?.Invoke();
    }
}
