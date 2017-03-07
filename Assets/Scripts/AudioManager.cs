using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;

	public enum SoundType
	{
		None,
		Dart_Flight,
		Dart_HitTarget,
		Dart_MissTarget,
		Target_Flip,
		Tweet_Sent,
		Phone_Raise,
		Democracy_Increase,
		Democracy_Decrease,
		Capitalism_Increase,
		Capitalism_Decrease,
		Law_Increase,
		Law_Decrease,
		Culture_Increase,
		Culture_Decrease,
		Curtain_Raise,
		GameOver_Sting,
		Button_Click,
		Character_Vocalize,
		Character_VocalizePositive,
		Character_VocalizeNegative,
		Character_VocalizeRandom,
		GameStart,
		Reelection,
	}

	public AudioSource m_sfxSource;
	public AudioSource m_musicSource;

	public AudioClip[] m_dartFly;
	public AudioClip[] m_dartHit;
	public AudioClip[] m_targetFlip;
	public AudioClip[] m_tweetSent;
	public AudioClip[] m_phoneRaise;
	public AudioClip[] m_vocalizations_Positive;
	public AudioClip[] m_vocalizations_Negative;
	public AudioClip[] m_vocalizations_Random;
	public AudioClip m_democracy;
	public AudioClip m_capitalism;
	public AudioClip m_law;
	public AudioClip m_culture;
	public AudioClip m_democracyDecrease;
	public AudioClip m_capitalismDecrease;
	public AudioClip m_lawDecrease;
	public AudioClip m_cultureDecrease;
	public AudioClip m_curtainRaise;
	public AudioClip m_gameOverSting;
	public AudioClip m_gameOverMusic;
	public AudioClip m_buttonClick;
	public AudioClip m_reelectionSting;

	private float m_sfxVolume = 0.5f;
	private float m_musicVolume = 0.5f;

	void Awake ()
	{
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start ()
	{
		if (!GameManager.instance.m_demoMode) {
			m_musicVolume = PlayerPrefs.GetFloat ("MusicVolume");
			m_sfxVolume = PlayerPrefs.GetFloat ("SFXVolume");
		}

		MusicVolumeChanged (m_musicVolume);
		SFXVolumeChanged (m_sfxVolume);
	}

	public void MusicVolumeChanged (float newVolume)
	{
		m_musicVolume = newVolume;
		m_musicSource.volume = m_musicVolume;
	}

	public void SFXVolumeChanged (float newVolume)
	{
		m_sfxVolume = newVolume;
	}

	public void PlaySound (SoundType s)
	{
		AudioClip a = null;
		float volume = 1.0f;

		switch (s)
		{
		case SoundType.Dart_Flight:

			a = m_dartFly [Random.Range (0, m_dartFly.Length)];
			volume = m_sfxVolume * 0.3f;

			break;
		case SoundType.Dart_HitTarget:

			a = m_dartHit [Random.Range (0, m_dartHit.Length)];
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Target_Flip:

			a = m_targetFlip [Random.Range (0, m_targetFlip.Length)];
			volume = m_sfxVolume;

			break;
		case SoundType.Tweet_Sent:

			a = m_tweetSent [Random.Range (0, m_tweetSent.Length)];
			volume = m_sfxVolume;

			break;

		case SoundType.Reelection:

			a = m_reelectionSting;
			volume = m_sfxVolume;

			break;

		case SoundType.Character_VocalizePositive:

			a = m_vocalizations_Positive [Random.Range (0, m_vocalizations_Positive.Length)];
			volume = m_sfxVolume*0.2f;

			break;
		case SoundType.Character_VocalizeNegative:

			a = m_vocalizations_Negative [Random.Range (0, m_vocalizations_Negative.Length)];
			volume = m_sfxVolume*0.2f;

			break;
		case SoundType.Character_VocalizeRandom:

			a = m_vocalizations_Random [Random.Range (0, m_vocalizations_Random.Length)];
			volume = m_sfxVolume*0.2f;

			break;
		case SoundType.Democracy_Decrease:

			a = m_democracyDecrease;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Democracy_Increase:

			a = m_democracy;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Capitalism_Decrease:

			a = m_capitalismDecrease;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Capitalism_Increase:

			a = m_capitalism;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Law_Decrease:

			a = m_lawDecrease;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Law_Increase:

			a = m_law;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Culture_Decrease:

			a = m_cultureDecrease;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Culture_Increase:

			a = m_culture;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Button_Click:

			a = m_buttonClick;
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Curtain_Raise:

			a = m_curtainRaise;
			volume = m_sfxVolume * 0.1f;

			break;
		case SoundType.GameOver_Sting:

			a = m_gameOverSting;
			m_musicSource.Stop ();
			m_sfxSource.PlayOneShot (m_gameOverMusic, m_musicVolume);

			break;

		case SoundType.GameStart:
			
			m_musicSource.Play ();

			break;
		}

		if (a != null) {

			m_sfxSource.PlayOneShot (a, volume);
		}
	}

	public float musicVolume {get{return m_musicVolume;}}
	public float sfxVolume {get{return m_sfxVolume;}}
}
