using UnityEngine;
using System.Collections;

/// <summary>
/// The target object for King of the Hill.
/// </summary>
public class KingofHill : MonoBehaviour {

	public BaseControl king = null;

    /// <summary> The position of the hill at the start of the game. </summary>
    private Vector3 spawnPosition;

	/// <summary>
    /// Initializes the hill.
    /// </summary>
	void Start () {
		this.GetComponentInChildren<ParticleSystem> ().enableEmission = false;
        spawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (king != null && !king.GetComponent<PlayerLife>().Alive) {
			transform.parent = null;
			this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			transform.position = king.GetComponent<PlayerLife>().PositionOfDeath;
			king = null;
			this.GetComponentInChildren<ParticleSystem>().enableEmission = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 50) && hit.collider.tag == "Kill") {
                transform.position = spawnPosition;
            }
		} 
	}

    /// <summary>
    /// Allows players to pick up the hill.
    /// </summary>
    /// <param name="other">The collider that hit the hill.</param>
	void OnTriggerEnter(Collider other) {
        if (other.GetComponent<BaseControl>() != null && other.GetComponent<MinionTuneSpawn>() == null && king == null) {
			king = other.GetComponent<BaseControl> ();
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			transform.parent = king.transform;
			this.GetComponentInChildren<ParticleSystem>().enableEmission = true;
		}
	}
}
