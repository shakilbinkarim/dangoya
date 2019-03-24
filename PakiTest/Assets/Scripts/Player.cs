using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class Player : MonoBehaviour {

    [SerializeField] private float speed = 500.0f; // Speed at which the player moves
    [SerializeField] private AudioClip dangoClip; // Sound made when player collides with Dango
    [SerializeField] private AudioClip bombClip; // Sound made when player collides with Bomb
    [SerializeField] private AudioClip tokeiClip; // Sound made when player collides with Clock
    [SerializeField] private AudioClip advance3Clip; // Sound made when player collides with Arrow
    [SerializeField] private AudioClip shieldClip; // Sound made when player collides with Shield
    [SerializeField] private AudioClip lifeClip; // Sound made when player collides with Life
    [SerializeField] private LevelManager levelManager; // Reference to Level Manager
    // Gameobjects parented to the player for showng players Dangos that they got
    [SerializeField] private GameObject[] dangoPositions;
    // contains the translucent shield cover ti indicate player currently has shield
    [SerializeField] private GameObject shieldCover; 

    private bool mobile = false; // Flag to see if player can move now
    private AudioSource audioSource; // The audio source the player has
    private bool willPlay; // Flag to see if sound can be played
    private int dangos; // To keep track of dangos the player has collected
    private float shieldTime;
    private bool isShielded;

    // Use this for initialization
    void Start ()
    {
        speed = -1 * Mathf.Sqrt(speed * speed); // using square root incase speed is negative
        audioSource = GetComponent<AudioSource>();
        int music = GamerPrefs.GetMusicOn(); // 悪い
        willPlay = (music == 1) ? true : false;
        dangos = -1;
        InitShieldFunc();
    }

    private void InitShieldFunc()
    {
        shieldTime = GamerPrefs.GetShieldDuration();
        MovingTingsFactory.CAN_MAKE_SHIELDS = true;
        shieldCover.SetActive(false);
        isShielded = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (mobile)
        {
            gameObject.transform.Translate(0, speed * Time.deltaTime, 0); 
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            mobile = true;
        }	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagsManager.LowerColliderTag)
        {
            levelManager.GameOver();
        }
        if (collision.gameObject.tag == TagsManager.UpperColliderTag)
        {
            mobile = false;
            // because speed was doubled when the stick was sent up
            speed = -1 * Mathf.Sqrt(speed / 2 * speed / 2);　
        }
        if (collision.gameObject.tag == TagsManager.PinkDungoTag ||
            collision.gameObject.tag == TagsManager.GreenDungoTag ||
            collision.gameObject.tag == TagsManager.WhiteDungoTag)
        {
            PlaySound(dangoClip);
            SendStickUp();
            // Destroying fish and playing particlefx
            stickDango(collision);
        }
        if (collision.gameObject.tag == TagsManager.BombTag)
        {
            PlaySound(bombClip);
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            SendStickUp();
            if (!isShielded)
            {
                levelManager.GameOverWithBang();
                Destroy(gameObject); 
            }
        }
        if (collision.gameObject.tag == TagsManager.ClockTag)
        {
            PlaySound(tokeiClip);
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.SlowTime();
        }
        if (collision.gameObject.tag == TagsManager.ArrowTag)
        {
            PlaySound(advance3Clip);
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            EmptyDungoStick();
            levelManager.AdvanceSanLevel();
        }
        if (collision.gameObject.tag == TagsManager.LifeTag)
        {
            SendStickUp();
            PlaySound(lifeClip);
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.IncreaseLife();
        }
        if (collision.gameObject.tag == TagsManager.OkaneTag)
        {
            PlaySound(tokeiClip); // TODO: Replace with Coin Clip
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.AddOkane();
        }
        if (collision.gameObject.tag == TagsManager.ShieldTag)
        {
            PlaySound(shieldClip);
            Debug.Log("Shield Triggered");
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            shieldCover.SetActive(true);
            isShielded = true;
            MovingTingsFactory.CAN_MAKE_SHIELDS = false;
            StartCoroutine(ResetShield(shieldTime));
        }
    }

    private IEnumerator ResetShield(float shieldTime)
    {
        yield return new WaitForSeconds(shieldTime);
        shieldCover.SetActive(false);
        isShielded = false;
        MovingTingsFactory.CAN_MAKE_SHIELDS = true;
    }

    private void SendStickUp()
    {
        mobile = true;
        // doubling spped when sending stick up
        speed = 2 * Mathf.Sqrt(speed * speed);
    }

    private void PlaySound(AudioClip clip)
    {
        if (willPlay)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void stickDango(Collider2D collision)
    {
        dangos = dangos + 1;
        collision.gameObject.GetComponent<MovingThings>().Vaporize();
        if (levelManager.DungoMatch(collision.gameObject.tag, dangos))
        {
            Sprite dangoSprite = collision.GetComponent<SpriteRenderer>().sprite;
            dangoPositions[dangos].GetComponent<SpriteRenderer>().sprite = dangoSprite;
            levelManager.AddHP(5.0f);
            if (dangos >= 2)
            {
                StartCoroutine(ScorePalayer(0.2f));
            }
        }
        else
        {
            EmptyDungoStick();
            levelManager.DeductLife();
            // TODO: deduct life from player
        }
    }

    private IEnumerator ScorePalayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EmptyDungoStick();
        levelManager.AddScore(1);
        levelManager.ResetIndicators(); /// TODO: check if redundant
    }

    public void EmptyDungoStick()
    {
        for (int i = 0; i < dangoPositions.Length; i++)
        {
            dangoPositions[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        dangos = -1;
        levelManager.ResetIndicators();
    }
}
