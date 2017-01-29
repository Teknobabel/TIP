using UnityEngine;
using System.Collections;

public class MenuState_Pause : MenuState {

	public Transform m_pauseMenu;

	public override void OnActivate()
	{
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
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {

			GameManager.instance.PopMenuState ();
		}
	}

	public override void BackButtonPressed (){

		GameManager.instance.PopMenuState ();
	}

	public void QuitButtonPressed () {

		Application.Quit ();
	}
}
