using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip _normalFoodEat; // Звук поедания обычной еды
    [SerializeField] private AudioClip _goldenFoodEat; // Звук поедания золотой еды
    [SerializeField] private AudioClip _poisonFoodEat; // Звук поедания ядовитой еды
    [SerializeField] private AudioClip _slowFoodEat;   // Звук поедания замедляющей еды
    [SerializeField] private AudioClip _crashSnake;    // Звук столкновения змейки
    [SerializeField] private AudioClip _levelUp;       // Звук повышения уровня
    
    private AudioSource _audioSource;   // Компонент AudioSource для воспроизведения звуков

    private int _countPlayEatFood = 0;  // Счетчик воспроизведений звука еды
        

    private void Awake()
    {
        // Инициализация синглтона
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource для воспроизведения звуков
        _audioSource.playOnAwake = false;                      // Отключаем автоматическое воспроизведение
        _audioSource.volume = 0.8f;                            // Устанавливаем громкость
    }

    
    private void OnEnable()
    {
        Snake.OnFoodEaten        += PlayEatFood;         // Подписываемся на событие съедания еды
        GameManager.OnWallsFlash += PlayCrashSnake;      // Подписываемся на событие окончания игры
        GameManager.OnLevelUp    += PlayLevelUpSound;    // Подписываемся на событие повышения уровня
    }

    private void OnDisable()
    {
        Snake.OnFoodEaten        -= PlayEatFood;         // Отписываемся от события съедания еды
        GameManager.OnWallsFlash -= PlayCrashSnake;      // Отписываемся от события окончания игры
        GameManager.OnLevelUp    -= PlayLevelUpSound;    // Отписываемся от события повышения уровня
    }

    
    private bool CanPlayEatFoodSound()
    {
        return GameManager.Instance.NormalFoodToLevelUp != _countPlayEatFood;
    }
    
    
    
    private void PlayEatFood(FoodType foodType) // Метод для воспроизведения звука съедания еды
    {
        if (foodType == FoodType.Normal)
        {
            _countPlayEatFood++;
        }
                
        
        if (foodType == FoodType.Normal && _normalFoodEat != null && CanPlayEatFoodSound())
        {
            _audioSource.PlayOneShot(_normalFoodEat);
            
        }
        else if (foodType == FoodType.Golden && _goldenFoodEat != null)
        {
            _audioSource.PlayOneShot(_goldenFoodEat);
        }
        else if (foodType == FoodType.Poison && _poisonFoodEat != null)
        {
            _audioSource.PlayOneShot(_poisonFoodEat);
        }
        else if (foodType == FoodType.Slow && _slowFoodEat != null)
        {
            _audioSource.PlayOneShot(_slowFoodEat);
        }


    }


    private void PlayCrashSnake() // Метод для воспроизведения звука столкновения змейки
    {
        if (_crashSnake != null)
        {
            _audioSource.PlayOneShot(_crashSnake);
        }
    }

    private void PlayLevelUpSound() // Метод для воспроизведения звука повышения уровня
    {
        if (_levelUp != null)
        {
            _audioSource.PlayOneShot(_levelUp);
        }
    }    


}


