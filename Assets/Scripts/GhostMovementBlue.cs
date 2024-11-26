using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovementBlue : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 2.6f;
    private float oscillate = 0.7f;
    private Vector2 startPosition;
    private bool escape = false;
    private float timer = 0f;
    public Vector2 escapePosition;
    public Vector2 middlePosition;
    public Vector2 centrePosition;
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
        middlePosition = new Vector2(-1f, 0.35f);
        centrePosition = new Vector2(0f, 0.35f);
    }

    void Update()
    {
        if (!escape)
        {
            Bounce();

            timer += Time.deltaTime;

            if (timer >= 5.5f)
            {
                escape = true;
                Escape();
            }
        }

        if (go)
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
        // Move to the middle position
        yield return StartCoroutine(MoveToMiddle());

        // Move to the center position
        yield return StartCoroutine(MoveToCentre());

        // Move to the escape position
        yield return StartCoroutine(MoveToOut());
    }


    private IEnumerator MoveToMiddle()
    {
        Vector2 startPos = transform.position;
        float duration = 0.6f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, middlePosition, speed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        go = false;

        transform.position = middlePosition;
    }

    private IEnumerator MoveToCentre()
    {
        Vector2 startPos = transform.position;
        float duration = 0.6f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, centrePosition, speed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        go = false;

        transform.position = centrePosition;
    }

    private IEnumerator MoveToOut()
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