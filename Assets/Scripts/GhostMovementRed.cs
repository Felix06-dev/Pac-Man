using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovementRed : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 3f;
    private float oscillate = 0.7f;
    private Vector2 startPosition;
    private bool escape = false;
    private float timer = 0f;
    public Vector2 escapePosition;
    private Vector2 movementDirection;
    private bool go = false;
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
        escapePosition = new Vector2(0f, 2.19f);
    }

    void Update()
    {
        if(!escape)
        {
            Bounce();

            timer += Time.deltaTime;

            if (timer >= 2f)
            {
                escape = true;
                Escape();
            }
        }

        if(go)
        {
            rb.velocity = movementDirection * speed;
        }
    }

    private void Escape()
    {
        StartCoroutine(MoveToEscapePosition());
    }

    private IEnumerator MoveToEscapePosition()
    {
        Vector2 startPos = transform.position;
        float duration = 0.6f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, escapePosition, speed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        go = true;

        transform.position = escapePosition;
    }

    private void Bounce()
    {
        float y = Mathf.PingPong(Time.time * speed, oscillate);
        transform.position = startPosition + new Vector2(0, y);
    }

    private Vector2 ChooseDirection()
    {
        List<Vector2> ValidDirections = new List<Vector2>();

        // Get the position of the center of the collider (Ghost Trigger)
        Vector2 triggerCenter = GetComponent<Collider2D>().bounds.center;

        foreach (Vector2 dir in directions)
        {
            int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ghosts")) | (1 << LayerMask.NameToLayer("Points")) | (1 << LayerMask.NameToLayer("GhostTriggers")));

            // Raycast from the center of the collider (Ghost Trigger)
            RaycastHit2D hit = Physics2D.Raycast(triggerCenter, dir, 1f, layerMask); // Adjust the raycast distance if necessary

            // Debugging: Draw the ray from the center of the collider for visualization
            Debug.DrawRay(triggerCenter, dir * 1f, Color.green);  // Main ray

            // Check for a valid direction (no wall hit)
            if (hit.collider == null) // If no collider is hit (no wall)
            {
                ValidDirections.Add(dir);
            }
        }

        if (ValidDirections.Count == 0)
        {
            Debug.LogWarning("No valid directions found! Defaulting to a random direction.");
        }

        // Choose a random valid direction
        int randomIndex = Random.Range(0, ValidDirections.Count);
        Vector2 randomDirection = ValidDirections[randomIndex];

        Debug.Log("Chosen direction: " + randomDirection); // Log the chosen direction

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
        movementDirection = ChooseDirection();
    }
}

// edit: to do, add ghost trigger script that contains all possible directions of travel from that trigger then use these directions when tiggered