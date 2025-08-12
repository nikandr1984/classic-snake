using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right; // ����������� ��������
    public float moveInterval = 0.3f;          // �������� ����� ������
    private float nextMoveTime;                // �����, ����� ����� ������� ��������� ���
    public GameObject bodyPrefab;              // ������ ���� ������

    private List<Transform> segments;

    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);          // ��������� ������ (��� ������, �� ������� ������)
    }

    private void Update()
    {            
        // ��������� ����� WASD
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }



        if (Time.time >= nextMoveTime)
        {
            Move(); // ����� ������ ��������
            nextMoveTime = Time.time + moveInterval; // �������, ����� ����� ����� ��������� �����
        }
    }

    private void Move()
    {
        // 1. ������� ����
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }


        // 2. ������� ������
        Vector3 nextPosition = transform.position;
        nextPosition.x = Mathf.Round(nextPosition.x) + direction.x;
        nextPosition.y = Mathf.Round(nextPosition.y) + direction.y;        
        transform.position = nextPosition;

        // 3. ���������, �� ��������� �� � ����
        CheckSelfCollision();        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            // ���������� ���
            Destroy(other.gameObject);

            // ����������� ������
            Grow();

            // �������� ��������, ��� ����� ������� �����
            FoodSpawner foodSpawner = FindFirstObjectByType<FoodSpawner>();
            if (foodSpawner != null)
            {
                foodSpawner.SpawnFood();
            }
        }
        else if (other.CompareTag("Walls"))
        {
            Debug.Log("Game Over! Hit the wall.");
        }
    }            
    

    public void Grow()
    {
        // ������� ����� ������� ����
        GameObject newSegment = Instantiate(bodyPrefab);

        // ������ ��� �� ����� ���������� �������� (������� �� ����� �� ��� �� �����, ��� � ����������)
        if (segments.Count > 0)
        {
            newSegment.transform.position = segments[segments.Count - 1].position;
        }

        // ��������� � ������ ���������
        segments.Add(newSegment.transform);
    }

    // ����� �������� ������ (�������������� �������� ��������)
    private void CheckSelfCollision()
    {
        // ������� ������ �����������
        Vector2 headPosition = transform.position;

        // ���������� ��� �������� ����� (0 = ������)
        for (int i = 1; i < segments.Count; i++)
        {
            // ����� ������� ��������
            Vector2 segmentPosition = segments[i].position;

            // ���������� � ������� (� ��������� ��������)
            if (Vector2.Distance(headPosition, segmentPosition) < 0.1f)
            {
                // ������������ � �����!
                Debug.Log("Game Over! You ate yourself");
                break;
            }
        }
    }
}
