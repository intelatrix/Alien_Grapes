using UnityEngine;
using System.Collections;

public class Steam : MonoBehaviour {

	SpriteRenderer SteamSprite;

	public Sprite[] SteamSprites;

	float SteamTime = 0.2f;
	int Frame = 0;

	void Start()
	{
		SteamSprite = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		SteamTime -= TimeManager.Instance.GetGameDeltaTime();

		if(SteamTime < 0) 
		{
			SteamTime = 0.2f;
			if(Frame == 0)
			{
				Frame = 1;
				SteamSprite.sprite = SteamSprites[0];
			}
			else
			{
				Frame = 0;
				SteamSprite.sprite = SteamSprites[1];
			}
		}
	}
}
