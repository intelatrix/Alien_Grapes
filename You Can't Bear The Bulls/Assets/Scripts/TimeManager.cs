using UnityEngine;
using System.Collections;

//Time Manager - Used to mess with game time without affecting the whole app's Time
public class TimeManager : MonoSingleton<TimeManager> {

    enum TimeState
    {
        TIME_NORMAL,
        TIME_SLOWDOWN
    };

	// Use this for initialization

	float GameDeltaTime = 0;
	float GameTimeScale = 1;
	float NormalTime = 0;
	float NormalTimeScale = 1;
	
	// Update is called once per frame
	void Update () {
		GameDeltaTime = Time.deltaTime;
		NormalTime = Time.deltaTime;
	
	}

	//Set Game Time Scale
	public void SetGameTimeScale(float NewScale)
	{
		GameTimeScale = NewScale;
	}

	//Return the Delta Time with the effect of the time scale
	public float GetGameDeltaTime()
	{
		return GameTimeScale*GameDeltaTime;
	}

	public void SetNormalTimeScale(float NewScale)
	{
		NormalTimeScale = NewScale;
	}

	//Return Delta time of the frame without any effects from any Time Scale
	public float GetNormalTime()
	{
		return NormalTime * NormalTimeScale;
	}
}
