using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    private GameObject ball;

    public float AISpeed = 10.0f;

    public string movementkey;
    public string specialKey;
    bool hasNewPlayer;

    SpriteRenderer sR;
    public Sprite hitAni;
    Sprite defSpr;

    public Sprite missAni;
    public Sprite bosstAni;

    float animationDelay = 0.3f;
    float lastTime;

    private void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        defSpr = sR.sprite;
    }

    void FixedUpdate()
    {
        HandleMovement();
        if (Time.fixedTime > lastTime + animationDelay && sR.sprite != defSpr)
        {
            sR.sprite = defSpr;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && sR.sprite != bosstAni && sR.sprite != missAni)
        {
            sR.sprite = hitAni;
            lastTime = Time.fixedTime;
        }
    }

    void HandleMovement()
    {
        //For the ai on the right monitor
        if(transform.position.x > 0)
        {
            //Checks if there is a ball and then if the balls position is on the ai's screen
            if(ball && ball.transform.position.x > 0)
            {
                //Moves the ai upwards
                if(ball.transform.position.y > gameObject.transform.position.y)
                {
                    transform.Translate(new Vector3(Time.deltaTime * AISpeed, 0.0f, 0.0f));
                }
                //Moves the ai downwards
                else if (ball.transform.position.y < gameObject.transform.position.y)
                {
                    transform.Translate(new Vector3(-Time.deltaTime * AISpeed, 0.0f, 0.0f));
                }
            }
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2.2f, 2.1f), transform.position.z);
        }

        //For the ai on the left monitor
        else if(transform.position.x < 0)
        {
            //Checks if there is a ball and then if the balls position is on the ai's screen
            if (ball && ball.transform.position.x < 0)
            {
                //Moves the ai upwards
                if (ball.transform.position.y < gameObject.transform.position.y)
                {
                    transform.Translate(new Vector3(Time.deltaTime * AISpeed, 0.0f, 0.0f));
                }
                //Moves the ai downwards
                else if (ball.transform.position.y > gameObject.transform.position.y)
                {
                    transform.Translate(new Vector3(-Time.deltaTime * AISpeed, 0.0f, 0.0f));
                }
            }
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2.2f, 2.2f), transform.position.z);
        }
    }

    //Gets the ball object reference
    public void GetBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    public void SwungRaket(bool didHit)
    {
        if (didHit)
        {
            sR.sprite = bosstAni;
        }
        else
        {
            sR.sprite = missAni;
        }
        lastTime = Time.fixedTime;
    }
}
