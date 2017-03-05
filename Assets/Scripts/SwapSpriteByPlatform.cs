using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwapSpriteByPlatform : MonoBehaviour {

	public Texture 
	m_sprite_iOS,
	m_sprite_standAlone;

	public RawImage
	m_image;

	// Use this for initialization
	void Start () {
	
		if (m_image != null) {

			#if UNITY_IOS

			if (m_sprite_iOS != null)
			{
				m_image.texture = m_sprite_iOS;
			}

			#else

			if (m_sprite_standAlone != null)
			{
				m_image.texture = m_sprite_standAlone;
			}

			#endif
		}

	}
}
