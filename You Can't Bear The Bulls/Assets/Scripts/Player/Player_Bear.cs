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
	BearSubDadEnum CurrentDadBearState = BearSubDadEnum.BEAR_DAD_MOVE_TOWARDS;
    public BasicBull targetedBull {get{return TargetedBull;}}

    public float ConstLerpTime = 0.5f;
    public float ConstLerpMomTime = 0.2f;
	public float ConstLerpDadTime = 0.2f;
    float LerpTimeLeft = 0;
	public float lerpTimeLeft { get { return LerpTimeLeft; } }

	public float ToRestMaxTime;
	float RemainingRestTime = 0;

	public float ConstAwayFrom = 2;
	public float MissDistance = 0.5f;

	public float MaxRoarTime = 0.3f;
	float RoarTimeLeft = 0;

	// Textures/Sprites
	public SpriteRenderer BearSprite;
	public List<NamedImage> ListOfSprite;
	Dictionary<string, Sprite> DictionaryOfSprite = new Dictionary<string, Sprite>();

	// Skills
	private GameObject heldSkill = null;
    private GameObject currentSkill = null;

    Vector3 MissTowards;

    //Health & Charge
    public Image ChargeBar;
    public Image BearToleranceLevel;

    public int ConstMaxCharge = 40;
    int CurrentCharge = 0;

    int Health = 3;
    public bool IsAlive { get { return Health > 0; } }

    int AttackType = 0;

    public GameObject SteamObject;

    public enum BearState
    {
        BEAR_NONE,
        BEAR_ATTACK_BABY,
		BEAR_ATTACK_MOTHER,
		BEAR_ATTACK_FATHER,
        BEAR_MISS,
        BEAR_ROAR
    }

    public enum BearSubMomEnum
    {
    	BEAR_MOM_MOVE_TOWARDS,
    	BEAR_MOM_ATTACK
    }

	public enum BearSubDadEnum
    {
    	BEAR_DAD_MOVE_TOWARDS,
    	BEAR_DAD_ATTACK
    }

    // Use this for initialization
    void Start()
    {
		
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

        // Move the skill to the player
        if (currentSkill != null)
        {
            currentSkill.transform.position = transform.position;
        }

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
			case BearState.BEAR_NONE:
				BearNoneUpdate();
				break;

		case BearState.BEAR_ROAR:
				BearRoarUpdate();
				break;
        }
    }

	void BearNoneUpdate()
    {
   	 	if(RemainingRestTime > 0)
    	{
    		RemainingRestTime -= TimeManager.Instance.GetGameDeltaTime();
    		if(RemainingRestTime <= 0)
    		{
				BearSprite.sprite = DictionaryOfSprite["BearFaceRight"];
    		}
    	}
    }

	void BearRoarUpdate()
	{
		RoarTimeLeft -= TimeManager.Instance.GetGameDeltaTime();
		if(RoarTimeLeft < 0)
		{
			CurrentBearState = BearState.BEAR_NONE;
			BearSprite.sprite = DictionaryOfSprite["BearFrontIdle"];
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

	public void FinishDad()
	{
		CurrentBearState = BearState.BEAR_NONE;
	}

	void BearFatherUpdate()
	{
		switch(CurrentDadBearState)
		{
			case BearSubDadEnum.BEAR_DAD_MOVE_TOWARDS:

				LerpTimeLeft += TimeManager.Instance.GetGameDeltaTime();
				Vector3 NewTargetPosition;

		    	if(TargetedBull.IfFacingRight)
					NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(ConstAwayFrom,0,0);
				else
					NewTargetPosition = new Vector3(TargetedBull.transform.position.x,transform.position.y,0) + new Vector3(-ConstAwayFrom,0,0);;
				 
				transform.position = Vector3.Lerp(new Vector3(transform.position.x,transform.position.y,0), NewTargetPosition, LerpTimeLeft/ConstLerpDadTime);

				if(transform.position == NewTargetPosition)
	    		{
	    			CurrentDadBearState = BearSubDadEnum.BEAR_DAD_ATTACK;
	    			GameSceneManager.Instance.ReachedDad();
	    		}
				break;
			case BearSubDadEnum.BEAR_DAD_ATTACK:
				break;
		}
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

    public void IncreaseCharge(int AmountOfNewCharge)
    {
    	if(CurrentCharge != ConstMaxCharge)
			CurrentCharge += AmountOfNewCharge;
			

		UpdateChargeBar();
    }

    void UpdateChargeBar()
    {
		ChargeBar.fillAmount = (float)CurrentCharge / (float)ConstMaxCharge;

		if(CurrentCharge == ConstMaxCharge)
			SteamObject.SetActive(true);
		else
			SteamObject.SetActive(false);
    }

    public void SetBearAttackBaby(BasicBull AttackThisBull)
    {
    	TargetedBull = AttackThisBull;
    	CurrentBearState = BearState.BEAR_ATTACK_BABY;
    	LerpTimeLeft = 0;
		BearAttack();
    }


    public void SetBearAttackMother(BasicBull AttackThisBull)
    {
    	TargetedBull = AttackThisBull;
    	CurrentBearState = BearState.BEAR_ATTACK_MOTHER;
		CurrentMomBearState = BearSubMomEnum.BEAR_MOM_MOVE_TOWARDS;
    	LerpTimeLeft = 0;
		BearAttack();
    }

	public void SetBearAttackFather(BasicBull AttackThisBull)
    {
    	TargetedBull = AttackThisBull;
    	CurrentBearState = BearState.BEAR_ATTACK_FATHER;
		CurrentDadBearState = BearSubDadEnum.BEAR_DAD_MOVE_TOWARDS;
    	LerpTimeLeft = 0;
		BearAttack();
    }

    public void BearAttack()
    {
		RemainingRestTime = ToRestMaxTime;

    	BearSprite.flipX = TargetedBull.ifFacingRight;
    	if(AttackType == 0)
    	{
			AttackType = 1;
			BearSprite.sprite = DictionaryOfSprite["Bear_Slap_1"];
    	}
    	else
    	{
    		AttackType = 0;
			BearSprite.sprite = DictionaryOfSprite["Bear_Slap_2"];
    	}
    }

	public void StopMissing()
    {
		BearSprite.sprite = DictionaryOfSprite["BearFaceRight"];
    }

    public void SetBearMiss(bool IfFacingRight)
    {
		CurrentBearState = BearState.BEAR_MISS; 
		if(IfFacingRight)
			MissTowards = transform.position + new Vector3(1,0,0);
		else
			MissTowards = transform.position + new Vector3(-1,0,0);

		LerpTimeLeft = 0;

		BearSprite.flipX = !IfFacingRight;
		BearSprite.sprite = DictionaryOfSprite["BearMiss"];
    }

    public BearState GetBearState()
    {
    	return CurrentBearState;
    }

	void OnTriggerEnter2D(Collider2D collision)
    {
    	if(collision.tag == "Bull" && (CurrentBearState == BearState.BEAR_MISS || CurrentBearState == BearState.BEAR_NONE))
    	{
           BearGetsHit(collision.gameObject);
        }
    }

    public void BearGetsHit(GameObject HittingBull)
    {
		// Normal Collision
			if (HittingBull.GetComponentInParent<BasicBull>().IsAlive)
            {
                // Check if any skill can interrupt
                if (currentSkill != null)
                {
                    Skill skill = currentSkill.GetComponent<Skill>();

                    // If the bee shield is present
                    if (skill != null && skill.SkillType == Skill.Type.BeeShield && skill.IsUsing)
                    {
                    // Shield absorbs the damage
                        skill.EndPrematurely();
                        // Kill the Bull
						HittingBull.GetComponentInParent<BasicBull>().StartKillSequence();

                        // Don't need to proceed and kill ourselves later anymore
                       return;
                    }

					GameSceneManager.Instance.ResetMultiplier();
					BearSprite.flipX = HittingBull.GetComponentInParent<BasicBull>().ifFacingRight;
					BearSprite.sprite = DictionaryOfSprite["BearHit"];
	                --Health;
	                UpdateTolerance();
					GameSceneManager.Instance.BearGetsHit(HittingBull.gameObject);

					if(Health == 0)
						GameOver();
                }
                else
                {	
                	GameSceneManager.Instance.ResetMultiplier();
					BearSprite.flipX = HittingBull.GetComponentInParent<BasicBull>().ifFacingRight;
					BearSprite.sprite = DictionaryOfSprite["BearHit"];
	                --Health;
	                UpdateTolerance();
					GameSceneManager.Instance.BearGetsHit(HittingBull.gameObject);

					if(Health == 0)
						GameOver();
                }
            }
    }

    void GameOver()
    {
    	GameSceneManager.Instance.GameOver();

    	GetComponent<BoxCollider2D>().enabled = false;
		this.enabled = false;
    }

    void UpdateTolerance()
    {
		BearToleranceLevel.sprite = DictionaryOfSprite["BearTolerance" + Health];
    }

	public void UseSkill()
	{
		if (heldSkill != null)
		{
            Skill skill = heldSkill.GetComponent<Skill>();
            heldSkill.transform.position = transform.position;

            if (skill.IsUnused)
            {
                currentSkill = heldSkill;
                heldSkill = null;
                skill.Use();
            }

        }
    }

	public void UseCharge()
	{
		if (CurrentCharge == ConstMaxCharge)
		{
			CurrentCharge = 0;
			GameSceneManager.Instance.KillAllBulls();
			BearSprite.sprite = DictionaryOfSprite["BearRoar"];

			CurrentBearState = BearState.BEAR_ROAR;
			RoarTimeLeft = MaxRoarTime;

			UpdateChargeBar();
        }
    }

    public void SetSkill(GameObject skill)
    {
        // Clean up the previous skill if it exists
        if (skill == null)
        {
            return;
        }

        heldSkill = skill;

        // Move the skill to the player
        heldSkill.transform.position = transform.position;
    }

}
