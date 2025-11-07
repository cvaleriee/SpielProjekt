using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;



public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;

    public List<GameObject> obstacles;
    
    // public GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(20, 0, 0);
    private float startDelay = 2f;
    private float repeatRate = 2f;
    private PlayerController playerControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScore(0);
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerControllerScript = GameObject. Find("Player One").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            int index = Random.Range(0, obstacles.Count);
            GameObject var = obstacles[index];
            Instantiate(var, spawnPos, var.transform.rotation);
        }

    }
    
        private void ScoreTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // AddScore(1);
        }
    }

}
