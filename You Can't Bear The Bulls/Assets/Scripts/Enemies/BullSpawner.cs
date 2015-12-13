using UnityEngine;
using System.Collections;

public class BullSpawner : MonoBehaviour
{
	// Spawn Speed
	[Tooltip("The minimum speed of a spawned bull.")]
	public float MinSpeed = 3.0f;
	[Tooltip("The maximum speed of a spawned bull.")]
	public float MaxSpeed = 5.0f;
	[Tooltip("The minimum distance between bulls that have to be adhered to.")]
	public float MinDistBetweenBulls = 5.0f;

	// Spawn Rate
	[Tooltip("The minimum spawn rate. Is not affected by SpawnAmplitude.")]
	public float MinSpawnRate = 0.5f;
	[Tooltip("The maximum spawn rate. Is not affected by SpawnAmplitude.")]
	public float MaxSpawnRate = 2.0f;
	[Tooltip("The range between the min and max spawn rate when auto generated based on game progress.")]
	public float SpawnAmplitude = 1.5f;

	// Spawn Conditions for Special Bulls
	[Tooltip("Chance for MotherBull to spawn at the start.")]
	public float MomStartSpawnChance = 10.0f;
	[Tooltip("Number of kill points needed before the Mother spawn chance is max.")]
	public int MinKillToMaxSpawnMom = 25;
    [Tooltip("The max percentage for spawn of the Mom.")]
    public int MaxMomSpawnChance = 30;
	[Tooltip("Chance for FatherBull to spawn at the start.")]
	public float DadStartSpawnChance = 1.0f;
	[Tooltip("Number of kill points needed before the Father spawn chance is max.")]
	public int MinKilledToMaxSpawnDad = 50;
    [Tooltip("The max percentage for spawn of the Mom.")]
    public int MaxDadSpawnChance = 20;

    // Spawn Conditions Variables for Special Bulls
    [Tooltip("Kill points a baby kill provides.")]
	public int BabyForceSpawnWeight = 1;
	[Tooltip("Kill points a mother kill provides.")]
	public int MotherForceSpawnWeight = 5;

	// Calculated Increments in Chances
	private float spawnIncrementFather;
	private float spawnIncrementMother;

	// Track spawn chances
	private float motherSpawnChance;
	private float fatherSpawnChance;

	// Memory of Previous Bull
	private BasicBull[] prevBullEachSide = new BasicBull[2];        // 0 - Left, 1 - Right

	// Bulls Prefabs
	[Tooltip("The prefab that is used to clone a baby bull for spawning.")]
	public GameObject BabyBullPrefab;
	[Tooltip("The prefab that is used to clone a mother bull for spawning.")]
	public GameObject MotherBullPrefab;
	[Tooltip("The prefab that is used to clone a father bull for spawning.")]
	public GameObject FatherBullPrefab;

	// Spawn Points
	[Tooltip("Position of the spawn point for bulls on the left.")]
	public Transform LeftSpawner;
	[Tooltip("Position of the spawn point for bulls on the right.")]
	public Transform RightSpawner;

	// Spawn Limiter
	float timeTillNextBull = 0.0f;

	// Start is called once at instantiation
	void Start()
	{
		// Calculate Increments in Chances
		spawnIncrementFather = (MaxDadSpawnChance - DadStartSpawnChance) / MinKilledToMaxSpawnDad;
		spawnIncrementMother = (MaxMomSpawnChance - MomStartSpawnChance) / MinKillToMaxSpawnMom;

		// Initialize trackers for spawn chances
		motherSpawnChance = MomStartSpawnChance;
		fatherSpawnChance = DadStartSpawnChance;
	}

