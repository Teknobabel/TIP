using UnityEngine;
using System.Collections;
using TMPro;

public class MenuState_GameOver : MenuState {

	public Transform 
		m_gameOverMenu,
		m_gameOverScene,
		m_statPanel;

	public TextMeshProUGUI
		m_gameOverText;

	public override void OnActivate()
	{
		m_gameOverMenu.gameObject.SetActive (true);
		m_gameOverScene.gameObject.SetActive (true);

		string gameOverText = "";

		for (int i=0; i < MenuState_GameState.instance.stats.Count; i++)
		{
			Stat s = MenuState_GameState.instance.stats [i];
			GameObject uiOBJ = (GameObject) Instantiate(MenuState_GameState.instance.m_statOBJ, m_statPanel);
			StatUI st = (StatUI)uiOBJ.GetComponent<StatUI> ();
			st.m_stat = s;
			st.m_icon.texture = s.m_icon;
			st.UpdateStatValue (s.currentScore);

			if (s.currentScore == 0) {

				st.SetColor (Color.red);
				gameOverText += s.m_losingStrings[Random.Range(0, s.m_losingStrings.Length)];

			} else if (s.currentScore == s.maxScore) {

				st.SetColor (Color.red);
				gameOverText += s.m_winningStrings[Random.Range(0, s.m_winningStrings.Length)];

			}
		}


		int years = 0;
		int months = 0;

		int turn = MenuState_GameState.instance.turnNumber + MenuState_GameState.instance.previousTermTurns;

		while (turn > 11) {

			turn -= 11;
			years++;
		}

		months = turn;

		gameOverText = "\nI survived ";

		if (years > 0) {

			gameOverText += years.ToString () + " years ";

			if (months > 0) {
				gameOverText += " and ";
			}
		}

		if (months > 0) {

			gameOverText += months.ToString () + " months ";
		}

		gameOverText += "before #TheIdiotPresident ";

		gameOverText += "destroyed the world.";

		m_gameOverText.text = gameOverText;

	}

	public override void OnHold()
	{
		m_gameOverMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_gameOverMenu.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_gameOverMenu.gameObject.SetActive (false);
		m_gameOverScene.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void PlayAgain ()
	{
		MenuState_GameState.instance.RestartGame ();
		GameManager.instance.PopMenuState ();
	}

	public void QuitButtonPressed () {

		Application.Quit ();
	}
}
