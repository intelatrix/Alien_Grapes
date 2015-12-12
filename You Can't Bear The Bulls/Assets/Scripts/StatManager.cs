using UnityEngine;
using System;
using System.Collections;

public class StatManager : MonoSingleton<StatManager>
{
	private float timeSinceStart = 0.0f;
	private int[] enemiesKilled = new int[Enum.GetNames(typeof(BasicBull.TypeOfBull)).Length];

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update the game elapsed time
		timeSinceStart += TimeManager.Instance.GetGameDeltaTime();
	}

	#region Statistics Update
	/*
	* Use these fields and functions to update the stats
	*/
	public void AddEnemyKilled(BasicBull.TypeOfBull type)
	{
		enemiesKilled[(int)type]++;
	}
	#endregion

	#region Statistics Access
	/*
	 * Use these fields and functions to obtain the stats
	 */
	public float TimeSinceStart { get { return timeSinceStart; } }

	public int TotalEnemiesKilled
	{
		get 
		{
			int total = 0;
			for (int i = 0; i < Enum.GetNames (typeof(BasicBull.TypeOfBull)).Length; ++i) 
			{
				total += enemiesKilled [i];
			}
			return total;
		}
	}

	public int GetNumEnemiesKilled(BasicBull.TypeOfBull type)
	{
		return enemiesKilled[(int)type];
	}
	#endregion
}
