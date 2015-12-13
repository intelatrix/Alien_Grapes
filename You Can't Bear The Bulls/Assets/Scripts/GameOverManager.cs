using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	public GameObject UN, BEAR, ABLE;
	public GameObject PressAnyKeyTo;  
	float TimeTillNextText = 0.1f;
	int CurrentStage = 0;

	public void PassScore(int Score)
	{
	}
	// Update is called once per frame
	void Update () 
	{
		if(CurrentStage == 3)
		{
			if (Input.anyKeyDown)
			{
				Application.LoadLevel("MainScene");
			}
		}
		else
		{
			TimeTillNextText -= Time.deltaTime;

			if(TimeTillNextText < 0)
			{
				++CurrentStage;
				if(CurrentStage == 1)
				{
					TimeTillNextText =0.5f;
					UN.SetActive(true);
				}
				else if(CurrentStage == 2)
				{
					TimeTillNextText = 0.5f;
					BEAR.SetActive(true);
				}
				else 
				{
					ABLE.SetActive(true);
					PressAnyKeyTo.SetActive(true);
				}
			}
		}
	}
}
