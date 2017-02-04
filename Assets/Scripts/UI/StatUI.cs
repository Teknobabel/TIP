using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour {

	public RectTransform m_progressBar_Fill;

	public RawImage 
	m_icon,
	m_progressBar,
	m_progressBar_BG;

	public TextMeshProUGUI m_statname;

	public Stat m_stat;

	public void UpdateStatValue (int newValue)
	{
		Rect r = m_progressBar_Fill.rect;
		r.height = newValue;

		float maxHeight = m_progressBar_BG.rectTransform.rect.size.y;
		float modifier = ((float)m_stat.currentScore) / ((float)m_stat.maxScore);
		float newHeight = maxHeight * modifier;
//		Debug.Log (m_stat.m_currentScore + " / " + m_stat.m_maxScore);
//		Debug.Log (maxHeight + " * " + modifier + " = " + newHeight);
		m_progressBar_Fill.sizeDelta = new Vector2 (m_progressBar_Fill.sizeDelta.x, newHeight);

	}

	public void SetColor (Color newColor)
	{
		m_icon.color = newColor;
		m_progressBar.color = newColor;

		Color c = m_progressBar_BG.color;
		m_progressBar_BG.color = new Color (newColor.r, newColor.g, newColor.b, c.a);
	}

	public void MouseEnter ()
	{
		m_statname.gameObject.SetActive (true);
	}

	public void MouseExit ()
	{
		m_statname.gameObject.SetActive (false);
	}
}
