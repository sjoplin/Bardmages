using UnityEngine;
using System.Collections;

public class BubbleShield : MonoBehaviour {

	public int numBlocks;

	public PlayerID owner;

	void Start() {
		Destroy(this.gameObject,1f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Attack>()) numBlocks--;
		if(numBlocks <= 0) Destroy(this.gameObject);
	}
}
