﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MenuState_Pause : MenuState {

	public Transform m_pauseMenu;

	public TextMeshProUGUI m_versionNumber;

	public Slider m_musicVolumeSlider;
	public Slider m_sfxVolumeSlider;

	private bool m_inputState = false;

	public override void OnActivate()
	{
		m_inputState = MenuState_GameState.instance.playerInputAllowed;
		Time.timeScale = 0.0f;

		MenuState_GameState.instance.playerInputAllowed = false;

		m_pauseMenu.gameObject.SetActive (true);

		m_versionNumber.text = GameManager.instance.versionNumber;

		// set volume slider values

		m_musicVolumeSlider.value = AudioManager.instance.musicVolume;
		m_sfxVolumeSlider.value = AudioManager.instance.sfxVolume;
	}

	public override void OnHold()
	{
		m_pauseMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_pauseMenu.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_pauseMenu.gameObject.SetActive (false);

//		MenuState_GameState.instance.playerInputAllowed = m_inputState;

		if (m_inputState == true) {

			StartCoroutine (MenuState_GameState.instance.EnableInput ());
		}

		Time.timeScale = 1.0f;

		if (!GameManager.instance.m_demoMode) {

			PlayerPrefs.SetFloat ("MusicVolume", AudioManager.instance.musicVolume);
			PlayerPrefs.SetFloat ("SFXVolume", AudioManager.instance.sfxVolume);

			if (Screen.fullScreen) {

				PlayerPrefs.SetInt ("FullscreenState", 0);

			} else {

				PlayerPrefs.SetInt ("FullscreenState", 1);
			}

			PlayerPrefs.SetInt ("ResolutionState_Width", Screen.width);
			PlayerPrefs.SetInt ("ResolutionState_Height", Screen.height);

			PlayerPrefs.Save ();
		}
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {

			GameManager.instance.PopMenuState ();
		}
	}

	public override void BackButtonPressed (){

		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		GameManager.instance.PopMenuState ();
	}

	public void QuitButtonPressed () {

		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		Application.Quit ();
	}

	public void DonateButtonPressed () {

		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		Application.OpenURL ("https://action.aclu.org/secure/donate-to-aclu");
	}


}
