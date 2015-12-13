﻿using UnityEngine;
using System.Collections;

public class BeeShieldSkill : Skill 
{
    SpriteRenderer visual;

	protected override void start ()
	{
		SoundManager.Instance.PlayEffect(SoundManager.Effects.EFFECT_BEE_SHIELD);
        type = Type.BeeShield;
        visual = GetComponent<SpriteRenderer>();
    }

	protected override void update ()
	{
    }

	protected override void use()
	{
        visual.enabled = true;
    }

	protected override void cleanUp ()
	{
        visual.enabled = false;
    }
}