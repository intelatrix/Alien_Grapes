using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoSingleton<SoundManager>
{
	public enum BGM
	{
		BGM_NONE,
		BGM_BEAR
	}	
	
	public enum Effects
	{
		EFFECT_NONE,
		EFFECT_CLICK
	}
	
	public List<SoundFile> AudioList = new List<SoundFile>();
	public Dictionary<BGM, SoundFile> BGMDict = new Dictionary<BGM, SoundFile>();
	public Dictionary<Effects,SoundFile> EffectsDict = new Dictionary<Effects, SoundFile>();
	
	//Current playing BGM
	SoundFile CurrentBGM;
	
	//After Awake, the codes enter Start(). All gameobject should be initialise by now. 
	//Loading of the sounds 
	void Start()
	{
		foreach(SoundFile Audio in AudioList)
		{
			switch(Audio.AudioType)
			{
				case SoundFile.A_TYPE.AUDIO_BGM:
					BGMDict.Add(Audio.ThisBGM, Audio);
					break;
				
				case SoundFile.A_TYPE.AUDIO_EFFECTS:
					EffectsDict.Add(Audio.ThisEffect, Audio);
					break;
			}
		}
	}
		
	public void PlayBGM(BGM BGMType)
	{
		if(CurrentBGM != null)
		{
			//Allow only one BGM to play at a time
			CurrentBGM.Stop();
		}
		BGMDict[BGMType].Play();
		CurrentBGM = BGMDict[BGMType];
	}
	
	public void PlayEffect(Effects EffectType)
	{
		EffectsDict[EffectType].Play();
	}
	
	//Stop Playing All Music
	public void StopAllSound()
	{
		foreach (SoundFile Audio in AudioList)
		{
			Audio.Stop();
		}
	}
	
//	public void ChangeVolume()
//	{	
//		foreach (AudioFile Audio in AudioList)
//		{
//			Audio.SetVolume(VolumeBar.f_Vol);
//		}
//	}
	
	public void MuteBGM()
	{
		foreach (SoundFile Audio in BGMDict.Values)
		{
			Audio.SetVolume(0);
		}
	}
	
	public void StopBGM()
	{
		if(CurrentBGM != null)
		{
			CurrentBGM.Stop();
		}
	}

	public void PauseAllSounds()
	{
		foreach (SoundFile Audio in AudioList)
		{
			Audio.Pause();
		}
	}

	public void UnpauseAllSounds()
	{
		foreach (SoundFile Audio in AudioList)
		{
			Audio.Unpause();
		}
	}
}

