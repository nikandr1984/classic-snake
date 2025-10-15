using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;         // Ссылка на текст для отображения счета
    [SerializeField] private TextMeshProUGUI highScoreText;     // Ссылка на текст для отображения рекорда
    [SerializeField] private TextMeshProUGUI levelText;         // Ссылка на текст для отображения уровня

    public void UpdateScore (int score)    // Метод для обновления текста счета
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)  // Метод для обновления текста рекорда
    {
        highScoreText.text = "Best: " + highScore;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level;
    }
    

    public void Initialize(int score, int highScore)  // Метод для инициализации UI
    {
        UpdateScore(score);          // Устанавливаем начальный счет
        UpdateHighScore(highScore);  // Устанавливаем начальный рекорд
    }
}
