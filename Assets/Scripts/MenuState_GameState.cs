using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.Analytics;

public class MenuState_GameState : MenuState {
	public static MenuState_GameState instance;

	public enum GameState
	{
		None,
		Gameplay,
		GameOver,
	}

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

	public Transform m_gameUI;

	public GameObject 
	m_statOBJ,
	m_tweetOBJ,
	m_timelineOBJ;

	public Transform
	m_statPanel,
	m_tweetPanel,
	m_timelinePanel,
	m_introPanel,
	m_titleText;

	public TextMeshProUGUI
	m_dateText,
	m_gameOverResults;

	public Dartboard[] m_dartboards;

	public Hand[] m_hands;

	public Stat[] m_statBank;

	public Animation
	m_phone,
	m_intro,
	m_outro,
	m_tapIcon;

	public TweetUI m_tweetHeader;

	public Texture[] m_tweetPortraitStates;

	public String[] m_interjections;

	public Transform[] m_clickReceivers;

	private List<Stat> m_stats = new List<Stat>();
	private List<TweetUI> m_tweets = new List<TweetUI>();
	private List<TimelineUI> m_timelinePips = new List<TimelineUI>();

	private Dictionary<int, Tweet> m_tweetList = new Dictionary<int, Tweet>();

	private Word m_currentNoun = null;
	private Word m_currentVerb = null;

	public bool 
	m_playerInputAllowed = false,
	m_waitingForNextTurn = false;

	private bool
	m_helpTapIconGameplayShown = false,
	m_helpTapIconPhoneShown = false;

	private PhoneState m_phoneState = PhoneState.Down;

	private int 
	m_turnNumber = 0,
	m_maxTurns = 4*12,
	m_previousTermTurns = 0;

	private Tweet m_currentTweet = null;

	private GameState m_gameState = GameState.Gameplay;

//	private string CONSUMER_KEY = "8FCY5K98HZ4vbC4EWUkrHI0fu";
//	private string CONSUMER_SECRET = "SOiF0UCjj5Y1FH5MhdfeEYcNdyPTJUTybUjm1dNPgng6Iv3X6w";
//	Twitter.RequestTokenResponse m_RequestTokenResponse;
//	Twitter.AccessTokenResponse m_AccessTokenResponse;

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
		if (!GameManager.instance.m_demoMode) {

			// initialize player prefs related variables

			int help_Gameplay = PlayerPrefs.GetInt ("Help_Gameplay");
			int help_Phone = PlayerPrefs.GetInt ("Help_Phone");

			if (help_Gameplay == 1) {

				m_helpTapIconGameplayShown = true;
			}

			if (help_Phone == 1) {

				m_helpTapIconPhoneShown = true;
			}
		}


		// initialize stats

		foreach (Stat thisStat in m_statBank) {

			Stat s = (Stat)ScriptableObject.CreateInstance ("Stat");
			s.m_icon = thisStat.m_icon;
			s.m_losingStrings = thisStat.m_losingStrings;
			s.m_winningStrings = thisStat.m_winningStrings;
			s.m_name = thisStat.m_name;
			s.m_category = thisStat.m_category;

			// create UI
			GameObject uiOBJ = (GameObject) Instantiate(m_statOBJ, m_statPanel);
			uiOBJ.transform.localScale = Vector3.one;
			StatUI st = (StatUI)uiOBJ.GetComponent<StatUI> ();

			s.Initialize (st );
			m_stats.Add (s);

		}

		// play curtain raise animation or skip if needed

		if (!GameManager.instance.m_skipIntro) {

			StartCoroutine (Intro ());

		} else {

			foreach (Stat s in m_stats) {

				s.UpdateValue (s.maxScore / 2);
			}

			m_introPanel.gameObject.SetActive (false);
//			m_titleText.gameObject.SetActive (true);

			UpdateTimeLine ();

			StartRound ();

			m_playerInputAllowed = true;
		}
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

		#if UNITY_IOS

//		if (Input.touches.Length > 0)
//		{
//			if (Input.touches[0].phase == TouchPhase.Ended)
//			{
//
//				if (Input.mousePosition.y < 50 || m_phoneState == PhoneState.Up)
//				{
//					if (m_phoneState == PhoneState.Up && m_playerInputAllowed) {
//
//						TogglePhone ();
//					}
//				} else {
//
//					if (m_phoneState == PhoneState.Up && m_playerInputAllowed) {
//
//						TogglePhone ();
//
//					} else if (m_playerInputAllowed) {
//
//						foreach (Hand h in m_hands) {
//
//							h.ThrowDart ();
//
//						}
//
//						CharacterController.instance.DartsThrown ();
//						AudioManager.instance.PlaySound (AudioManager.SoundType.Dart_Flight);
//
//						foreach (Dartboard d in m_dartboards) {
//
//							float flipChance = 0.1f;
//
//							if (!d.flipped && MenuState_GameState.instance.turnNumber > 3 && UnityEngine.Random.Range (0.0f, 1.0f) <= flipChance) {
//
//								d.ToggleTargetFlip ();
//							}
//						}
//
//						m_playerInputAllowed = false;
//					}
//				}
//			}
//		}

//		if (Input.anyKeyDown && m_playerInputAllowed) {
//
//			foreach (Hand h in m_hands) {
//
//				h.ThrowDart ();
//
//				}
//		}

