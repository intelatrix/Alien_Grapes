using UnityEngine;
using System.Collections;

public class BasicBull : MonoBehaviour 
{
	public enum TypeOfBull
	{
		BULL_BABY,
		BULL_MOTHER,
		BULL_FATHER
	}

	public void Launch(TypeOfBull type, Vector3 spawnPos, float moveSpeed, bool faceRight)
	{
		bullType = type;
		transform.position = spawnPos;
		movementSpeed = moveSpeed;
		ifFacingRight = faceRight;
	}

	public TypeOfBull bullType;
	private float movementSpeed = 0.0f;
	public bool ifFacingRight = true;

	// Getter Fields
	public TypeOfBull BullType { get { return bullType; } }
	public bool IfFacingRight { get { return ifFacingRight; } }
	public float MovementSpeed { get { return movementSpeed; } }
}
