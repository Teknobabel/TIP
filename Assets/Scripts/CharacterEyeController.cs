using UnityEngine;
using System.Collections;

public class CharacterEyeController : MonoBehaviour {

	public enum BlinkState {

		Blinking,
		NotBlinking,
	}

	public Transform[] m_eyeStates;

	private float 
	m_minBlinkTime = 1.0f,
	m_maxBlinkTime = 6.0f,
	m_blinkTimer = 0.0f,
	m_blinkTime = 0.0f,
	m_blinkDuration = 0.2f;

	private BlinkState m_blinkState = BlinkState.NotBlinking;

	// Use this for initialization
	void Start () {
		m_blinkTime = Random.Range (m_minBlinkTime, m_maxBlinkTime);
	}
	
	// Update is called once per frame
	void Update () {

		if (m_blinkState == BlinkState.NotBlinking) {
			
			m_blinkTimer += Time.deltaTime;

			if (m_blinkTimer > m_blinkTime) {

				m_blinkTimer = 0.0f;
				m_blinkState = BlinkState.Blinking;
				m_blinkTime = m_blinkDuration;

				m_eyeStates [0].gameObject.SetActive (false);
				m_eyeStates [1].gameObject.SetActive (true);

			}
		} else {

			m_blinkTimer += Time.deltaTime;

			if (m_blinkTimer > m_blinkTime) {

				m_blinkTimer = 0.0f;
				m_blinkState = BlinkState.NotBlinking;
				m_blinkTime = Random.Range (m_minBlinkTime, m_maxBlinkTime);

				m_eyeStates [0].gameObject.SetActive (true);
				m_eyeStates [1].gameObject.SetActive (false);

			}

		}
	
	}
}
