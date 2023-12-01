using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SadScene : MonoBehaviour
{
    // Added both WIN and LOST music (2 differents scripts) - Luke B.
    public AudioClip sad;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = sad;
            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Scene1");
        }
    }
}