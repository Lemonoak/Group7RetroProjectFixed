using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRacket : MonoBehaviour
{
    public bool ballSwing = false;
    public ParticleSystem hitParticlesFailed;
    public bool playParticles = true;
    public AI myMama;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            if (Random.Range(0,4) == 0)
            {
                myMama.SwungRaket(true);
                collision.GetComponent<BallMovement>().AddSpeed();
                ballSwing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && !ballSwing && Random.Range(0, 2) == 0)
        {
            myMama.SwungRaket(false);
            //Added Error handling for missing particles
            if (playParticles && hitParticlesFailed != null)
            {
                if (hitParticlesFailed.isStopped)
                {
                    hitParticlesFailed.Play();
                }
            }
        }
        ballSwing = false;
    }
}
