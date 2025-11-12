using System.Collections;
using UnityEngine;

public class FoodsLife : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;        // Время жизни объекта в секундах
    [SerializeField] private float _blinkStartTime = 3f;  // Время начала мигания перед исчезновением
    
    public FoodType foodType = FoodType.Normal;           // Тип еды 

    private SpriteRenderer _spriteRenderer;               // Компонент SpriteRenderer для управления видимостью



    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, _lifeTime);
        StartCoroutine(BlinkBeforeDestroy());
    }

    IEnumerator BlinkBeforeDestroy()
    {
        yield return new WaitForSeconds(_blinkStartTime);

        while (true)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
