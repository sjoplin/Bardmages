using UnityEngine;
using System.Collections;

/// <summary>
/// A large obstacle that damages and pushes players.
/// </summary>
public class LandSlideSpawn : Attack {

    /// <summary>
    /// What should this object do if a critical play was achieved?
    /// </summary>
    /// <param name="crit">Whether or not a critical play was achieved</param>
	public override void Crit (bool crit) {
		if (crit) {
			transform.localScale = new Vector3 (1.4f, 1f, 1f);
		}
        base.Crit(crit);
	}

    /// <summary>
    /// Initializes fields depending on critical hit status.
    /// </summary>
	protected override void Start () {
        destroyAfterTime = .8f;
        damage = crit ? .4f : .2f;
        base.Start();
	}

	void Update() {
		transform.Translate (0, 0, 50f * Time.deltaTime, Space.Self);
	}
}