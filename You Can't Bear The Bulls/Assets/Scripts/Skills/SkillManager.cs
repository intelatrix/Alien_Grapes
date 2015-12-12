using UnityEngine;
using System;
using System.Collections;

public class SkillManager : MonoSingleton<SkillManager> 
{
	// To load PreFab skills
	public GameObject SlowDownSkillPrefab;
	public GameObject BeeShieldSkillPrefab;
    [Tooltip("Handle to the Skill Icon so that we can change the icon to what the player is holding to.")]
    public GameObject SkillIconMarker;
    [Tooltip("Default icon for no skills.")]
    public Sprite DefaultSkillIcon;

    // Easy handle to what we really want to access in SkillIconMarker
    private SpriteRenderer SkillIconMarkerSprite;

	// List to easily access prefab Skills
	private GameObject[] SkillList = new GameObject[Enum.GetNames (typeof(Skill.Type)).Length];

	// Use this for initialization
	void Start () 
	{
		// Load the prefabs that were specified into this array
		SkillList [(int)Skill.Type.SlowDown] = SlowDownSkillPrefab;
		SkillList [(int)Skill.Type.BeeShield] = BeeShieldSkillPrefab;

        // Set up the handle to the Icon sprite
        SkillIconMarkerSprite = SkillIconMarker.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        
	}

	public GameObject SpawnRandomSkill()
	{
		// Get a random skill type
		Skill.Type skillType = (Skill.Type)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Skill.Type)).Length);

        // Check if it is a skill
		Skill skill = SkillList [(int)skillType].GetComponent<Skill> ();

		// Calculate if probabilities will let us spawn it
		if (skill != null)
		{
            // Update the Skill Icon
            // -- Get the icon of the skill to update
            Sprite skillIcon = skill.Icon;
            if (skillIcon != null)
            {
                SkillIconMarkerSprite.sprite = skillIcon;
            }
            else
            {
                SkillIconMarkerSprite.sprite = DefaultSkillIcon;
            }

            // Pass the skill back
            return Instantiate (SkillList [(int)skillType]);
		}

		return null;
	}

    public void ResetSkillIcon()
    {
        SkillIconMarkerSprite.sprite = DefaultSkillIcon;
    }
}
