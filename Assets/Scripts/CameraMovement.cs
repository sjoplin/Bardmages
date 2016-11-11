using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform[] targets;

	private Vector3 offset;

	private Vector3 lookPosition;

	private float initialFOV;

	// Use this for initialization
	void Start () {
		offset = transform.position;
		initialFOV = GetComponent<Camera>().fieldOfView;
		lookPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 averagePosition = Vector3.zero;

		int numDead = 0;
		float maxDistance = 0f;

		for(int i = 0; i < 4; i++) {
			for(int j = 0; j < 4; j++) {

			}
		}

		foreach(Transform t in targets) {
            if(t.gameObject.activeSelf && t.GetComponent<PlayerLife>().Alive) {
				averagePosition += t.position;
			} else {
				numDead++;
			}
		}

        if (targets.Length <= numDead) {
            return;
        }

		averagePosition /= (targets.Length - numDead);

		GetComponent<Camera>().fieldOfView = LevelManager.instance.BeatValue(0f)/2f + initialFOV;
		transform.GetChild(0).GetComponent<Camera>().fieldOfView = LevelManager.instance.BeatValue(0f)/2f + initialFOV;

		transform.localPosition = Vector3.MoveTowards(transform.localPosition,averagePosition + offset, Time.deltaTime*Vector3.Distance(transform.localPosition,averagePosition + offset)/2f);
		lookPosition = Vector3.MoveTowards(lookPosition,averagePosition,Time.deltaTime*Vector3.Distance(lookPosition,averagePosition));
		transform.LookAt(lookPosition);

	}
}
