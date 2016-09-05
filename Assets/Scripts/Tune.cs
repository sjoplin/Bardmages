using UnityEngine;
using System.Collections;

public class Tune : MonoBehaviour {

	public ControllerInputWrapper.Buttons[] tune;
	public GameObject spawnObject;

	public int tuneProgress;
	private bool perfectTiming = true;

	public Transform ownerTransform;

	/// <summary>
	/// What should happen when the tune completes?
	/// </summary>
	/// <param name="crit">Was the tune played perfectly?</param>
	public virtual void TuneComplete(bool crit) {	}

	/// <summary>
	/// What is the next button in the tune?
	/// </summary>
	/// <returns>The button.</returns>
	public ControllerInputWrapper.Buttons NextButton() {
		return tune[tuneProgress];
	}

	/// <summary>
	/// Iterates the tune;
	/// </summary>
	/// <returns><c>true</c>, if tune was finished, <c>false</c> otherwise.</returns>
	public bool IterateTune() {
		tuneProgress++;

		if(LevelManager.instance.PerfectTiming() < 0.75f) {
			perfectTiming = false;
		}

		if(tuneProgress == tune.Length) {
			TuneComplete(perfectTiming);
			perfectTiming = true;
			return true;
		}
		return false;
	}

	/// <summary>
	/// Resets the tune.
	/// </summary>
	public void ResetTune() {
		tuneProgress = 0;
		perfectTiming = true;
	}
}
