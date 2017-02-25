using UnityEngine;
using System.Collections;

public class MenuState_MainMenu : MenuState {

	public Transform
	m_introPanel;

	public Animation
		m_intro;

	private bool m_introPlayed = false;

	// Use this for initialization
	void Start () {
	
	}

	public override void OnActivate()
	{
		m_introPanel.gameObject.SetActive (true);

		if (!Application.isShowingSplashScreen) {
			m_introPlayed = true;
			StartCoroutine (PlayIntro ());
		}
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{

	}

	private IEnumerator PlayIntro ()
	{
//		yield return new WaitForSeconds (2.0f);
		m_intro.Play ("Curtain_FadeIn01");
		yield return true;
	}

	public override void OnUpdate()
	{
		if (!m_introPlayed && !Application.isShowingSplashScreen) {

			m_introPlayed = true;
			StartCoroutine (PlayIntro ());
		}
		if (Input.anyKeyDown) {

			GameManager.instance.PushMenuState (MenuState.State.GameState);
		}
	}
}
