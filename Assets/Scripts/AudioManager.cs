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
	}

	public AudioSource m_sfxSource;
	public AudioSource m_musicSource;

	public AudioClip[] m_dartFly;
	public AudioClip[] m_dartHit;
	public AudioClip[] m_targetFlip;
	public AudioClip[] m_tweetSent;
	public AudioClip[] m_phoneRaise;
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

	private float m_sfxVolume = 1.0f;
	private float m_musicVolume = 1.0f;

	void Awake ()
	{
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	public void PlaySound (SoundType s)
	{
		AudioClip a = null;
//		float pitch = 1.0f;
		float volume = 1.0f;

		switch (s)
		{
		case SoundType.Dart_Flight:

//			pitch = 1.0f;
			a = m_dartFly [Random.Range (0, m_dartFly.Length)];
			volume = m_sfxVolume * 0.3f;

			break;
		case SoundType.Dart_HitTarget:

			a = m_dartHit [Random.Range (0, m_dartHit.Length)];
//			pitch = Random.Range (0.75f, 1.25f);
			volume = m_sfxVolume * 0.5f;

			break;
		case SoundType.Target_Flip:

//			pitch = 1.0f;
			a = m_targetFlip [Random.Range (0, m_targetFlip.Length)];
			volume = m_sfxVolume;

			break;
		case SoundType.Tweet_Sent:

//			pitch = 1.0f;
			a = m_tweetSent [Random.Range (0, m_tweetSent.Length)];
			volume = m_sfxVolume;

			break;
		case SoundType.Phone_Raise:

//			a = m_phoneRaise [Random.Range (0, m_phoneRaise.Length)];
//			m_sfxSource.PlayOneShot (a, m_sfxVolume * 0.05f);

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
		}

		if (a != null) {

//			m_sfxSource.pitch = pitch;
			m_sfxSource.PlayOneShot (a, volume);
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
