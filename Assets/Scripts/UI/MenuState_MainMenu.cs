using UnityEngine;
using System.Collections;

public class MenuState_MainMenu : MenuState {

	public Transform m_mainMenu;

	public override void OnActivate()
	{
		m_mainMenu.gameObject.SetActive (true);
	}

	public override void OnHold()
	{
		m_mainMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_mainMenu.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_mainMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}
}
