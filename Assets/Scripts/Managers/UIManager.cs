using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;             // Ссылка на текст для отображения счета
    [SerializeField] private TextMeshProUGUI _highScoreText;         // Ссылка на текст для отображения рекорда
    [SerializeField] private TextMeshProUGUI _levelText;             // Ссылка на текст для отображения уровня
    [SerializeField] private TextMeshProUGUI _startGameButtonText;   // Ссылка на текст кнопки начала игры

    public void UpdateScore (int score)    // Метод для обновления текста счета
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)  // Метод для обновления текста рекорда
    {
        _highScoreText.text = "Best: " + highScore;
    }

    public void UpdateLevel(int level)
    {
        _levelText.text = "Level: " + level;
    }
    

    public void SetStartButtonToNewGame()
    {
        _startGameButtonText.text = "New Game";
    }
    

    public void Initialize(int score, int highScore)  // Метод для инициализации UI
    {
        UpdateScore(score);          // Устанавливаем начальный счет
        UpdateHighScore(highScore);  // Устанавливаем начальный рекорд
    }
}
