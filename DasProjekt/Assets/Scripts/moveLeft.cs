using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeft : MonoBehaviour
{
    private float baseSpeed = 10f;
    private float acceleration = 0.2f;
    private float speed;

    private PlayerController playerControllerScript;
    private GameManager gameManager;
    public float leftBound = -10;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerControllerScript = null; 
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null && gameManager.isGameActive)
        {
            if (playerControllerScript == null)
            {
                GameObject playerObj = GameObject.Find("Player One");
                if (playerObj != null)
                {
                    playerControllerScript = playerObj.GetComponent<PlayerController>();
                } 
            }
            if (playerControllerScript != null && playerControllerScript.gameOver == false)
            {
                speed = Mathf.Min(baseSpeed + (Time.time * acceleration), 30f);
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        if (transform.position.x < leftBound && gameObject.CompareTag("Portal"))
        {
            Destroy(gameObject);
        }
    }
}
