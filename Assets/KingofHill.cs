using UnityEngine;
using System.Collections;

public class KingofHill : MonoBehaviour {

	public PlayerBard king = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (king.GetComponent<PlayerLife>().Health <= 0) {
			this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			transform.parent = null;
			king = null;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerControl>() != null && king == null) {
			king = other.GetComponent<PlayerBard> ();
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			//transform.parent = other.gameObject.transform;
			transform.parent = king.transform;
		}
	}
}
