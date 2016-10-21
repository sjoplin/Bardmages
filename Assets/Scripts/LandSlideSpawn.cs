using UnityEngine;
using System.Collections;

public class LandSlideSpawn : MonoBehaviour, Spawnable {

	private PlayerID owner;
	private bool crit;
	private float secondRockSpawn = .3f;
	private float thirdRockSpawn = .6f;
	public GameObject rock2, rock3;

	public void Crit (bool crit) {
		this.crit = crit;
		if (crit) {
			transform.localScale = new Vector3 (1.2f, 1f, 1f);
		}
	}
	public void Owner (PlayerID owner) {
		this.owner = owner;
	}

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerControl> () != null) {
			if (other.GetComponent<PlayerControl> ().player != this.owner) {
				other.GetComponent<PlayerControl> ().Knockback (transform.forward);
				if (crit) {
					other.GetComponent<PlayerLife> ().DealDamage (.4f);
				} else {
					other.GetComponent<PlayerLife> ().DealDamage (.2f);
				}
			}
		}
	}
		
	void Update() {
		Vector3 dwn = transform.TransformDirection(Vector3.down);
		transform.Translate (0, 0, 20f * Time.deltaTime, Space.Self);
		if (!Physics.Raycast (transform.position, dwn, 2f)) {
			Destroy (this.gameObject);
		}

		secondRockSpawn -= Time.deltaTime;
		thirdRockSpawn -= Time.deltaTime;
		if (secondRockSpawn <= 0) {
			rock2.SetActive (true);
		}
		if (thirdRockSpawn <= 0) {
			rock3.SetActive (true);
		}
	}

}