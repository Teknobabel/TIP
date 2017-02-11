using UnityEngine;
using System.Collections;

[System.Serializable]
public class Response {

	public ResponseFragment[] m_fragments;

	public enum FragmentType
	{
		String,
		Noun,
		Verb,
		Statement,
		Question,
		Noun_Capitolized,
		Verb_Capitolized,
	}

	[System.Serializable]
	public class ResponseFragment
	{
		public FragmentType m_type = FragmentType.String;

		public string m_string;
	}
}
