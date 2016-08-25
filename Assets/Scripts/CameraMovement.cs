using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform[] targets;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 averagePosition = Vector3.zero;

		foreach(Transform t in targets) {
			averagePosition += t.position;
		}

		averagePosition /= targets.Length;

		transform.position = Vector3.MoveTowards(transform.position,averagePosition + offset, Time.deltaTime*Vector3.Distance(transform.position,averagePosition + offset));
	}
}
