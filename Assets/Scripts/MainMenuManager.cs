using UnityEngine;
using System.Collections;
using Bardmages.AI;

public class MainMenuManager : MonoBehaviour {

	[SerializeField]
	private MainMenuAI[] mainMenuAI;

	private bool inPlayerSelect;

	public void GoToMainMenu() {
		GetComponent<Animator>().SetInteger("State",0);
	}

	public void GoToPlayMenu() {
		GetComponent<Animator>().SetInteger("State",1);
	}

	public void GoToPlayerSelect() {
		foreach(MainMenuAI ai in mainMenuAI) {
			ai.leaveArena = true;
		}
		inPlayerSelect = true;
	}

	void Update() {
		if(inPlayerSelect) {
			ControllerManager.instance.AddPlayer(ControllerInputWrapper.Buttons.A);
			ControllerManager.instance.AllowPlayerRemoval(ControllerInputWrapper.Buttons.B);
		}
	}

	public void LeavePlayerSelect() {
		foreach(MainMenuAI ai in mainMenuAI) {
			ai.leaveArena = false;
		}
		inPlayerSelect = true;
	}

}
