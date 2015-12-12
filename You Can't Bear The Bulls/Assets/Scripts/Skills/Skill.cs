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
			skillTimer += TimeManager.Instance.GetGameDeltaTime ();
		}

		// Call derived update function
		update ();
	}
		
	public void Use ()
	{
		activated = true;

		// Call derived use function
		use ();
	}

	public void CleanUp()
	{
		skillTimer = Duration;
		activated = false;

		// Call derived clean up function
		cleanUp ();
	}

	public Skill Clone()
	{
		return clone ();
	}

	// Functions to implement
	protected abstract void start();
	protected abstract void update();
	protected abstract void use();
	protected abstract void cleanUp();
	protected virtual void clone()
	{

	}
}
