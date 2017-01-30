using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dartboard : MonoBehaviour {

	public float m_rotateSpeed = 50;

	public Target[] m_targets;

	public Transform m_rotationParent;

	public Animation m_targetFlipAnimation;

	public TextMesh m_resultText;

	// Use this for initialization
	void Start () {
	
	}

	public void SetNoun (List<Word> n)
	{
		for (int i = 0; i < m_targets.Length; i++) {

			if (i < n.Count) {

				m_targets [i].SetTarget (n [i]);
			}
		}
	}

	public void SetVerb (List<Word> v)
	{
		for (int i = 0; i < m_targets.Length; i++) {

			if (i < v.Count) {

				m_targets [i].SetTarget (v [i]);
			}
		}
	}

	public void ToggleTargetFlip ()
	{
		if (m_targetFlipAnimation != null) {
			m_targetFlipAnimation.Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		m_rotationParent.Rotate (Vector3.up * Time.deltaTime * m_rotateSpeed);
	}
}
