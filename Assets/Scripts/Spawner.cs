using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// TODO: create variable for rigidbody instead of using getcomponent in update

public class Spawner : MonoBehaviour
{
    public Transform[] Fruits;

    public Transform ScoreElement;
    public GameObject GameOverPopup;//
    public Transform FinalScoreElement;//

    private static TextMeshProUGUI scoreText;
    private static TextMeshProUGUI finalScoreText;//
    private static GameObject gameOverPopup;//
    
    public enum SpawnedState
    {
        Yes,
        No,
        Wait
    }
    public static SpawnedState SpawnedYet = SpawnedState.No;

    public static Vector2 CloudPosition;
    public static Vector2 SpawnPosition;

    public static bool IsNewFruitSpawning = false;
    public static bool IsGameOver = false;
    public static int NewFruitIndex;
    public static int BestFruit = 2;

    public static int[] Points = new int[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66 };
    private static int score = 0;
    public static void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        finalScoreText.text = "Score: " + score;//
    }

    void Start()
    {
        scoreText = ScoreElement.GetComponent<TextMeshProUGUI>();
        finalScoreText = FinalScoreElement.GetComponent<TextMeshProUGUI>();//
        gameOverPopup = GameOverPopup;//
    }

    void Update()
    {
        spawnFruit();
        replaceFruit();

        // handle spawner cat movement
        if (Input.GetKey("a"))
        {
            // spawner cat moves left when a is pressed
            GetComponent<Rigidbody2D>().velocity = new Vector2(-4, 0);
        }
        else if (Input.GetKey("d"))
        {
            // spawner cat moves right when d is pressed
            GetComponent<Rigidbody2D>().velocity = new Vector2(4, 0);
        }
        else
        {
            // if neither are pressed, stop spawner cat from moving
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        // handle stopping spawner cat movement at the edges of the box
        if (GetComponent<Rigidbody2D>().velocity.x < 0 && transform.position.x < -2.5)
        {
            // stop spawner cat from moving to far to the left
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0 && transform.position.x > 2.5)
        {
            // stop spawner cat from moving to far to the right
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        CloudPosition = transform.position;
        
        if (Input.GetKeyDown("space") && SpawnedYet == SpawnedState.Yes && IsGameOver == false)//end (add gameover clause)
        {
            SpawnedYet = SpawnedState.No;
        }
    }
    
    private void spawnFruit()
    {
        if (SpawnedYet == SpawnedState.No)
        {
            SpawnedYet = SpawnedState.Wait;
            StartCoroutine(spawnTimer());
        }
    }
    
    private void replaceFruit()
    {
        if (IsNewFruitSpawning)
        {
            IsNewFruitSpawning = false;

            if (NewFruitIndex > 10) return;

            Instantiate(Fruits[NewFruitIndex], SpawnPosition, Fruits[0].rotation);
            IncreaseScore(Points[NewFruitIndex]);

            if (NewFruitIndex > BestFruit && NewFruitIndex < 7)
            {
                BestFruit = NewFruitIndex;
            }
        }
    }

    // wait 0.75 seconds between dropping fruit and spawning new fruit
    private IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(0.75f);

        int randInd = Random.Range(0, BestFruit - 1);
        Instantiate(Fruits[randInd], transform.position, Fruits[0].rotation);
        SpawnedYet = SpawnedState.Yes;
    }
    
    public static void EndGame()
    {
        gameOverPopup.SetActive(true);
        IsGameOver = true;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        score = 0;
        IsGameOver = false;
    }
}