using UnityEngine;
using System.Collections;

public class SnipeAttack : Attack {

	protected override void Start ()
	{
		base.Start();
		GetComponent<Rigidbody>().AddForce(transform.forward*100f, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool sphereCast = true;
		RaycastHit hitInf = new RaycastHit();
		if(Physics.Raycast(new Ray(transform.position,transform.forward),out hitInf)) {
			if(hitInf.collider.transform.root.GetComponent<PlayerControl>()) {
				sphereCast = false;
			}
		}
		if(sphereCast) {
			bool sphereHit = false;
			Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward*10f,10f);
			foreach(Collider c in cols) {
				if(c.transform.root.GetComponent<PlayerControl>() && this.agressor != c.transform.root.GetComponent<PlayerControl>().player) {
					GetComponent<Rigidbody>().AddForce(((c.transform.root.position - transform.position) - transform.forward*2f).normalized*1000f, ForceMode.Acceleration);
					sphereHit = true;
				}
			}
		}
		if(GetComponent<Rigidbody>().velocity != Vector3.zero) {
			transform.forward = GetComponent<Rigidbody>().velocity;
		}
	}
}
