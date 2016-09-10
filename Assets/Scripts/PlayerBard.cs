using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBard : MonoBehaviour {

	public Tune[] tunes;
	public AudioClip instrumentSound;

	public float buttonPressDelay = 0.1f;
	private float buttonPressDelayTimer;

	public float volumeOverride;

	private PlayerControl control;

	private List<Tune> currentTunes;

	void Start() {
		control = GetComponent<PlayerControl>();

		Tune[] tempTunes = new Tune[tunes.Length];
		currentTunes = new List<Tune>();

		for(int i = 0; i < tunes.Length; i++) {
			Tune temp = (Tune)GameObject.Instantiate(tunes[i],Vector3.zero,Quaternion.identity);
			tempTunes[i] = temp;
			tempTunes[i].ownerTransform = transform;
			temp.transform.parent = transform;
		}

		tunes = tempTunes;

		if(volumeOverride != 0) GetComponent<AudioSource>().volume = volumeOverride;
	}
	
	// Update is called once per frame
	void Update () {
		if(buttonPressDelayTimer < 0f) {
			bool soundPlayed = false; //prevents two sounds from being played the same frame
			if(currentTunes.Count > 0) {
				//This makes sure that if we have started a tune, that we only continue iterating that tune
				for(int i = 0; i < currentTunes.Count; i++) {
					if(ControllerManager.instance.GetButtonDown(currentTunes[i].NextButton(),control.player)) {
						if(!soundPlayed) {
							GetComponent<AudioSource>().pitch = LevelManager.instance.buttonPitchMap[currentTunes[i].NextButton()];
							GetComponent<AudioSource>().PlayOneShot(instrumentSound, volumeOverride);
							soundPlayed = true;
						}
						IterateTune(currentTunes[i]);
						for (int j = 0; j < currentTunes.Count; j++) {
							if(j != i && !ControllerManager.instance.GetButton(currentTunes[j].NextButton(),control.player)) {
								i = 0;
								currentTunes.Remove(currentTunes[j]);
								break;
							}
						}
					}
				}
			} else {
				foreach(Tune t in tunes) {
					if(ControllerManager.instance.GetButtonDown(t.NextButton(),control.player)) {
						if(!currentTunes.Contains(t)) currentTunes.Add(t);
						if(!soundPlayed) {
							GetComponent<AudioSource>().pitch = LevelManager.instance.buttonPitchMap[t.NextButton()];
							GetComponent<AudioSource>().PlayOneShot(instrumentSound, volumeOverride);
							soundPlayed = true;
						}
						IterateTune(t);
					}
				}
			}
		} else {
			foreach(Tune t in tunes) {
				if(ControllerManager.instance.GetButtonDown(t.NextButton(),control.player)) {
					StopAllCoroutines();
					foreach (Tune x in tunes) {
						x.ResetTune();
						LevelManager.instance.playerUI[(int)control.player - 1].TuneReset();
					}
				}
			}
			buttonPressDelayTimer -= Time.deltaTime;
		}
	}

	private IEnumerator TuneTimeOut() {
		yield return new WaitForSeconds(LevelManager.instance.Tempo*2f);
		foreach (Tune x in tunes) {
			x.ResetTune();
			LevelManager.instance.playerUI[(int)control.player - 1].TuneReset();
			currentTunes.Clear();
		}
		yield return null;
	}

	private void IterateTune(Tune t) {
		buttonPressDelayTimer = buttonPressDelay;

		LevelManager.instance.playerUI[(int)control.player - 1].TuneProgressed(t);

		StopAllCoroutines();
		StartCoroutine(TuneTimeOut());

		if(t.IterateTune()) {
			foreach (Tune x in tunes) {
				x.ResetTune();
				LevelManager.instance.playerUI[(int)control.player - 1].TuneReset();
				currentTunes.Clear();
			}
		}
	}
}
