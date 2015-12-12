using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTEManager : MonoBehaviour 
{
	List<QTEQue> ListOfQTE = new List<QTEQue>();
	public GameObject QTEPrefab;

	public enum QTEType
	{
		QTE_MOTHER,
		QTE_FATHER
	}

	public QTEType ThisType;

	public void StartQTE(int NumberOfKeys)
	{
		StartCoroutine(GenerateQTE(NumberOfKeys));
	}

	void NewKey()
	{
		switch(ThisType)
			{
				case QTEType.QTE_MOTHER:
					int RandomNumber = Random.Range(0,2);
					GameObject NewQTE = Instantiate(QTEPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					QTEQue NewQTEQue = NewQTE.GetComponent<QTEQue>();
					ListOfQTE.Add(NewQTEQue);
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
