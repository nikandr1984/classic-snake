using UnityEngine;
using System;

public class RulesPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _rulesPanel; // Ссылка на панель с правилами игры

    public static event Action OnRulesPanelShown;    // Событие для уведомления о показе панели с правилами
    public static event Action OnRulesPanelHidden;   // Событие для уведомления о скрытии панели с правилами




    private void OnEnable()
    {
        InputManager.OnClosePressed += HideRulesPanel; // Подписываемся на событие нажатия закрытия окна
    }

    private void OnDisable()
    {
        InputManager.OnClosePressed -= HideRulesPanel; // Отписываемся от события при уничтожении объекта
    }




    public void ShowRulesPanel()      // Метод для показа панели с правилами
    {      
        _rulesPanel.SetActive(true);
        OnRulesPanelShown?.Invoke();
    }

    public void HideRulesPanel()      // Метод для скрытия панели с правилами
    {
        _rulesPanel.SetActive(false);
        OnRulesPanelHidden?.Invoke();
    }
}
