using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public bool m_skipIntro = false;

	public Word[] m_verbs;
	public Word[] m_nouns;

	public MenuState[] m_menuStates;

	private MenuState m_menuState = null;
	private List<MenuState> m_menuStateStack = new List<MenuState>();

	private int m_newID = 0;

	void Awake ()
	{
		Application.targetFrameRate = 60;

		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {

		PushMenuState (MenuState.State.GameState);
	}
		



	// Update is called once per frame
	void Update () {

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
}
