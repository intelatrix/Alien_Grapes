using UnityEngine;
using System;
using System.Collections;

public class SkillManager : MonoBehaviour 
{
	// To load PreFab skills
	public GameObject SlowDownSkillPrefab;
	public GameObject BeeShieldSkillPrefab;

	// List to easily access prefab Skills
	private GameObject[] SkillList = new GameObject[Enum.GetNames (typeof(Skill.Type)).Length];

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

	public GameObject SpawnRandomSkill()
	{
		// Get a random skill type
		Skill.Type skillType = (Skill.Type)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Skill.Type)).Length);

		Skill skill = SkillList [(int)skillType].GetComponent<Skill> ();

		// Calculate if probabilities will let us spawn it
		if (skill != null)
		{
			return Instantiate (SkillList [(int)skillType]);
		}

		return null;
	}
}
