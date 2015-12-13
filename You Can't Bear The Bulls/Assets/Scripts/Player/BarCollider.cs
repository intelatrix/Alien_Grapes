using UnityEngine;
using System.Collections;

public class BarCollider : MonoBehaviour {

	SpriteRenderer CollidersSpriteRender;
	public Sprite[] BarSprites;

	void Start()
	{
		CollidersSpriteRender = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if(gameObject.name == "Left Collider")
		{
			if(GameSceneManager.Instance.SizeOfLeftList == 0)
			{
				CollidersSpriteRender.sprite = BarSprites[0];
			}
			else
			{
				CollidersSpriteRender.sprite= BarSprites[1];
			}
		}
		else if(gameObject.name == "Right Collider")
		{
			if(GameSceneManager.Instance.SizeOfRightList == 0)
			{
				CollidersSpriteRender.sprite = BarSprites[0];
			}
			else
			{
				CollidersSpriteRender.sprite = BarSprites[1];
			}
		}
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
    	if(collision.tag == "Bull")
    	{
	        Debug.Log("Bull Enter");   
	        BasicBull EnteringBull = collision.GetComponent<BasicBull>();

	        if(!EnteringBull.BullEntered)
	        {
	        	EnteringBull.BullEntered = true;
	        	GameSceneManager.Instance.AddBullInsideList(EnteringBull);
	        }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
    {
    	if(collision.tag == "Bull")
    	{
	        Debug.Log("Bull Exit");   
	        BasicBull ExitingBull = collision.GetComponent<BasicBull>();

	        if(ExitingBull.BullEntered)
	        {
	        	ExitingBull.BullEntered = false;
				GameSceneManager.Instance.RemoveBullFromList(ExitingBull);
	        }
        }
    }
}
