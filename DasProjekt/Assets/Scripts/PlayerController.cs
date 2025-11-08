using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playeroneRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    public int hitPortal = 0;
    public GameManager gameManager;
    public int pointValue;



    // Start is called before the first frame update
    void Start()
    {
        playeroneRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPortal % 2 == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround && gameOver == false)
            {
                playeroneRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                gameManager.UpdateScore(1);
            }

            if (Input.GetKeyDown(KeyCode.P) && gameOver == false)
            {
                destroyNearObstacle();
            }
        }

        else if (hitPortal % 2 != 0)
        {
            if (Input.GetKeyDown(KeyCode.Return) && isOnGround && gameOver == false)
            {
                playeroneRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                gameManager.UpdateScore(1);
            }
            if (Input.GetKeyDown(KeyCode.A) && gameOver == false)
            {
                destroyNearObstacle();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boden"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            isOnGround = false;
            Debug.Log("Game Over!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            hitPortal++;
            StartCoroutine(TemporaryLog("SWAP!", 2f));
        }
    }

    IEnumerator TemporaryLog(string message, float duration)
    {
        Debug.Log(message);
        yield return new WaitForSeconds(duration);
        Debug.Log(" ");
    }

    private void destroyNearObstacle()
    {
        if (gameManager.GetScore() < 5)
        {
            Debug.Log("Not enough points!");
            return;
        }

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); 

        if (obstacles.Length == 0)
        {
            return;
        }

        GameObject nearest = obstacles[0];
        float minDist = Vector3.Distance(transform.position, nearest.transform.position);

        foreach (GameObject obs in obstacles)
        {
            float dist = Vector3.Distance(transform.position, obs.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obs;
            }
        }

        // Destroy it and reduce score
        Destroy(nearest);
        gameManager.UpdateScore(-5);
        StartCoroutine(TemporaryLog("Destroyed obstacle (-5 points)", 1.5f));
    }
}