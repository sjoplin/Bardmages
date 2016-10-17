using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

	public static EffectManager instance;

	[SerializeField]
	private GameObject deathEffect;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void SpawnDeathEffect(Vector3 position) {
		GameObject.Instantiate(deathEffect,position,Quaternion.identity);
	}

	public void SpawnRespawnEffect() {

	}
}
