using UnityEngine;
using System.Collections;

public class SlowDownSkill : Skill 
{
	[Tooltip("The multiplier to modify the speed of the game.")]
	public float TimeScale = 0.5f;

	protected override void start ()
	{
        type = Type.SlowDown;
	}

	protected override void update ()
	{
		
	}

	protected override void use()
	{
		TimeManager.Instance.SetGameTimeScale (TimeScale);
	}

	protected override void cleanUp ()
	{
		TimeManager.Instance.SetGameTimeScale (1.0f);
	}
}
