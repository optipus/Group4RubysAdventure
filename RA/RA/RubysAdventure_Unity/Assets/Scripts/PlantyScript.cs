using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantyScript : MonoBehaviour
{
    //Planty NPC Script -- Ema P
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    public AudioClip DeathSound;

    Rigidbody2D rigidbody2D;

    Animator animator;

    AudioSource audioSource;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Kill()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        dialogBox.SetActive(false);
        animator.SetTrigger("dead");

        PlaySound(DeathSound);

    }
}