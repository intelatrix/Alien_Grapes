using UnityEngine;
using System;
using System.Collections;

public abstract class Skill : MonoBehaviour
{	
	[Tooltip("The duration of the whole skill.")]
	public float Duration;
	[Tooltip("The chance for this skill to drop.")]
	public float DropChance;

	private bool activated = false;
	protected float skillTimer = 0.0f;

	public enum Type
	{
		SlowDown,
		BeeShield
	}

	// Use this for initialization
	void Start () 
	{
		skillTimer = Duration;

		// Call derived start function
		start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (activated)
		{
			skillTimer -= TimeManager.Instance.GetGameDeltaTime ();

			// If the skill has expired
			if (skillTimer < 0.0f)
			{
				CleanUp ();
			}
		}

		// Call derived update function
		update ();
	}
		
	public void Use ()
	{
		if (IsUnused)
		{
			activated = true;

			// Call derived use function
			use ();
		}
	}

	public void CleanUp()
	{
		if (HasExpired)
		{
			activated = false;

			// Call derived clean up function
			cleanUp ();
		}
	}

	// To Query the state of this Skill
	public bool IsUnused { get { return (activated == false && skillTimer > 0.0f); } }
	public bool IsUsing { get { return activated; } }
	public bool HasExpired { get { return skillTimer < 0.0f; } }

	// Functions to implement
	protected abstract void start();
	protected abstract void update();
	protected abstract void use();
	protected abstract void cleanUp();
}
