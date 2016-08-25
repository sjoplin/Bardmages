using UnityEngine;
using System.Collections;

public class LoopDetector : MonoBehaviour {

	private float prevTime, curTime;
	
	void Update () {
		curTime = GetComponent<AudioSource>().time;

//		if(prevTime > curTime) LevelManager.instance.ResetTimer();

//		Debug.Log(GetComponent<AudioSource>().timeSamples + " " + GetComponent<AudioSource>().time);

		prevTime = curTime;
	}
}
