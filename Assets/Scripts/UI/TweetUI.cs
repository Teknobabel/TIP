using UnityEngine;
using System.Collections;
using TMPro;

public class TweetUI : MonoBehaviour {

	public TextMeshProUGUI 
	m_text,
	m_retweets,
	m_favs;

	public int m_ID = -1;

	public void Tweet (Tweet t)
	{
		string s = t.m_body;
		s += "\n<color=#A8A8A8>" + t.m_date + "</color>";
		m_text.text = s;
		m_ID = t.m_id;

		m_retweets.text = t.m_retweets.ToString ();
		m_favs.text = t.m_favs.ToString ();

	}
}
