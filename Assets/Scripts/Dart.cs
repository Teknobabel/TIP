using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

	public Animation m_anim;

	public Collider m_collider;

	public Transform m_trail;

	public Word.WordType m_dartType = Word.WordType.None;

	// Use this for initialization
	void Start () {
	
	}

	public void DartThrow ()
	{
		m_trail.gameObject.SetActive (true);
	}

	public void DartLand ()
	{
		m_collider.enabled = false;
		m_anim.Play ();
//		m_trail.gameObject.SetActive (false);
	}

	public void DartMiss ()
	{

	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
