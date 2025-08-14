using UnityEngine;

// Скрипт для управления общими данными игры: счет, рекорд, состояние
public class GameManager : MonoBehaviour
{
    // Статические поля - доступны из любого скрипта без ссылки
    public static GameManager Instance; // Синглтон для доступа к GameManager

    public int score { get; private set; }      // Текущий счет
    public int highScore { get; private set; }  // Рекордный счет

    private void Awake()
    {
        // Паттерн Singleton - гарантируем, что GameManager будет единственным экземпляром
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняется между сценами
            Debug.Log("GameManager создан инициализирован.");
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0); // Загружаем рекорд из памяти
    }

    public void AddScore() // Метод вызывается, когда змейка съедает еду
    {
        score += 10; // Увеличиваем счет на 10

        // Проверяем побит ли рекорд
        if (score > highScore)
        {
            highScore = score; // Обновляем рекорд
            PlayerPrefs.SetInt("HighScore", highScore); // Сохраняем рекорд в памяти
        }            
    }

    public void ResetScore() // Метод для сброса счета
    {
        score = 0; 
    }
}
