using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //Types of enemies to spawn
    public GameObject Lineman = null;
    public GameObject Linebacker = null;
    public GameObject LineEnd = null;


    //Enemy holder
    public GameObject EnemySpawn = null;

    //Player has 5 drives/game
    public int drives = 5;

    public Text scoreText = null;
    public Text oppScoreText = null;
    public Text drivesText = null;

    public Canvas pauseMenu = null;

    public int score = 0;
    public int opponentScore = 0;

    public GameObject Player = null;
    public int Down = 1;
    public Vector3 playerPos = new Vector3(0, 0);
    public GameObject[] enemies;

    public bool preSnap = true;

    public static GameController SingletonInstance = null;
    public static GameController ThisInstance
    {
        get
        {
            if (SingletonInstance == null)
            {
                GameObject Controller = new GameObject("DefaultController");
                SingletonInstance = Controller.AddComponent<GameController>();
            }
            return SingletonInstance;
        }
    }

    public void touchDown()
    {
        score += 7;
        destroyEnemies();
        resetPlayer();
        preSnap = true;
        updateText();
    }

    public void updateText()
    {
        scoreText.text = "Player Score: " + score;
        oppScoreText.text = "Opponent Score: " + opponentScore;
        drivesText.text = "Drives Left: " + drives;
    }

    private void resetCamera()
    {
        Camera.main.transform.position = new Vector3(-13, 0, -10);
    }

    private void Awake()
    {
        if (SingletonInstance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        SingletonInstance = this;
        DontDestroyOnLoad(gameObject);
    }
    //Initialize variables
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playBook("Long");
        Time.timeScale = 0;
        preSnap = true;
    }
    //Get the player position
    private Vector3 getPlayerPosition()
    {
        return Player.transform.position;
    }
    //Find X players
    private GameObject[] findEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy");
    }
    //remove X players
    private void destroyEnemies()
    {
        enemies = findEnemies();
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }
    //starts new drive
    private void resetPlayer()
    {
        drives--;
        

        if(drives == 0)
        {
            //end game
            finishGame();
        }
        Player.GetComponent<PlayerController>().resetVel();
        Player.transform.position = new Vector3(-15, 0);
        Down = 1;
        playBook("Long");
        resetCamera();
        pauseGame();
        updateText();
    }

    private void pauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void unpauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
    }

    //This function breaks the pause in the game between plays
    public void hike()
    {
        if (preSnap)
        {
            Player.GetComponent<PlayerController>().resetVel();
            unpauseGame();
            preSnap = false;
            
        }
    }

    public void tacklePlayer()
    {
        preSnap = true;
        pauseGame();
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, -10);
        Player.GetComponent<PlayerController>().resetVel();
        //Check the down
        if (Down == 4)
        {
            //Player loses round
            opponentScore += 7;
            oppScoreText.text = "Opponent Score: " + opponentScore;
            destroyEnemies();
            resetPlayer();
            
        }
        else
        {
            //Continue Game
            //Clean up defense
            destroyEnemies();
            //Get player position
            playerPos = getPlayerPosition();
            //Set defense
            if(playerPos.x < 0)
            {
                //The player is behind the 50, we can play deep defesnse
                playBook("Long");
            }
            else if(playerPos.x < 6.5)
            {
                //The player is beyond the 50, but behind the 30, Play more d-line men
                playBook("Medium");
            }
            else
            {
                //Player is beyond the 30 yard line, play only a few linebackers
                playBook("Short");
            }
            //Set player
            //Player will be marked down at their spot and shifted to the middle of the field
            Player.transform.position = new Vector3(playerPos.x, 0);
            //add down
            Down++;
        }
    }

    private void finishGame()
    {
        //When you use up all your drives
        //Game saves to schedule
        //Starts new game
        if (TeamController.ThisInstance.isPlayerAway())
        {
            TeamController.ThisInstance.updateGame(opponentScore, score);
        }
        else
            TeamController.ThisInstance.updateGame(score, opponentScore);

        drives = 5;
        score = 0;
        opponentScore = 0;
        updateText();
        //updateAchievements
        AchievementHandler.ThisInstance.updateStatistics();

    }

    private float defensiveLine()
    {
        //This is the x that the defensive line will line up on
        return getPlayerPosition().x + 2;
    }

    private void playBook(string type)
    {
        int num = (Mathf.RoundToInt(Random.value * 3));
        if(type == "Long")
        {
            //Pick a long play
            if(num == 1)
            {
                //spawn line
                for(int i = 0; i < 2; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -1 + i);
                }
                spawnEnemy(LineEnd, defensiveLine(), -2);
                spawnEnemy(Linebacker, defensiveLine() + 3, 0);
                spawnEnemy(Linebacker, defensiveLine() + 5, 0);
                spawnEnemy(Linebacker, defensiveLine() + 12, 0);

            }
            else if(num == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), 0 + i);
                }
                spawnEnemy(LineEnd, defensiveLine(), -1);
                spawnEnemy(LineEnd, defensiveLine(), 2);
                spawnEnemy(Linebacker, defensiveLine() + 5, 0);
                spawnEnemy(Linebacker, defensiveLine() + 7, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -1 + i);
                }
                spawnEnemy(Linebacker, defensiveLine() + 3, 0);
                spawnEnemy(Linebacker, defensiveLine() + 5, 1);
                spawnEnemy(Linebacker, defensiveLine() + 5, -1);
            }
        }
        else if (type == "Medium")
        {
            if (num == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -1 + i);
                }
                spawnEnemy(LineEnd, defensiveLine(), -2);
                spawnEnemy(Linebacker, defensiveLine() + 3, 0);
                spawnEnemy(Linebacker, defensiveLine() + 12, 0);
            }
            else if (num == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -1 + i);
                }
                spawnEnemy(LineEnd, defensiveLine(), -2);
                spawnEnemy(Linebacker, defensiveLine() + 5, 0);
                spawnEnemy(Linebacker, defensiveLine() + 12, -1);
                spawnEnemy(Linebacker, defensiveLine() + 12, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -1 + i);
                }
                spawnEnemy(Linebacker, defensiveLine() + 3, 0);
                spawnEnemy(Linebacker, defensiveLine() + 5, 0);
                spawnEnemy(Linebacker, defensiveLine() + 12, 0);
            }
        }
        else
        {
            if (num == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -3 + i);
                }
                spawnEnemy(Linebacker, defensiveLine() + 1, 0);
            }
            else if (num == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -3 + i);

                }
                spawnEnemy(Linebacker, defensiveLine(), 2);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnEnemy(Lineman, defensiveLine(), -2 + i);
                }
                spawnEnemy(Linebacker, defensiveLine(), -3);
            }
        }


    }

    private void spawnEnemy(GameObject enemy, float x, float y)
    {
        Vector3 spawnPoint = new Vector3(x, y);
        GameObject enemyIn = Instantiate(enemy, spawnPoint, Quaternion.identity);
        enemyIn.GetComponent<Mover>().range = Mathf.Round(Random.value * 3) + 2;
        
    }



}
