using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;



public class GameManager : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;

    public TextMeshProUGUI scoreText;
    private int score;

    public List<GameObject> obstacles;
    public GameObject portalPrefab;

    private Vector3 spawnPos = new Vector3(20, 0, 0);
    private Vector3 spawnPos2 = new Vector3(20, 1.5f, 0);
    private float startDelay = 5f;

    private PlayerController playerControllerScript;
    public int GetScore()
        {
        return score;
        }

    private Material playerOneMaterial;
    private Material playerTwoMaterial;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScore(0);
        StartCoroutine(SpawnObstacleRoutine());
        StartCoroutine(SpawnPortalRoutine());
        playerControllerScript = playerOne.GetComponent<PlayerController>();

        // Material
        playerOneMaterial = new Material(playerOne.GetComponent<Renderer>().material);
        playerTwoMaterial = new Material(playerTwo.GetComponent<Renderer>().material);

        playerOne.GetComponent<Renderer>().material = playerOneMaterial;
        playerTwo.GetComponent<Renderer>().material = playerTwoMaterial;
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

    public void SwapPlayers()
    {
        // Swap positions
        Vector3 tempPos = playerOne.transform.position;
        playerOne.transform.position = playerTwo.transform.position;
        playerTwo.transform.position = tempPos;

        // Swap roles 
        var p1Controller = playerOne.GetComponent<PlayerController>();
        var p2Controller = playerTwo.GetComponent<PlayerController>();

        bool playerOneWasRunner = p1Controller.isRunner;
        p1Controller.isRunner = p2Controller.isRunner;
        p2Controller.isRunner = playerOneWasRunner;

        // change gravity
        Rigidbody rb1 = playerOne.GetComponent<Rigidbody>();
        Rigidbody rb2 = playerTwo.GetComponent<Rigidbody>();

        rb1.useGravity = p1Controller.isRunner;
        rb2.useGravity = p2Controller.isRunner;

        // change material
        Renderer materialOne = playerOne.GetComponent<Renderer>();
        Renderer materialTwo = playerTwo.GetComponent<Renderer>();

        Material tempMat = playerOneMaterial;
        playerOneMaterial = playerTwoMaterial;
        playerTwoMaterial = tempMat;

        materialOne.material = playerOneMaterial;
        materialTwo.material = playerTwoMaterial;
    }

}
