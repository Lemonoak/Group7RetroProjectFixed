using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballSpawnPoint1;
    public GameObject ballSpawnPoint2;
    public GameObject spawnrefObject;
    GameObject spawnedObject;
    public ScoreBoard scoreBoard;
    public GameObject[] AIReference;
    public GameObject musk;

    Vector2 ballSpawnPosition;
    GameObject mainBall;


    private void Start()
    {
        mainBall = (GameObject)Instantiate(spawnrefObject, ballSpawnPosition, Quaternion.identity);
        mainBall.GetComponent<BallMovement>().board = scoreBoard;
        mainBall.GetComponent<BallMovement>().SetSpawner(this);
        mainBall.SetActive(false);
    }

    public void SpawnBall()
    {
        musk.GetComponent<AudioSource>().volume = 1f;
        //TODO: Removed instantiation of ball everytime someone scores
        if (scoreBoard.PlayerScoar == 1)
        {
            ballSpawnPosition = ballSpawnPoint2.transform.position;
        }
        else
        {
            ballSpawnPosition = ballSpawnPoint1.transform.position;
        }
        //Set transform of ball to spawnpoint
        mainBall.transform.position = ballSpawnPosition;
        mainBall.SetActive(true);

        ///////////////////

        AIReference = GameObject.FindGameObjectsWithTag("AI");
        //ERROR HANDLING
        if (AIReference.Length == 1)
        {
            AIReference[0].GetComponent<AI>().GetBall();
        }
        if (AIReference.Length > 1)
        {
            AIReference[0].GetComponent<AI>().GetBall();
            AIReference[1].GetComponent<AI>().GetBall();
        }
    }

    public void DisableBall()
    {
        mainBall.SetActive(false);
    }
}
