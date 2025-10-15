using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{    
    private float _nextMoveTime;                // Время, когда можно сделать следующий шаг
    private List<Transform> _segments;          // Список сегментов тела змейки

    [SerializeField] private float _moveInterval = 0.3f;     // Интервал между шагами змейки (в секундах)
    [SerializeField] private float _minMoveInterval = 0.08f; // Минимальный интервал между шагами (макс. скорость)
    [SerializeField] private SnakeMovement _snakeMovement;   // Ссылка на обработчик ввода
    [SerializeField] private GameObject _bodyPrefab;         // Префаб сегмента тела змейки

    public static event Action<FoodType> OnFoodEaten;        // Событие, вызываемое при съедании еды    




    private void Start()
    {
        _segments = new List<Transform>(); // Инициализируем список сегментов
        _segments.Add(transform);          // Добавляем голову (сам объект, на котором скрипт)
         
        Debug.Log("Snake: Initialized with move interval " + _moveInterval);

    }

    private void Update()
    {
        // Двигаем змейку с заданным интервалом, если игра не на паузе
        if (GameManager.Instance.CanPlay && Time.time >= _nextMoveTime)
        {
            Move(); // Вызов метода движения
            _nextMoveTime = Time.time + _moveInterval; // Считаем, когда можно будет двигаться снова
        }
    }



    private void OnEnable()
    {
        if (_snakeMovement != null)
        {
            GameManager.OnLevelUp += HandleLevelUp;                     // Подписываемся на событие повышения уровня
        }
    }
    

    private void OnDisable()
    {
        if (_snakeMovement != null)
        {
            GameManager.OnLevelUp -= HandleLevelUp;                     // Отписываемся от события повышения уровня
        }
    }
    

    private void Move()
    {
        if (_snakeMovement == null) return;
                
        // 1. Получаем текущее направление движения из SnakeMovement
        Vector2 direction = _snakeMovement.GetNextDirection();


        // 2. Двигаем тело
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // 3. Двигаем голову
        Vector3 headPosition = transform.position;
        Vector3 nextHeadPosition = new Vector3(
            Mathf.Round(headPosition.x + direction.x),
            Mathf.Round(headPosition.y + direction.y),
            headPosition.z
        );
        transform.position = nextHeadPosition;

        // 4. Проверяем, не врезалась ли в себя
        CheckSelfCollision();        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            FoodsLife food = other.GetComponent<FoodsLife>();
            FoodType type = food != null ? food.foodType : FoodType.Normal;

            Destroy(other.gameObject); // Удаляем съеденную еду

            Grow();                    // Вызываем метод роста змейки

            OnFoodEaten?.Invoke(type); // Уведомляем, что съедена еда такого-то типа            
        }
        else if (other.CompareTag("Walls"))
        {        
            CrashedSnake();
        }
    }            
    

    public void Grow() 
    {
        // Создаем новый сегмент тела
        GameObject newSegment = Instantiate(_bodyPrefab);

        // Ставим его на место последнего сегмента (сначала он будет на том же месте, что и предыдущий)
        if (_segments.Count > 0)
        {
            newSegment.transform.position = _segments[_segments.Count - 1].position;
        }

        // Добавляем в список сегментов
        _segments.Add(newSegment.transform);        
    }


    // Метод проверки столкновения змейки с самой собой
    private void CheckSelfCollision()
    {
        // Позиция головы округленная
        Vector2 headPosition = transform.position;

        // Перебираем все сегменты тедла (0 = голова)
        for (int i = 1; i < _segments.Count; i++)
        {
            // Берем позицию сегмента
            Vector2 segmentPosition = _segments[i].position;

            // Сравниваем с головой (с небольшим допуском)
            if (Vector2.Distance(headPosition, segmentPosition) < 0.1f)
            {
                CrashedSnake();
                break;
            }
        }
    }

    private void CrashedSnake() // Метод для обработки окончания игры
    {                
        if (GameManager.Instance != null)  // Вызываем событие окончания игры
        {
            Debug.Log("Snake: Game Over вызван!");
            GameManager.Instance.GameOver(); 
        }        
    }

    private void HandleLevelUp()
    {
        // Обнуляем тело (оставляем только голову)
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.RemoveRange(1, _segments.Count - 1); // Оставляем только голову

        float newInterval = _moveInterval * 0.9f;                  // Уменьшаем интервал на 10%
        _moveInterval = Mathf.Max(newInterval, _minMoveInterval);  // Не даем интервалу стать меньше минимального
        
        Debug.Log("Snake: Speeed Up! New speed: " + _moveInterval + ". Min speed: " + _minMoveInterval);

    }
}
