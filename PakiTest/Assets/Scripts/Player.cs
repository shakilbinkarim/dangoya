using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class Player : MonoBehaviour {

    [SerializeField] private float speed = 500.0f; // Speed at which the player moves

    [SerializeField] private LevelManager levelManager; // Reference to Level Manager
    [SerializeField] private GameObject[] dangoPositions;// Gameobjects parented to the player for showng players Dangos that they got
    [SerializeField] private GameObject shieldCover; // contains the translucent shield cover ti indicate player currently has shield

    private bool inMotion = false; // Flag to see if player can move now
    private int dangos; // To keep track of dangos the player has collected
    private float shieldTime;
    private bool isShielded;

    public void EmptyDungoStick()
    {
        for (var i = 0; i < dangoPositions.Length; i++) 
            dangoPositions[i].GetComponent<SpriteRenderer>().sprite = null;
        dangos = -1;
        levelManager.ResetIndicators();
    }
    
    private void Start ()
    {
        speed = -1 * Mathf.Sqrt(speed * speed); // using square root incase speed is negative
        dangos = -1;
        InitShieldFunc();
    }

    private void InitShieldFunc()
    {
        shieldTime = GamerPrefs.GetShieldDuration();
        MovingThingsFactory.CanMakeShields = true;
        shieldCover.SetActive(false);
        isShielded = false;
    }

    private void Update ()
    {
        if (inMotion) gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) inMotion = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagsManager.LowerColliderTag) levelManager.GameOver();
        if (collision.gameObject.tag == TagsManager.UpperColliderTag)
        {
            inMotion = false;
            // because speed was doubled when the stick was sent up
            speed = -1 * Mathf.Sqrt(speed / 2 * speed / 2);　
        }

        var collidedWithADungo = collision.gameObject.tag == TagsManager.PinkDungoTag ||
                collision.gameObject.tag == TagsManager.GreenDungoTag ||
                collision.gameObject.tag == TagsManager.WhiteDungoTag;
        if (collidedWithADungo)
        {
            SendStickUp();
            // Destroying fish and playing particlefx
            StickDango(collision);
        }
        if (collision.gameObject.tag == TagsManager.BombTag)
        {
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
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.SlowTime();
        }
        if (collision.gameObject.tag == TagsManager.ArrowTag)
        {
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            EmptyDungoStick();
            levelManager.AdvanceSanLevel();
        }
        if (collision.gameObject.tag == TagsManager.LifeTag)
        {
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.IncreaseLife();
        }
        if (collision.gameObject.tag == TagsManager.OkaneTag)
        {
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            levelManager.AddOkane();
        }
        if (collision.gameObject.tag == TagsManager.ShieldTag)
        {
            Debug.Log("Shield Triggered");
            SendStickUp();
            collision.gameObject.GetComponent<MovingThings>().Vaporize();
            shieldCover.SetActive(true);
            isShielded = true;
            MovingThingsFactory.CanMakeShields = false;
            StartCoroutine(ResetShield(shieldTime));
        }
    }

    private IEnumerator ResetShield(float shieldTime)
    {
        yield return new WaitForSeconds(shieldTime);
        shieldCover.SetActive(false);
        isShielded = false;
        MovingThingsFactory.CanMakeShields = true;
    }

    private void SendStickUp()
    {
        inMotion = true;
        // doubling speed when sending stick up
        speed = 2 * Mathf.Sqrt(speed * speed);
    }

    private void StickDango(Collider2D collision)
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
        }
    }

    private IEnumerator ScorePalayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EmptyDungoStick();
        levelManager.AddScore(1);
        levelManager.ResetIndicators(); /// TODO: check if redundant
    }

    
}
