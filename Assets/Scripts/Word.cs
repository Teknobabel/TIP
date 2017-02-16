﻿using UnityEngine;
using System.Collections;


[CreateAssetMenu()]
public class Word : ScriptableObject {

	public enum WordType
	{
		None,
		Verb,
		Noun,
	}

	public enum WordCategory
	{
		General,
		Law,
		Captialism,
		Democracy,
		Culture,
	}

	public enum WordQuality
	{
		Neutral,
		Positive,
		Negative,
	}



	[System.Serializable]
	public class Affinity
	{
		public WordCategory m_category = WordCategory.General;
		public WordQuality m_quality = WordQuality.Neutral;
	}

	public string m_targetName;
	public WordType m_wordType;
//	public string[] m_responses;
	public Response[] m_responseBank;
	public Affinity[] m_affinities;
	public bool m_proper = false;
	public bool m_random = false;
}
