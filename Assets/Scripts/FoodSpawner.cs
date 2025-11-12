using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;  // Синглтон для доступа к FoodSpawner

    [Header("Food Prefabs")]
    [SerializeField] private GameObject _normalFoodPrefab;  // Префаб обычной еды
    [SerializeField] private GameObject _goldenFoodPrefab;  // Префаб золотой еды
    [SerializeField] private GameObject _poisonFoodPrefab;  // Префаб ядовитой еды
    [SerializeField] private GameObject _slowFoodPrefab;    // Префаб скоростной еды

    private int _xRange = 6;  // Границы спавна еды по X
    private int _yRange = 6;  // Границы спавна еды по Y

    [Header("Golden food settings")]
    [SerializeField] private int _triggerGoldenFood = 3;  // Количество созданной еды для спавна золотой еды
    public int TriggerGoldenFood => _triggerGoldenFood;   // Геттер для количества созданной еды для спавна золотой еды

    [Header("Poison food settings")]
    [SerializeField] private int _poisonFoodCount = 2;         // Количество ядовитой еды для спавна
    [SerializeField] private int _poisonSpawnAfterLevel = 3;   // Уровень, с которого начинается спавн ядовитой еды
    [SerializeField] private float _minPoisonSpawnDelay = 2f;  // Минимальная задержка перед спавном ядовитой еды
    [SerializeField] private float _maxPoisonSpawnDelay = 20f; // Максимальная задержка перед спавном ядовитой еды

    [Header("Slow food settings")]
    [SerializeField] private int _slowFoodCount = 3;          // Количество замедляющей еды для спавна
    [SerializeField] private int _slowSpawnEveryLevel = 4;    // Спавн замедляющей еды на каждом n уровне   
    public int SlowFoodCount => _slowFoodCount;               // Геттер для количества замедляющей еды

    private int _countSpawnedNormalFood = 0;                  // Счетчик созданной еды
    

    private void Awake()
    {
        // Инициализация синглтона
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }
    }


    void Start()
    {
       Instantiate(_normalFoodPrefab, GetRandomPosition(), Quaternion.identity); // Спавн первой еды
       _countSpawnedNormalFood = 1;
    }


    private void OnEnable()
    {
        Snake.OnFoodEaten     += SpawnFood;         // Подписка на событие съедания еды
        GameManager.OnLevelUp += PoisonFoodSpawn;   // Подписка на событие повышения уровня для ядовитой еды
        GameManager.OnLevelUp += SlowFoodSpawn;     // Подписка на событие повышения уровня для замедляющей еды
    }

    private void OnDisable()
    {
        Snake.OnFoodEaten     -= SpawnFood;        // Отписка от события съедания еды
        GameManager.OnLevelUp -= PoisonFoodSpawn;  // Отписка от события повышения уровня
        GameManager.OnLevelUp -= SlowFoodSpawn;    // Отписка от события повышения уровня
    }
    

    private void SpawnFood(FoodType foodType)  
    {
        // Если съедена золотая, ядовитая или скоростная еда, не спавним новую
        if (foodType == FoodType.Golden || foodType == FoodType.Poison || foodType == FoodType.Slow)
        {
            return; 
        }


        // Спавн обычной еды
        Instantiate(_normalFoodPrefab, GetRandomPosition(), Quaternion.identity); // Спавним обычную еду
        _countSpawnedNormalFood++;


        // Спавн золотой еды
        if (_countSpawnedNormalFood % _triggerGoldenFood == 0) 
        {
            Instantiate(_goldenFoodPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    private void PoisonFoodSpawn()  // Спавн ядовитой еды
    {
        if (GameManager.Instance.LevelCount >= _poisonSpawnAfterLevel)
        {
            StartCoroutine(PoisonFoodCoroutine());
        }
    }

    private IEnumerator PoisonFoodCoroutine()     // Корутин для спавна ядовитой еды с задержкой
    {
        float delay = Random.Range(_minPoisonSpawnDelay, _maxPoisonSpawnDelay);      // Случайная задержка перед спавном ядовитой еды
        yield return new WaitForSeconds(delay);
        
        for (int i = 0; i < _poisonFoodCount; i++)
        {
            Instantiate(_poisonFoodPrefab, GetRandomPosition(), Quaternion.identity);  // Спавним ядовитую еду
            yield return new WaitForSeconds(1f);                                       // Небольшая задержка между спавнами
        }        
    }


    private void SlowFoodSpawn() // Спавн замедляющей еды
    {
        if (GameManager.Instance.LevelCount % _slowSpawnEveryLevel == 0)
        {
            StartCoroutine(SlowFoodCoroutine());
        }
    }


    private IEnumerator SlowFoodCoroutine()  // Корутин для спавна замедляющей еды
    {
        for (int i = 0; i < _slowFoodCount; i++)
        {
            Instantiate(_slowFoodPrefab, GetRandomPosition(), Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }

    private Vector3 GetRandomPosition()
    {
        int x = Random.Range(-_xRange, _xRange + 1);
        int y = Random.Range(-_yRange, _yRange + 1);
        return new Vector3(x, y, 0);
    }    
}
