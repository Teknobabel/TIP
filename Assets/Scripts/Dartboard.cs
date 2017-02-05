using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dartboard : MonoBehaviour {

	public enum State
	{
		None,
		Active,
		Hit,
		Missed,
	}

	public float m_rotateSpeed = 50;

	public Target[] m_targets;

	public Transform m_rotationParent;

	public Animation m_targetFlipAnimation;

	public TextMesh m_resultText;

	private State m_state = State.Active;

	private bool m_flipped = false;

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
		Debug.Log ("toggleflip");
		if (!m_flipped && m_targetFlipAnimation != null) {

			m_flipped = true;
			m_targetFlipAnimation.Play ("Target_Flip01");

		} else if (m_flipped && m_targetFlipAnimation != null) {

			m_targetFlipAnimation.Play ("Target_Flip02");
			m_flipped = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (m_state == State.Active) {
			m_rotationParent.Rotate (Vector3.up * Time.deltaTime * m_rotateSpeed);
		}
	}

	public State currentState {get{ return m_state;}set{m_state = value;}}
	public bool flipped {get{ return m_flipped;}}
}
