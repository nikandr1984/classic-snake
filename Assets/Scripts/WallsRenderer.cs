using UnityEngine;
using System.Collections;
using System;

// Скрипт для управления цветом рамки (мигание при столкновении)
public class WallsRenderer : MonoBehaviour
{
    // Ссылка на LineRenderer
    [SerializeField] private LineRenderer lineRenderer;


    private void OnEnable()
    {
        if (lineRenderer != null)
        {
            GameManager.OnWallsFlash += HandleWallsFlash; // Подписываемся на событие мигания стен
        }        
    }

    private void OnDisable()
    {
        if (lineRenderer != null)
        {
            GameManager.OnWallsFlash -= HandleWallsFlash; // Отписываемся от события мигания стен
        }            
    }


    private void HandleWallsFlash() // Обработчик события мигания стен
    {
        if (lineRenderer != null)
        {
            Flash(Color.red, 0.15f, 2);
        }            
    }


    // Вызови этот метод, чтобы рамка мигнула
    public void Flash(Color flashColor, float duration, int times, Action onFinished = null)
    {
        StartCoroutine(AnimateFlash(flashColor, duration, times));
    }

    private IEnumerator AnimateFlash(Color flashColor, float duration, int times)
    {
        Color original = lineRenderer.startColor;
                
        for (int i = 0; i < times; i++)
        {
            // Включаем цвет
            lineRenderer.startColor = flashColor;
            lineRenderer.endColor = flashColor;
            yield return new WaitForSecondsRealtime(duration); 
           
            // Возвращаем оригинальный цвет
            lineRenderer.startColor = original;
            lineRenderer.endColor = original;
            yield return new WaitForSecondsRealtime(duration);
        }     
    }
}
