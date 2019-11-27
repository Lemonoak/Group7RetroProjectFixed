using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    //TODO: Removed unnecessary commented/unused code
    //TODO: Split each section of variables with a empty line for readability
    Vector3 leftStartPosition = new Vector3(-15.75f,0,0);
    Vector3 rightStartPosition = new Vector3(15.75f,0,0);

    Quaternion ref_LeftRotation;
    Quaternion ref_RightRotation;

    bool playerOneIsIn = false;
    bool playerTwoIsIn = false;

    public GameObject playerGameobject;
    public GameObject aIGameobject;

    GameObject playerOne;
    GameObject playerTwo;

    GameObject aiOne;
    GameObject aiTwo;

    GameModeController gameMode;
    PlayerJoinTextHandler textManager;

    float restartTime = 0;
    float idleTime = 60;

    bool original = false;
    public bool screenlayingDown = false;

    private void Awake()
    {
        gameMode = GetComponent<GameModeController>();
        if (FindObjectsOfType<PlayerManager>().Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            if (SceneManager.GetActiveScene().name != "MainMenuScreen")
            {
                CreatePlayers();
            }
            restartTime = Time.fixedTime;                            
            original = true;      
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("QuitGame"))
        {
            if (!GameObject.FindWithTag("Player"))
            {
                Application.Quit();
            }
            else
            {
                DisablePlayerOne();
                DisablePlayerTwo();
            }
        }

        if(GameObject.FindWithTag("Player") && (Input.GetButtonDown("Return1") && playerOneIsIn))
        {
            DisablePlayerOne();
        }
        else if(GameObject.FindWithTag("Player") && (Input.GetButtonDown("Return2") && playerTwoIsIn))
        {
            DisablePlayerTwo();
        }

        if (idleTime != 0 && (playerOneIsIn || playerTwoIsIn))
            {
                if (Input.GetAxis("Movement1") != 0 || Input.GetAxis("Movement2") != 0)
                {
                    restartTime = Time.fixedTime;
                }
                else if (Time.fixedTime > idleTime + restartTime)
                {
                    DisablePlayerOne();
                    DisablePlayerTwo();
                }
            }

        if (Input.GetButtonDown("Start1") && !playerOneIsIn)
        {
            PlayerOneJoin();
        }
        if (Input.GetButtonDown("Start2") && !playerTwoIsIn)
        {
            PlayerTwoJoin();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (original)
        {
            RotateField();
            restartTime = Time.fixedTime;
            if (!GameObject.FindWithTag("AI") && !GameObject.FindWithTag("Player"))
            {
                CreatePlayers();
            }
        }
    }

    //TODO: Create the players and ai objects when entering main game scene for enabling / disabling later
    void CreatePlayers()
    {
        textManager = FindObjectOfType<PlayerJoinTextHandler>();
        textManager.GetPlayers(playerOneIsIn, playerTwoIsIn, screenlayingDown);

        //Spawn Players
        playerOne = Instantiate(playerGameobject, leftStartPosition, ref_LeftRotation);
        playerOne.transform.eulerAngles = new Vector3(0, 0, -90);
        playerOne.SetActive(false);

        playerTwo = Instantiate(playerGameobject, rightStartPosition, ref_RightRotation);
        playerTwo.transform.eulerAngles = new Vector3(0, 0, 90);
        playerTwo.SetActive(false);

        //Spawn AI
        aiOne = Instantiate(aIGameobject, leftStartPosition, ref_LeftRotation);
        aiOne.transform.eulerAngles = new Vector3(0, 0, -90);

        aiTwo = Instantiate(aIGameobject, rightStartPosition, ref_RightRotation);
        aiTwo.transform.eulerAngles = new Vector3(0, 0, 90);

    }

    void RotateField()
    {
        gameMode.RotateAll(screenlayingDown);
    }

    //TODO: I created all functions below to enable / disable players and ai instead of reloading the scene, this decreased ms spikes, Only the playerone is commented since playertwo works the same
    void PlayerOneJoin()
    {
        //Sets playerOneIsIn to true to be able to leave properly
        playerOneIsIn = true;
        //Disables ai
        aiOne.SetActive(false);

        //Sets player transform position to start position and enables player
        playerOne.transform.position = leftStartPosition;
        playerOne.SetActive(true);
        //Disables "JOIN GAME" text
        textManager.GetPlayers(playerOneIsIn, playerTwoIsIn, screenlayingDown);
    }

    void DisablePlayerOne()
    {
        //sets playerOneIsIn to false and disables player object
        playerOneIsIn = false;
        playerOne.SetActive(false);
        //Sets ai transform to startpostion and enables ai object
        aiOne.transform.position = leftStartPosition;
        aiOne.SetActive(true);

        //Enables "JOIN GAME" text
        textManager.GetPlayers(playerOneIsIn, playerTwoIsIn, screenlayingDown);
    }

    void PlayerTwoJoin()
    {
        playerTwoIsIn = true;
        aiTwo.SetActive(false);

        playerTwo.transform.position = rightStartPosition;
        playerTwo.SetActive(true);

        textManager.GetPlayers(playerOneIsIn, playerTwoIsIn, screenlayingDown);
    }

    void DisablePlayerTwo()
    {
        playerTwoIsIn = false;
        playerTwo.SetActive(false);
        aiTwo.transform.position = rightStartPosition;
        aiTwo.SetActive(true);

        textManager.GetPlayers(playerOneIsIn, playerTwoIsIn, screenlayingDown);
    }



}
