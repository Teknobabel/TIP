using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class MenuState_GameState : MenuState {
	public static MenuState_GameState instance;

	public enum PhoneState
	{
		None,
		Up,
		Down,
	}
	public enum Month
	{
		January,
		February,
		March,
		April,
		May,
		June,
		July,
		August,
		September,
		October,
		November,
		December,
	}

//	public enum Verb
//	{
//		None,
//		TargetMiss,
//		Love,
//		Hate,
//		Worship,
//		Kill,
//		Import,
//		Export,
//		Destroy,
//		Seize,
//		Ban,
//		Bully,
//		Grope,
//		Fondle,
//		Dismiss,
//		DeclareWarOn,
//		SurrenderTo,
//		SpyOn,
//		Deport,
//	}
//
//	public enum Noun
//	{
//		None,
//		TargetMiss,
//		Whites,
//		Blacks,
//		Jews,
//		Mexicans,
//		USA,
//		Russia,
//		China,
//		Poor,
//		Rich,
//		Tacos,
//		Burritos,
//		Congress,
//		TheMedia,
//		Democracy,
//		Abortion,
//		Climate,
//		Fraud,
//		Debt,
//		Communism,
//		TheCyber,
//		Law,
//		Anarchy,
//		Dinosaurs,
//		Terrorists,
//		Facts,
//		Lies,
//		Science,
//		Women,
//		Emails,
//	}

	public Transform m_gameUI;

	public GameObject 
	m_statOBJ,
	m_tweetOBJ,
	m_timelineOBJ;

	public Transform
	m_statPanel,
	m_tweetPanel,
	m_timelinePanel;

	public TextMeshProUGUI
	m_dateText;

	public Dartboard[] m_dartboards;

	public Hand[] m_hands;

	public Stat[] m_statBank;

	public Animation
	m_phone;

	public TweetUI m_tweetHeader;

	private List<Stat> m_stats = new List<Stat>();
	private List<TweetUI> m_tweets = new List<TweetUI>();
	private List<TimelineUI> m_timelinePips = new List<TimelineUI>();

	private Dictionary<int, Tweet> m_tweetList = new Dictionary<int, Tweet>();

	private Word m_currentNoun = null;
	private Word m_currentVerb = null;

	private bool m_playerInputAllowed = false;

	private PhoneState m_phoneState = PhoneState.Down;

	private int 
	m_turnNumber = 0,
	m_maxTurns = 4*12,
	m_previousTermTurns = 0;

	private Tweet m_currentTweet = null;

	void Awake ()
	{
		Application.targetFrameRate = 60;

		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public override void OnActivate()
	{
		foreach (Stat thisStat in m_statBank) {

			Stat s = (Stat)ScriptableObject.CreateInstance ("Stat");
			s.m_icon = thisStat.m_icon;
			s.m_losingStrings = thisStat.m_losingStrings;
			s.m_winningStrings = thisStat.m_winningStrings;
			s.m_name = thisStat.m_name;
			s.m_category = thisStat.m_category;

			// create UI
			GameObject uiOBJ = (GameObject) Instantiate(m_statOBJ, m_statPanel);
			StatUI st = (StatUI)uiOBJ.GetComponent<StatUI> ();

			s.Initialize (st );
			m_stats.Add (s);

		}

		UpdateTimeLine ();

		StartRound ();

		m_playerInputAllowed = true;
	}

	public override void OnHold()
	{
		m_gameUI.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_gameUI.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_gameUI.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {

			GameManager.instance.PushMenuState (State.Pause);
		}
		else if (Input.anyKeyDown) {

			if (m_phoneState == PhoneState.Up) {

				TogglePhone ();

			} else if (m_playerInputAllowed) {
				
				foreach (Hand h in m_hands) {

					h.ThrowDart ();

//				foreach (Dartboard d in m_dartboards) {
//
//					d.ToggleTargetFlip ();
//				}
				}

				m_playerInputAllowed = false;
			}
		} 
			
	}

	private void UpdateTimeLine ()
	{
		while (m_timelinePips.Count > 0) {

			GameObject g = m_timelinePips [0].gameObject;
			m_timelinePips.RemoveAt (0);
			Destroy (g);

		}

		for (int i = 0; i < m_maxTurns; i++) {

			GameObject uiOBJ = (GameObject) Instantiate(m_timelineOBJ, m_timelinePanel);
			TimelineUI tUI = (TimelineUI)uiOBJ.GetComponent<TimelineUI> ();
			m_timelinePips.Add (tUI);

			if (i == m_turnNumber) {

				tUI.SetState (TimelineUI.State.Present);
			} else if (i < m_turnNumber) {

				if ( i > 0 && i % 12 == 0) {
					tUI.SetState (TimelineUI.State.Past_YearMark);
				} else {
					tUI.SetState (TimelineUI.State.Past);
				}
			} else {
				if (i % 12 == 0) {
					tUI.SetState (TimelineUI.State.Future_YearMark);
				} else {
					tUI.SetState (TimelineUI.State.Future);
				}
			}
		}

		m_dateText.text = GetDate (m_turnNumber).ToUpper();
	}

	private void StartRound ()
	{
		

		List<Word> n = new List<Word> ();
		List<Word> selectedNoun = new List<Word> ();

//		Array nounEnums = Enum.GetValues(typeof(Noun));

		foreach( Word val in GameManager.instance.m_nouns )
		{
//			if (val != Noun.None && val != Noun.TargetMiss) {
				n.Add (val);
//			}
		}

		for (int i = 0; i < 3; i++) {

			int rand = UnityEngine.Random.Range (0, n.Count);
			selectedNoun.Add (n [rand]);
			n.RemoveAt (rand);
		}

		m_dartboards [0].SetNoun (selectedNoun);

		List<Word> v = new List<Word> ();
		List<Word> selectedVerb = new List<Word> ();

//		Array verbEnums = Enum.GetValues(typeof(Verb));

		foreach( Word val in GameManager.instance.m_verbs )
		{
//			if (val != Verb.None && val != Verb.TargetMiss) {
				v.Add (val);
//			}
		}

		for (int i = 0; i < 3; i++) {

			int rand = UnityEngine.Random.Range (0, v.Count);
			selectedVerb.Add (v [rand]);
			v.RemoveAt (rand);
		}

		m_dartboards [1].SetVerb (selectedVerb);

		foreach (Hand h in m_hands) {

			h.SpawnDart ();
			h.ChangeState (Hand.State.Moving);
		}
	}

	private void EndRound ()
	{
		m_turnNumber++;
		UpdateTimeLine ();

		foreach (Hand h in m_hands) {

			h.KillDart ();
		}

		foreach (Dartboard d in m_dartboards)
		{
			foreach (Target t in d.m_targets) {

				if (t.currentState == Target.State.Active) {

					t.ChangeState (Target.State.Inactive);
				}
			}
		}

		foreach (KeyValuePair<int, Tweet> pair in m_tweetList) {

			pair.Value.m_favs += UnityEngine.Random.Range (100, 10000);
			pair.Value.m_retweets += UnityEngine.Random.Range (100, 10000);
		}

		m_currentNoun = null;
		m_currentVerb = null;
	}

	public void DartMissed ()
	{
		TargetMissed ();
	}

	private void TargetMissed ()
	{
		Debug.Log ("TARGET MISSED");
		foreach (Stat thisStat in m_stats) {

			thisStat.UpdateValue (UnityEngine.Random.Range(-20, 20));

			// check for game over state

			if (thisStat.currentScore == 0 || thisStat.currentScore == thisStat.maxScore) {
				GameOver (thisStat);
				return;
			}
		}

		Tweet t = new Tweet ();
		t.m_body = "TWEETSTORM!!!";
		t.m_date = GetDate (m_turnNumber);
		m_currentTweet = t;

		StartCoroutine (NextRoundTimer ());
	}

	public void SetNoun (Word n)
	{
		if (m_currentNoun == null) {
			m_currentNoun = n;

			if (m_currentVerb != null) {

				SetPhrase (m_currentNoun, m_currentVerb);
			}
		} else if (m_currentNoun != null && n != null) {

			DartMissed ();
		} else {
			m_currentNoun = n;
		}
	}

	public void SetVerb (Word v)
	{

		if (m_currentVerb == null) {
			m_currentVerb = v;

			if (m_currentNoun != null) {

				SetPhrase (m_currentNoun, m_currentVerb);
			} 
		} else if (m_currentVerb != null && v != null) {

			DartMissed ();
		} else {
			m_currentVerb = v;
		}
	}

	private void SetPhrase (Word n, Word v)
	{

		Tweet t = new Tweet ();
		t.m_body = v.m_responses[UnityEngine.Random.Range(0, v.m_responses.Length)] + " " + n.m_targetName.ToUpper ();
		t.m_date = GetDate (m_turnNumber);
		m_currentTweet = t;

		StartCoroutine (NextRoundTimer ());
	}

	public string GetDate (int turnNumber)
	{
		int year = 2017;

		int turn = m_turnNumber + m_previousTermTurns;

		while (turn > 11) {

			turn -= 11;
			year++;
		}

		Month m = (Month)(turn);

		String s = m.ToString () + year.ToString ();
		return s;
	}

	private IEnumerator NextRoundTimer ()
	{
		// update retweet / fav counts on current tweets

		if (m_tweetHeader.m_text.gameObject.activeSelf) {

			m_tweetHeader.m_text.gameObject.SetActive (false);
		}

		foreach (TweetUI tUI in m_tweets) {

			Tweet thisTweet = m_tweetList[tUI.m_ID];
			tUI.Tweet (thisTweet);
		}

		yield return new WaitForSeconds (1.0f);

		m_phoneState = PhoneState.Up;
		m_phone.clip = m_phone.GetClip ("Phone_Raise01");
		m_phone.Play ();

		yield return new WaitForSeconds (1.0f);

		m_currentTweet.m_id = GameManager.instance.newID;

		GameObject newTweetOBJ = (GameObject)Instantiate (m_tweetOBJ, m_tweetPanel);

		newTweetOBJ.transform.localScale = Vector3.one;
		newTweetOBJ.transform.localEulerAngles = Vector3.zero;
		newTweetOBJ.transform.localPosition = Vector3.zero;

		newTweetOBJ.transform.SetAsFirstSibling ();

		TweetUI t = newTweetOBJ.GetComponent<TweetUI> ();
		t.Tweet (m_currentTweet);
		m_tweets.Add (t);

		m_tweetList.Add (m_currentTweet.m_id, m_currentTweet);
		m_currentTweet = null;

		int incAmount = 20;
		int amt = 0;

		foreach (Word.Affinity af in m_currentNoun.m_affinities) {

			Word.Affinity vAf = m_currentVerb.m_affinities [0];

			if (af.m_quality == Word.WordQuality.Negative && vAf.m_quality == Word.WordQuality.Negative) {

				amt = incAmount;

			} else if (af.m_quality == Word.WordQuality.Positive && vAf.m_quality == Word.WordQuality.Negative) {

				amt = incAmount*-1;

			} else if (af.m_quality == Word.WordQuality.Negative && vAf.m_quality == Word.WordQuality.Positive) {

				amt = incAmount*-1;

			} else if (af.m_quality == Word.WordQuality.Positive && vAf.m_quality == Word.WordQuality.Positive) {

				amt = incAmount;

			} else if (af.m_quality == Word.WordQuality.Positive) {

				amt = incAmount;

			} else if (af.m_quality == Word.WordQuality.Negative) {

				amt = incAmount*-1;
			}

			if (amt != 0) {

				foreach (Stat thisStat in m_stats) {

					if (thisStat.m_category == af.m_category) {
						
						thisStat.UpdateValue (amt);

						if (thisStat.currentScore == 0 || thisStat.currentScore == thisStat.maxScore) {
							
							GameOver (thisStat);
							yield break;

						} else {

							if (amt > 0) {
					
								thisStat.ui.SetColor (Color.green);
					
							} else if (amt < 0) {
								
								thisStat.ui.SetColor (Color.red);
							}

						}

						break;
					}
				}
			}
		}

//		int randStat = UnityEngine.Random.Range (0, m_stats.Count);
//		Stat thisStat = m_stats [randStat];
//
//		int amtChange = UnityEngine.Random.Range (-20, 20);
//		thisStat.UpdateValue (amtChange);
//
//		if (amtChange > 0) {
//
//			thisStat.ui.SetColor (Color.green);
//
//		} else if (amtChange < 0) {
//			
//			thisStat.ui.SetColor (Color.red);
//		}
//
		yield return new WaitForSeconds (1.0f);

		foreach (Stat thisStat in m_stats) {

			thisStat.ui.SetColor (Color.white);
		}
//
//		// check for game over state
//
//		if (thisStat.currentScore == 0 || thisStat.currentScore == thisStat.maxScore) {
//			
//			GameOver (thisStat);
//			yield break;
//		}

		yield return new WaitForSeconds (1.0f);

		EndRound ();

		StartRound ();

//		thisStat.ui.SetColor (Color.white);

//		m_phone.clip = m_phone.GetClip ("Phone_Lower01");
//		m_phone.Play ();
//
//		yield return new WaitForSeconds (1.0f);

		// check if at end of timeline, four more years if so

		if (m_turnNumber == m_maxTurns) {

			yield return StartCoroutine (FourMoreYears ());
		}

//		m_playerInputAllowed = true;



		yield return true;
	}

	private void GameOver (Stat s)
	{
		GameManager.instance.PushMenuState (State.GameOver);
	}

	public void TogglePhone ()
	{
		
		if (m_phoneState == PhoneState.Down) {

			m_phoneState = PhoneState.Up;
			m_playerInputAllowed = false;

			m_phone.clip = m_phone.GetClip ("Phone_Raise01");
			m_phone.Play ();

		} else if (m_phoneState == PhoneState.Up) {

			m_phoneState = PhoneState.Down;
			m_playerInputAllowed = true;
			m_phone.clip = m_phone.GetClip ("Phone_Lower01");
			m_phone.Play ();

		}
	}

	public void RestartGame ()
	{
		m_turnNumber = 0;

		foreach (Stat s in m_stats) {

			s.SetValue (s.maxScore / 2);
		}

		if (m_phoneState == PhoneState.Up) {

			TogglePhone ();
		}

		UpdateTimeLine ();
	}

	public IEnumerator FourMoreYears ()
	{
		m_phone.clip = m_phone.GetClip ("Phone_Raise01");
		m_phone.Play ();

		yield return new WaitForSeconds (1.0f);

		Tweet thisT = new Tweet ();
		thisT.m_body = "FOUR MORE YEARS!!!";
		thisT.m_date = GetDate (m_turnNumber);
		m_currentTweet = thisT;

		m_currentTweet.m_id = GameManager.instance.newID;

		GameObject newTweetOBJ = (GameObject)Instantiate (m_tweetOBJ, m_tweetPanel);

		newTweetOBJ.transform.localScale = Vector3.one;
		newTweetOBJ.transform.localEulerAngles = Vector3.zero;
		newTweetOBJ.transform.localPosition = Vector3.zero;

		newTweetOBJ.transform.SetSiblingIndex (1);

		TweetUI t = newTweetOBJ.GetComponent<TweetUI> ();
		t.Tweet (m_currentTweet);
		m_tweets.Add (t);

		m_tweetList.Add (m_currentTweet.m_id, m_currentTweet);
		m_currentTweet = null;

		yield return new WaitForSeconds (1.0f);

		m_phone.clip = m_phone.GetClip ("Phone_Lower01");
		m_phone.Play ();

		yield return new WaitForSeconds (1.0f);

		m_previousTermTurns = m_turnNumber;
		m_turnNumber = 0;

		UpdateTimeLine ();

		yield return true;
	}

	public List<Stat> stats {get{ return m_stats;}}
	public int turnNumber {get{return m_turnNumber;}}
	public int maxTurns {get{return m_maxTurns;}}
	public int previousTermTurns {get{return m_previousTermTurns;}}
}
