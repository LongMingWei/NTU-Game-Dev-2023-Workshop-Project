using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float gameSpeed = 1.0f; //The velocity of the player running
    public float speedMultiplier = 1.0f;//Will be used to affect the game speed
    public float maxGameSpeed = 10.0f; //The max speed we can reach
    private float distance = 0.0f;
    public float maxAcceleration = 10.0f; //the max acceleration we can hit
    public float difficultyTweak = 0.02f;

    private bool isGameOver = false;

    public Player player; //The reference to the player
    public UIController uiController; //The reference to the UI controller
    public AudioSource hitSound;
    public AudioSource sniperSound;

    private float highScore = 0.0f;

    public GameObject groundPrefab;
    public GameObject enemyPrefab;
    private GameObject lastSpawnGround = null;
    private void Awake()
    {
        //if the instance has not been created before, link instance to this class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        uiController.SetSpeedText(GetGameSpeed().ToString("0.00"));
        uiController.SetDistanceText(distance.ToString("0.00"));
    }

    public void GameOver()
    {
        isGameOver = true;
        gameSpeed = 0.0f;

        hitSound.Play();
        uiController.GameOver(distance.ToString("0.00"));

        //Save if it is higher or else we need not do anything
        if(distance > highScore)
        {
            SaveHighScore(distance);
        }
    }
    public float GetGameSpeed()
    {
        return Mathf.Clamp(gameSpeed * speedMultiplier, 0f, maxGameSpeed);
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            //Only when we are on the ground the game speed changes
            if (player.GetIsGrounded())
            {
                float velocityRatio = GetGameSpeed() / maxGameSpeed;
                float acceleration = maxAcceleration * (1.0f - velocityRatio);

                player.SetMaxJumpingTime(velocityRatio); //Limit the jump time based on your current speed 
                //More speed more jump

                gameSpeed += acceleration * Time.fixedDeltaTime;
                gameSpeed = Mathf.Clamp(gameSpeed, 0f, maxGameSpeed);
            }
            //Add to the distance (score)
            distance += GetGameSpeed() * Time.fixedDeltaTime;

            maxGameSpeed += difficultyTweak * Time.fixedDeltaTime;
            maxAcceleration += difficultyTweak * Time.fixedDeltaTime;
        }
    }

    public bool GetGameOver()
    {
        return isGameOver;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    void SaveHighScore(float score)
    {
        highScore = score;
        PlayerPrefs.SetFloat("highScore", highScore);
        uiController.SetHighScoreText(highScore.ToString("0.00"));
    }

    private void LoadHighScore()
    {
        //Only load if we have saved before
        if(PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetFloat("highScore");
            uiController.SetHighScoreText(highScore.ToString("0.00"));
        }
    }

    public void GenerateGround(GameObject obj)
    {
        GameObject go = Instantiate(groundPrefab);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();

        if(lastSpawnGround == null)
        {
            lastSpawnGround = obj;
        }

        Ground lastGround = lastSpawnGround.GetComponent<Ground>();
        float maxJumpVelocity = player.GetMaxJumpVelocity();

        float h1 = maxJumpVelocity * player.GetCurrentMaxJumpingTime();
        float t = maxJumpVelocity / -Physics2D.gravity.y;
        float h2 = maxJumpVelocity * t + (0.5f * Physics2D.gravity.y * (t * t));

        float maxJumpHeight = h1 + h2;
        float minY = 1.0f;
        float maxY = maxJumpHeight * 0.5f;
        maxY = Mathf.Clamp(maxY, 1.0f, 2.0f);
        maxY += lastGround.GetGroundHeight();

        float minX = 2.0f;
        float totalTime = Mathf.Abs(2.0f * t);
        float maxX = GetGameSpeed() * totalTime * 0.7f;

        float finalX = Random.Range(minX, maxX);
        float finalY = Random.Range(minY, maxY);

        Vector2 pos;
        pos.x = lastGround.GetGroundRight() + finalX + goCollider.bounds.size.x / 2.0f;
        pos.y = finalY - goCollider.bounds.size.y / 2.0f;
        go.transform.position = pos;

        go.GetComponent<Ground>().SetHasGeneratedGround(true);
        lastSpawnGround = go;

        int hasObstacle = Random.Range(0, 10) + 1;
        if(hasObstacle > 5)
        {
            SpawnObstacle();
        }
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }

    private void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(enemyPrefab);
        obstacle.transform.parent = lastSpawnGround.transform;

        Vector2 epos;
        epos.x = lastSpawnGround.transform.position.x + Random.Range(-lastSpawnGround.GetComponent<Collider2D>().bounds.size.x / 2.0f, lastSpawnGround.GetComponent<Collider2D>().bounds.size.x / 2.0f);
        float groundHeight = lastSpawnGround.GetComponent<Collider2D>().bounds.size.y;
        float obstacleHeight = obstacle.GetComponent<Collider2D>().bounds.size.y;

        epos.y = lastSpawnGround.transform.position.y + groundHeight / 2.0f + obstacleHeight / 2.0f;

        // Set the obstacle's position
        obstacle.transform.position = epos;
    }

    public void PlayHit()
    {
        hitSound.Play();
    }

    public void SetScore(float score)
    {
        distance += score;
    }

    public void PlaySnipe()
    {
        sniperSound.time = 0.1f;
        sniperSound.Play();
    }
}
