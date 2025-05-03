using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] private List<Transform> movePoints;
    [SerializeField] private float moveSpeed; // Speed of movement
    private int currentPointIndex = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private float t = 0f; // Interpolation factor

    private float speed = 0f;

    private void Start()
    {
        speed = moveSpeed;
    }
    private void Update()
    {
        Move();

    }
    private void Move()
    {
        if (movePoints.Count < 2) return;

        Transform start = movePoints[currentPointIndex];
        Transform end = movePoints[currentPointIndex + direction];

        t += Time.deltaTime * moveSpeed / Vector3.Distance(start.position, end.position);
        transform.position = Vector3.Lerp(start.position, end.position, t);
        // Flip the sprite based on movement direction
        if (end.position.x > start.position.x)
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        else
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        if (t >= 1f)
        {
            t = 0f;
            currentPointIndex += direction;

            if (currentPointIndex == movePoints.Count - 1 || currentPointIndex == 0)
            {
                direction *= -1; // Reverse direction at end points
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ReavelTriggerObj>()) 
        {
            moveSpeed = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ReavelTriggerObj>())
        {
            moveSpeed = speed;
        }
    }
}
