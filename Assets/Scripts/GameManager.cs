using UnityEngine;

// ������ ��� ���������� ������ ������� ����: ����, ������, ���������
public class GameManager : MonoBehaviour
{
    // ����������� ���� - �������� �� ������ ������� ��� ������
    public static GameManager Instance; // �������� ��� ������� � GameManager
    public UIManager uiManager;         // ������ �� UIManager ��� ���������� ����������

    public int score { get; private set; }      // ������� ����
    public int highScore { get; private set; }  // ��������� ����

    private void Awake()
    {
        // ������� Singleton - �����������, ��� GameManager ����� ������������ �����������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ����������� ����� �������
            Debug.Log("GameManager ������ ���������������.");
        }
        else
        {
            Destroy(gameObject); // ������� ���������
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0); // ��������� ������ �� ������

        if (uiManager != null)
        {
            uiManager.Initialize(score, highScore); // �������������� UI � ������� ������ � ��������
        }
    }

    public void AddScore() // ����� ����������, ����� ������ ������� ���
    {
        score += 10; // ����������� ���� �� 10

        // ��������� ����� �� ������
        if (score > highScore)
        {
            highScore = score; // ��������� ������
            PlayerPrefs.SetInt("HighScore", highScore); // ��������� ������ � ������
        } 
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(score); // ��������� ���� � UI
        }
    }

    public void ResetScore() // ����� ��� ������ �����
    {
        score = 0; 
    }
}
