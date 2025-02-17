using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovementRed : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 2.6f;
    private Vector2 startPosition;
    private Vector2 movementDirection;
    private bool go = true;
    private Vector2 direction;


    Vector2[] directions = new Vector2[]
    {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
    };

    void Start()
    {
        movementDirection = Random.Range(0f, 1f) > 0.5f ? Vector2.right : Vector2.left;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(go)
        {
            rb.velocity = movementDirection * speed;
        }
    }

    private Vector2 ChooseDirection(Vector2 movementDirection)
    {
        List<Vector2> ValidDirections = new List<Vector2>();

        Vector2 triggerCenter = GetComponent<Collider2D>().bounds.center;

        foreach (Vector2 dir in directions)
        {
            if (dir == -movementDirection) continue;

            int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ghosts")) | (1 << LayerMask.NameToLayer("Points")) | (1 << LayerMask.NameToLayer("GhostTriggers")));

            RaycastHit2D hit = Physics2D.Raycast(triggerCenter, dir, 1f, layerMask);

            if (hit.collider == null)
            {
                ValidDirections.Add(dir);
            }
        }

        if (ValidDirections.Count == 0)
        {
            Debug.LogWarning("No valid directions found! Defaulting to stationary.");
            return Vector2.zero;
        }

        int randomIndex = Random.Range(0, ValidDirections.Count);
        Vector2 randomDirection = ValidDirections[randomIndex];

        return randomDirection;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TeleportRight")
        {
            transform.position = new Vector3(9.323f, -0.01662342f, 0.04814792f);
        }
        else if (collision.gameObject.name == "TeleportLeft")
        {
            transform.position = new Vector3(-9.323f, -0.01662342f, 0.04814792f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Teleport"))
        {
            return;
        }
        else
        {
            movementDirection = ChooseDirection(movementDirection);
        }
    }
}