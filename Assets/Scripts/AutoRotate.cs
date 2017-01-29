using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

	public float m_rotateSpeed = 100;
	public Transform m_rotateObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		m_rotateObject.Rotate (Vector3.forward * Time.deltaTime * m_rotateSpeed);
	}
}
