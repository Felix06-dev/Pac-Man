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
    //check for change in direction trigger
    private void CheckForTrigger()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, layerMask);
        Debug.DrawRay(transform.position, direction * 0.1f, Color.red);
        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }
    //choose which new direction to get
    private void ChooseDirection()
    {
        List<Vector2> ValidDirections = new List<Vector2>();
        //check for the directions that contain a wall
        foreach (Vector2 dir in possibleDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.1f, wallLayerMask);
            if (hit.collider == null)
            {
                validDirections.Add(dir);
            }
        }
        // Use Random.Range to pick a random index from the list
        if (validDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, validDirections.Count);
            Vector2 randomDirection = validDirections[randomIndex];
            return randomDirection;

            Debug.Log("Chosen direction: " + randomDirection);
        }

    }
}
