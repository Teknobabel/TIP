using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	public enum State
	{
		None,
		Active,
		Inactive,
	}

	public TextMesh m_text;
	public TextMesh m_textDropShadow;
	public MeshRenderer m_material;

	private Color m_startingColor = Color.magenta;

	private Word m_noun = null;
	private Word m_verb = null;

	private State m_state = State.Inactive;

	public Dartboard m_dartboard;

	void Start ()
	{
		Renderer r = m_material.GetComponent<Renderer> ();
		m_startingColor = r.material.color;
	}

	public void SetTarget (Word w)
	{
		m_text.text = w.m_targetName;

		if (m_textDropShadow != null) {
			m_textDropShadow.text = w.m_targetName;
		}

		if (w.m_wordType == Word.WordType.Noun) {

			m_noun = w;

		} else if (w.m_wordType == Word.WordType.Verb) {

			m_verb = w;
		}
	}

//	public void SetTarget (MenuState_GameState.Noun n)
//	{
//		m_text.text = n.ToString ();
//		m_noun = n;
//	}
//
//	public void SetTarget (MenuState_GameState.Verb v)
//	{
//		m_text.text = v.ToString ();
//		m_verb = v;
//	}

	public void ChangeState (State newState)
	{
		m_state = newState;

		switch (newState) {

		case State.Active:

			m_text.color = Color.black;
			Renderer r = m_material.GetComponent<Renderer> ();
			r.material.SetColor ("_Color", Color.yellow);

			break;
		case State.Inactive:

			m_text.color = Color.white;
			Renderer r2 = m_material.GetComponent<Renderer> ();
			r2.material.SetColor ("_Color", m_startingColor);
			m_noun = null;
			m_verb = null;
			m_dartboard.m_resultText.gameObject.SetActive (false);

			break;
		}
	}

	void OnCollisionEnter (Collision other)
	{

		Rigidbody r = other.collider.GetComponent<Rigidbody> ();

		if (r != null) {

			r.isKinematic = true;

			Dart d = other.collider.GetComponent<Dart> ();

			d.transform.SetParent (this.transform);

			d.DartLand ();

			if (m_noun != null && d.m_dartType == Word.WordType.Noun && MenuState_GameState.instance.currentNoun == null) {

				MenuState_GameState.instance.SetNoun (m_noun);

				m_dartboard.m_resultText.text = m_noun.m_targetName;
				m_dartboard.m_resultText.gameObject.SetActive (true);
				m_dartboard.m_resultText.GetComponent<Animation> ().Play ();

				if (m_state != State.Active) {
					ChangeState (State.Active);
				}

			} else if (m_verb != null && d.m_dartType == Word.WordType.Verb && MenuState_GameState.instance.currentVerb == null) {
				
				MenuState_GameState.instance.SetVerb (m_verb);

				m_dartboard.m_resultText.text = m_verb.m_targetName;
				m_dartboard.m_resultText.gameObject.SetActive (true);
				m_dartboard.m_resultText.GetComponent<Animation> ().Play ();

				if (m_state != State.Active) {
					ChangeState (State.Active);
				}
			} else if ((m_verb != null && d.m_dartType == Word.WordType.Noun) || (m_noun != null && d.m_dartType == Word.WordType.Verb) )
			{
				MenuState_GameState.instance.DartMissed (d);
			}


		}
	}

	public State currentState {get{ return m_state; }}
}
