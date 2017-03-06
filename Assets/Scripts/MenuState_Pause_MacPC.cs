using UnityEngine;
using System.Collections;
using TMPro;

public class MenuState_Pause_MacPC : MenuState {

	public Transform m_pauseMenu;

	public TextMeshProUGUI m_versionNumber;

	private bool m_inputState = false;

	public override void OnActivate()
	{
		m_inputState = MenuState_GameState.instance.playerInputAllowed;
		Time.timeScale = 0.0f;

		MenuState_GameState.instance.playerInputAllowed = false;

		m_pauseMenu.gameObject.SetActive (true);

		m_versionNumber.text = GameManager.instance.versionNumber;
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

	public void FullscreenButtonPressed () {

		int fullscreenState = PlayerPrefs.GetInt ("FullscreenState");

		if (fullscreenState == 0) {

			GameManager.instance.SetFullscreenState (false, true);

		} else if (fullscreenState == 1) {

			GameManager.instance.SetFullscreenState (true, true);
		}
	}
}