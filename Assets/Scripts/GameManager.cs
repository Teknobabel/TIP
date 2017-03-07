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

	public bool 
	m_skipIntro = false,
	m_demoMode = true,
	m_resetPlayerPrefs = false;


	// Bank of scriptableobjects for game to pull from each round

	public Word[] m_verbs;
	public Word[] m_nouns;

	public MenuState[] m_menuStates;

	public Transform[] m_shiftableObjectTransforms;
	public RawImage[] m_curtains;
	public Texture m_4by3Curtains;
	public Texture m_16by9Curtains;
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

		if (m_resetPlayerPrefs) {

			PlayerPrefs.DeleteAll ();
		}

		if (!m_demoMode) {



			// initialize player prefs entries if not present

			bool savePrefs = false;

			if (!PlayerPrefs.HasKey ("Help_Gameplay")) {
				PlayerPrefs.SetInt ("Help_Gameplay", 0);
				savePrefs = true;
			}

			if (!PlayerPrefs.HasKey ("Help_Phone")) {
				PlayerPrefs.SetInt ("Help_Phone", 0);
				savePrefs = true;
			}

//			if (!PlayerPrefs.HasKey ("FullscreenState")) {
//				PlayerPrefs.SetInt ("FullscreenState", 0);
//				savePrefs = true;
//			}
//
//			if (!PlayerPrefs.HasKey ("ResolutionState_Width")) {
//				PlayerPrefs.SetInt ("ResolutionState_Width", Screen.width);
//				savePrefs = true;
//			}
//
//			if (!PlayerPrefs.HasKey ("ResolutionState_Height")) {
//				PlayerPrefs.SetInt ("ResolutionState_Height", Screen.height);
//				savePrefs = true;
//			}

			if (!PlayerPrefs.HasKey ("MusicVolume")) {
				PlayerPrefs.SetFloat ("MusicVolume", 0.5f);
				savePrefs = true;
			}

			if (!PlayerPrefs.HasKey ("SFXVolume")) {
				PlayerPrefs.SetFloat ("SFXVolume", 0.5f);
				savePrefs = true;
			}

			if (savePrefs) {

				PlayerPrefs.Save ();
			}


		}
	}

	void Start () {

//		#if !UNITY_IOS
//		int w = PlayerPrefs.GetInt ("ResolutionState_Width");
//		int h = PlayerPrefs.GetInt ("ResolutionState_Height");
//		int f = PlayerPrefs.GetInt ("FullscreenState");
//
//		bool fullscreen = true;
//
//		if (f == 1) {
//			fullscreen = false;
//		}
//
//		if (w != Screen.width || h != Screen.height) {
//			Screen.SetResolution (w, h, fullscreen);
//		}
//
//		Debug.Log(w.ToString() + " x " + h.ToString() + " / " + f.ToString());
//		#endif

		DOTween.Init ();

		if (!m_skipIntro) {

			float w = Screen.width;
			float h = Screen.height;

			if (w / h < 1.5f) {

				m_titleText.fontSize = 64.0f;
			}

			PushMenuState (MenuState.State.MainMenu);
		} else {
			PushMenuState (MenuState.State.GameState);
		}
	}

	public void UpdatedPositionByResolution ()
	{
		float w = Screen.width;
		float h = Screen.height;
//		Debug.Log (w / h);
		if (w / h < 1.5f) {

			// move to 4x3 positions

			m_shiftableObjectTransforms [0].position = new Vector3 (-6.4f, -1.8f, 3.84f);
			m_shiftableObjectTransforms [1].position = new Vector3 (-6.4f, 2.9f, 3.84f);

			m_shiftableObjectTransforms [2].position = new Vector3 (-2.5f, 3.31f, 0.0f);
			m_shiftableObjectTransforms [3].position = new Vector3 (-1.9f, -0.74f, 0.0f);

			m_shiftableObjectTransforms [4].position = new Vector3 (5.7f, 3.8f, 0.0f);
			m_shiftableObjectTransforms [5].position = new Vector3 (5.7f, -0.2f, 0.0f);

//			foreach (RawImage r in m_curtains) {
//
//				r.texture = m_4by3Curtains;
//			}

		} else {

			// move to 16x9 positions

			m_shiftableObjectTransforms [0].position = new Vector3 (-8.39f, -1.8f, 3.84f);
			m_shiftableObjectTransforms [1].position = new Vector3 (-8.36f, 2.9f, 3.84f);

			m_shiftableObjectTransforms [2].position = new Vector3 (-1.51f, 3.21f, 0.0f);
			m_shiftableObjectTransforms [3].position = new Vector3 (-0.97f, -0.74f, 0.0f);

			m_shiftableObjectTransforms [4].position = new Vector3 (7.19f, 3.8f, 0.0f);
			m_shiftableObjectTransforms [5].position = new Vector3 (7.19f, -0.2f, 0.0f);

//			m_titleText.fontSize = 76.0f;
//
//			foreach (RawImage r in m_curtains) {
//
//				r.texture = m_16by9Curtains ;
//			}
		}
	}
		
//	public void SetFullscreenState (bool fullScreenTrue)
//	{
//		if (fullScreenTrue) {
//			Screen.fullScreen = Screen.fullScreen;
//
////			if (!GameManager.instance.m_demoMode) {
////				PlayerPrefs.SetInt ("FullscreenState", 0);
////			}
//		} else {
//			Screen.fullScreen = !Screen.fullScreen;
////			if (!GameManager.instance.m_demoMode) {
////				PlayerPrefs.SetInt ("FullscreenState", 1);
////			}
//		}
//	}

//	public void SetResolutionState (int w, int h, bool doSave)
//	{
//		Screen.SetResolution(w, h, Screen.fullScreen);
//	}


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

//	void OnApplicationQuit () {
//
//		#if !UNITY_EDITOR
//		PlayerPrefs.SetInt("Screenmanager Resolution Width", Screen.width);
//		PlayerPrefs.SetInt("Screenmanager Resolution Height", Screen.height);
//		PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
//		#endif
//	}

	public int newID {get{ m_newID++; return m_newID; }}
	public string versionNumber {get{ return m_versionNumber; }}
}
