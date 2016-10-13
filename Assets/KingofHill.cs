using UnityEngine;
using System.Collections;

public class KingofHill : MonoBehaviour {

	public PlayerBard king = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//king.GetComponent<PlayerLife> ().DiedLastFrame
		if (king.GetComponent<PlayerLife> ().Health <= 0) {
			transform.parent = null;
			this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			//king.GetComponent<PlayerLife> ().DiedLastFrame = false;
			king = null;
		} 
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerControl>() != null && king == null) {
			king = other.GetComponent<PlayerBard> ();
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			transform.parent = king.transform;
		}
	}
}
