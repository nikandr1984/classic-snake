using System;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public event Action<Vector2> OnDirectionChanged;    // Событие для уведомления об изменении направления

    private Vector2 _currentDirection = Vector2.right;  // Текущее направление движения змейки
    private Vector2 _nextDirection;                     // Следующее направление движения змейки

    private bool _inputEnabled = true;                  // Флаг: можно обрабатывать ввод?


    //===============================================================================================

    private void OnEnable()
    {
        InputManager.OnMoveUpPressed    += HandleMoveUp;
        InputManager.OnMoveDownPressed  += HandleMoveDown;
        InputManager.OnMoveLeftPressed  += HandleMoveLeft;
        InputManager.OnMoveRightPressed += HandleMoveRight;
    }

    private void OnDisable()
    {
        InputManager.OnMoveUpPressed -= HandleMoveUp;
        InputManager.OnMoveDownPressed -= HandleMoveDown;
        InputManager.OnMoveLeftPressed -= HandleMoveLeft;
        InputManager.OnMoveRightPressed -= HandleMoveRight;
    }


    private void HandleMoveUp()
    {
        if (!_inputEnabled) return;
        if (_currentDirection != Vector2.down) _nextDirection = Vector2.up;
    }

    private void HandleMoveDown()
    {
        if (!_inputEnabled) return;
        if (_currentDirection != Vector2.up) _nextDirection = Vector2.down;
    }

    private void HandleMoveLeft()
    {
        if (!_inputEnabled) return;
        if (_currentDirection != Vector2.right) _nextDirection = Vector2.left;
    }

    private void HandleMoveRight()
    {
        if (!_inputEnabled) return;
        if (_currentDirection != Vector2.left) _nextDirection = Vector2.right;
    }


    // Важный метод! Его будет вызывать Snake после каждого шага.
    // Он применяет валидное направление из буфера и сбрасывает флаг.
    public void ApplyDirectionChange()
    {
        // Если направление изменилось, применяем его и уведомляем подписчиков
        if (_nextDirection != _currentDirection && _nextDirection != Vector2.zero)
        {
            _currentDirection = _nextDirection;
            OnDirectionChanged?.Invoke(_currentDirection);

            // _nextDirection не сбрасываем в zero, чтобы сохранить последнее валидное направление

            _inputEnabled = true; // Разрешаем ввод
        }
    }


    // Метод для включения/выключения обработки ввода извне
    public void SetInputEnabled(bool state)
    {
        _inputEnabled = state;
    }


    // Метод для сброса состояния (при рестарте игры)
    public void ResetDirection(Vector2 startDirection)
    {
        _currentDirection = startDirection;
        _nextDirection = startDirection;
        _inputEnabled = true;
    }
}