		#else

		if (Input.GetKeyDown (KeyCode.Escape) && m_gameState != GameState.GameOver) {

			PauseButtonPressed();

		} 
//		else if (Input.GetMouseButtonDown (0) && (Input.mousePosition.y < 50 || Input.mousePosition.y > Screen.height - 50 || m_phoneState == PhoneState.Up)) {
//
//			if (m_phoneState == PhoneState.Up && m_playerInputAllowed) {
//
//				TogglePhone ();
//			}
//		}
//		else if (Input.anyKeyDown) {
//			
//			if (m_phoneState == PhoneState.Up && m_playerInputAllowed) {
//
//				TogglePhone ();
//
//			} else if (m_playerInputAllowed) {
//				
//				foreach (Hand h in m_hands) {
//
//					h.ThrowDart ();
//
//				}
//
//				CharacterController.instance.DartsThrown ();
//				AudioManager.instance.PlaySound (AudioManager.SoundType.Dart_Flight);
//
//				foreach (Dartboard d in m_dartboards) {
//
//					float flipChance = 0.1f;
//
//					if (!d.flipped && MenuState_GameState.instance.turnNumber > 3 && UnityEngine.Random.Range (0.0f, 1.0f) <= flipChance) {
//
//						d.ToggleTargetFlip ();
//					}
//				}
//
//				m_playerInputAllowed = false;
//			}
//		} 
		#endif
	}

	private void UpdateTimeLine ()
	{
//		while (m_timelinePips.Count > 0) {
//
//			GameObject g = m_timelinePips [0].gameObject;
//			m_timelinePips.RemoveAt (0);
//			Destroy (g);
//
//		}

//		for (int i = 0; i < m_maxTurns; i++) {
//
//			GameObject uiOBJ = (GameObject) Instantiate(m_timelineOBJ, m_timelinePanel);
//			uiOBJ.transform.localScale = Vector3.one;
//			TimelineUI tUI = (TimelineUI)uiOBJ.GetComponent<TimelineUI> ();
//			m_timelinePips.Add (tUI);
//
//			if (i == m_turnNumber) {
//
//				tUI.SetState (TimelineUI.State.Present);
//			} else if (i < m_turnNumber) {
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

		string dateString = GetDate (m_turnNumber).ToUpper () + "\n<size=18>" + GetTimeRemaining(m_turnNumber) + "</size>";
		m_dateText.text = dateString;
	}


	private void StartRound ()
	{
		// choose nouns from bank for the noun dartboard

		List<Word> n = new List<Word> ();
		List<Word> selectedNoun = new List<Word> ();

		foreach( Word val in GameManager.instance.m_nouns )
		{
				n.Add (val);
		}

		for (int i = 0; i < 4; i++) {

			int rand = UnityEngine.Random.Range (0, n.Count);
			selectedNoun.Add (n [rand]);
			n.RemoveAt (rand);

			// debug

//			selectedNoun.Add(GameManager.instance.m_nouns[31]);
		}

		m_dartboards [0].SetNoun (selectedNoun);

		// choose verbs from bank for verb darboard

		List<Word> v = new List<Word> ();
		List<Word> selectedVerb = new List<Word> ();

		foreach( Word val in GameManager.instance.m_verbs )
		{
			v.Add (val);
		}

		for (int i = 0; i < 4; i++) {

			int rand = UnityEngine.Random.Range (0, v.Count);
			selectedVerb.Add (v [rand]);
			v.RemoveAt (rand);

			// debug

//			selectedVerb.Add (GameManager.instance.m_verbs[18]);
		}

		m_dartboards [1].SetVerb (selectedVerb);

		// populate dartboards

		foreach (Dartboard d in m_dartboards) {

			if (d.flipped) {
				
				d.ToggleTargetFlip ();

			} else {
				
				float flipChance = 0.2f;

				if (!d.flipped && MenuState_GameState.instance.turnNumber > 3 && UnityEngine.Random.Range (0.0f, 1.0f) <= flipChance) {

					d.ToggleTargetFlip ();
				}
				
			}

			d.currentState = Dartboard.State.Active;
		}

		// spawn darts in hands

		foreach (Hand h in m_hands) {

			h.SpawnDart ();
			h.ChangeState (Hand.State.Moving);
		}
	}

	private void EndRound ()
	{
		m_turnNumber++;
		UpdateTimeLine ();
		CharacterController.instance.UpdateFace ();

		// remove any arrows on stat UI

		foreach (Stat s in m_stats) {

			s.ui.SetArrow (StatUI.ArrowType.None);
		}

		// remove darts

		foreach (Hand h in m_hands) {

			h.KillDart ();
		}

		// dartboards stop spinning

		foreach (Dartboard d in m_dartboards)
		{
			foreach (Target t in d.m_targets) {

				if (t.currentState == Target.State.Active) {

					t.ChangeState (Target.State.Inactive);
				}
			}
		}

		// increase the # of favs & retweets in all tweets

		foreach (KeyValuePair<int, Tweet> pair in m_tweetList) {

			pair.Value.m_favs += UnityEngine.Random.Range (100, 10000);
			pair.Value.m_retweets += UnityEngine.Random.Range (100, 10000);
		}

		// zero out verb and noun from ending round

		m_currentNoun = null;
		m_currentVerb = null;
	}

	public void DartMissed (Dart d)
	{

		// if noun or verb dartboard are missed, pull a random interjection from the bank to replace it

		Word w = (Word)ScriptableObject.CreateInstance ("Word");
		w.m_wordType = d.m_dartType;
		w.m_affinities = new Word.Affinity[1];
		w.m_affinities [0] = new Word.Affinity ();
		w.m_affinities [0].m_category = Word.WordCategory.General;
		w.m_affinities [0].m_quality = Word.WordQuality.Negative;

		if (d.m_dartType == Word.WordType.Verb) {

			w.m_responseBank = new Response[1];

			Response r = new Response();
			r.m_fragments = new Response.ResponseFragment[2];
			r.m_fragments [0] = new Response.ResponseFragment ();
			r.m_fragments [0].m_type = Response.FragmentType.String;
			r.m_fragments [0].m_string = m_interjections[UnityEngine.Random.Range(0, m_interjections.Length)] + " ";

			r.m_fragments [1] = new Response.ResponseFragment ();
			r.m_fragments [1].m_type = Response.FragmentType.Noun_Capitolized;

			w.m_responseBank [0] = r;

			SetVerb (w);
		} else if (d.m_dartType == Word.WordType.Noun) {

			w.m_targetName = m_interjections[UnityEngine.Random.Range(0, m_interjections.Length)];
			SetNoun (w);
		}
	}

	public void SetNoun (Word n)
	{
		if (m_currentNoun == null) {
			m_currentNoun = n;

			if (m_currentVerb != null) {

				SetPhrase (m_currentNoun, m_currentVerb);
			}
		} else if (m_currentNoun != null && n != null) {

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

		} else {
			m_currentVerb = v;
		}
	}

	private void SetPhrase (Word n, Word v)
	{
		CharacterController.instance.DartsLanded ();

		foreach (Dartboard d in m_dartboards)
		{
			d.currentState = Dartboard.State.Hit;
		}
			

		// get random response

		Response r = v.m_responseBank[UnityEngine.Random.Range(0, v.m_responseBank.Length)];
		String s = "";

		string nounColor = "#060A67FF";
		string verbColor = "#3A0D0DFF";

		#if UNITY_IOS

		nounColor = "#0C13B0FF";
		verbColor = "#8C2121FF";

		#endif

		// populate response fragments

		foreach (Response.ResponseFragment rf in r.m_fragments) {

			switch (rf.m_type) {

			case Response.FragmentType.String:

				s += rf.m_string;
				break;
			case Response.FragmentType.Noun:
				
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {

					s += "<color=" + nounColor + ">" + n.m_targetName.ToUpper () + "</color>";

				} else {

					if (n.m_proper)
					{
						s += "<color=" + nounColor + ">" + n.m_targetName + "</color>";
					} else {
						s += "<color=" + nounColor + ">" + n.m_targetName.ToLower () + "</color>";
					}
				}
				break;
			case Response.FragmentType.Noun_Capitolized:
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
					s += "<color=" + nounColor + ">" + n.m_targetName.ToUpper () + "</color>";
				} else {
					
					string st = n.m_targetName;
					st = char.ToUpper(st[0]) + st.Substring(1);
					s += "<color=" + nounColor + ">" + st + "</color>";
				}
				break;
			case Response.FragmentType.Verb_Capitolized:
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
					s += "<color=" + verbColor + ">" + rf.m_string.ToUpper () + "</color>";
				} else {

					string st = rf.m_string;
					st = char.ToUpper(st[0]) + st.Substring(1);
					s += "<color=" + verbColor + ">" + st + "</color>";

				}
				break;
			case Response.FragmentType.Verb:
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
					s += "<color=" + verbColor + ">" + rf.m_string.ToUpper () + "</color>";
				} else {
					s += "<color=" + verbColor + ">" + rf.m_string.ToLower () + "</color>";
				}
				break;
			case Response.FragmentType.Question:
				
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.1f) {
					s += "?!";
				} else if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.3f) {

					s += "??";

				} else if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.5f) {

					s += "???";

				} else {

					s += "?";
				}
				break;
			case Response.FragmentType.Statement:

				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.2f) {
					s += ".";
				} else if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {

					s += "!!";

				} else if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.6f) {

				s += "!!";

				} else {
					
					s += "!";
				}
				break;
			}
		}

		// chance for interjection

		if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.3f) {

			string interjection = " " + m_interjections[UnityEngine.Random.Range(0, m_interjections.Length)];

			if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.3f) {
				interjection = interjection.ToUpper ();
			}

			s += interjection;
		}

		// chance for all caps

		if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.2f) {
			s = s.ToUpper ();
		}


		Tweet t = new Tweet ();
		t.m_body = s;
		t.m_date = GetDate (m_turnNumber);
		m_currentTweet = t;

		StartCoroutine (NextRoundTimer ());
	}

	public string GetDate (int turnNumber)
	{
		int year = 2017;

		int turn = m_turnNumber + m_previousTermTurns;

		while (turn > 11) {

			turn -= 12;
			year++;
		}

		Month m = (Month)(turn);

		String s = m.ToString () + " " + year.ToString ();
		return s;
	}

	public string GetTimeRemaining (int turnNumber)
	{
		int turn = turnNumber;
		int years = 0;
		int months = 0;
//		Debug.Log (turn);
//		while (turn > turnNumber) {
//
//			turn -= 12;
//			years++;
////			Debug.Log (turn);
//		}

		while (turn < m_maxTurns) {

			turn++;
			months++;

			if (months == 11) {
				months = 0;
				years++;
			}
			//			Debug.Log (turn);
		}

		String s = years.ToString() + " Years and " + months.ToString() +  " Months Remain";
		return s;
	}

	private IEnumerator NextRoundTimer ()
	{

		// remove the 'no tweets' text

		if (m_tweetHeader.m_text.gameObject.activeSelf) {

			m_tweetHeader.m_text.gameObject.SetActive (false);
		}

		// refresh current tweets with updated favs & retweet numbers

		foreach (TweetUI tUI in m_tweets) {

			Tweet thisTweet = m_tweetList[tUI.m_ID];
			tUI.Tweet (thisTweet, tUI.m_portrait.texture);
		}

		yield return new WaitForSeconds (1.0f);

		// phone raises up

		m_phoneState = PhoneState.Up;
		m_phone.clip = m_phone.GetClip ("Phone_Raise01");
		m_phone.Play ();

		AudioManager.instance.PlaySound (AudioManager.SoundType.Phone_Raise);

		yield return new WaitForSeconds (1.0f);

//		m_clickReceivers [1].gameObject.SetActive (true);
//		m_clickReceivers [0].gameObject.SetActive (false);

		m_currentTweet.m_id = GameManager.instance.newID;

		// new tweet appears

		GameObject newTweetOBJ = (GameObject)Instantiate (m_tweetOBJ, m_tweetPanel);

		newTweetOBJ.transform.localScale = Vector3.one;
		newTweetOBJ.transform.localEulerAngles = Vector3.zero;
		newTweetOBJ.transform.localPosition = Vector3.zero;

		newTweetOBJ.transform.SetAsFirstSibling ();

		AudioManager.instance.PlaySound (AudioManager.SoundType.Tweet_Sent);

		TweetUI t = newTweetOBJ.GetComponent<TweetUI> ();

		// get face state

		Texture tex = m_tweetPortraitStates[0];

		switch (CharacterController.instance.currentFaceState) {

		case CharacterController.FaceState.Uncertain:
			tex = m_tweetPortraitStates[1];
			break;
		case CharacterController.FaceState.Distraught:
			tex = m_tweetPortraitStates[2];
			break;
		case CharacterController.FaceState.Anger:
			tex = m_tweetPortraitStates[3];
			break;
		case CharacterController.FaceState.Fear:
			tex = m_tweetPortraitStates[4];
			break;

		}

		t.Tweet (m_currentTweet, tex);
		m_tweets.Add (t);

		m_tweetList.Add (m_currentTweet.m_id, m_currentTweet);
		m_currentTweet = null;


		// play audio cue

		if (UnityEngine.Random.Range (0.0f, 1.0f) > 0.8f) {
			
			yield return new WaitForSeconds (0.5f);

			if (m_currentNoun.m_random) {
				AudioManager.instance.PlaySound (AudioManager.SoundType.Character_VocalizeRandom);
			} else if (m_currentVerb.m_affinities [0].m_quality == Word.WordQuality.Positive) {
				AudioManager.instance.PlaySound (AudioManager.SoundType.Character_VocalizePositive);
			} else if (m_currentVerb.m_affinities [0].m_quality == Word.WordQuality.Negative) {
				AudioManager.instance.PlaySound (AudioManager.SoundType.Character_VocalizeNegative);
			}
		}

//		int incAmount = 15;
		int incAmount = 20;
		int amt = 0;


		for (int i=0; i < m_currentNoun.m_affinities.Length; i++){

			Word.Affinity af = m_currentNoun.m_affinities [i];

			Word.Affinity vAf = m_currentVerb.m_affinities [0];

			bool specificResponse = false;

			// uncomment and fix if specific responses are needed

//			if (m_currentVerb.m_specificResponses.Length > 0) {
//
//				foreach (Word.SpecificResponse s in m_currentVerb.m_specificResponses) {
//
//					if (s.m_noun == m_currentNoun) {
//						Debug.Log ("SPECIFIC RESPONSE");
//						if (s.m_quality == Word.WordQuality.Negative) {
//							
//							amt = incAmount*-1;
//						} else if (s.m_quality == Word.WordQuality.Positive) {
//							amt = incAmount;
//						}
//						specificResponse = true;
//						break;
//					}
//				}
//
//			}

			if (!specificResponse) {
				
				if (af.m_quality == Word.WordQuality.Negative && vAf.m_quality == Word.WordQuality.Negative) {

					amt = incAmount;

				} else if (af.m_quality == Word.WordQuality.Positive && vAf.m_quality == Word.WordQuality.Negative) {

					amt = incAmount * -1;

				} else if (af.m_quality == Word.WordQuality.Negative && vAf.m_quality == Word.WordQuality.Positive) {

					amt = incAmount * -1;

				} else if (af.m_quality == Word.WordQuality.Positive && vAf.m_quality == Word.WordQuality.Positive) {

					amt = incAmount;

				} else if (af.m_quality == Word.WordQuality.Positive) {

					amt = incAmount;

				} else if (af.m_quality == Word.WordQuality.Negative) {

					amt = incAmount * -1;
				}
			}

			if (amt != 0) {

				yield return new WaitForSeconds (1.0f);

				// update stat values

				foreach (Stat thisStat in m_stats) {

					if (thisStat.m_category == af.m_category) {
						
						thisStat.UpdateValue (amt);

						switch (thisStat.m_category) {

						case Word.WordCategory.Captialism:

							if (amt > 0) {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Capitalism_Increase);
								thisStat.ui.SetArrow (StatUI.ArrowType.Increase);
							} else {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Capitalism_Decrease);
								thisStat.ui.SetArrow (StatUI.ArrowType.Decrease);
							}
							break;

						case Word.WordCategory.Law:

							if (amt > 0) {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Law_Increase);
								thisStat.ui.SetArrow (StatUI.ArrowType.Increase);
							} else {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Law_Decrease);
								thisStat.ui.SetArrow (StatUI.ArrowType.Decrease);
							}
							break;

						case Word.WordCategory.Democracy:
							
							if (amt > 0) {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Democracy_Increase);
								thisStat.ui.SetArrow (StatUI.ArrowType.Increase);
							} else {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Democracy_Decrease);
								thisStat.ui.SetArrow (StatUI.ArrowType.Decrease);
							}
							break;

						case Word.WordCategory.Culture:

							if (amt > 0) {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Culture_Increase);
								thisStat.ui.SetArrow (StatUI.ArrowType.Increase);
							} else {
								AudioManager.instance.PlaySound (AudioManager.SoundType.Culture_Decrease);
								thisStat.ui.SetArrow (StatUI.ArrowType.Decrease);
							}
							break;
						}

