using UnityEngine;
using System.Collections;

// ������ ��� ���������� ������ ����� (������� ��� ������������)
public class WallsRenderer : MonoBehaviour
{
    // ������ �� LineRenderer
    [SerializeField] private LineRenderer lineRenderer;

    // ������ ���� �����, ����� ����� �������
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
