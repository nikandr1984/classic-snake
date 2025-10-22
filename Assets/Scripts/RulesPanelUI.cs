using UnityEngine;
using System;

public class RulesPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _rulesPanel; // Ссылка на панель с правилами игры

    public static event Action OnRulesPanelShown;    // Событие для уведомления о показе панели с правилами
    public static event Action OnRulesPanelHidden;   // Событие для уведомления о скрытии панели с правилами



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
