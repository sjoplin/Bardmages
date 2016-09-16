using UnityEngine;
using System.Collections;

public class MinionTuneSpawn : MonoBehaviour, Spawnable {

	public PlayerID owner;
	private bool crit;

	public void Crit (bool crit)
	{
		this.crit = crit;
	}
	public void Owner (PlayerID owner)
	{
		this.owner = owner;
	}

	// Use this for initialization
	void Start () {
		if (crit) {
			Destroy (this.gameObject, 10f);
		} else {
			Destroy (this.gameObject, 5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
