using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	public enum State
	{
		None,
		Moving,
		Waiting,
	}

	public float
	m_minSpeed = 1,
	m_maxSpeed = 1,
	m_minSpeedChangeTime = 1,
	m_maxSpeedChangeTime = 5;

	public Animation 
	m_anim,
	m_throwAnim;

	public GameObject m_dart;

	public Transform m_dartParent;

	public Transform[]
	m_handStates;

	private GameObject m_currentDart  = null;

	private State m_state = State.Moving;

	private float 
	m_timer = 0.0f,
	m_nextSpeedChange = 1.0f;

	// Use this for initialization
	void Start () {

		m_anim["Hand_Movement01"].speed = Random.Range (m_minSpeed, m_maxSpeed);
		m_nextSpeedChange = Random.Range (m_minSpeedChangeTime, m_maxSpeedChangeTime);

		m_throwAnim["Hand_Throw01"].speed = Random.Range (0.75f, 1.0f);

	}

	public void ChangeState (State newState)
	{
		m_state = newState;

		switch (newState) {

		case State.Moving:
			m_anim ["Hand_Movement01"].speed = Random.Range (m_minSpeed, m_maxSpeed);
			m_handStates [0].gameObject.SetActive (true);
			m_handStates [1].gameObject.SetActive (false);
			break;
		case State.Waiting:
			m_anim ["Hand_Movement01"].speed = 0;
			m_handStates [0].gameObject.SetActive (false);
			m_handStates [1].gameObject.SetActive (true);
			break;
		}
	}

	public void SpawnDart ()
	{
		m_currentDart = (GameObject)Instantiate (m_dart, m_dartParent);
		m_currentDart.transform.localPosition = Vector3.zero;

		m_throwAnim["Hand_Throw01"].speed = Random.Range (0.75f, 1.3f);

	}

	public void ThrowDart ()
	{
		Rigidbody r = m_currentDart.GetComponent<Rigidbody> ();

		if (r != null) {

			m_currentDart.transform.SetParent (null);

			r.isKinematic = false;

			r.AddForce (Vector3.right * 1000);

			m_currentDart.GetComponent<Dart> ().DartThrow ();
		}

		m_throwAnim.Play ();

		ChangeState (State.Waiting);
	}

	public void KillDart ()
	{
		if (m_currentDart != null) {

			Destroy (m_currentDart.gameObject);
			m_currentDart = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_state == State.Moving) {
			
			m_timer += Time.deltaTime;

			if (m_timer >= m_nextSpeedChange) {

				m_timer = 0.0f;
				m_nextSpeedChange = Random.Range (m_minSpeedChangeTime, m_maxSpeedChangeTime);
				m_anim ["Hand_Movement01"].speed = Random.Range (m_minSpeed, m_maxSpeed);
			}
		}

	}

	public State currentState {get{ return m_state;}}
}
