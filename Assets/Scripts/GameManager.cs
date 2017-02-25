using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// Debug 

	public bool m_skipIntro = false;

	// Bank of scriptableobjects for game to pull from each round

	public Word[] m_verbs;
	public Word[] m_nouns;

	public MenuState[] m_menuStates;

	public Transform[] m_shiftableObjectTransforms;
	public RawImage[] m_curtains;
	public Texture m_4by3Curtains;
	public TextMeshProUGUI m_titleText;

	private MenuState m_menuState = null;
	private List<MenuState> m_menuStateStack = new List<MenuState>();

	private int m_newID = 0;

	private string m_versionNumber = "Version 0.1.3";

	void Awake ()
	{
		Application.targetFrameRate = 60;

		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start () {

		if (Camera.main.aspect < 1.7) {

			m_shiftableObjectTransforms [0].position = new Vector3 (-6.4f, -1.8f, 3.84f);
			m_shiftableObjectTransforms [1].position = new Vector3 (-6.4f, 2.9f, 3.84f);

			m_shiftableObjectTransforms [2].position = new Vector3 (-2.5f, 3.6f, 0.0f);
			m_shiftableObjectTransforms [3].position = new Vector3 (-1.9f, -0.2f, 0.0f);

			m_shiftableObjectTransforms [4].position = new Vector3 (5.7f, 3.8f, 0.0f);
			m_shiftableObjectTransforms [5].position = new Vector3 (5.7f, -0.2f, 0.0f);

			m_titleText.fontSize = 64.0f;

			foreach (RawImage r in m_curtains) {

				r.texture = m_4by3Curtains;
			}
		}

		DOTween.Init ();

		if (!m_skipIntro) {
			PushMenuState (MenuState.State.MainMenu);
		} else {
			PushMenuState (MenuState.State.GameState);
		}
	}
		



	// Update is called once per frame
	void Update () {
//		Debug.Log (MenuState_GameState.instance.m_playerInputAllowed);
		if (m_menuState != null) {
			m_menuState.OnUpdate ();
		}
	}

	public void PushMenuState (MenuState.State newState)
	{
		Debug.Log ("Push State: " + newState);

		MenuState menuState = null;
		foreach (MenuState m in m_menuStates) {
			if (m.state == newState) {
				menuState = m;
				break;
			}
		}

		if (m_menuState != null)
		{
			m_menuState.OnHold();
		}

		m_menuState = menuState;

		if (m_menuState != null)
		{
			m_menuStateStack.Add (m_menuState);
			m_menuState.OnActivate();
		}
	}

	public void PopMenuState ()
	{
		if (m_menuState)
		{
			Debug.Log("Pop State: " + m_menuState);
			//			m_previousState = m_gameState.state;
			m_menuState.OnDeactivate();
		}

		if (m_menuStateStack.Count > 1) {
			m_menuStateStack.RemoveAt(m_menuStateStack.Count-1);
			m_menuState = m_menuStateStack[m_menuStateStack.Count-1];

			if (m_menuState != null)
			{
				Debug.Log("New State: " + m_menuState);
				m_menuState.OnReturn();
			}
		}
	}

	public int newID {get{ m_newID++; return m_newID; }}
	public string versionNumber {get{ return m_versionNumber; }}
}