	// Update is called once per frame
	void Update () 
	{
		// Ensure that the spawner and it's children spawn points are always centered around the player
		transform.position = new Vector3(Player_Bear.Instance.transform.position.x, Player_Bear.Instance.transform.position.y, transform.position.z);

		// Update the spawn timer
		timeTillNextBull -= TimeManager.Instance.GetGameDeltaTime();

		// Spawn if the timer has reached
		if(timeTillNextBull <= 0)
		{
			// Calculate chance for this spawn
			int chance = Random.Range(0, 100);
            if (chance <= fatherSpawnChance)
            {
                // Spawn
                spawnBull(BasicBull.TypeOfBull.BULL_FATHER);
                // Reset Spawn Chance
                fatherSpawnChance = DadStartSpawnChance;
            }
            else if (chance <= fatherSpawnChance + motherSpawnChance)
            {
                // Spawn
                spawnBull(BasicBull.TypeOfBull.BULL_MOTHER);
                // Reset Spawn Chance
                motherSpawnChance = MomStartSpawnChance;
                // Increase the spawn chances for specials
                fatherSpawnChance += spawnIncrementFather * MotherForceSpawnWeight;
            }
            else
            {
                // Spawn
                spawnBull(BasicBull.TypeOfBull.BULL_BABY);
                // Increase the spawn chances for specials
                fatherSpawnChance += spawnIncrementFather * BabyForceSpawnWeight;
                motherSpawnChance += spawnIncrementMother * BabyForceSpawnWeight;
            }

            // Prevent the chances from going over the max
            if (motherSpawnChance > MaxMomSpawnChance)
            {
                motherSpawnChance = MaxMomSpawnChance;
            }
            if (fatherSpawnChance > MaxDadSpawnChance)
            {
                fatherSpawnChance = MaxDadSpawnChance;
            }
		}
	}

	private void spawnBull(BasicBull.TypeOfBull bullType)
	{
		Vector3 spawnPos;
		BasicBull prevBull;
		bool spawnLeft;

		// Select the correct previous bull and the position to spawn
		if (Random.Range(0, 2) == 0)
		{
			spawnLeft = true;
			prevBull = prevBullEachSide[0];
			spawnPos = LeftSpawner.position;
		}
		else
		{
			spawnLeft = false;
			prevBull = prevBullEachSide[1];
			spawnPos = RightSpawner.position;
		}

		// Create the type of bull specified
		GameObject newBull;
		switch (bullType)
		{
			case BasicBull.TypeOfBull.BULL_BABY:
				newBull = Instantiate(BabyBullPrefab, spawnPos, Quaternion.identity) as GameObject;
				break;
			case BasicBull.TypeOfBull.BULL_MOTHER:
				newBull = Instantiate(MotherBullPrefab, spawnPos, Quaternion.identity) as GameObject;
				break;
			case BasicBull.TypeOfBull.BULL_FATHER:
				newBull = Instantiate(FatherBullPrefab, spawnPos, Quaternion.identity) as GameObject;
				break;
			default:
				newBull = Instantiate(BabyBullPrefab, spawnPos, Quaternion.identity) as GameObject;
				break;
		}

		// Calculate the move speed
		float moveSpeed;
		if (prevBull != null)
		{
			float totalDistToPlayer = Mathf.Abs(spawnPos.x - transform.position.x);
			float timeForPrevToReach = totalDistToPlayer / prevBull.MovementSpeed;
			float maxMoveSpeed = totalDistToPlayer / timeForPrevToReach;

			moveSpeed = Random.Range(MinSpeed, maxMoveSpeed);
		}
		else
		{
			moveSpeed = Random.Range(MinSpeed, MaxSpeed);
		}

		// Spawn the bull
		newBull.GetComponent<BasicBull>().Launch(bullType, spawnPos, moveSpeed, spawnLeft);

		// Upon successful spawn, set the spawn timer
		// -- Calculate the next spawn timer
		float sinAngle = Mathf.Abs(StatManager.Instance.TimeSinceStart % (Mathf.PI * 2));
		float minSpawnTime = Mathf.Clamp(SpawnAmplitude + (StatManager.Instance.TimeSinceStart * Mathf.Sin(sinAngle) - StatManager.Instance.TimeSinceStart * 0.05f), MinSpawnRate, MaxSpawnRate);
		timeTillNextBull = Random.Range(minSpawnTime, MaxSpawnRate);

		// Store this bull for later
		if (spawnLeft)
		{
			prevBullEachSide[0] = newBull.GetComponent<BasicBull>();
		}
		else
		{
			prevBullEachSide[1] = newBull.GetComponent<BasicBull>();
		}

        // Store this bull in the all list
        GameSceneManager.Instance.AddBullInsideList(newBull.GetComponent<BasicBull>());
	}
}
