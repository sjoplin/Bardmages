using UnityEngine;
using System.Collections;

public class KingofHill : MonoBehaviour {

	public BaseControl king = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//king.GetComponent<PlayerLife> ().DiedLastFrame
		if (king != null && !king.GetComponent<PlayerLife>().Alive) {
			transform.parent = null;
			this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			//king.GetComponent<PlayerLife> ().DiedLastFrame = false;
			king = null;
		} 
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<BaseControl>() != null && king == null) {
			king = other.GetComponent<BaseControl> ();
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			transform.parent = king.transform;
		}
	}
}
