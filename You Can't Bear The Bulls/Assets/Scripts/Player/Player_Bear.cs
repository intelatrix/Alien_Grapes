using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[Serializable]
public struct NamedImage 
{
	public string name;
	public Sprite image;
}


public class Player_Bear : MonoSingleton<Player_Bear>
{
	BasicBull TargetedBull = null;
	BearState CurrentBearState = BearState.BEAR_NONE;
	BearSubMomEnum CurrentMomBearState = BearSubMomEnum.BEAR_MOM_MOVE_TOWARDS;
	public BasicBull targetedBull {get{return TargetedBull;}}

	public float ConstLerpTime = 0.5f;
	public float ConstLerpMomTime = 0.2f;
	float LerpTimeLeft = 0;
	public float lerpTimeLeft { get { return LerpTimeLeft; } }


	public int ConstMaxCharge = 100;
	int CurrentCharge = 0;

	public float ConstAwayFrom = 2;
	public float MissDistance = 0.5f;

	// Textures/Sprites
	public SpriteRenderer BearSprite;
	public List<NamedImage> ListOfSprite;
	Dictionary<string, Sprite> DictionaryOfSprite = new Dictionary<string, Sprite>();

	// Skills
	private GameObject playerSkill = null;

	Vector3 MissTowards;

	public Image ChargeBar;

	public enum BearState
	{
		BEAR_NONE,
		BEAR_ATTACK_BABY,
		BEAR_ATTACK_MOTHER,
		BEAR_ATTACK_FATHER,
		BEAR_MISS
	}

	public enum BearSubMomEnum
	{
		BEAR_MOM_MOVE_TOWARDS,
		BEAR_MOM_ATTACK
	}

	// Use this for initialization
	void Start()
	{
		//ChargeBar.fillAmount = 0.5f;
		foreach(NamedImage NewSprite in ListOfSprite)
		{
			DictionaryOfSprite.Add(NewSprite.name, NewSprite.image);
		}

		ListOfSprite = null;
	}

	// Update is called once per frame
	void Update()
	{
		BearUpdate();
	}

	void BearUpdate()
	{
		switch (CurrentBearState)
		{
			case BearState. BEAR_ATTACK_BABY:
				BearAttackBabyUpdate();
				break;
			case BearState.BEAR_MISS:
				BearMissUpdate();
				break;
			case BearState.BEAR_ATTACK_MOTHER:
				BearMotherUpdate();
				break;
		case BearState.BEAR_ATTACK_FATHER:
				BearFatherUpdate();
				break;
		}
	}

	void BearAttackBabyUpdate()
	{
		LerpTimeLeft += TimeManager.Instance.GetGameDeltaTime();
		Vector3 NewTargetPosition;
		if(TargetedBull.IfFacingRight)
			NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(ConstAwayFrom,0,0);
		else
			NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(-ConstAwayFrom,0,0);;
			 
		transform.position = Vector3.Lerp(new Vector3(transform.position.x,transform.position.y,0), NewTargetPosition, LerpTimeLeft/ConstLerpTime);

		if(transform.position == NewTargetPosition)
		{
			CurrentBearState = BearState.BEAR_NONE;
			GameSceneManager.Instance.BullGetPunched(TargetedBull);
			TargetedBull = null;
		}
	}

	void BearMotherUpdate()
	{
		switch(CurrentMomBearState)
		{
			case BearSubMomEnum.BEAR_MOM_MOVE_TOWARDS:

				LerpTimeLeft += TimeManager.Instance.GetGameDeltaTime();
				Vector3 NewTargetPosition;

				if(TargetedBull.IfFacingRight)
					NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(ConstAwayFrom,0,0);
				else
					NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(-ConstAwayFrom,0,0);;
				 
				transform.position = Vector3.Lerp(new Vector3(transform.position.x,transform.position.y,0), NewTargetPosition, LerpTimeLeft/ConstLerpMomTime);

				if(transform.position == NewTargetPosition)
				{
					CurrentMomBearState = BearSubMomEnum.BEAR_MOM_ATTACK;
					GameSceneManager.Instance.ReachedMom();
				}
				break;
			case BearSubMomEnum.BEAR_MOM_ATTACK:
				break;
		}
	}

	public void FinishMom()
	{
		CurrentBearState = BearState.BEAR_NONE;
	}

	void BearFatherUpdate()
	{
		
	}

	void BearMissUpdate()
	{
		LerpTimeLeft += TimeManager.Instance.GetGameDeltaTime();
		transform.position = Vector3.Lerp(new Vector3(transform.position.x,transform.position.y,0), MissTowards, LerpTimeLeft/ConstLerpTime);

		if(transform.position == MissTowards)
		{
			CurrentBearState = BearState.BEAR_NONE;
		}
	}

	void IncreaseCharge(int AmountOfNewCharge)
	{
		CurrentCharge += AmountOfNewCharge;

	}

	public void SetBearAttackBaby(BasicBull AttackThisBull)
	{
		TargetedBull = AttackThisBull;
		CurrentBearState = BearState.BEAR_ATTACK_BABY;
		LerpTimeLeft = 0;
	}


	public void SetBearAttackMother(BasicBull AttackThisBull)
	{
		TargetedBull = AttackThisBull;
		CurrentBearState = BearState.BEAR_ATTACK_MOTHER;
		CurrentMomBearState = BearSubMomEnum.BEAR_MOM_MOVE_TOWARDS;
		LerpTimeLeft = 0;

	}

//	public void StopMissing()
//    {
//		CurrentBearState = BearState.BEAR_NONE;
//    }

	public void SetBearMiss(bool IfFacingRight)
	{
		CurrentBearState = BearState.BEAR_MISS; 
		if(IfFacingRight)
			MissTowards = transform.position + new Vector3(1,0,0);
		else
			MissTowards = transform.position + new Vector3(-1,0,0);

		LerpTimeLeft = 0;
	}

	public BearState GetBearState()
	{
		return CurrentBearState;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Bull" && (CurrentBearState == BearState.BEAR_MISS || CurrentBearState == BearState.BEAR_NONE))
		{
			GameSceneManager.Instance.BearGetsHit(collision.gameObject);
		}
	}

	public void UseSkill()
	{
		if (playerSkill != null)
		{
			Skill skill = playerSkill.GetComponent<Skill>();

			if (skill.IsUnused)
			{
				skill.Use();
			}
		}
	}

	public void SetSkill(GameObject skill)
	{
		if (skill == null)
		{
			return;
		}

		playerSkill = skill;

        // Move the skill to the player
        playerSkill.transform.position = transform.position;
	}
}
