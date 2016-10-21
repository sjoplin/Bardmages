using UnityEngine;
using System.Collections;

public class MinionTuneSpawn : MonoBehaviour, Spawnable {

	public PlayerID owner;
	public bool copy;
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
			if (!copy) {
				GameObject temp = (GameObject)GameObject.Instantiate (this.gameObject, transform.position + Vector3.right, transform.rotation);
				temp.GetComponent<MinionTuneSpawn> ().copy = true;
			}	
		} else {
			Destroy (this.gameObject, 10f);
		}
	}
	
	// Update is called once per frame
	//TODO add points to parent when minion kills
	void Update () {
		if (!(LevelManager.instance.playerDict [owner].GetComponent<PlayerLife> ().Alive)) {
			Destroy (this.gameObject);
		}
	}
}
