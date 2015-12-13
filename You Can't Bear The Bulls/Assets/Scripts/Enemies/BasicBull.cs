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

	// LifeCycle of the Bull
	private LifeCycle state = LifeCycle.Living;
	// Store rotation of the bull flying away
	private Vector3 spinDirection;
    private Vector3 gravity = new Vector3(0.0f, -1.0f);

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
					if (IfFacingRight)
					{
						transform.position += new Vector3(1, 0, 0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
					}
					else
					{
						transform.position += new Vector3(-1, 0, 0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
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

	public void Launch(TypeOfBull type, Vector3 spawnPos, float moveSpeed, bool faceRight)
	{
		bullType = type;
		transform.position = spawnPos;
		movementSpeed = moveSpeed;
		ifFacingRight = faceRight;

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

	public bool BullEntered = false;
}
