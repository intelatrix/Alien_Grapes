using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTEManager : MonoBehaviour 
{
	List<QTEQue> ListOfQTE = new List<QTEQue>();
	public GameObject QTEPrefab;

	public GameObject QueArea, LeftMost;
	public float ConstDistanceBetweenQues, ConstDistanceFromLeft; 

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
					int RandomNumber = Random.Range(0,2);

					Vector3 DistanceFromLeft = new Vector3(LeftMost.transform.position.x + ListOfQTE.Count *ConstDistanceBetweenQues + ConstDistanceFromLeft, LeftMost.transform.position.y, 0);

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
						break;

						case 1:
						NewQTEQue.TypeOfQTE = GameSceneManager.ArrowKeysPressed.KEYS_RIGHT;
						break;
					}

					break;
				case QTEType.QTE_FATHER:
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
