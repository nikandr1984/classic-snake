using UnityEngine;
using System.Collections;

// Скрипт для управления цветом рамки (мигание при столкновении)
public class WallsRenderer : MonoBehaviour
{
    // Ссылка на LineRenderer
    [SerializeField] private LineRenderer lineRenderer;

    // Вызови этот метод, чтобы рамка мигнула
    public void Flash(Color flashColor, float duration)
    {
        StartCoroutine(AnimateFlash(flashColor, duration));
    }

    private IEnumerator AnimateFlash(Color flashColor, float duration)
    {
        Color original = lineRenderer.startColor;

        lineRenderer.startColor = flashColor;
        lineRenderer.endColor = flashColor;

        yield return new WaitForSeconds(duration);

        lineRenderer.startColor = original;
        lineRenderer.endColor = original;
    }
}
