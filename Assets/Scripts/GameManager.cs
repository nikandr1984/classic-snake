using UnityEngine;

// ������ ��� ���������� ������ ������� ����: ����, ������, ���������
public class GameManager : MonoBehaviour
{
    // ����������� ���� - �������� �� ������ ������� ��� ������
    public static GameManager Instance; // �������� ��� ������� � GameManager

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
    }

    public void ResetScore() // ����� ��� ������ �����
    {
        score = 0; 
    }
}
