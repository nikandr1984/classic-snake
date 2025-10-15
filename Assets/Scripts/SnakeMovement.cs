using System;
using System.Collections;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private Vector2 _currentDirection = Vector2.right;    // Текущее направление движения змейки
    private Vector2 _pendindDirection = Vector2.right;    // Буфер направления (для предотвращения обратного хода)   

    private bool _isInvertedControls = false;             // Флаг инверсии управления

    private void OnEnable()
    {
        InputManager.OnMoveUpPressed    += HandleMoveUp;
        InputManager.OnMoveDownPressed  += HandleMoveDown;
        InputManager.OnMoveLeftPressed  += HandleMoveLeft;
        InputManager.OnMoveRightPressed += HandleMoveRight;

        Snake.OnFoodEaten += ApplyGoldenAppleEffect;           // Подписка на событие съедания золтой еды
    }

    private void OnDisable()
    {
        InputManager.OnMoveUpPressed -= HandleMoveUp;
        InputManager.OnMoveDownPressed -= HandleMoveDown;
        InputManager.OnMoveLeftPressed -= HandleMoveLeft;
        InputManager.OnMoveRightPressed -= HandleMoveRight;

        Snake.OnFoodEaten -= ApplyGoldenAppleEffect;           // Отписка от события съедания золтой еды
    }


    // Обработчики событий, запоминающие направление последнего направления в буфер
    private void HandleMoveUp() => SetPendingDirection(Vector2.up);       // Вверх
    private void HandleMoveDown() => SetPendingDirection(Vector2.down);   // Вниз
    private void HandleMoveLeft() => SetPendingDirection(Vector2.left);   // Влево
    private void HandleMoveRight() => SetPendingDirection(Vector2.right); // Вправо


    // Метод для установки буферного направления с учетом возможной инверсии
    private void SetPendingDirection(Vector2 direction)
    {
        Vector2 actualDirection;

        if (_isInvertedControls)
        {
            actualDirection = -direction;
        }
        else
        {
            actualDirection = direction;
        }

        _pendindDirection = actualDirection;
    }


    // Метод для применения эффекта от съедания золотой еды (инверсия)
    private void ApplyGoldenAppleEffect(FoodType foodType)
    {
        if (foodType == FoodType.Golden)
        {
            StartCoroutine(InvertControlsCoroutine(10f)); // Инвертируем управление на 10 секунд
        }
    }
        

    // Корутин для переключения флага инверсии управления на заданное время
    private IEnumerator InvertControlsCoroutine(float duration) 
    {
        _isInvertedControls = true;
        Debug.Log("SnakeMovement: Control is inverted to " + duration + "sec." );
        yield return new WaitForSeconds(duration);
        _isInvertedControls = false;
        Debug.Log("SnakeMovement: Control is back to normal.");
    }


    // Метод для получения Змейкой следующего направления движения из буфера
    public Vector2 GetNextDirection()
    {
        if (_pendindDirection != -_currentDirection)
        {
            _currentDirection = _pendindDirection;
        }
        return _currentDirection;
    }

}
