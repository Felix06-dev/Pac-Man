using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 2.5f;
    private float oscillate = 0.7f;
    private Vector2 startPosition;
    private bool go = false;
    private float timer = 0f;
    public Vector2 escapePosition;

    Vector2[] directions = new Vector2[]
    {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
    };

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        escapePosition = new Vector2(0f, 2.19f);
    }

    void Update()
    {
        if(!go)
        {
            Bounce();

            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                go = true;
                Escape();
            }
        }
    }

    private void Escape()
    {
        StartCoroutine(MoveToEscapePosition());
    }

    private IEnumerator MoveToEscapePosition()
    {
        Vector2 startPos = transform.position;
        float duration = 2f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, escapePosition, speed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

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

        foreach (Vector2 dir in directions)
        {
            int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("GhostTriggers")) | (1 << LayerMask.NameToLayer("Ghosts")));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.1f, layerMask);
            if (hit.collider == null)
            {
                ValidDirections.Add(dir);
            }
        }

        int randomIndex = Random.Range(0, ValidDirections.Count);
        Vector2 randomDirection = ValidDirections[randomIndex];
        return randomDirection;

        Debug.Log("Chosen direction: " + randomDirection);
    }
}
