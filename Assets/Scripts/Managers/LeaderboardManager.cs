using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;                               // Синглтон для доступа к LeaderbordManager

    [SerializeField] private int _maxEntries = 5;                            // Максимальное количество записей в таблице лидеров
    [SerializeField] private string _saveKey = "LeaderbordData";             // Ключ для сохранения данных в PlayerPrefs

    private List<LeaderboardEntry> _entries = new List<LeaderboardEntry>();  // Список записей таблицы лидеров

    private void Awake()
    {
        if (Instance == null)  // Инициализация синглтона
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
            Debug.Log("LeaderboardManager: Singleton initialized. Uploaded records: " + _entries.Count);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveLeaderboard() // Сохранение таблицы лидеров в PlayerPrefs
    {
        string json = JsonUtility.ToJson(new LeaderboardData { entries = _entries });
        PlayerPrefs.SetString(_saveKey, json);
        PlayerPrefs.Save();
    }


    private void LoadLeaderboard() // Загрузка таблицы лидеров из PlayerPrefs
    {
        string json = PlayerPrefs.GetString(_saveKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            var data = JsonUtility.FromJson<LeaderboardData>(json);
            _entries = data.entries ?? new List<LeaderboardEntry>();
        }
        else
        {
            _entries = new List<LeaderboardEntry>(); // Если нет данных - создаем пустую таблицу
        }
    }


    public void AddEntry(string playerName, int score, float gameTime = 0f, int level = 1) // Добавляем новый результат
    {
        // Создаем новую запись
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score, gameTime);

        // Добавляем запись в список
        _entries.Add(newEntry);

        // Сортируем по убыванию очков 
        _entries = _entries.OrderByDescending(e => e.score).ToList();

        // Обрезаем список до максимального количества записей
        if (_entries.Count > _maxEntries)
        {
            _entries.RemoveAt(_entries.Count - 1);
        }

        // Сохраняем
        SaveLeaderboard();
    }


    public List<LeaderboardEntry> GetEntries() // Получаем текущие записи таблицы лидеров
    {
        return new List<LeaderboardEntry>(_entries); // Возвращаем копию списка для безопасности
    }


    public bool IsTopScore(int score)
    {
        if (_entries.Count < _maxEntries) // Если таблица не заполнена - любой результат попадает
        {
            return true;
        }

        return score > _entries[_maxEntries - 1].score; // Сравниваем с последним результатом в таблице
    }
}

    
[System.Serializable] // Вспомогательный класс для сериализации
public class LeaderboardData
{
   public List<LeaderboardEntry> entries;
}

