using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    Vector2[] directions = new Vector2[]
    {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
    };

    void start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    private void CheckForTrigger()
    {

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
