﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public static CharacterController instance;

	public enum BlinkState {

		Blinking,
		NotBlinking,
	}

	public Transform[] 
	m_eyeStates,
	m_heads;

	public Animation
	m_anim;

	private float 
	m_minBlinkTime = 1.0f,
	m_maxBlinkTime = 6.0f,
	m_blinkTimer = 0.0f,
	m_blinkTime = 0.0f,
	m_blinkDuration = 0.2f;

	private BlinkState m_blinkState = BlinkState.NotBlinking;
	private Transform m_currentHead = null;

	void Awake ()
	{
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start () {
		m_blinkTime = Random.Range (m_minBlinkTime, m_maxBlinkTime);
		m_currentHead = m_heads [0];
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

	public void UpdateFace ()
	{
		// set face based on distance to game over

		int distanceToGameOver = 100;
		m_currentHead.gameObject.SetActive (false);

		foreach (Stat s in MenuState_GameState.instance.stats) {

			int dist = 100;

			if (s.currentScore >= 50) {

				dist = 100 - s.currentScore;
			} else {
				dist = s.currentScore;
			}

			if (dist < distanceToGameOver) {

				distanceToGameOver = dist;
			}
		}

		if (distanceToGameOver >= 30) {
			m_currentHead = m_heads [0];
		} else if (distanceToGameOver >= 24) {
			m_currentHead = m_heads [1];
		} else if (distanceToGameOver >= 8) {
			m_currentHead = m_heads [4];
		} else {
			m_currentHead = m_heads [5];
		}

		m_currentHead.gameObject.SetActive (true);

		m_anim ["rig|rigAction"].speed = Random.Range (0.9f, 1.2f);
		m_anim.Play ();
	}

	public void DartsLanded ()
	{
//		m_currentHead.gameObject.SetActive (false);
//
//		List<Transform> tList = new List<Transform> ();
////		tList.Add (m_heads [0]);
////		tList.Add (m_heads [0]);
//		tList.Add (m_heads [0]);
//		tList.Add (m_heads [1]);
//		tList.Add (m_heads [2]);
//		tList.Add (m_heads [3]);
//		tList.Add (m_heads [4]);
//		tList.Add (m_heads [5]);
//
//		m_currentHead = tList [Random.Range(0, tList.Count)];
//		m_currentHead.gameObject.SetActive (true);

		m_currentHead.gameObject.SetActive (false);
		m_currentHead = m_heads[3];
		m_currentHead.gameObject.SetActive (true);

//		m_anim ["rig|rigAction"].time = 3.18f;
//		m_anim ["rig|rigAction"].speed = 0.0f;

		m_anim.Play ("rig|rigAction0");
//		Debug.Log (m_anim ["rig|rigAction"].time);

	}

	public void DartsThrown ()
	{
//		m_currentHead.gameObject.SetActive (false);
//		m_currentHead = m_heads[3];
//		m_currentHead.gameObject.SetActive (true);
	}
}