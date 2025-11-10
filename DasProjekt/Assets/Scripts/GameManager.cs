using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    private Vector3 beginningGravity;

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

    public Renderer playerOneRenderer;
    public Renderer playerTwoRenderer;

    public Material playerOneMaterial;
    public Material playerTwoMaterial;

    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public TextMeshProUGUI startText;
    public Button startButton;

    public bool isGameActive = false; 
    // Start is called before the first frame update
    void Start()
    {
        startText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        playerOne.SetActive(false);
        playerTwo.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        isGameActive = true;
        
        startText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        playerOne.SetActive(false);
        playerTwo.SetActive(false);
        scoreText.gameObject.SetActive(false);

        playerOne.SetActive(true);
        playerTwo.SetActive(true);
        scoreText.gameObject.SetActive(true);

        beginningGravity = Physics.gravity;

        score = 0;
        UpdateScore(0);
        StartCoroutine(SpawnObstacleRoutine());
        StartCoroutine(SpawnPortalRoutine());
        playerControllerScript = playerOne.GetComponent<PlayerController>();
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
        if (p1Controller.isRunner)
        {
            playerOneRenderer.material = Instantiate(playerOneMaterial);
        }
        else
        {
            playerOneRenderer.material = Instantiate(playerTwoMaterial);
        }

        if (p2Controller.isRunner)
        {
            playerTwoRenderer.material = Instantiate(playerTwoMaterial);
        }
        else
        {
            playerTwoRenderer.material = Instantiate(playerOneMaterial);
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        playerOne.SetActive(false);
        playerTwo.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        var p1Controller = playerOne.GetComponent<PlayerController>();
        var p2Controller = playerTwo.GetComponent<PlayerController>();
        p1Controller.gameOver = false;
        p1Controller.isOnGround = true;
        p1Controller.hitPortal = 0;

        p2Controller.gameOver = false;
        p2Controller.isOnGround = true;
        p2Controller.hitPortal = 0;

        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        

        StopAllCoroutines();
        StartGame();
    }
}