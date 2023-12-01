using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    // Added both VICTORY and LOST music (2 differents scripts) - Luke B.
    public AudioClip victory;
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
            audioSource.clip = victory;
            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Scene1");
        }
    }
}