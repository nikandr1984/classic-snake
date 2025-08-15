using UnityEngine;

// ������ ��� �������� URL ��� ����� �� ������
public class OpenURLOnClick : MonoBehaviour
{
    // ���� ��� ������ - ����� ������ � ����������
    public string url = "https://vk.com/gamedev_after_30";

    // �����, ������������ ���������� ��� ����� �� ������
    public void Open()
    {
        Application.OpenURL(url); // ��������� URL � ��������
    }
}
