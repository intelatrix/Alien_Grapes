using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Bear : MonoSingleton<Player_Bear>
{

    BasicBull TargetedBull = null;
    BearState CurrentBearState = BearState.BEAR_NONE;
    public float ConstLerpTime = 0.5f;
    float LerpTimeLeft = 0;

    public int ConstMaxCharge = 100;
    int CurrentCharge = 0;

	public float ConstAwayFrom = 2;
	public float MissDistance = 0.5f;

    Vector3 MissTowards;

    public Image ChargeBar;

    enum BearState
    {
        BEAR_NONE,
        BEAR_ATTACK_BABY,
		BEAR_ATTACK_MOTHER,
		BEAR_ATTACK_FATHER,
        BEAR_MISS
    }

    // Use this for initialization
    void Start()
    {
		//ChargeBar.fillAmount = 0.5f;
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

	void OnTriggerEnter2D(Collider2D collision)
    {
    	if(collision.tag == "Bull" && (CurrentBearState == BearState.BEAR_MISS || CurrentBearState == BearState.BEAR_NONE))
    	{
	        GameSceneManager.Instance.BearGetsHit(collision.gameObject);
        }
    }
}