//						if (amt > 0) {
//
//							thisStat.ui.SetColor (Color.green);
//
//						} else if (amt < 0) {
//
//							thisStat.ui.SetColor (Color.red);
//						}
//						if (amt != 0) {


//							if (amt == 0 || amt == 100) {
//
//								thisStat.ui.SetColor (Color.red);
//
//							} else {
								
//								thisStat.ui.SetBorderColor (Color.red);
//							}
//						}

						if (thisStat.currentScore == 0 || thisStat.currentScore == thisStat.maxScore) {

							foreach (Stat s in m_stats) {

//								if (s != thisStat) {

									thisStat.ui.SetColor (Color.white);
//								}
							}

							thisStat.ui.SetColor (Color.red);

							GameOver (thisStat);
							yield break;

						} else if (amt != 0 && m_gameState != GameState.GameOver) {

							thisStat.ui.SetBorderColor (Color.red);
						}

						break;
					}
				}
			}
		}

		yield return new WaitForSeconds (1.0f);

		// reset stat colors back to white

		foreach (Stat thisStat in m_stats) {

			thisStat.ui.SetColor (Color.white);
		}

		// check if at end of timeline, four more years if so

//		if (m_turnNumber+1 == m_maxTurns) {
//
//			yield return StartCoroutine (FourMoreYears ());
//		}

		m_playerInputAllowed = true;
		m_waitingForNextTurn = true;


		if (!m_helpTapIconPhoneShown) {
			m_tapIcon.gameObject.SetActive (true);
			m_tapIcon.Play ("Help_TapIcon01");
		}


		yield return true;
	}

	private void GameOver (Stat s)
	{
		Debug.Log ("Game Over");

		m_gameState = GameState.GameOver;

		Analytics.CustomEvent("gameOver", new Dictionary<string, object>
			{
				{ "stat", s.m_name },
				{ "score", s.currentScore },
				{ "turns", m_turnNumber + m_previousTermTurns },
			});

		string gameOverResults = "IN " + GetDate(m_turnNumber).ToUpper() + ", " ;

		if (s.currentScore == 100) {

			gameOverResults += s.m_winningStrings [0];
		} else if (s.currentScore == 0) {
			gameOverResults += s.m_losingStrings [0];
		}

		m_gameOverResults.text = gameOverResults;

		StartCoroutine (GameOverAnimation ());
	}

	public IEnumerator GameOverAnimation () {

		m_playerInputAllowed = false;

		AudioManager.instance.PlaySound (AudioManager.SoundType.GameOver_Sting);

		yield return new WaitForSeconds (3.0f);

		m_outro.gameObject.SetActive (true);
		TogglePhone ();
		m_outro.Play ();

	}
		
	public void PhoneButtonPressed ()
	{
		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		TogglePhone ();
	}

	public void TogglePhone ()
	{
		
		if (m_phoneState == PhoneState.Down) {

			m_phoneState = PhoneState.Up;
//			m_playerInputAllowed = false;

			m_phone.clip = m_phone.GetClip ("Phone_Raise01");
			m_phone.Play ();

			AudioManager.instance.PlaySound (AudioManager.SoundType.Phone_Raise);

//			m_clickReceivers [1].gameObject.SetActive (true);
//			m_clickReceivers [0].gameObject.SetActive (false);

		} else if (m_phoneState == PhoneState.Up) {

			if (m_waitingForNextTurn) {

				if (m_turnNumber + 1 == m_maxTurns) {
		
					StartCoroutine (FourMoreYears ());
					return;

				} else {
					
					m_waitingForNextTurn = false;
					EndRound ();

					StartRound ();
				}
			}

			m_phoneState = PhoneState.Down;

			if (m_gameState != GameState.GameOver) {
				m_playerInputAllowed = true;
			}
			m_phone.clip = m_phone.GetClip ("Phone_Lower01");
			m_phone.Play ();

//			m_clickReceivers [0].gameObject.SetActive (true);
//			m_clickReceivers [1].gameObject.SetActive (false);

			AudioManager.instance.PlaySound (AudioManager.SoundType.Phone_Raise);

//			if (m_waitingForNextTurn) {
//
//				m_waitingForNextTurn = false;
//				EndRound ();
//
//				StartRound ();
//			}

		}
	}

	public void RestartGame ()
	{
		m_turnNumber = -1;

		foreach (Stat s in m_stats) {

			s.SetValue (s.maxScore / 2);
			s.ui.SetColor (Color.white);
		}

		if (m_phoneState == PhoneState.Up) {

			TogglePhone ();
		}

		// empty list of tweets and tweets on phone

		m_tweetList.Clear ();

		while (m_tweets.Count > 0) {

			TweetUI t = m_tweets[0];
			m_tweets.RemoveAt (0);
			Destroy (t.gameObject);
		}

		// renable empty feed state

		m_tweetHeader.m_text.gameObject.SetActive (true);

		// remove game over background

		m_outro.gameObject.SetActive (false);

		// re enable music
		AudioManager.instance.PlaySound (AudioManager.SoundType.GameStart);
		m_gameState = GameState.Gameplay;

		EndRound ();

		StartRound ();

		StartCoroutine (EnableInput ());
	}

	public IEnumerator FourMoreYears ()
	{
		// if the player reaches the end of the timeline, Drumpf gives himself four more years and resets the timeline

		m_playerInputAllowed = false;

		AudioManager.instance.PlaySound (AudioManager.SoundType.Reelection);

		yield return new WaitForSeconds (0.75f);

		AudioManager.instance.PlaySound (AudioManager.SoundType.Character_VocalizePositive);

		Tweet thisT = new Tweet ();
		thisT.m_body = "FOUR MORE YEARS!!!";
		thisT.m_date = GetDate (m_turnNumber);
		m_currentTweet = thisT;

		m_currentTweet.m_id = GameManager.instance.newID;

		GameObject newTweetOBJ = (GameObject)Instantiate (m_tweetOBJ, m_tweetPanel);

		newTweetOBJ.transform.localScale = Vector3.one;
		newTweetOBJ.transform.localEulerAngles = Vector3.zero;
		newTweetOBJ.transform.localPosition = Vector3.zero;

		newTweetOBJ.transform.SetAsFirstSibling ();

		TweetUI t = newTweetOBJ.GetComponent<TweetUI> ();
		t.Tweet (m_currentTweet, m_tweetPortraitStates[5]);
		m_tweets.Add (t);

		m_tweetList.Add (m_currentTweet.m_id, m_currentTweet);
		m_currentTweet = null;

		yield return new WaitForSeconds (1.0f);

		m_previousTermTurns += m_maxTurns;
		m_turnNumber = -1;

		m_playerInputAllowed = true;

		yield return true;
	}

	public IEnumerator Intro ()
	{
		m_introPanel.gameObject.SetActive (true);

		UpdateTimeLine ();

		StartRound ();

		m_intro.Play ();

		AudioManager.instance.PlaySound (AudioManager.SoundType.Curtain_Raise);

		yield return new WaitForSeconds (0.5f);

		m_stats[0].UpdateValue (m_stats[0].maxScore);
//		m_stats [0].ui.ShowName (true);

		yield return new WaitForSeconds (0.5f);

		m_stats[1].UpdateValue (m_stats[1].maxScore);
//		m_stats [1].ui.ShowName (true);
		m_stats[0].UpdateValue ((m_stats[0].maxScore / 2) * -1);

		yield return new WaitForSeconds (0.5f);

		m_stats[2].UpdateValue (m_stats[2].maxScore);
//		m_stats [2].ui.ShowName (true);
		m_stats[1].UpdateValue ((m_stats[1].maxScore / 2) * -1);

		yield return new WaitForSeconds (0.5f);

		m_stats[3].UpdateValue (m_stats[3].maxScore);
//		m_stats [3].ui.ShowName (true);
		m_stats[2].UpdateValue ((m_stats[2].maxScore / 2) * -1);

		yield return new WaitForSeconds (0.5f);

		m_stats[3].UpdateValue ((m_stats[3].maxScore / 2) * -1);




//		yield return new WaitForSeconds (2.5f);

//		m_stats [0].ui.ShowName (false);
//		m_stats [1].ui.ShowName (false);
//		m_stats [2].ui.ShowName (false);
//		m_stats [3].ui.ShowName (false);



//		yield return new WaitForSeconds (2.5f);

		m_introPanel.gameObject.SetActive (false);
//		m_titleText.gameObject.SetActive (true);

		m_playerInputAllowed = true;

		if (!m_helpTapIconGameplayShown) {
			m_tapIcon.gameObject.SetActive (true);
			m_tapIcon.Play ("Help_TapIcon02");
		}
		
		yield return true;
	}

	public void GameOverTweetButtonPressed ()
	{
		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);

		string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
