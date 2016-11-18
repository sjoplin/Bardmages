using UnityEngine;
using System.Collections;

public class ColiseumSpectacter : MonoBehaviour {
	// Use this for initialization
	Random rand;
	public GameObject toBeThrown;
	public GameObject spawnLoc;
	private float randomNumLim = 1000f;
	void Start () {
		rand = new Random();
	}
	
	// Update is called once per frame
	void Update () {
		int fire = 0;
		if (randomNumLim > 1) {
			randomNumLim -= Time.deltaTime;
			fire = (int)(Random.value * randomNumLim);
		} else {
			fire = 1;
		}
		if (fire == 1) {
			GameObject temp = (GameObject) Object.Instantiate (toBeThrown, spawnLoc.transform);
			temp.transform.position = spawnLoc.transform.position;
			temp.transform.rotation = spawnLoc.transform.rotation;
		}
	
	}
}
