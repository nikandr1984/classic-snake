using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right; // Направление движения
    public float moveInterval = 0.3f;          // Интервал между шагами
    private float nextMoveTime;                // Время, когда можно сделать следующий шаг
    public GameObject bodyPrefab;              // Префаб тела змейки
    public WallsRenderer wallsRenderer;        // Ссылка на скрипт мигания рамки

    private List<Transform> segments;

    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);          // Добавляем голову (сам объект, на котором скрипт)
    }

    private void Update()
    {            
        // Обработка ввода WASD
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (direction != Vector2.down) // Если не движемся вниз, тогда можем двигаться вверх
            {
                direction = Vector2.up;
            }                      
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (direction != Vector2.up)  // Если не движемся вверх, тогда можем двигаться вниз
            {
                direction = Vector2.down;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (direction != Vector2.right) // Если не движемся вправо, тогда можем двигаться влево
            {
                direction = Vector2.left;
            }      
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (direction != Vector2.left) // Если не движемся влево, тогда можем двигаться вправо
            {
                direction = Vector2.right;
            }                
        }



        if (!GameManager.Instance.IsPaused && Time.time >= nextMoveTime)
        {
            Move(); // Вызов метода движения
            nextMoveTime = Time.time + moveInterval; // Считаем, когда можно будет двигаться снова
        }
    }

    private void Move()
    {
        // 1. Двигаем тело
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }


        // 2. Двигаем голову
        Vector3 nextPosition = transform.position;
        nextPosition.x = Mathf.Round(nextPosition.x) + direction.x;
        nextPosition.y = Mathf.Round(nextPosition.y) + direction.y;        
        transform.position = nextPosition;

        // 3. Проверяем, не врезалась ли в себя
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
            GameOver();
        }
    }            
    

    public void Grow() 
    {
        // Создаем новый сегмент тела
        GameObject newSegment = Instantiate(bodyPrefab);

        // Ставим его на место последнего сегмента (сначала он будет на том же месте, что и предыдущий)
        if (segments.Count > 0)
        {
            newSegment.transform.position = segments[segments.Count - 1].position;
        }

        // Добавляем в список сегментов
        segments.Add(newSegment.transform);
    }

    // Метод проверки столкновения змейки с самой собой
    private void CheckSelfCollision()
    {
        // Позиция головы округленная
        Vector2 headPosition = transform.position;

        // Перебираем все сегменты тедла (0 = голова)
        for (int i = 1; i < segments.Count; i++)
        {
            // Берем позицию сегмента
            Vector2 segmentPosition = segments[i].position;

            // Сравниваем с головой (с небольшим допуском)
            if (Vector2.Distance(headPosition, segmentPosition) < 0.1f)
            {
                GameOver();
                break;
            }
        }
    }

    private void GameOver() // Метод для обработки окончания игры
    {                
        if (GameManager.Instance != null)  // Вызываем событие окончания игры
        {
            GameManager.Instance.GameOver(); 
        }

        if (wallsRenderer != null) // Мигание рамки при окончании игры
        {
            wallsRenderer.Flash(Color.red, 0.2f); 
        }

    }
}
