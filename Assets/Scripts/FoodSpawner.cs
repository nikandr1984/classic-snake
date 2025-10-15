using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _normalFoodPrefab;  // Префаб обычной еды
    [SerializeField] private GameObject _goldenFoodPrefab;  // Префаб золотой еды
    [SerializeField] private GameObject _poisonFoodPrefab;  // Префаб ядовитой еды

    [SerializeField] private int _xRange = 7;               // Границы спавна по X
    [SerializeField] private int _yRange = 7;               // Границы спавна по Y

    [SerializeField] private int _triggerGoldenFood = 3;    // Количество созданной еды для спавна золотой еды
    [SerializeField] private int _poisonFoodCount = 3;      // Количество ядовитой еды для спавна

    private int _countSpawnedFood = 0;                      // Счетчик созданной еды
    


    void Start()
    {
       Instantiate(_normalFoodPrefab, GetRandomPosition(), Quaternion.identity); // Спавн первой еды
       _countSpawnedFood = 1;
    }


    private void OnEnable()
    {
        Snake.OnFoodEaten     += SpawnFood;         // Подписка на событие съедания еды
        GameManager.OnLevelUp += SpawnPoisonFood;   // Подписка на событие повышения уровня
    }
    private void OnDisable()
    {
        Snake.OnFoodEaten     -= SpawnFood;        // Отписка от события съедания еды
        GameManager.OnLevelUp -= SpawnPoisonFood;  // Отписка от события повышения уровня
    }
    

    private void SpawnFood(FoodType foodType)  
    {
        if (foodType == FoodType.Golden || foodType == FoodType.Poison)
        {
            return; // Если съедена золотая или ядовитая еда, не спавним новую
        }

        Instantiate(_normalFoodPrefab, GetRandomPosition(), Quaternion.identity); // Спавним обычную еду
        _countSpawnedFood++;   
        
        if (_countSpawnedFood % _triggerGoldenFood == 0)
        {
            Instantiate(_goldenFoodPrefab, GetRandomPosition(), Quaternion.identity); // Спавним золотую еду
        }        
    }

    private void SpawnPoisonFood()  // Метод для спавна ядовитой еды
    {
        if (GameManager.Instance.levelCount >= 3)
        {
            StartCoroutine(SpawnPoisonAfterDelay());
        }
    }

    private IEnumerator SpawnPoisonAfterDelay()   // Корутин для спавна ядовитой еды с задержкой
    {
        float delay = Random.Range(2f, 20f);      // Случайная задержка перед спавном ядовитой еды
        yield return new WaitForSeconds(delay);
        
        for (int i = 0; i < _poisonFoodCount; i++)
        {
            Instantiate(_poisonFoodPrefab, GetRandomPosition(), Quaternion.identity);  // Спавним ядовитую еду
            yield return new WaitForSeconds(1f);                                       // Небольшая задержка между спавнами
        }
        
    }

    private Vector3 GetRandomPosition()
    {
        int x = Random.Range(-_xRange, _xRange + 1);
        int y = Random.Range(-_yRange, _yRange + 1);
        return new Vector3(x, y, 0);
    }
    
}
