using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;             // Ссылка на текст для отображения счета    
    [SerializeField] private TextMeshProUGUI _levelText;             // Ссылка на текст для отображения уровня
    [SerializeField] private TextMeshProUGUI _startGameButtonText;   // Ссылка на текст кнопки начала игры
    [SerializeField] private TextMeshProUGUI _timerText;             // Ссылка на текст для отображения таймера 
    


    private void Update()
    {
        if (GameManager.Instance.CanPlay)
        {
            UpdateTimer(); // Обновляем таймер, если игра не на паузе и не окончена
        }
    }


    public void UpdateScore (int score)    // Метод для обновления текста счета
    {
        _scoreText.text = "Score: " + score;
    }
    

    public void UpdateLevel(int level)
    {
        _levelText.text = "Level: " + level;
    }
    

    public void SetStartButtonToNewGame()
    {
        _startGameButtonText.text = "New Game";
    }
    

    public void UpdateTimer()
    {
        float gameTime = GameManager.Instance.GetGameTime();
        int minutes = (int)(gameTime / 60f);
        int seconds = (int)(gameTime % 60f);
        _timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }

    public void Initialize(int score)  // Метод для инициализации UI
    {
        UpdateScore(score);          // Устанавливаем начальный счет        
    }
}
