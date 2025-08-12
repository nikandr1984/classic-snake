using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // ������ ���, ������� ����� ���������

    // ������� ������
    public int xRange = 8;
    public int yRange = 8;


    void Start()
    {
        SpawnFood();
    }

    
    public void SpawnFood()  // �����, ������� ������� ���
    {        
        // ��������� ����������
        int x = Random.Range(-xRange, xRange + 1);
        int y = Random.Range(-yRange, yRange + 1);

        // ������� ����� ���
        Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
