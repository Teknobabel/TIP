using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Word : ScriptableObject {

	public enum WordType
	{
		None,
		Verb,
		Noun,
	}

	public string m_targetName;
	public WordType m_wordType;
	public string[] m_responses;
}
