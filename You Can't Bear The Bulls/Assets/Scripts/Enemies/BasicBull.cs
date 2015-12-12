using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBull : MonoBehaviour 
{

	public List<BasicBull> ThisBullList = null;
    [Tooltip("The direction that this bull would be flown if killed on the right side.")]
    public Vector3 FlyDirection;
    public float FlySpeed;
    private LifeCycle state = LifeCycle.Living;

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
    void Init()
    {
        // Normalize the direction
        FlyDirection.Normalize();
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
                if (transform.position.y < Screen.height)
                {
                    transform.Translate(FlyDirection * FlySpeed * TimeManager.Instance.GetGameDeltaTime());
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
