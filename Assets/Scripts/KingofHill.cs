using UnityEngine;
using System.Collections;

public class KingofHill : MonoBehaviour {

	public BaseControl king = null;
	private float nextUpdate = 1;

	// Use this for initialization
	void Start () {
		this.GetComponentInChildren<ParticleSystem> ().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (king != null) {
			if (!king.GetComponent<PlayerLife> ().Alive) {
				transform.parent = null;
				this.GetComponentInChildren<MeshRenderer> ().enabled = true;
				transform.position = king.GetComponent<PlayerLife> ().PositionOfDeath;
				if (king.GetComponent<PlayerLife> ().FellOffMap) {
					transform.position = Vector3.up;
					king.GetComponent<PlayerLife> ().FellOffMap = false;
				}
				king = null;
				this.GetComponentInChildren<ParticleSystem> ().enableEmission = false;
				this.GetComponent<Rigidbody> ().velocity.Set (0, 0, 0);
				this.GetComponent<Rigidbody> ().isKinematic = false;
			} else {
				if (Time.time >= nextUpdate) {
					nextUpdate = Time.time + 1;
					UpdateEverySecond (king);
				}
			}
		}
	}

	void UpdateEverySecond(BaseControl k) {
		Debug.Log ("hello");
		Assets.Scripts.Data.RoundHandler.Instance.AddScore (k.player);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Debug.Log ("HERE");
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<BaseControl>() != null && king == null) {
			king = other.gameObject.GetComponent<BaseControl> ();
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			transform.parent = king.transform;
			this.GetComponent<Rigidbody> ().isKinematic = true;
			//TODO change color of particle emission
			this.GetComponentInChildren<ParticleSystem>().enableEmission = true;
			this.GetComponentInChildren<ParticleSystem> ().startColor = king.GetComponentInChildren<Renderer>().material.color;
		}
	}
}
