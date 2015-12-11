﻿using UnityEngine;
using System.Collections;

public class BullSpawner : MonoBehaviour
{
	// Spawn Speed
	public float MinSpeed = 3.0f;
	public float MaxSpeed = 5.0f;
	public float MinDistBetweenBulls = 5.0f;

	// Spawn Rate
	[Tooltip("The minimum spawn rate. Is not affected by SpawnAmplitude.")]
	public float MinSpawnRate;
	[Tooltip("The maximum spawn rate. Is not affected by SpawnAmplitude.")]
	public float MaxSpawnRate;
	[Tooltip("The range between the min and max spawn rate when auto generated based on game progress.")]
	public float SpawnAmplitude;        // Controls

	// Memory of Previous Bull
	private BasicBull[] prevBullEachSide = new BasicBull[2];        // 0 - Left, 1 - Right

	public GameObject BabyBullPrefab;
	float TimeTillNextBull = 0.0f;
	public Transform LeftSpawner, RightSpawner;
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(Player_Bear.Instance.transform.position.x, Player_Bear.Instance.transform.position.y, transform.position.z);

		TimeTillNextBull -= TimeManager.Instance.GetGameDeltaTime();


		if(TimeTillNextBull <= 0)
		{
			spawnBull(BasicBull.TypeOfBull.BULL_BABY, Random.Range(0, 2) == 0);
		}
	}

	private void spawnBull(BasicBull.TypeOfBull bullType, bool spawnLeft)
	{
		Vector3 spawnPos;
		BasicBull prevBull;
		// Select the correct previous bull
		if (spawnLeft)
		{
			prevBull = prevBullEachSide[0];
		}
		else
		{
			prevBull = prevBullEachSide[1];
		}


		// Determine the position to spawn
		if (spawnLeft)
		{
			spawnPos = LeftSpawner.position;
		}
		else
		{
			spawnPos = RightSpawner.position;
		}

		// Create the type of bull specified
		GameObject newBull;
		switch (bullType)
		{
			case BasicBull.TypeOfBull.BULL_BABY:
				newBull = Instantiate(BabyBullPrefab, spawnPos, Quaternion.identity) as GameObject;
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
		float minSpawnTime = Mathf.Clamp(MinSpawnRate + (SpawnAmplitude * Mathf.Sin(sinAngle)), MinSpawnRate, MaxSpawnRate);
		TimeTillNextBull = Random.Range(minSpawnTime, MaxSpawnRate);

		// Store this bull for later
		if (spawnLeft)
		{
			prevBullEachSide[0] = newBull.GetComponent<BasicBull>();
		}
		else
		{
			prevBullEachSide[1] = newBull.GetComponent<BasicBull>();
		}
	}
}
