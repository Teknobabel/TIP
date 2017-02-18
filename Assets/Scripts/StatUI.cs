﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StatUI : MonoBehaviour {

	public RectTransform m_progressBar_Fill;

	public RawImage 
	m_icon,
	m_progressBar,
	m_progressBar_BG,
	m_iconBorder;

	public TextMeshProUGUI m_statname;

	public Stat m_stat;

	public void UpdateStatValue (int newValue, bool doAnimate)
	{
		Rect r = m_progressBar_Fill.rect;
		r.height = newValue;

		float maxHeight = m_progressBar_BG.rectTransform.rect.size.y;
		float modifier = ((float)m_stat.currentScore) / ((float)m_stat.maxScore);
		float newHeight = maxHeight * modifier;

		if (doAnimate) {
			DOTween.To (() => m_progressBar_Fill.sizeDelta, x => m_progressBar_Fill.sizeDelta = x, new Vector2 (m_progressBar_Fill.sizeDelta.x, newHeight), 1);
		} else {
			m_progressBar_Fill.sizeDelta = new Vector2 (m_progressBar_Fill.sizeDelta.x, newHeight);
		}

	}

	public void SetColor (Color newColor)
	{
		m_icon.color = newColor;
		m_progressBar.color = newColor;
		m_iconBorder.color = newColor;

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