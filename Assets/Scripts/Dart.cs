using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

	public Animation m_anim;

	public Collider m_collider;

	// Use this for initialization
	void Start () {
	
	}

	public void DartLand ()
	{
		m_collider.enabled = false;
		m_anim.Play ();
	}

	public void DartMiss ()
	{

	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
