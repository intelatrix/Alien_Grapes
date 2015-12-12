using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBull : MonoBehaviour 
{

	public List<BasicBull> ThisBullList = null;
	
	public enum TypeOfBull
	{
		BULL_BABY,
		BULL_MOTHER,
		BULL_FATHER
	}

	// Update is called once per frame
	void Update()
	{
		if(!IfGetHit)
		{
			if (IfFacingRight)
				transform.position += new Vector3(1,0,0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
			else
				transform.position += new Vector3(-1,0,0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
		}
	}

	public void Launch(TypeOfBull type, Vector3 spawnPos, float moveSpeed, bool faceRight)
	{
		bullType = type;
		transform.position = spawnPos;
		movementSpeed = moveSpeed;
		ifFacingRight = faceRight;
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
