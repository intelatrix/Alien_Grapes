using UnityEngine;
using System.Collections;

public class BarCollider : MonoBehaviour {

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
}
