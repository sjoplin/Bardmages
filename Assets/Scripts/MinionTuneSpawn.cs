﻿using UnityEngine;
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

        // Switch the minion's robe color to the color of its owner.
        Transform[] robes = new Transform[2];
        robes[0] = transform.FindChild("bardmage_export").FindChild("pCube2");
        robes[1] = transform.FindChild("bardmage_export").FindChild("pCube3");

        Material robeMaterial = LevelManager.instance.playerDict[owner].transform.FindChild("bardmage_export").FindChild("pCube2").GetComponent<Renderer>().material;
        foreach (Transform robe in robes) {
            robe.GetComponent<Renderer>().material = robeMaterial;
        }
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
		if (LevelManager.instance.playerDict [owner].GetComponent<PlayerLife> ().Health <= 0) {
			Destroy (this.gameObject);
		}
	}
}
