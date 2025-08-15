using UnityEngine;

// Скрипт для открытия URL при клике на объект
public class OpenURLOnClick : MonoBehaviour
{
    // Поле для ссылки - можно задать в инспекторе
    public string url = "https://vk.com/gamedev_after_30";

    // Метод, которыйбудет вызываться при клике на объект
    public void Open()
    {
        Application.OpenURL(url); // Открываем URL в браузере
    }
}
