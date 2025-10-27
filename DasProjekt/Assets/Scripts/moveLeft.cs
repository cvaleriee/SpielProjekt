using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeft : MonoBehaviour
{
    private float speed = 10f;
    private PlayerController playerControllerScript;
    public float leftBound = -10;


    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find ("Player One").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (transform.position.x < leftBound && gameObject. CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
