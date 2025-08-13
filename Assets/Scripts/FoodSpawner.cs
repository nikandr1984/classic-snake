using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // Префаб еды, который будем создавать

    // Границы спавна
    public int xRange = 7;
    public int yRange = 7;


    void Start()
    {
        SpawnFood();
    }

    
    public void SpawnFood()  // Метод, который создает еду
    {        
        // Случайные координаты
        int x = Random.Range(-xRange, xRange + 1);
        int y = Random.Range(-yRange, yRange + 1);

        // Создаем новую еду
        Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
