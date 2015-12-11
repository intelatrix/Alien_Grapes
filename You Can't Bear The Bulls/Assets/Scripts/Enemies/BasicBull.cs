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

	public TypeOfBull BullType;

	public bool IfFacingRight = true;

}
