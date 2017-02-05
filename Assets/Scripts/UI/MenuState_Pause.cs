using UnityEngine;
using System.Collections;

public class MenuState_Pause : MenuState {

	public Transform m_pauseMenu;

	private bool m_inputState = false;

	public override void OnActivate()
	{
		m_inputState = MenuState_GameState.instance.playerInputAllowed;
		Time.timeScale = 0.0f;

		MenuState_GameState.instance.playerInputAllowed = false;

		m_pauseMenu.gameObject.SetActive (true);
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

		MenuState_GameState.instance.playerInputAllowed = m_inputState;
		Time.timeScale = 1.0f;
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {

			GameManager.instance.PopMenuState ();
		}
	}

	public override void BackButtonPressed (){

		GameManager.instance.PopMenuState ();
	}

	public void QuitButtonPressed () {

		Application.Quit ();
	}

	public void DonateButtonPressed () {

		Application.OpenURL ("https://action.aclu.org/secure/donate-to-aclu");
	}
}
