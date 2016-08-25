using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grenade : Attack {

	float timer = 2f;

	void Start() {
		GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up)*3f, ForceMode.VelocityChange);
	}

	void Update() {
		if(timer > 0f) {
			timer -= Time.deltaTime;
		} else {
			List<SphereCollider> cols = new List<SphereCollider>();
			GetComponents<SphereCollider>(cols);
			foreach(SphereCollider s in cols) s.enabled = true;
			transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			Destroy(transform.GetChild(0),1f);
			transform.GetChild(0).parent = null;
			Destroy(this.gameObject,0.1f);
			timer = 9999f;
		}
		
	}

	protected override void OnTriggerEnter (Collider other)
	{
		base.OnTriggerEnter (other);


	}
	
}
