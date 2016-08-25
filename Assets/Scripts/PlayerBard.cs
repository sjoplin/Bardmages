using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBard : MonoBehaviour {

	public Tune[] tunes;
	public AudioClip instrumentSound;

	private PlayerControl control;

	void Start() {
		control = GetComponent<PlayerControl>();

		Tune[] tempTunes = new Tune[tunes.Length];

		for(int i = 0; i < tunes.Length; i++) {
			Tune temp = (Tune)GameObject.Instantiate(tunes[i],Vector3.zero,Quaternion.identity);
			tempTunes[i] = temp;
			tempTunes[i].ownerTransform = transform;
			temp.transform.parent = transform;
		}

		tunes = tempTunes;
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Tune t in tunes) {
			if(ControllerManager.instance.GetButtonDown(t.NextButton(),control.player)) {

				GetComponent<AudioSource>().pitch = LevelManager.instance.buttonPitchMap[t.NextButton()];
				GetComponent<AudioSource>().PlayOneShot(instrumentSound);

				StopAllCoroutines();
				StartCoroutine(TuneTimeOut());
					
				if(t.IterateTune()) {
					foreach (Tune x in tunes) {
						x.ResetTune();
					}
				}
			}
		}
	}

	private IEnumerator TuneTimeOut() {
		yield return new WaitForSeconds(LevelManager.instance.Tempo*2f);
		foreach (Tune x in tunes) {
			x.ResetTune();
		}
		yield return null;
	}
}
