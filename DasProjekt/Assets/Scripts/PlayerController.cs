using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Vector3 beginningGravity;

    private Rigidbody playerRb;
    public float jumpForce = 20f;
    public float gravityModifier = 5f;
    public bool isOnGround = true;
    public bool gameOver = false;
    public int hitPortal = 0;

    public GameManager gameManager;
    public int pointValue;

    public bool isRunner = true;
    private bool canSwap = true; 

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        if (isRunner)
        {
            Physics.gravity = Physics.gravity * gravityModifier;
        }
        playerRb.useGravity = isRunner;

    }

    // Update is called once per frame
    void Update()
    {

        if (hitPortal % 2 == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround && gameOver == false && isRunner)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                gameManager.UpdateScore(1);
            }

            if (Input.GetKeyDown(KeyCode.P) && gameOver == false && !isRunner)
            {
                destroyNearObstacle();
            }
        }

        else if (hitPortal % 2 != 0)
        {
            if (Input.GetKeyDown(KeyCode.Return) && isOnGround && gameOver == false && isRunner)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                gameManager.UpdateScore(1);
            }
            if (Input.GetKeyDown(KeyCode.A) && gameOver == false && !isRunner)
            {
                destroyNearObstacle();
            }
        }

        if (!isRunner && !gameOver)
        {
            Vector3 pos = transform.position;
            pos.y = 6f;  
            transform.position = pos;
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
            gameManager.GameOver();
            Debug.Log("Game Over!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal") && canSwap)
        {
            StartCoroutine(HandlePortalSwap());
        }
    }

    IEnumerator HandlePortalSwap()
    {
        canSwap = false;
        hitPortal++;
        StartCoroutine(TemporaryLog("SWAP!", 2f));
        gameManager.SwapPlayers();

        yield return new WaitForSeconds(0.5f);
        canSwap = true;
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