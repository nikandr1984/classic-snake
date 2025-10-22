using UnityEngine;

public class LeaderboardPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _leaderboardPanel;  // Ссылка на панель с таблицей лидеров

    public static event System.Action OnLeaderboardPanelShown;    // Событие для уведомления о показе панели с таблицей лидеров
    public static event System.Action OnLeaderboardPanelHidden;   // Событие для уведомления о скрытии панели с таблицей лидеров


    public void ShowLeaderboardPanel()      // Метод для показа панели с таблицей лидеров
    {      
        _leaderboardPanel.SetActive(true);
        OnLeaderboardPanelShown?.Invoke();
    }

    public void HideLeaderboardPanel()      // Метод для скрытия панели с таблицей лидеров
    {
        _leaderboardPanel.SetActive(false);
        OnLeaderboardPanelHidden?.Invoke();
    }
}
