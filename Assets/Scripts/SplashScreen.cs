using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1f;         // Время появления заставки
    [SerializeField] private float _splashDuration = 2f;         // Длительность показа заставки
    [SerializeField] private float _fadeOutDuration = 1f;        // Время исчезновения заставки
    [SerializeField] private string _nextSceneName = "_Scene_0"; // Имя следующей сцены

    [SerializeField] private Image _logoImage;                   // Ссылка на изображение логотипа


    private CanvasGroup _canvasGroup; // Компонент CanvasGroup для управления прозрачностью

    private void Start()
    {
        // Инициализация CanvasGroup
        if (_logoImage == null) // Проверяем наличие ссылки на изображение
        {
            return;
        }
        
        _canvasGroup = _logoImage.GetComponent<CanvasGroup>(); // Пытаемся получить существующий CanvasGroup
        
        if (_canvasGroup == null) // Если его нет, добавляем новый
        {
            _canvasGroup = _logoImage.gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(ShowLogoWithFade());
    }

    private IEnumerator ShowLogoWithFade()
    {
        // 1. Плавное появление логотипа
        float elapsed = 0f;
        while (elapsed < _fadeInDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / _fadeInDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1f; // Гарантируем полную видимость


        // 2. Пауза - держим логотип
        yield return new WaitForSeconds(_splashDuration);


        // 3. Плавное исчезновение логотипа
        elapsed = 0f;
        while (elapsed < _fadeOutDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0f; // Гарантируем полную прозрачность


        // 4. Загружаем следующую сцену
        SceneManager.LoadScene(_nextSceneName);
    }
}
