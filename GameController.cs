using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] enemies; // Array of enemies
    public GameObject player;
    public GameObject coins;
    public Vector3 playerValue;
    public Vector3 spawnValue;
    private bool isEnded;
    private bool gameOver;
    private int score = 0;
    private int winScore = 860;
    private int maxEnemies = 4;
    private int enemyCount;
    private float ElapsedTime = 0f;    
    private float spawnCondition = 0f;
    private float breakTimer = 1f;    

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    public GUIText gameQuitText;
    public GUIText winText; // Initialise fields

    private void Start()
    {                
        Vector3 playerSpawn = new Vector3(playerValue.x, playerValue.y, playerValue.z); // Vector3 position assigned in Unity Editor
        Quaternion playerRotation = Quaternion.identity;
        isEnded = false;
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";
        gameQuitText.text = "";
        winText.text = "";
        score = 0;
        enemyCount = 0; // Set default values 
        Instantiate(player, playerSpawn, playerRotation); // Instantiate player prefab
        Instantiate(coins, playerSpawn, playerRotation); // Instantiate coin prefab
        updateScore(); // Update the score text
        StartCoroutine(ExtraSpawn()); // Start ExtraSpawn IEnumerable
    }

    private void Update()
    {
        if (score == winScore) // if the current score equals the winScore
        {
            gameOver = true;
            winText.text = "You win!"; // Display WinText      
            if (isEnded) 
            {
                if (Input.GetKeyDown(KeyCode.R)) // If R is pressed
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads game Scene
                }
                if (Input.GetKeyDown(KeyCode.Escape)) // If ESC is pressed
                {
                    Application.Quit(); // Quits the application
                }
            }
        }

        if (isEnded) 
        {            
            if (Input.GetKeyDown(KeyCode.R)) // If R is pressed
            {                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads game Scene
            }        
            if (Input.GetKeyDown(KeyCode.Escape)) // If ESC is pressed
            {                
                Application.Quit(); // Quits the application
            }
        }
    }

    private IEnumerator ExtraSpawn ()
    {              
        while (true) // while running
        {
            for (int i = 0; i < 4; i++) // Iterate through for loop
            {
                GameObject enemyArray = enemies[Random.Range(0, enemies.Length)]; // Create a random selection of enemy Prefabs
                Vector3 spawnPosition = new Vector3(spawnValue.x, spawnValue.y, spawnValue.z);
                Quaternion spawnRotation = Quaternion.identity;
                ElapsedTime += Time.deltaTime;
                if (ElapsedTime > spawnCondition && enemyCount < maxEnemies)
                {
                    spawnCondition += ElapsedTime + 0.3f; // Change spawnCondition to spawn enemies later
                    ElapsedTime = 0; // Reset Elapsed time
                    enemyCount++; // enemyCount plus one
                    Instantiate(enemyArray, spawnPosition, spawnRotation); // Instantate enemy Prefab
                    break;
                }                
                              
            }
            yield return new WaitForSeconds(breakTimer); // Wait for breakTimer

            if (gameOver)
            {
                restartText.text = "Press R to restart"; 
                gameQuitText.text = "Press Esc to quit"; // Set text for RestartText && gameQuitText
                isEnded = true; 
                break; // break loop
            }
        }
    }

    public void AddScore(int newScore) 
    {
        score += newScore; // Add score from CollisionDestroy script
        updateScore(); // Call updateScore method
    }

    public void updateScore()
    {

        scoreText.text = "score: " + score; // Set scoreText to score
    }
    
    public void GameOver ()
    {
        gameOverText.text = "Game Over"; // Set text for gameOverText
        gameOver = true;
    }       
}
