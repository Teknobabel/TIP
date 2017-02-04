using UnityEngine;
using System.Collections;

public class KillVolume : MonoBehaviour {

	void OnCollisionEnter (Collision other)
	{

		Rigidbody r = other.collider.GetComponent<Rigidbody> ();

		if (r != null) {

			r.isKinematic = true;

			Dart d = other.collider.GetComponent<Dart> ();

			d.transform.SetParent (this.transform);

			d.DartMiss ();

			MenuState_GameState.instance.DartMissed (d);
		}
	}
}
