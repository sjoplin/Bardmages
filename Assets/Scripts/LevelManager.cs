using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public PlayerUIController[] playerUI;

	public Dictionary<PlayerID, PlayerControl> playerDict;

	public Dictionary<ControllerInputWrapper.Buttons, float> buttonPitchMap;

	public AudioSource music;

	[SerializeField]
	private float tempo;
	[SerializeField]
	private float perfectLeniency;
	[SerializeField]
	private float leniencyOffset;

	private float tempoTimer;

	// Use this for initialization
	void Start () {
		instance = this;
		playerDict = new Dictionary<PlayerID, PlayerControl>();

		buttonPitchMap = new Dictionary<ControllerInputWrapper.Buttons, float>();
		buttonPitchMap.Add(ControllerInputWrapper.Buttons.A, 1f);
		buttonPitchMap.Add(ControllerInputWrapper.Buttons.B, Mathf.Pow(2,2/12f));
		buttonPitchMap.Add(ControllerInputWrapper.Buttons.Y, Mathf.Pow(2,4/12f));
		buttonPitchMap.Add(ControllerInputWrapper.Buttons.X, Mathf.Pow(2,7/12f));
	}
	
	// Update is called once per frame
	void Update () {
		tempoTimer += Time.deltaTime;
		if(tempoTimer > tempo) tempoTimer = 0f;
	}

	public float PerfectTiming () {
		float val = 2f;
		int samplesInTempo = (int)(music.clip.frequency*tempo);
		int samplesPastBeat = music.timeSamples%(samplesInTempo);
		int lenientSamples = (int)(music.clip.frequency*(perfectLeniency/2f));
		if (samplesPastBeat < lenientSamples
			|| samplesInTempo - samplesPastBeat < lenientSamples) {
			val = 0f;
		} else if ((samplesPastBeat > lenientSamples
			&& samplesPastBeat < samplesInTempo/2f)) {
			val = 1f;
		} else if ((samplesPastBeat > lenientSamples
			&& samplesPastBeat > samplesInTempo/2f)) {
			val = -1f;
		}
		return val;
	}

	public float BeatValue(float offset) {
		int samplesInTempo = (int)(music.clip.frequency*tempo);
		int samplesPastBeat = (music.timeSamples+(int)(offset*music.clip.frequency))%(samplesInTempo);

		return 1f-((float)samplesPastBeat/(float)samplesInTempo);

	}

	public void ResetTimer() {
		tempoTimer = 0f;
	}

	public float Tempo {
		get { return tempo;	}
	}

	public float Timer {
		get { return tempoTimer; }
	}
}
