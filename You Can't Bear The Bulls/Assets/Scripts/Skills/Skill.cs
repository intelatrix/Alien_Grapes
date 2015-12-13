using UnityEngine;
using System;
using System.Collections;

public abstract class Skill : MonoBehaviour
{	
	[Tooltip("The duration of the whole skill.")]
	public float Duration;
	[Tooltip("The chance for this skill to drop.")]
	public float DropChance;
    [Tooltip("The sprite of this skill's icon.")]
    public Sprite Icon;

	private bool activated = false;
	protected float skillTimer = 0.0f;
    protected Type type;

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
			skillTimer -= TimeManager.Instance.GetNormalTime();

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

            SkillManager.Instance.ResetSkillIcon();
		}
	}

    public void EndPrematurely()
    {
        if (activated)
        {
            skillTimer = -1.0f;
        }
    }

	// To Query the state of this Skill
	public bool IsUnused { get { return (activated == false && skillTimer > 0.0f); } }
	public bool IsUsing { get { return activated; } }
	public bool HasExpired { get { return skillTimer < 0.0f; } }
    public Type SkillType { get { return type; } }

	// Functions to implement
	protected abstract void start();
	protected abstract void update();
	protected abstract void use();
	protected abstract void cleanUp();
}
