using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [SerializeField] private float _pitchIncreasePerLevel = 0.02f; // Увеличение скорости воспроизведения на уровень
    [SerializeField] private float _pitchResetDuration = 2f;       // За сколько секунд вернуть скорость к базовой

    private AudioSource _audioSource;       // Компонент AudioSource для воспроизведения музыки
    private float _basePitch = 1f;          // Базовая скорость воспроизведения
    private Coroutine _resetPitchCoroutine; // Короутина для плавного сброса скорости воспроизведения


    private void Awake()
    {
        // Инициализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _audioSource = gameObject.GetComponent<AudioSource>(); // Добавляем AudioSource для воспроизведения музыки
            
            if (_audioSource != null)
            {
                _basePitch = _audioSource.pitch; // Сохраняем базовую скорость воспроизведения
            }            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        GameManager.OnLevelUp  += IncreasePitch;   // Подписываемся на событие повышения уровня
        GameManager.OnGameOver += StartPitchReset; // Подписываемся на событие окончания игры
    }

    private void OnDisable()
    {
        GameManager.OnLevelUp  -= IncreasePitch;   // Отписываемся от события повышения уровня
        GameManager.OnGameOver -= StartPitchReset; // Отписываемся от события окончания игры
    }


    private void IncreasePitch() // Увеличиваем скорость воспроизведения
    {
        float newPitch = _audioSource.pitch + _pitchIncreasePerLevel;
        _audioSource.pitch = Mathf.Min(newPitch, 1.3f);                // Ограничиваем макс скорость             
    }


    public void DecreasePitch() // Уменьшаем скорость воспроизведения
    {
        _audioSource.pitch -= _pitchIncreasePerLevel;        
    }


    private void StartPitchReset() // Запускаем корутину для плавного сброса скорости воспроизведения
    {
        if (_resetPitchCoroutine != null) 
        {
           StopCoroutine( _resetPitchCoroutine );
        }

        _resetPitchCoroutine = StartCoroutine( ResetPitchSmoothlyCoroutine() );
    }

    private IEnumerator ResetPitchSmoothlyCoroutine() // Корутина для плавного сброса скорости воспроизведения
    {
        if (_audioSource == null) yield break;

        float startPitch  = _audioSource.pitch; // Текущая скорость воспроизведения
        float targetPitch = _basePitch;         // Целевая скорость воспроизведения
        float elapsedTime = 0f;                 // Время, прошедшее с начала сброса

        while (elapsedTime < _pitchResetDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;                       // Используем unscaledDeltaTime, чтобы игнорировать паузу
            float t = Mathf.Clamp01(elapsedTime / _pitchResetDuration);  // Доля пройденного времени от общей длительности
            _audioSource.pitch = Mathf.Lerp(startPitch, targetPitch, t); // Линейная интерполяция скорости воспроизведения
            yield return null;
        }

        _audioSource.pitch = targetPitch; // Устанавливаем точную целевую скорость воспроизведения
        _resetPitchCoroutine = null;

    }
}
