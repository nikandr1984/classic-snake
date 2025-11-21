using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _leaderboardPanel;  // Ссылка на панель с таблицей лидеров
    [SerializeField] private Transform _entriesContainer;   // Ссылка на родительский объект для записей
    [SerializeField] private GameObject _entryPrefab;       // Ссылка на префаб записи таблицы лидеров

    private List<GameObject> _spawnedEntries = new List<GameObject>(); // Кэшируем созданные записи таблицы лидеров

    public static event System.Action OnLeaderboardPanelShown;    // Событие для уведомления о показе панели с таблицей лидеров
    public static event System.Action OnLeaderboardPanelHidden;   // Событие для уведомления о скрытии панели с таблицей лидеров

     
    private void OnEnable()
    {
        GameManager.OnGameOver      += RefreshLeaderboard;   // Подписываемся на событие окончания игры для обновления таблицы лидеров
        InputManager.OnClosePressed += HideLeaderboardPanel; // Подписываемся на событие нажатия закрытия окна                                              
    }

    private void OnDisable()
    {
        GameManager.OnGameOver      -= RefreshLeaderboard;   // Отписываемся от события при уничтожении объекта
        InputManager.OnClosePressed -= HideLeaderboardPanel; // Отписываемся от события при уничтожении объекта
    }


    public void ShowLeaderboardPanel()      // Метод для показа панели с таблицей лидеров
    {      
        _leaderboardPanel.SetActive(true);
        RefreshLeaderboard();              // Обновляем таблицу лидеров при показе панели
        OnLeaderboardPanelShown?.Invoke();
    }

    public void HideLeaderboardPanel()      // Метод для скрытия панели с таблицей лидеров
    {
        _leaderboardPanel.SetActive(false);
        OnLeaderboardPanelHidden?.Invoke();
    }

    private void RefreshLeaderboard()      // Метод для обновления таблицы лидеров
    {  
        if (LeaderboardManager.Instance == null)
        {            
            return;  // Проверяем, что ссылка не null
        }            

        var entries = LeaderboardManager.Instance.GetEntries(); // Получаем записи таблицы лидеров
        
        foreach (var entry in _spawnedEntries) // Удаляем старые записи
        {
            Destroy(entry);
        }
        
        _spawnedEntries.Clear(); // Очищаем кэш созданных записей

        for (int i = 0; i < entries.Count; i++) // Создаем новые записи
        {
            GameObject entryObj = Instantiate(_entryPrefab, _entriesContainer); // Создаем новую запись из префаба
            _spawnedEntries.Add(entryObj);  // Добавляем в кэш

            var texts = entryObj.GetComponentsInChildren<TextMeshProUGUI>(); // Получаем все текстовые компоненты в записи

            if (texts.Length >= 4) // Проверяем, что есть минимум 3 текстовых компонента
            {
                texts[0].text = (i + 1) + ".";                 // Устанавливаем позицию
                texts[1].text = entries[i].playerName;         // Устанавливаем имя игрока
                texts[2].text = entries[i].score.ToString();   // Устанавливаем счет игрока
                texts[3].text = entries[i].GetFormattedTime(); // Устанавливаем время игры игрока
            }
        }
    }
}
