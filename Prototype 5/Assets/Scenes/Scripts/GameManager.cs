using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
// man kan verschiedene Scenen manipulieren
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targetlist;
    private float spawnRate = 1.0f;
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public bool isGameActive;
    // bool wird auf false gestellt, sobald man ihn erstellt
    public GameObject titleScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetlist.Count);
            Instantiate(targetlist[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // hole die Szene und starte sie von vorne
        // man kann auch unterschiedliche szenen machen und aufrufen
    }
    public void StartGame(int difficulty)
    {
        isGameActive = true;
        titleScreen.SetActive(false);
        spawnRate /= difficulty;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }
}