//		string LINK = "http://";

		string s = "I survived ";

		int years = 0;
		int months = 0;

		int turn = MenuState_GameState.instance.turnNumber + MenuState_GameState.instance.previousTermTurns;

		while (turn > 11) {

			turn -= 11;
			years++;
		}

		months = turn;

		if (years > 0) {

			if (years == 1) {
				s += years.ToString () + " year ";
			} else {
				s += years.ToString () + " years ";
			}

			if (months > 0) {
				s += "and ";
			}
		}

		if (months > 0) {

			if (months == 1) {
				s += months.ToString () + " month ";
			} else {
				s += months.ToString () + " months ";
			}
		}

		s += "under #TheIdiotPresident";

		Application.OpenURL (TWITTER_ADDRESS + "?text=" + WWW.EscapeURL (s));
	}

	public void TwitterScreenshotButtonPressed ()
	{
//		if (m_RequestTokenResponse == null) {
//			StartCoroutine (Twitter.API.GetRequestToken (CONSUMER_KEY, CONSUMER_SECRET,
//				new Twitter.RequestTokenCallback (this.OnRequestTokenCallback)));
//		}
	}

	void OnRequestTokenCallback(bool success, Twitter.RequestTokenResponse response)
	{
//		if (success)
//		{
//			string log = "OnRequestTokenCallback - succeeded";
//			log += "\n    Token : " + response.Token;
//			log += "\n    TokenSecret : " + response.TokenSecret;
//			print(log);
//
//			m_RequestTokenResponse = response;
//
//			Twitter.API.OpenAuthorizationPage(response.Token);
//		}
//		else
//		{
//			print("OnRequestTokenCallback - failed.");
//		}
	}

	void LoadTwitterUserInfo()
	{
//		m_AccessTokenResponse = new Twitter.AccessTokenResponse();
//
//		m_AccessTokenResponse.UserId        = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_ID);
//		m_AccessTokenResponse.ScreenName    = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
//		m_AccessTokenResponse.Token         = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN);
//		m_AccessTokenResponse.TokenSecret   = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET);
//
//		if (!string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
//			!string.IsNullOrEmpty(m_AccessTokenResponse.ScreenName) &&
//			!string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
//			!string.IsNullOrEmpty(m_AccessTokenResponse.TokenSecret))
//		{
//			string log = "LoadTwitterUserInfo - succeeded";
//			log += "\n    UserId : " + m_AccessTokenResponse.UserId;
//			log += "\n    ScreenName : " + m_AccessTokenResponse.ScreenName;
//			log += "\n    Token : " + m_AccessTokenResponse.Token;
//			log += "\n    TokenSecret : " + m_AccessTokenResponse.TokenSecret;
//			print(log);
//		}
	}

	public void Click ()
	{

		if (m_phoneState == PhoneState.Up && m_playerInputAllowed) {

			if (!m_helpTapIconPhoneShown) {

				m_tapIcon.gameObject.SetActive (false);
				m_helpTapIconPhoneShown = true;

				if (!GameManager.instance.m_demoMode) {

					// save first time help shown to player prefs

					PlayerPrefs.SetInt("Help_Phone", 1);
					PlayerPrefs.Save ();
				}
			}

			TogglePhone ();

		} else if (m_playerInputAllowed) {

			if (!m_helpTapIconGameplayShown) {

				m_tapIcon.gameObject.SetActive (false);
				m_helpTapIconGameplayShown = true;

				if (!GameManager.instance.m_demoMode) {

					// save first time help shown to player prefs

					PlayerPrefs.SetInt("Help_Gameplay", 1);
					PlayerPrefs.Save ();
				}
			}

			foreach (Hand h in m_hands) {

				h.ThrowDart ();

			}

			CharacterController.instance.DartsThrown ();
			AudioManager.instance.PlaySound (AudioManager.SoundType.Dart_Flight);

			foreach (Dartboard d in m_dartboards) {

				float flipChance = 0.1f;

				if (!d.flipped && MenuState_GameState.instance.turnNumber > 3 && UnityEngine.Random.Range (0.0f, 1.0f) <= flipChance) {

					d.ToggleTargetFlip ();
				}
			}

			m_playerInputAllowed = false;
		}
	}

	public void PlayAgain ()
	{
		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		RestartGame ();
	}

	public void QuitButtonPressed () {

		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		Application.Quit ();
	}

	public void DonateButtonPressed () {

		AudioManager.instance.PlaySound (AudioManager.SoundType.Button_Click);
		Application.OpenURL ("https://action.aclu.org/secure/donate-to-aclu");
	}

	public IEnumerator EnableInput ()
	{
		yield return new WaitForSeconds (0.1f);
		MenuState_GameState.instance.playerInputAllowed = true;
	}

	public void PauseButtonPressed ()
	{
		if (m_gameState != GameState.GameOver) {
			GameManager.instance.PushMenuState (State.Pause);
		}
	}

	public List<Stat> stats {get{ return m_stats;}}
	public int turnNumber {get{return m_turnNumber;}}
	public int maxTurns {get{return m_maxTurns;}}
	public int previousTermTurns {get{return m_previousTermTurns;}}
	public bool playerInputAllowed {get{return m_playerInputAllowed;} set{ m_playerInputAllowed = value; }}
	public Word currentNoun {get{return m_currentNoun;}}
	public Word currentVerb {get{return m_currentVerb;}}
}
