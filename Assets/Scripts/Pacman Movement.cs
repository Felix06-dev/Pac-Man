using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class PacmanMovement : MonoBehaviour
{
    public float speed = 2.5f;
    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection = Vector2.zero;
    private string teleportLayer = "Teleport";
    private Rigidbody2D rb;
    private int targetLayer;
    public Tilemap TilemapPoints;
    public TileBase pointTile;
    public int scorePerPoint = 50;
    public int Score = 0;
    public TextMeshProUGUI scoreBoard;
    private Animator anim;
    public bool die = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetLayer = LayerMask.NameToLayer(teleportLayer);
        anim = GetComponent<Animator>();
        IncreaseScore();
    }

    void Update()
    {
        CaptureInput();

        Move();

        PointCollection();
    }

    private void CaptureInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            nextDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            nextDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            nextDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            nextDirection = Vector2.right;
        }
    }

    private void Move()
    {
        if (CanMove(nextDirection))
        {
            direction = nextDirection;
        }

        if (HitWall(direction))
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        anim.SetBool("Up", false);
        anim.SetBool("Down", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);

        if (direction == Vector2.up)
        {
            anim.SetBool("Up", true);
        }
        else if (direction == Vector2.down)
        {
            anim.SetBool("Down", true);
        }
        else if (direction == Vector2.left)
        {
            anim.SetBool("Left", true);
        }
        else if (direction == Vector2.right)
        {
            anim.SetBool("Right", true);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("GhostTriggers")));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, layerMask);
        if (hit.collider != null)
        {
            return false;
        }

        return true;
    }

    private bool HitWall(Vector2 direction)
    {
        int layerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("GhostTriggers")));
        RaycastHit2D stop = Physics2D.Raycast(transform.position, direction, 0.01f, layerMask);
        if (stop.collider != null)
        {
            return false;
        }

        return true;
    }

    private bool CollectPoint(Vector2 direction)
    {
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D point = Physics2D.Raycast(transform.position, direction, 0.5f, layerMask);
        if (point.collider != null)
        {
            return true;
        }

        return false;
    }

    private void PointCollection()
    {
        if (CollectPoint(direction))
        {
            Vector3 worldPosition = transform.position;
            Vector3Int cellPosition = TilemapPoints.WorldToCell(worldPosition);

            TileBase tileAtPosition = TilemapPoints.GetTile(cellPosition);

            if (tileAtPosition == pointTile)
            {

                TilemapPoints.SetTile(cellPosition, null);

                Score = Score + scorePerPoint;
                IncreaseScore();
            }
        }
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

        if (collision.gameObject.CompareTag("Ghosts"))
        {
            die = true;
            Die();
        }
    }

    private void Die()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void IncreaseScore()
    {
        scoreBoard.text = "Score: " + Score.ToString();
    }

}
