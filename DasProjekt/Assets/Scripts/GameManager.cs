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
    public GameObject portalPrefab;

    private Vector3 spawnPos = new Vector3(20, 0, 0);
    private Vector3 spawnPos2 = new Vector3(20, 1.5f, 0);
    private float startDelay = 5f;

    // private float repeatRate = 2f;

    private PlayerController playerControllerScript;
    public int GetScore()
        {
        return score;
        }


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScore(0);
        StartCoroutine(SpawnObstacleRoutine());
        StartCoroutine(SpawnPortalRoutine());
        playerControllerScript = GameObject.Find("Player One").GetComponent<PlayerController>();
    }

    IEnumerator SpawnObstacleRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (playerControllerScript.gameOver == false)
        {
            // spawning random obstacles
            int index = Random.Range(0, obstacles.Count);
            GameObject var = obstacles[index];
            Instantiate(var, spawnPos, var.transform.rotation);

            // wait a random time
            float randomDelay = Random.Range(0.8f, (2.2f - Time.timeSinceLevelLoad * 0.0005f));
            yield return new WaitForSeconds(randomDelay);
        }
    }
    
    IEnumerator SpawnPortalRoutine()
    {
        yield return new WaitForSeconds(startDelay + 10f);
 
        while (playerControllerScript.gameOver == false)
        {
            Instantiate(portalPrefab, spawnPos2, portalPrefab.transform.rotation);
 
            // wait a random time
            float randomDelay = Random.Range(8f, 22f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int scoreToAdd)
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

}
