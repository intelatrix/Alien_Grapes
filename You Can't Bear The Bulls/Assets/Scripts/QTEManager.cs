using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour 
{
	
	List<QTEQue> ListOfQTE = new List<QTEQue>();
	public GameObject QTEPrefab;
	public int ListLength{get{return ListOfQTE.Count;}}
	public GameObject QueArea, LeftMost;
	public float ConstDistanceBetweenQues;

	public List<NamedImage> ListOfSprite;
	Dictionary<string, Sprite> DictionaryOfSprite = new Dictionary<string, Sprite>();

	void Start()
	{
		ConstDistanceBetweenQues *= (float)((float)Screen.width/(float)800);
		Debug.Log(Screen.width);
		foreach(NamedImage NewSprite in ListOfSprite)
		{
			DictionaryOfSprite.Add(NewSprite.name, NewSprite.image);
		}

		ListOfSprite = null;
	}

	public enum QTEType
	{
		QTE_MOTHER,
		QTE_FATHER
	}

	public QTEType ThisType;

	public int QTEButtonPressed(GameSceneManager.ArrowKeysPressed PressedKey)
	{
		if(ListOfQTE[0].TypeOfQTE == PressedKey)
		{
			Destroy(ListOfQTE[0].gameObject);
			ListOfQTE.RemoveAt(0);

			if(ListOfQTE.Count == 0)
			{
				return 1;
			}
			else
			{
				foreach(QTEQue Que in ListOfQTE)
				{
					Que.transform.Translate(new Vector3(-ConstDistanceBetweenQues,0,0));
				}	
				return 0;
			}
		}

		return -1;
	}

	public void StartQTE(int NumberOfKeys)
	{
		foreach(QTEQue Que in ListOfQTE)
		{
			Destroy(Que.gameObject);
		}	

		ListOfQTE.Clear();
		StartCoroutine(GenerateQTE(NumberOfKeys));
	}

	public void ShowKey()
	{
		QueArea.SetActive(true);
		LeftMost.SetActive(true);
	}

	public void HideKey()
	{
		QueArea.SetActive(false);
		LeftMost.SetActive(false);
	}

	void NewKey()
	{
		switch(ThisType)
			{
				case QTEType.QTE_MOTHER:
				{
					int RandomNumber = Random.Range(0,2);

					Vector3 DistanceFromLeft = new Vector3(LeftMost.transform.position.x + ListOfQTE.Count *ConstDistanceBetweenQues, LeftMost.transform.position.y, 0);

					GameObject NewQTE = Instantiate(QTEPrefab, DistanceFromLeft, Quaternion.identity) as GameObject;

					QTEQue NewQTEQue = NewQTE.GetComponent<QTEQue>();
					ListOfQTE.Add(NewQTEQue);
					//NewQTE.transform.parent = QueArea.transform;
					NewQTE.transform.SetParent(QueArea.transform);
					NewQTE.transform.localScale = new Vector3(1,1,1);

					switch(RandomNumber)
					{
						case 0:
						NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_LEFT;
						NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Left"];
						break;

						case 1:
						NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_RIGHT;
						NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Right"];
						break;
					}
				}
					break;
				case QTEType.QTE_FATHER:
					{
						int RandomNumber = Random.Range(0,4);

						Vector3 DistanceFromLeft = new Vector3(LeftMost.transform.position.x + ListOfQTE.Count *ConstDistanceBetweenQues, LeftMost.transform.position.y, 0);

						GameObject NewQTE = Instantiate(QTEPrefab, DistanceFromLeft, Quaternion.identity) as GameObject;

						QTEQue NewQTEQue = NewQTE.GetComponent<QTEQue>();
						ListOfQTE.Add(NewQTEQue);

						//NewQTE.transform.parent = QueArea.transform;
						NewQTE.transform.SetParent(QueArea.transform);
						NewQTE.transform.localScale = new Vector3(1,1,1);

						switch(RandomNumber)
						{
							case 0:
							NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_LEFT;
						NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Left"];
							break;

							case 1:
							NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_RIGHT;
						NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Right"];
							break;

							case 2:
							NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_UP;
							NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Up"];
							NewQTE.transform.Translate(new Vector3(0,15,0));
							break;

							case 3:
							NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_DOWN;
							NewQTE.GetComponent<Image>().sprite = DictionaryOfSprite["Down"];
							NewQTE.transform.Translate(new Vector3(0,-15,0));
							break;
						}
					}
					break;
			}
	}

	IEnumerator GenerateQTE(int NumberOfKeys)
	{
		while(NumberOfKeys != 0)
		{
			NewKey();
			--NumberOfKeys;
		}
		yield break;
	}
}
