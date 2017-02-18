using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TweetUI : MonoBehaviour {

	public TextMeshProUGUI 
	m_text,
	m_date,
	m_retweets,
	m_favs;

	public RawImage m_portrait;

	public int m_ID = -1;

	public void Tweet (Tweet t, Texture portrait)
	{
		m_portrait.texture = portrait;

		string s = t.m_body;
		m_text.text = s;
		m_ID = t.m_id;
		m_date.text = "- " + t.m_date.ToUpper ();

		m_retweets.text = t.m_retweets.ToString ();
		m_favs.text = t.m_favs.ToString ();

	}
}
