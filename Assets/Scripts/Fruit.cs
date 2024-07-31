using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool isInCloud = true;
    private bool timeToCheck = false;
    private int numColliding = 0;

    void Start()
    {
        // if below cloud
        if (transform.position.y < 3.5)
        {
            isInCloud = false;
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

    void Update()
    {
        if (isInCloud)
        {
            GetComponent<Transform>().position = Spawner.CloudPosition;
        }
        
        if (Input.GetKeyDown("space"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
            isInCloud = false;
            
            StartCoroutine(checkGameOver());
        }

        if (Spawner.IsGameOver)
        {
            Spawner.IncreaseScore(Spawner.Points[int.Parse(gameObject.tag)]);
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == gameObject.tag)
        {
            if (numColliding < 3)
            {
                numColliding += 1;
                Spawner.SpawnPosition = transform.position;
                Spawner.IsNewFruitSpawning = true;
                Spawner.NewFruitIndex = int.Parse(gameObject.tag) + 1;
                Destroy(gameObject);
            }
            else
            {
                numColliding = 0;
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "limit" && timeToCheck)
        {
            Spawner.EndGame();
        }
    }
    
    private IEnumerator checkGameOver()
    {
        yield return new WaitForSeconds(0.75f);
        timeToCheck = true;
    }
}