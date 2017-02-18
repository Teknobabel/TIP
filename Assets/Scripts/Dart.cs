using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

	public Animation m_anim;

	public Collider m_collider;

	public Transform m_trail;

	public Word.WordType m_dartType = Word.WordType.None;

	public void DartThrow ()
	{
		m_trail.gameObject.SetActive (true);
	}

	public void DartLand ()
	{
		m_collider.enabled = false;
		m_anim.Play ();

		AudioManager.instance.PlaySound (AudioManager.SoundType.Dart_HitTarget);

	}
}
