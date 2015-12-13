using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBull : MonoBehaviour 
{

	public List<BasicBull> ThisBullList = null;
	[Tooltip("The direction that this bull would be flown if killed on the right side. Will be normalized and multiplied by FlySpeed.")]
	public Vector3 FlyDirection;
    [Tooltip("The speed of the bull flying away.")]
	public float FlySpeed;
	[Tooltip("The speed of spin while the bull flies away.")]
	public float SpinSpeed;

	[Tooltip("Name + Sprites")]
	public List<NamedImage> ListOfSprite;
	[Tooltip("Base Name for the Bull Sprite Animation")]
	public string BullSpriteBaseName;
	[Tooltip("The Time between each frame of the bull")]
	public float TimeBetweenFrame;
	[Tooltip("Total Amount Of Frames")]
	public int ConstMaxFrame;
	int CurrentFrame = 0;
	float AnimationTimeLeft ;
	Dictionary<string, Sprite> DictionaryOfSprite = new Dictionary<string, Sprite>();



	// LifeCycle of the Bull
	private LifeCycle state = LifeCycle.Living;
	// Store rotation of the bull flying away
	private Vector3 spinDirection;
    private Vector3 gravity = new Vector3(0.0f, -1.0f);
    private SpriteRenderer BullRenderer;

    public enum TypeOfBull
	{
		BULL_BABY,
		BULL_MOTHER,
		BULL_FATHER
	}

	public enum LifeCycle
	{
		Living,
		Flying,
		Death
	}

	// Start is called once during instantiation
	void Start()
	{
		AnimationTimeLeft = TimeBetweenFrame;
		// Normalize the direction
		FlyDirection.Normalize();
		// Create the Vector that controls rotation while flying
		spinDirection = new Vector3(0.0f, 0.0f, SpinSpeed);
    }

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case LifeCycle.Living:
				if (!IfGetHit)
				{
					
					float QTEMultiplier;
					if(GameSceneManager.Instance.currentGameState == GameSceneManager.GameState.GAME_NORMAL)
						QTEMultiplier = 1; 
					else
						QTEMultiplier = 0.02f;

					if (IfFacingRight)
					{
						transform.position += new Vector3(1, 0, 0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime() * QTEMultiplier;
					}
					else
					{
						transform.position += new Vector3(-1, 0, 0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime()  * QTEMultiplier;
					}
				}
				break;
			case LifeCycle.Flying:
				if (transform.position.y > -20.0f && transform.position.y < 20)
				{
                    transform.position += (FlyDirection * FlySpeed * TimeManager.Instance.GetGameDeltaTime());
					transform.Rotate(spinDirection * TimeManager.Instance.GetGameDeltaTime());
                    FlyDirection += gravity * TimeManager.Instance.GetGameDeltaTime();
                    FlyDirection.Normalize();
				}
				else
				{
					state = LifeCycle.Death;
				}
				break;
			case LifeCycle.Death:
				Destroy(this.gameObject);
				break;
		}
	}

	void UpdateAnimation()
	{
		
	}

	public void Launch(TypeOfBull type, Vector3 spawnPos, float moveSpeed, bool faceRight)
	{
		bullType = type;
		transform.position = spawnPos;
		movementSpeed = moveSpeed;
		ifFacingRight = faceRight;

		BullRenderer = GetComponent<SpriteRenderer>();
		BullRenderer.flipX = IfFacingRight;

		// Y-Flip the FlyDirection if going from the left
		if (ifFacingRight)
		{
			FlyDirection.x = -FlyDirection.x;
			spinDirection.z = -spinDirection.z;
		}
	}
	
	public void StartKillSequence()
	{
		ThisBullList.Remove(this);
		state = LifeCycle.Flying;
	}
		
	public void GetHit()
	{
		IfGetHit = true;
	}

	public TypeOfBull bullType;
	private float movementSpeed = 0.0f;
	public bool ifFacingRight = true;
	public bool IfGetHit = false;

	// Getter Fields
	public TypeOfBull BullType { get { return bullType; } }
	public bool IfFacingRight { get { return ifFacingRight; } }
	public float MovementSpeed { get { return movementSpeed; } }
    public bool IsAlive { get { return state == LifeCycle.Living; } }

	public bool BullEntered = false;
}
