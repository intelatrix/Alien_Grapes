using UnityEngine;
using System.Collections;

public class BullSpawner : MonoBehaviour {

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
			TimeTillNextBull = 2f;
			if(Random.Range(0,2) == 0)
			{
				GameObject NewBull = Instantiate(BabyBullPrefab, LeftSpawner.position, Quaternion.identity) as GameObject;
				NewBull.GetComponent<BasicBull>().IfFacingRight = true;
			}
			else
			{
				GameObject NewBull = Instantiate(BabyBullPrefab, RightSpawner.position, Quaternion.identity) as GameObject;
				NewBull.GetComponent<BasicBull>().IfFacingRight = false;
			}
		}
	}
}
