//--------------------------------------------------------
//                         Minox
// Copyright © UberGamers 2012-2013. All rights reserved.
//--------------------------------------------------------
using UnityEngine;

/// <summary>
/// Generic Based Singleton for MonoBehaviours
/// Adapted from http://wiki.unity3d.com/index.php?title=Singleton
/// </summary>

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	static private bool exists_ = false;
	static public bool exists{
		get{
			return exists_;
		}
	}
	private static T instance = null;
	
	public static T Instance {
		get {
			// Instance required for the first time, we look for it.
			if (instance == null) {
				T[] instances = GameObject.FindObjectsOfType (typeof(T)) as T[];
				if (instances.Length > 1) {
					Debug.LogError ("More than 1 instance of " + typeof(T) + " was found in the scene, but only 1 is allowed.");
				} else if (instances.Length < 1) {
					Debug.LogError ("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
				} else {
					instance = instances [0];
					instance.Init ();
				}
			}
			return instance;
		}
	}
	
	// If no other monobehaviour requests the instance in an Awake function
	// executing before this one, no need to search for the object.
	void Awake ()
	{
		if (instance == null) {
			exists_ = true;
			instance = this as T;
			instance.Init ();
		}
	}
	
	// This function is called when the instance is used for the first time.
	// Put all the initializations you need here, as you would do in Awake.
	public virtual void Init ()
	{
	}
	
	// Make sure the instance isn't referenced anymore when the gameObject is destroyed.
	void OnDestroy ()
	{
		if(instance == this)
		{
			instance = null;
			exists_ = false;
		}
	}
 
	// Make sure the instance isn't referenced anymore when the user quit, just in case.
	void OnApplicationQuit ()
	{
		instance = null;
	}
}
