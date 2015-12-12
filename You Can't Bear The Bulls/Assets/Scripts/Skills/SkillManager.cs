using UnityEngine;
using System;
using System.Collections;

public class SkillManager : MonoBehaviour 
{
	// To load PreFab skills
	public Skill SlowDownSkillPrefab;
	public Skill BeeShieldSkillPrefab;

	private Skill[] SkillList = new Skill[Enum.GetNames (typeof(Skill.Type)).Length];

	// Use this for initialization
	void Start () 
	{
		// Load the prefabs that were specified into this array
		SkillList [(int)Skill.Type.SlowDown] = SlowDownSkillPrefab;
		SkillList [(int)Skill.Type.BeeShield] = BeeShieldSkillPrefab;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public Skill SpawnRandomSkill()
	{
		// Get a random skill type
		Skill.Type skillType = (Skill.Type)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Skill.Type)).Length);

		// Calculate if probabilities will let us spawn it
		if (UnityEngine.Random.Range (0, 99) < SkillList [(int)skillType].DropChance)
		{
			return SkillList[(int)skillType].Clone();
		}

		return null;
	}
}
