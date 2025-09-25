using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right; // Направление движения
    public float moveInterval = 0.3f;           // Интервал между шагами
    private float nextMoveTime;                 // Время, когда можно сделать следующий шаг
    public GameObject bodyPrefab;               // Префаб тела змейки    

    [SerializeField] private SnakeMovement _snakeMovement; // Ссылка на обработчик ввода

    private List<Transform> _segments;          // Список сегментов тела змейки



    //===============================================================================================

    private void Start()
    {
        _segments = new List<Transform>(); // Инициализируем список сегментов
        _segments.Add(transform);          // Добавляем голову (сам объект, на котором скрипт)

        // Сбрасываем направление движения в право при старте
        if (_snakeMovement != null)
        {
            _snakeMovement.ResetDirection(Vector2.right);
        }
    }

    
    
    private void OnEnable()
    {
        if (_snakeMovement != null)
        {
            _snakeMovement.OnDirectionChanged += HandleDirectionChange; // Подписываемся на событие изменения направления
        }
    }
    private void OnDisable()
    {
        if (_snakeMovement != null)
        {
            _snakeMovement.OnDirectionChanged -= HandleDirectionChange; // Отписываемся от события при отключении
        }
    }


    private void Update()
    {     
        // Двигаем змейку с заданным интервалом, если игра не на паузе
        if (PauseManager.CanPlay && Time.time >= nextMoveTime)
        {
            Move(); // Вызов метода движения
            nextMoveTime = Time.time + moveInterval; // Считаем, когда можно будет двигаться снова
        }
    }


    // Этот метод вызывается автоматически, когда игрок меняет направление
    private void HandleDirectionChange(Vector2 newDirection)
    {
        _direction = newDirection; // Обновляем направление движения
    }



    private void Move()
    {
        // 1. Двигаем тело
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // 2. Двигаем голову
        Vector3 nextPosition = transform.position;
        nextPosition.x = Mathf.Round(nextPosition.x) + _direction.x;
        nextPosition.y = Mathf.Round(nextPosition.y) + _direction.y;        
        transform.position = nextPosition;


        // 3. Применяем изменение направления из SnakeMovement, если оно есть
        if (_snakeMovement != null)
        {
            _snakeMovement.ApplyDirectionChange();
        }             
        
        // 4. Проверяем, не врезалась ли в себя
        CheckSelfCollision();        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            // Уничтожаем еду
            Destroy(other.gameObject);

            // Увеличиваем змейку
            Grow();

            // Увеличиваем счет
            if (GameManager.Instance != null) // Проверяем, что GameManager существует
            {
                GameManager.Instance.AddScore();
            }       
            else
            {
                Debug.Log("GameManager не найден!");
            }


            // Сообщаем спавнеру, что нужно создать новую
            FoodSpawner foodSpawner = FindFirstObjectByType<FoodSpawner>();
            if (foodSpawner != null)
            {
                foodSpawner.SpawnFood();
            }
        }
        else if (other.CompareTag("Walls"))
        {        
            CrashedSnake();
        }
    }            
    

    public void Grow() 
    {
        // Создаем новый сегмент тела
        GameObject newSegment = Instantiate(bodyPrefab);

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
}
