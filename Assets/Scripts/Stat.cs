using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Stat : ScriptableObject  {

	public string m_name;
	public Texture m_icon;
	public Word.WordCategory m_category = Word.WordCategory.General;

	public string[] m_winningStrings;
	public string[] m_losingStrings;

	private int m_maxScore = 100;
	private int m_currentScore = 0;

	private StatUI m_ui;

	public void Initialize (StatUI s)
	{
		m_ui = s;
		s.m_stat = this;
		s.m_statname.text = m_name.ToUpper();
//		UpdateValue (m_maxScore / 2);
		s.m_icon.texture = m_icon;

	}

	public void UpdateValue (int amt)
	{
		m_currentScore = Mathf.Clamp (m_currentScore + amt, 0, m_maxScore);
		m_ui.UpdateStatValue (m_currentScore, true);
	}

	public void SetValue (int newValue)
	{
		m_currentScore = Mathf.Clamp (newValue, 0, m_maxScore);
		m_ui.UpdateStatValue (m_currentScore, false);
	}

	public int maxScore {get{return m_maxScore;}}
	public int currentScore {get{return m_currentScore;}}
	public StatUI ui {get{return m_ui;}}
	
}
