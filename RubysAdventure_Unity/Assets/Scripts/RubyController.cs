using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public int score;

    public TextMeshProUGUI scoreText;

    public GameObject projectilePrefab;

    //   Win/Lose Screen Here

    public GameObject WinTextObject;

    public GameObject LostTextObject;

    //   Audio Code Here

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip collectedClip;

    public AudioSource Background;

    //   Health Code Here

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    //   Particle Systems

    public ParticleSystem damageEffect;
    public ParticleSystem healingEffect;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        //Particle Heal/Damage Here
        damageEffect.GetComponent<ParticleSystem>().enableEmission = false;
        healingEffect.GetComponent<ParticleSystem>().enableEmission = false;

        audioSource = GetComponent<AudioSource>();

        //Win/Lose Screen Here
        WinTextObject.SetActive(false);
        LostTextObject.SetActive(false);

        //Score Text Here
        score = 0;
        SetScoreText();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (score >= 2)
        {
            WinTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            damageEffect.GetComponent<ParticleSystem>().enableEmission = true;

            GameObject ParticleSystem = Instantiate(damageEffect.gameObject, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            PlaySound(hitSound);
        }
        //gain health check
        if (amount > 0)
        {
            healingEffect.GetComponent<ParticleSystem>().enableEmission = true;
            GameObject ParticleSystem = Instantiate(healingEffect.gameObject, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        if (currentHealth == 0)
        {
            LostTextObject.SetActive(true);
            Destroy(this);
        }

        //UIHealthBar Here

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    //New Stuff!
    public void ChangeScore(int scoreAmount)
    {
        score += scoreAmount;
        SetScoreText();
    }
    void SetScoreText()
    {
        scoreText.text = "Fixed Robots: " + score.ToString();

    }

}