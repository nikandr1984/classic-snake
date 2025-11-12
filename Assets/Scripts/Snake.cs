using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{    
    private float _nextMoveTime;                             // Время, когда можно сделать следующий шаг
    private int _eatenSlowFood = 0;                          // Счетчик съеденной замедляющей еды
    private Color _originalHeadColor;                        // Исходный цвет головы змейки
    private Color _newHeadColor = new Color(1f, 0.84f, 0f);  // Новый цвет головы (золотой)
    
    private List<Transform> _segments;                       // Список сегментов тела змейки

    [SerializeField] private float _moveInterval = 0.3f;     // Интервал между шагами змейки (в секундах)
    [SerializeField] private float _minMoveInterval = 0.08f; // Минимальный интервал между шагами (макс. скорость)
    [SerializeField] private float _maxMoveInterval = 0.4f;  // Максимальный интервал между шагами (мин. скорость)

    [SerializeField] private SnakeMovement _snakeMovement;   // Ссылка на обработчик ввода
    [SerializeField] private GameObject _bodyPrefab;         // Ссылка на префаб сегмента тела змейки
    [SerializeField] private SpriteRenderer _headRenderer;   // Ссылка на спрайт рендерер головы змейки    

    public static event Action<FoodType> OnFoodEaten;        // Событие, вызываемое при съедании еды    




    private void Start()
    {
        _segments = new List<Transform>(); // Инициализируем список сегментов
        _segments.Add(transform);          // Добавляем голову (сам объект, на котором скрипт)


        // Если спрайт рендерер головы не назначен в инспекторе, пытаемся получить его с текущего объекта
        if (_headRenderer == null) _headRenderer = GetComponent<SpriteRenderer>(); 
        _originalHeadColor = _headRenderer.color;                                  // Сохраняем исходный цвет головы     
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
            GameManager.OnLevelUp += HandleLevelUp;  // Подписываемся на событие повышения уровня
        }
        Snake.OnFoodEaten += ChangeHeadColor;        // Подписываемся на событие съедания еды, чтобы менять цвет головы
        Snake.OnFoodEaten += SlowingDownSnake;       // Подписываемся на событие съедания еды, чтобы замедлять змейку

    }
    

    private void OnDisable()
    {
        if (_snakeMovement != null)
        {
            GameManager.OnLevelUp -= HandleLevelUp;  // Отписываемся от события повышения уровня
        }
        Snake.OnFoodEaten -= ChangeHeadColor;        // Отписываемся от события съедания еды
        Snake.OnFoodEaten -= SlowingDownSnake;       // Отписываемся от события съедания еды
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
            FoodsLife food = other.GetComponent<FoodsLife>(); // Получаем компонент FoodsLife у съеденной еды

            FoodType type;                                    // Тип съеденной еды
             
            if (food != null)                                 // Если компонент найден
            {
                type = food.foodType;                         // Получаем тип еды из компонента
            }
            else
            {
                type = FoodType.Normal;                       // Если компонента нет, считаем еду обычной
            }

            Destroy(other.gameObject);       // Удаляем съеденную еду

            if (type == FoodType.Normal)
            {
                Grow();                      // Змейка растет только от обычной еды
            }                                      

            OnFoodEaten?.Invoke(type);       // Уведомляем, что съедена еда такого-то типа
                                          
        }
        else if (other.CompareTag("Walls"))  // Столкновение со стеной
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
        
        _eatenSlowFood = 0;      // Сбрасываем счетчик замедляющей еды (чтобы не переходил на новый уровень)
    }



    private void ChangeHeadColor(FoodType foodType)
    {
        if (foodType == FoodType.Golden)
        {
            StartCoroutine(SwitchHeadColorCoroutine(10f)); // Меняем цвет головы на 10 секунд
        }
    }

    private IEnumerator SwitchHeadColorCoroutine(float totalDuration)
    {
        _headRenderer.color = _newHeadColor;        // Меняем цвет головы на новый (золотой)

        float blinkStartTime = totalDuration - 2f;  // Начинаем мигать за 3 секунды до конца эффекта
        float elapsedTime = 0f;                     // Время, прошедшее с начала эффекта

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= blinkStartTime)
            {
                bool isGolden = ((int)(elapsedTime * 4f)) % 2 == 0;
                
                if (isGolden)
                {
                    _headRenderer.color = _newHeadColor;       // Золотой
                }
                else
                {
                    _headRenderer.color = _originalHeadColor;  // Исходный
                }
            }

            yield return null;
        }
        _headRenderer.color = _originalHeadColor; // Страховка - точно возвращаем исходный цвет
    }

    private void SlowingDownSnake(FoodType foodType)
    {
        if (foodType == FoodType.Slow)
        {
            _eatenSlowFood++;           
        }


        if (_eatenSlowFood == FoodSpawner.Instance.SlowFoodCount)
        {
            float newInterval = _moveInterval * 1.1f;                  // Уменьшаем скорость на 10%
            _moveInterval = Mathf.Min(newInterval, _maxMoveInterval);  // Не даем интервалу стать больше максимального
            BackgroundMusic.Instance.DecreasePitch();                  // Уменьшаем скорость музыки
              
            _eatenSlowFood = 0;                                        // Сбрасываем счетчик замедляющей еды            
        }
    }
}
