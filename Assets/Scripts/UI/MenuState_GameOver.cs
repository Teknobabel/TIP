using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MenuState_GameOver : MenuState {

	public GameObject
		m_timelineOBJ;

	public Transform 
		m_gameOverMenu,
		m_gameOverScene,
		m_statPanel,
		m_timelinePanel;

	public TextMeshProUGUI
		m_gameOverText,
		m_dateText;

	public Animation
	m_outro;

	private List<TimelineUI> m_timelinePips = new List<TimelineUI>();
	private List<StatUI> m_stats = new List<StatUI>();

	public override void OnActivate()
	{
		m_gameOverMenu.gameObject.SetActive (true);
//		m_gameOverScene.gameObject.SetActive (true);

		string gameOverText = "";

//		for (int i=0; i < MenuState_GameState.instance.stats.Count; i++)
//		{
//			Stat s = MenuState_GameState.instance.stats [i];
//			GameObject uiOBJ = (GameObject) Instantiate(MenuState_GameState.instance.m_statOBJ, m_statPanel);
//			StatUI st = (StatUI)uiOBJ.GetComponent<StatUI> ();
//			m_stats.Add (st);
//			st.m_stat = s;
//			st.m_icon.texture = s.m_icon;
//			st.UpdateStatValue (s.currentScore, false);
//
//			if (s.currentScore == 0) {
//
//				st.SetColor (Color.red);
//				gameOverText += s.m_losingStrings[Random.Range(0, s.m_losingStrings.Length)];
//
//			} else if (s.currentScore == s.maxScore) {
//
//				st.SetColor (Color.red);
//				gameOverText += s.m_winningStrings[Random.Range(0, s.m_winningStrings.Length)];
//			}
//		}


		int years = 0;
		int months = 0;

		int turn = MenuState_GameState.instance.turnNumber + MenuState_GameState.instance.previousTermTurns;

		while (turn > 11) {

			turn -= 11;
			years++;
		}

		months = turn;

		gameOverText += "\n\nI survived ";

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

//		UpdateTimeLine ();

		m_outro.gameObject.SetActive (true);
		m_outro.Play ();


	}

//	private void UpdateTimeLine ()
//	{
//		while (m_timelinePips.Count > 0) {
//
//			GameObject g = m_timelinePips [0].gameObject;
//			m_timelinePips.RemoveAt (0);
//			Destroy (g);
//
//		}
//
//		for (int i = 0; i < MenuState_GameState.instance.maxTurns; i++) {
//
//			GameObject uiOBJ = (GameObject) Instantiate(m_timelineOBJ, m_timelinePanel);
//			TimelineUI tUI = (TimelineUI)uiOBJ.GetComponent<TimelineUI> ();
//			m_timelinePips.Add (tUI);
//
//			if (i == MenuState_GameState.instance.turnNumber) {
//
//				tUI.SetState (TimelineUI.State.Present);
//			} else if (i < MenuState_GameState.instance.turnNumber) {
//
//				if ( i > 0 && i % 12 == 0) {
//					tUI.SetState (TimelineUI.State.Past_YearMark);
//				} else {
//					tUI.SetState (TimelineUI.State.Past);
//				}
//			} else {
//				if (i % 12 == 0) {
//					tUI.SetState (TimelineUI.State.Future_YearMark);
//				} else {
//					tUI.SetState (TimelineUI.State.Future);
//				}
//			}
//		}
//
//		m_dateText.text = MenuState_GameState.instance.GetDate (MenuState_GameState.instance.turnNumber).ToUpper();
//	}

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
		while (m_timelinePips.Count > 0) {

			GameObject g = m_timelinePips [0].gameObject;
			m_timelinePips.RemoveAt (0);
			Destroy (g);

		}

		while (m_stats.Count > 0) {

			GameObject g = m_stats [0].gameObject;
			m_stats.RemoveAt (0);
			Destroy (g);

		}

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
