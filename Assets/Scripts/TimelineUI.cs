using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimelineUI : MonoBehaviour {

	public enum State {
		None,
		Past,
		Present,
		Future,
		Past_YearMark,
		Future_YearMark,
	}

	public Texture[] m_sprites;

	public RawImage m_image;

	public void SetState (State newState)
	{

		switch (newState) {

		case State.Past:
			m_image.texture = m_sprites [0];
			break;
		case State.Present:
			m_image.texture = m_sprites [1];
			break;
		case State.Future:
			m_image.texture = m_sprites [2];
			break;
		case State.Past_YearMark:
			m_image.texture = m_sprites [4];
			break;
		case State.Future_YearMark:
			m_image.texture = m_sprites [3];
			break;
		}
	}


}
