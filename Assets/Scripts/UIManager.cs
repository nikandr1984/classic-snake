using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;      // ������ �� ����� ��� ����������� �����
    public TextMeshProUGUI highScoreText;  // ������ �� ����� ��� ����������� �������

    public void UpdateScore (int score)    // ����� ��� ���������� ������ �����
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHighScore(int highScore)  // ����� ��� ���������� ������ �������
    {
        highScoreText.text = "Best: " + highScore;
    }

    public void Initialize(int score, int highScore)  // ����� ��� ������������� UI
    {
        UpdateScore(score);          // ������������� ��������� ����
        UpdateHighScore(highScore);  // ������������� ��������� ������
    }


}
