using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;             // Ссылка на текст для отображения счета    
    [SerializeField] private TextMeshProUGUI _levelText;             // Ссылка на текст для отображения уровня
    [SerializeField] private TextMeshProUGUI _startGameButtonText;   // Ссылка на текст кнопки начала игры
    [SerializeField] private TextMeshProUGUI _timerText;             // Ссылка на текст для отображения таймера
    [SerializeField] private Image _playPauseIconImage;              // Ссылка Image-компонент иконки play/pause
    [SerializeField] private Sprite _playIcon;                       // Ссылка на спрайт иконки play
    [SerializeField] private Sprite _pauseIcon;                      // Ссылка на спрайт иконки pause                                                                 

      


    private void Update()
    {
        if (GameManager.Instance.CanPlay)
        {
            UpdateTimer(); // Обновляем таймер, если игра не на паузе и не окончена
        }

        
        if (GameManager.Instance.IsGameStarted)
        {
            _playPauseIconImage.gameObject.SetActive(true);
        }
        else
        {
            _playPauseIconImage.gameObject.SetActive(false);
        }


        if (GameManager.Instance.IsGameStarted && GameManager.Instance.IsPaused)
        {
            _playPauseIconImage.sprite = _pauseIcon; // Показываем иконку пауза
        }
        else if (GameManager.Instance.IsGameStarted && !GameManager.Instance.IsPaused)
        {
            _playPauseIconImage.sprite = _playIcon; // Показываем иконку плей
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
        
    

    public void UpdateTimer()
    {
        float gameTime = GameManager.Instance.GetGameTime();
        int minutes = (int)(gameTime / 60f);
        int seconds = (int)(gameTime % 60f);
        _timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }


    private void ShowPayPauseUcon()
    {
        if (GameManager.Instance.IsGameStarted)
        {
            _playPauseIconImage.gameObject.SetActive(true);
        }
    }
       
    

    public void Initialize(int score)  // Метод для инициализации UI
    {
        UpdateScore(score);            // Устанавливаем начальный счет        
    }
}
