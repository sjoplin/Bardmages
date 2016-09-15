using UnityEngine;
using System.Collections;

public class Tune : MonoBehaviour {

	public ControllerInputWrapper.Buttons[] tune;
	public GameObject spawnObject;

	public int tuneProgress;
	private bool perfectTiming = true;

	public Transform ownerTransform;

    /// <summary> The name of the tune. </summary>
    public string tuneName;

    /// <summary> The percentage threshold for a note being considered on the beat. </summary>
    public const float PERFECT_THRESHOLD = 0.75f;

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

        if(LevelManager.instance.PerfectTiming() < PERFECT_THRESHOLD) {
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

    /// <summary>
    /// Determines whether the specified <see cref="Tune"/> is equal to the current <see cref="Tune"/>.
    /// </summary>
    /// <param name="other">The <see cref="Tune"/> to compare with the current <see cref="Tune"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Tune"/> is equal to the current <see cref="Tune"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(Tune other) {
        return other != null && tuneName.Equals(other.tuneName);
    }
}
