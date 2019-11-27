using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public ScoreBoard board;
    [SerializeField]
    private Rigidbody2D RB;
    public BallSpawner spawner;
    //for the AI to see the ball
    public GameObject Ball;

    //The Speed that the ball current has, Used to calculate score
    public float CurrentBallSpeed = 0.0f;
    //The value that the balls speed is increased by every tick
    public float SpeedUpValue = 0.00001f;
    //The value that the balls speed is increased by when it hits a player
    public float HitPlayerSpeedUpValue = 0.0f;

    //the speed the balls starts with on the X axis, the Y is also this value but divided by 2
    public float StartSpeed = 1.0f;
    public AudioSource tickSource;

    bool PlayerSmashed = false;
    Vector2 oldVelocty;
    float maxVelocityPart = 1;
    float funXSpeed = 8;
    float funYSpeed = 0.2f;

    void Start()
    {
        tickSource = GetComponent<AudioSource>();
        RB = Ball.GetComponent<Rigidbody2D>();       
        Ball = gameObject;
        //Randomzies direction to start and adds force on the ball
        //RandomizeStartDirection();
        oldVelocty = RB.velocity;
    }

    private void OnEnable()
    {
        RB = Ball.GetComponent<Rigidbody2D>();
        RB.velocity = Vector2.zero;
        RandomizeStartDirection();
    }

    private void OnDisable()
    {
        ResetVelocity();
    }

    void Update()
    {
        RB.velocity += new Vector2(RB.velocity.x * SpeedUpValue, RB.velocity.y * SpeedUpValue);

        if(Ball.transform.position.x < - 5000)
        {
            spawner.DisableBall();
        }
        else if (Ball.transform.position.x > 5000)
        {
            spawner.DisableBall();
        }
        
        if(RB.velocity.x != 0)
        {
            RB.velocity = Maxvelocity(); // the y speed relative to x need to be fixt
        }
    }

    Vector2 Maxvelocity()
    {
        Vector2 velocityClamp = new Vector2( Mathf.Clamp(Mathf.Abs(RB.velocity.x), 0,45), Mathf.Clamp(Mathf.Abs(RB.velocity.y), 0, 45) );
        float remainder;
        if (velocityClamp.x < funXSpeed)
        {       
            for (; velocityClamp.y/ velocityClamp.x > maxVelocityPart;)
            {
                remainder = velocityClamp.y * 0.05f;
                velocityClamp.y -= remainder;
                velocityClamp.x += remainder;
            }
        }
        if (velocityClamp.y < funYSpeed)
        {
            velocityClamp.x -= (funYSpeed - velocityClamp.y);
            velocityClamp.y = funYSpeed;
        }
        if (velocityClamp.x + velocityClamp.y < Mathf.Abs(oldVelocty.x) + Mathf.Abs(oldVelocty.y))
        {
            Debug.Log("GotSlower");
            velocityClamp.x = Mathf.Clamp(Mathf.Abs(oldVelocty.x), 0, 45);
            velocityClamp.y = Mathf.Clamp(Mathf.Abs(oldVelocty.y), 0, 45);
        }
        if (RB.velocity.x < 0)
        {
            velocityClamp.x *= -1;
        }
        if (RB.velocity.y < 0)
        {
            velocityClamp.y *= -1;
        }        
        return velocityClamp;
    }
    //Sets the start direction and adds force to the ball in that direction
    public void RandomizeStartDirection()
    {
        int[] num = new int[] {-1,1};
        int startDir = Random.Range(0, 2);
        float startAngle = Random.Range(1.5f,3);

        if (transform.position.x > 0)
        {
            RB.AddForce(new Vector2(StartSpeed, (StartSpeed / startAngle) * num[startDir]));
        }
        else
        {
            RB.AddForce(new Vector2(-StartSpeed, (StartSpeed / startAngle) * num[startDir]));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        tickSource.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PickUp" && collision.GetComponent<OnBoardPickUp>().isOn)
        {            
            RB.velocity += new Vector2(RB.velocity.x * SpeedUpValue, RB.velocity.y * SpeedUpValue);
        }
        else if (collision.tag == "Player" || collision.tag == "AI")
        {
            //Debug.Log("Entered Player");
            //ERROR HANDLING
            if(PlayerSmashed)
            {
                RemoveSpeed();
            }
            else if(Mathf.Abs(oldVelocty.x) + Mathf.Abs(oldVelocty.x) < Mathf.Abs(RB.velocity.x) + Mathf.Abs(RB.velocity.x))
            {
                oldVelocty = RB.velocity;
            }
            else
            {
                oldVelocty.x = Mathf.Abs(oldVelocty.x);
                oldVelocty.y = Mathf.Abs(oldVelocty.y);
                if (RB.velocity.x < 0)
                {
                    oldVelocty.x *= -1;
                }
                if (RB.velocity.y < 0)
                {
                    oldVelocty.y *= -1;
                }
                RB.velocity = oldVelocty;
                Debug.Log("I Has ComeTo THs");
            }
        }
        else if (collision.tag == "Goal")
        {
            if (transform.position.x < 0)
            {
                board.Scored(2, Mathf.Abs(RB.velocity.x) + Mathf.Abs(RB.velocity.y));
            }
            else
            {
                board.Scored(1, Mathf.Abs(RB.velocity.x) + Mathf.Abs(RB.velocity.y));
            }
            //TODO: Disabled Ball
            spawner.DisableBall();
        }
    }

    public void AddSpeed()
    {
        PlayerSmashed = true;
        oldVelocty = RB.velocity;
        RB.velocity += new Vector2(RB.velocity.x * HitPlayerSpeedUpValue, RB.velocity.y * HitPlayerSpeedUpValue);
        Debug.Log("got smashed");
    }

    public void RemoveSpeed()
    {
        PlayerSmashed = false;
        RB.velocity = new Vector2(oldVelocty.x, oldVelocty.y);
    }

    public void SetSpawner(BallSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void ResetVelocity()
    {
        RB.velocity = Vector2.zero;
        RB.angularVelocity = 0;
    }
}
