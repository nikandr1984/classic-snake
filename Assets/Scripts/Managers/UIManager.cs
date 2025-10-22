using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;             // Ссылка на текст для отображения счета
    [SerializeField] private TextMeshProUGUI _highScoreText;         // Ссылка на текст для отображения рекорда
    [SerializeField] private TextMeshProUGUI _levelText;             // Ссылка на текст для отображения уровня
    [SerializeField] private TextMeshProUGUI _startGameButtonText;   // Ссылка на текст кнопки начала игры
    [SerializeField] private TextMeshProUGUI _timerText;             // Ссылка на текст для отображения таймера

    private float _gameTime;                                         // Время, прошедшее с начала игры


    public void UpdateScore (int score)    // Метод для обновления текста счета
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)  // Метод для обновления текста рекорда
    {
        _highScoreText.text = "Best Result: " + highScore;
    }

    public void UpdateLevel(int level)
    {
        _levelText.text = "Level: " + level;
    }
    

    public void SetStartButtonToNewGame()
    {
        _startGameButtonText.text = "New Game";
    }
    

    public void UpdateTimer(float deltaTime)
    {
        _gameTime += deltaTime;
        int minutes = (int)(_gameTime / 60f);
        int seconds = (int)(_gameTime % 60f);
        _timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }

    public void Initialize(int score, int highScore)  // Метод для инициализации UI
    {
        UpdateScore(score);          // Устанавливаем начальный счет
        UpdateHighScore(highScore);  // Устанавливаем начальный рекорд
    }
}
