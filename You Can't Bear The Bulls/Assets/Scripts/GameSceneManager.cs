using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameSceneManager : MonoSingleton<GameSceneManager>
{ 
	List<BasicBull> ListOfBullLeft = new List<BasicBull>();
	List<BasicBull> ListOfBullRight = new List<BasicBull>();
	List<BasicBull> ListofAllBulls = new List<BasicBull>();

	public int SizeOfLeftList{get{return ListOfBullLeft.Count;}} 
	public int SizeOfRightList{get{return ListOfBullRight.Count;}} 

	public QTEManager MotherQTEManager, FatherQTEManager;

	bool IfMiss = false;
	public float ConstMissTime = 1.0f;
	public float ConstMaximumBlackAlpha = 0.5f;

	public SpriteRenderer FirstBlackBackground, SecondBlackBackground;

	float MissTimeLeft = 0f;
	float TimeLeftForQTE = 0f;

	int MotherQTENumber = 3;
	int FatherQTENumber = 5;

	int Multiplier = 0;
	int Score = 0;

	public Text ScoreText, MultiplierText;

	public enum GameState
	{
		GAME_NORMAL,
		GAME_MOTHER_QTE,
		GAME_FATHER_QTE
	};

	enum SubMomGameState
	{
		SUB_MOM_MOVE_TOWARDS,
		SUB_MOM_REACHED,
		SUB_MOM_FINISHING,
		SUB_MOM_GET_HIT
	};

	enum SubDadGameState
	{
		SUB_DAD_MOVE_TOWARDS,
		SUB_DAD_REACHED,
		SUB_DAD_FINISHING,
		SUB_DAD_GET_HIT
	};

	public enum ArrowKeysPressed
	{
		KEYS_NONE,
		KEYS_LEFT,
		KEYS_RIGHT,
		KEYS_UP,
		KEYS_DOWN
	}

	GameState CurrentState = GameState.GAME_NORMAL;
	SubMomGameState SubMomCurrentState = SubMomGameState.SUB_MOM_MOVE_TOWARDS;
	SubDadGameState SubDadCurrentState = SubDadGameState.SUB_DAD_MOVE_TOWARDS;

	public GameState currentGameState{get{return CurrentState;}}
	public GameOverManager GameOverObject;


	void Start()
	{
		MotherQTEManager.StartQTE(MotherQTENumber);
		FatherQTEManager.StartQTE(FatherQTENumber);
	}

	void Update()
	{
		GameUpdate();
	}
	
	void GameUpdate()
	{
		switch(CurrentState)
		{
		case GameState.GAME_NORMAL:
			NormalGameUpdate();
			break;
		case GameState.GAME_MOTHER_QTE:
			MotherGameUpdate();
			break;
		case GameState.GAME_FATHER_QTE:
			FatherGameUpdate();
			break;
		}
	}

	void NormalGameUpdate()
	{
		if(!IfMiss)
		{
			ArrowKeysPressed CurrentKeyPressed = NormalControlsUpdate();
			List<BasicBull> TempList = null;
			bool? IfPressRight = null;
			switch (CurrentKeyPressed)
			{
				case ArrowKeysPressed.KEYS_LEFT:
					TempList = ListOfBullLeft;
					IfPressRight = false;
					break;
				case ArrowKeysPressed.KEYS_RIGHT:
					TempList = ListOfBullRight;
					IfPressRight = true;
					break;
			}

            // Use skill
			if(CurrentKeyPressed == ArrowKeysPressed.KEYS_NONE)
            {
            	if (Input.GetKeyDown(KeyCode.Z))
            	{
                	Player_Bear.Instance.UseSkill();
            	}
				else if(Input.GetKeyDown(KeyCode.X))
				{
					Player_Bear.Instance.UseCharge();	
				}
            }

            if (TempList == null)
			{
			   
			}
			else 
			{
				if (TempList.Count > 0)
				{
					BearPunchBull(TempList);
				}
				else
				{
					if(Player_Bear.Instance.GetBearState() == Player_Bear.BearState.BEAR_NONE)
					{
						Player_Bear.Instance.SetBearMiss((bool)IfPressRight);
						MissTimeLeft =  ConstMissTime;
						IfMiss = true;
					}
				}
			}
		}
		else
		{
			MissTimeLeft -= TimeManager.Instance.GetGameDeltaTime();
			if(MissTimeLeft <=0)
			{
				Player_Bear.Instance.StopMissing();
				IfMiss = false;
			}

		}
	}

	void MotherGameUpdate()
	{
		switch(SubMomCurrentState)
		{
			case SubMomGameState.SUB_MOM_MOVE_TOWARDS:
				float CurrentAlpha = (Player_Bear.Instance.lerpTimeLeft/Player_Bear.Instance.ConstLerpDadTime) * ConstMaximumBlackAlpha;

				FirstBlackBackground.color = new Color(0,0,0, CurrentAlpha);
				SecondBlackBackground.color = new Color(0,0,0, CurrentAlpha);

				break;
			case SubMomGameState.SUB_MOM_REACHED:
				ArrowKeysPressed CurrentKeyPressed = MotherControlUpdate();
				int IfLastKey = 2;  
				if(CurrentKeyPressed != ArrowKeysPressed.KEYS_NONE)
					IfLastKey = MotherQTEManager.QTEButtonPressed(CurrentKeyPressed);

				TimeLeftForQTE -= TimeManager.Instance.GetGameDeltaTime();

				if(IfLastKey == 1)
				{
					ResetMomQTE();

					Player_Bear.Instance.FinishMom();
					BullGetPunched(Player_Bear.Instance.targetedBull);

                    // Give the player a Power up
                    Player_Bear.Instance.SetSkill(SkillManager.Instance.SpawnRandomSkill());

                    CurrentState = GameState.GAME_NORMAL;
				}
				else if(IfLastKey == -1 ||  TimeLeftForQTE < 0)
				{
					ResetMomQTE();
					Player_Bear.Instance.BearGetsHit(Player_Bear.Instance.targetedBull.gameObject);
                    
                    CurrentState = GameState.GAME_NORMAL;
				}
				else if(IfLastKey == 0)
				{
					Player_Bear.Instance.BearAttack();
				}

				break;
			case SubMomGameState.SUB_MOM_FINISHING:
                break;

		}
	}

	void FatherGameUpdate()
	{
		switch(SubDadCurrentState)
		{
			case SubDadGameState.SUB_DAD_MOVE_TOWARDS:
				float CurrentAlpha = (Player_Bear.Instance.lerpTimeLeft/Player_Bear.Instance.ConstLerpDadTime) * ConstMaximumBlackAlpha;

				FirstBlackBackground.color = new Color(0,0,0, CurrentAlpha);
				SecondBlackBackground.color = new Color(0,0,0, CurrentAlpha);

				break;
			case SubDadGameState.SUB_DAD_REACHED:
				ArrowKeysPressed CurrentKeyPressed = FatherControlUpdate();
				int IfLastKey = 2;  
				if(CurrentKeyPressed != ArrowKeysPressed.KEYS_NONE)
					IfLastKey = FatherQTEManager.QTEButtonPressed(CurrentKeyPressed);

				TimeLeftForQTE -= TimeManager.Instance.GetGameDeltaTime();

				if(IfLastKey == 1)
				{
					ResetDadQTE();

					Player_Bear.Instance.FinishDad();
					BullGetPunched(Player_Bear.Instance.targetedBull);

					CurrentState = GameState.GAME_NORMAL;
				}
				else if(IfLastKey == -1 ||  TimeLeftForQTE < 0)
				{
					ResetDadQTE();
					Player_Bear.Instance.BearGetsHit(Player_Bear.Instance.targetedBull.gameObject);
					CurrentState = GameState.GAME_NORMAL;
				}
				else if(IfLastKey == 0)
				{
					Player_Bear.Instance.BearAttack();
				}

				break;
			case SubDadGameState.SUB_DAD_FINISHING:
				
				break;

		}
	}

	void ResetMomQTE()
	{
			FirstBlackBackground.color = new Color(0,0,0, 0);
			SecondBlackBackground.color = new Color(0,0,0,0);

			FirstBlackBackground.enabled = false;
			SecondBlackBackground.enabled = false;

			MotherQTENumber += 1;

			MotherQTEManager.HideKey();
			MotherQTEManager.StartQTE(MotherQTENumber);
	}

	void ResetDadQTE()
	{
			FirstBlackBackground.color = new Color(0,0,0, 0);
			SecondBlackBackground.color = new Color(0,0,0,0);

			FirstBlackBackground.enabled = false;
			SecondBlackBackground.enabled = false;

			FatherQTENumber += 1;

			FatherQTEManager.HideKey();
			FatherQTEManager.StartQTE(FatherQTENumber);
	}

	ArrowKeysPressed MotherControlUpdate()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			return ArrowKeysPressed.KEYS_LEFT;

		else if (Input.GetKeyDown(KeyCode.RightArrow))
			return ArrowKeysPressed.KEYS_RIGHT;

		else
			return ArrowKeysPressed.KEYS_NONE;
	}



	public void ReachedMom()
	{
		TimeLeftForQTE = 3.0f + (0.5f * MotherQTEManager.ListLength);
		SubMomCurrentState = SubMomGameState.SUB_MOM_REACHED;
		MotherQTEManager.ShowKey();
	}

	public void ReachedDad()
	{
		TimeLeftForQTE = 3.0f + (0.6f * FatherQTEManager.ListLength);
		SubDadCurrentState = SubDadGameState.SUB_DAD_REACHED;
		FatherQTEManager.ShowKey();
	}

	ArrowKeysPressed FatherControlUpdate()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			return ArrowKeysPressed.KEYS_LEFT;

		else if (Input.GetKeyDown(KeyCode.RightArrow))
			return ArrowKeysPressed.KEYS_RIGHT;

		else if (Input.GetKeyDown(KeyCode.UpArrow))
			return ArrowKeysPressed.KEYS_UP;

		else if (Input.GetKeyDown(KeyCode.DownArrow))
			return ArrowKeysPressed.KEYS_DOWN;

		else
			return ArrowKeysPressed.KEYS_NONE;
	}

	void BearPunchBull(List<BasicBull> TempList)
	{
		BasicBull TempBull = TempList[0];

		switch(TempBull.BullType)
		{
			case BasicBull.TypeOfBull.BULL_BABY:
				Player_Bear.Instance.SetBearAttackBaby(TempBull);
				BabyBull TempBabyBull = (BabyBull)TempBull;
				TempBabyBull.GetHit();
				break;
			case BasicBull.TypeOfBull.BULL_MOTHER:
				Player_Bear.Instance.SetBearAttackMother(TempBull);
				TempBull.GetHit();
				CurrentState = GameState.GAME_MOTHER_QTE;
				SubMomCurrentState = SubMomGameState.SUB_MOM_MOVE_TOWARDS;

				FirstBlackBackground.color = new Color(0,0,0, 0);
				SecondBlackBackground.color = new Color(0,0,0,0);

				FirstBlackBackground.enabled = true;
				SecondBlackBackground.enabled = true;
				break;

		case BasicBull.TypeOfBull.BULL_FATHER:
				Player_Bear.Instance.SetBearAttackFather(TempBull);
				TempBull.GetHit();
				CurrentState = GameState.GAME_FATHER_QTE;
				SubDadCurrentState = SubDadGameState.SUB_DAD_MOVE_TOWARDS;

				FirstBlackBackground.color = new Color(0,0,0, 0);
				SecondBlackBackground.color = new Color(0,0,0,0);

				FirstBlackBackground.enabled = true;
				SecondBlackBackground.enabled = true;

				break;
		}
	}

	ArrowKeysPressed NormalControlsUpdate()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			return ArrowKeysPressed.KEYS_LEFT;

		else if (Input.GetKeyDown(KeyCode.RightArrow))
			return ArrowKeysPressed.KEYS_RIGHT;
		else
			return ArrowKeysPressed.KEYS_NONE;
	}

	public void BearGetsHit(GameObject HittingBull)
	{
        if (HittingBull.GetComponent<BasicBull>().IsAlive)
        {
            HittingBull.GetComponent<BasicBull>().ThisBullList.Remove(HittingBull.GetComponent<BasicBull>());
            Destroy(HittingBull);
        }
	}

	public void AddBullInsideList(BasicBull EnteringBull)
	{
		if (!EnteringBull.IfFacingRight)
		{
			EnteringBull.ThisBullList = ListOfBullRight;
			ListOfBullRight.Add(EnteringBull);
		}
		else
		{
			EnteringBull.ThisBullList = ListOfBullLeft;
			ListOfBullLeft.Add(EnteringBull);
		}
	}

    public void AddBullAllList(BasicBull NewBull)
    {
        ListofAllBulls.Add(NewBull);
    }

	public void RemoveBullFromList(BasicBull ExitingBull)
	{
		if (!ExitingBull.IfFacingRight)
		{
			ExitingBull.ThisBullList = ListOfBullRight;
			ListOfBullRight.Remove(ExitingBull);
		}
		else
		{
			ExitingBull.ThisBullList = ListOfBullLeft;
			ListOfBullLeft.Remove(ExitingBull);
		}

        
    }

	public void BullGetPunched(BasicBull TargetBull)
	{
        if (TargetBull.IsAlive)
        {
            TargetBull.StartKillSequence();

            ListofAllBulls.Remove(TargetBull);

            OneBullGetKilled(TargetBull);

            switch(TargetBull.bullType)
            {
				case BasicBull.TypeOfBull.BULL_BABY:
				SoundManager.Instance.PlayEffect(SoundManager.Effects.EFFECT_BABY_MOO);
				Player_Bear.Instance.IncreaseCharge(1);
				break;	
			case BasicBull.TypeOfBull.BULL_MOTHER:
				SoundManager.Instance.PlayEffect(SoundManager.Effects.EFFECT_MOTHER_MOO);
				Player_Bear.Instance.IncreaseCharge(1);
				break;
			case BasicBull.TypeOfBull.BULL_FATHER:
				SoundManager.Instance.PlayEffect(SoundManager.Effects.EFFECT_FATHER_MOO);
				Player_Bear.Instance.IncreaseFatherCharge();
				break;
            }
        }
	}

	public void OneBullGetKilled(BasicBull TargetBull)
	{
		++Multiplier;

		switch(TargetBull.bullType)
		{
			case BasicBull.TypeOfBull.BULL_BABY:
				Score += 1 * Multiplier;
				break;
			case BasicBull.TypeOfBull.BULL_MOTHER:
				Score += 10 * Multiplier;
				break;	
			case BasicBull.TypeOfBull.BULL_FATHER:
				Score += 25 * Multiplier;
				break;	
		}

		UpdateScoreAndMultiplier();
	}

	void UpdateScoreAndMultiplier()
	{
		ScoreText.text = Score.ToString();
		MultiplierText.text = "x" + Multiplier;
	}

	public void ResetMultiplier()
	{	
		Multiplier = 0;
		UpdateScoreAndMultiplier();
	}

    public void KillAllBulls()
    {
        foreach (var bull in ListofAllBulls)
        {
            bull.StartKillSequence();
        }

        // Clear it from the all list
        ListofAllBulls.Clear();
    }

    public void GameOver()
    {
 		GameOverObject.enabled = true;
    	this.enabled = false;
    }
}
