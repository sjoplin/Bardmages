using UnityEngine;
using System.Collections;

public class BubbleShield : MonoBehaviour, Spawnable {

	public int numBlocks;

	public PlayerID owner;

	void Start() {
		Destroy(this.gameObject,1f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Attack>()) numBlocks--;
		if(numBlocks <= 0) Destroy(this.gameObject);
	}

	public void Crit(bool value) {
		if(value) {
			transform.GetChild(0).gameObject.SetActive(true);
			Collider[] colliders = Physics.OverlapSphere(transform.position, 15f);
			foreach (Collider c in colliders) {
				if(c.transform.root.GetComponent<PlayerControl>()
					&& c.transform.root.GetComponent<PlayerControl>().player != owner) {
					c.transform.root.GetComponent<PlayerControl>().Knockback(c.transform.root.position - transform.position);
				}
			}
		}
	}

	public void Owner(PlayerID owner) {
		this.owner = owner;
	}
}
