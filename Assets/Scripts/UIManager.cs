using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;      // Ссылка на текст для отображения счета
    public TextMeshProUGUI highScoreText;  // Ссылка на текст для отображения рекорда

    public void UpdateScore (int score)    // Метод для обновления текста счета
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)  // Метод для обновления текста рекорда
    {
        highScoreText.text = "Best: " + highScore;
    }

    public void Initialize(int score, int highScore)  // Метод для инициализации UI
    {
        UpdateScore(score);          // Устанавливаем начальный счет
        UpdateHighScore(highScore);  // Устанавливаем начальный рекорд
    }


}
