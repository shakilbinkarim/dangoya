using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThingsFactory : MonoBehaviour {

    public static bool DungoFactoryWorking = false;
    public static bool CanJumpWaves = false;
    public static bool CanSlowHunger = false;
    public static bool CanMakeShields = false;
    
    // TODO: turn into dictionary
    [SerializeField] private GameObject[] movingThings; // Array containing all the moving game objects to be spawned
    [SerializeField] private float spawnInterval = 0.5f; // Interval between which moving things will be spawned
    [SerializeField] private float spawnPosYUpperBound = -0.5f; // Upper limit of spawn position for moving things
    [SerializeField] private float spawnPosYLowerBound = -4.0f; // Lower limit of spawn position for moving things
    [SerializeField] private float movingObjectLifeTime = 4.0f; // Seconds after which moving things will be destroyed

    private Vector3 spawnPosition; // Position of currently spawned moving object
    private int shield, life, bomb, arrow, coin, clock; // Variables to store positions of non Dango moving things
    
    private void Start ()
    {
        InitNonDangoMovingThingsPos();
        StartCoroutine(SpawnMovingObject(spawnInterval));
    }

    private void InitNonDangoMovingThingsPos() // TODO: replace with enum
    {
        bomb = 3;
        clock = 4;
        arrow = 5;
        shield = 6;
        life = 7;
        coin = 8;
    }
    
    private IEnumerator SpawnMovingObject(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (!DungoFactoryWorking) continue; // TODO: maybe change this
            var determiner = Random.Range(1, 100);
            if (determiner > 97 && CanJumpWaves) CreateMovingThing(arrow);
            else if (determiner > 90 && CanSlowHunger) CreateMovingThing(clock);
            else if (determiner > 85 && CanMakeShields) CreateMovingThing(shield);
            else if (determiner > 80) CreateMovingThing(life);
            else if (determiner > 75) CreateMovingThing(bomb);
            else if (determiner > 70) CreateMovingThing(coin);
            else CreateMovingThing(Random.Range(0, 3)); // Random.Range(0, 3) is spawnObjectIndex
        }
    }

    private void CreateMovingThing(int spawnObjectIndex)
    {
        spawnPosition = CalculateSpawnPosition();
        GameObject movingThing = Instantiate(movingThings[spawnObjectIndex], spawnPosition, Quaternion.identity);
        Destroy(movingThing, movingObjectLifeTime);
    }

    private Vector3 CalculateSpawnPosition()
    {
        var yPos = Random.Range(spawnPosYLowerBound, spawnPosYUpperBound);
        return new Vector3(gameObject.transform.position.x, yPos, 0.0f);
    }
    
}
