using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTingsFactory : MonoBehaviour {

    public static bool DUNGO_FACTORY_WORKING = false;
    public static bool CAN_JUMP_WAVES = false;
    public static bool CAN_SLOW_HUNGER = false;
    public static bool CAN_MAKE_SHIELDS = false;
    
    // Array containing all the moving game objects to be spawned
    [SerializeField] private GameObject[] movingThings;
    // Interval between which moving things will be spawned
    [SerializeField] private float spawnInterval = 0.5f;
    // Upper limit of spawn position for moving things
    [SerializeField] private float spawnPosYUpperBound = -0.5f;
    // Lower limit of spawn position for moving things
    [SerializeField] private float spawnPosYLowerBound = -4.0f;
    // Seconds after which moving things will be destroyed
    [SerializeField] private float movingObjectLifeTime = 4.0f;

    private Vector3 spawPosition; // Position of currently spawned moving object
    // Variables to store positions of non Dango moving things
    private int shield, life, bomb, arrow, coin, clock;

	// Use this for initialization
	void Start ()
    {
        InitNonDangoMovingThingsPos();
        StartCoroutine(SpawnMovingObject(spawnInterval));
    }

    private void InitNonDangoMovingThingsPos()
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
            if (DUNGO_FACTORY_WORKING)
            {
                int determiner = Random.Range(1, 100);
                if (determiner > 97 && CAN_JUMP_WAVES)
                {
                    CreateMovingThing(arrow);
                }
                else if (determiner > 90 && CAN_SLOW_HUNGER)
                {
                    CreateMovingThing(clock);
                }
                else if (determiner > 85 && CAN_MAKE_SHIELDS)
                {
                    CreateMovingThing(shield);
                }
                else if (determiner > 80)
                {
                    CreateMovingThing(life);
                }
                else if (determiner > 75)
                {
                    CreateMovingThing(bomb);
                }
                else if (determiner > 70)
                {
                    CreateMovingThing(coin);
                }
                else
                {
                    int spawnObjectIndex = Random.Range(0, 3);
                    CreateMovingThing(spawnObjectIndex);
                }
            }
        }
    }

    private void CreateMovingThing(int spawnObjectIndex)
    {
        spawPosition = CalulateSpawnPosition();
        GameObject movingThing =
            GameObject.Instantiate(
                movingThings[spawnObjectIndex],
                spawPosition,
                Quaternion.identity);
        Destroy(movingThing, movingObjectLifeTime);
    }

    private Vector3 CalulateSpawnPosition()
    {
        float yPos = Random.Range(spawnPosYLowerBound, spawnPosYUpperBound);
        return new Vector3(gameObject.transform.position.x, yPos, 0.0f);
    }
}
