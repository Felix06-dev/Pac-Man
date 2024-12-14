using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverGhostRun : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GameOver()
    {
        GameObject[] Ghosts = GameObject.FindGameObjectsWithTag("Ghosts");

        foreach (GameObject ghost in Ghosts)
        {
            // Get the Rigidbody2D component from each ghost
            Rigidbody2D rb = ghost.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Set the velocity to zero (or any desired value)
                rb.velocity = Vector2.zero;
            }
            else
            {
                Debug.LogWarning($"No Rigidbody2D found on {ghost.name}");
            }
        }
    }
}
