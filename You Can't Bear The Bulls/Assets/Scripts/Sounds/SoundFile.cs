using UnityEngine;
using System.Collections;

public class SoundFile : MonoBehaviour 
{
    //Audio's Type
    public enum A_TYPE
    {
        AUDIO_BGM,
        AUDIO_EFFECTS
    }
	
    AudioSource ThisSound;
    
    public A_TYPE AudioType;
	public SoundManager.BGM ThisBGM;
	public SoundManager.Effects ThisEffect;

	bool SoundPaused = false;
    //Initialisation
    void Start()
    {
		ThisSound = GetComponent<AudioSource>();
    }
    
    //Play
    public void Play()
    {
		if (ThisSound != null)
		{
			ThisSound.Play();
		}
    }

    //Stop
    public void Stop()
    {
		if (ThisSound != null)
        {
			ThisSound.Stop();
        }
    }

    //Set Volume
    public void SetVolume(float f_Vol)
    {
		if ( ThisSound != null)
			ThisSound.volume = f_Vol;
    }

	//Pause Sound
	public void Pause()
	{
		if (ThisSound != null && ThisSound.isPlaying && ThisEffect != SoundManager.Effects.EFFECT_CLICK)
		{
			ThisSound.Pause();
			SoundPaused = true;
		}
	}

	//Pause Sound
	public void Unpause()
	{
		if (ThisSound != null && SoundPaused)
		{
			ThisSound.Play();
			SoundPaused = false;
		}
	}
}
