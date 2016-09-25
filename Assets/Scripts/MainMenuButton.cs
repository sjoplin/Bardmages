using UnityEngine;
using System.Collections;

public class MainMenuButton : PhysicalButton {

	public float raisedHeight = 5f;

	protected override void HandleHover ()
	{
		if(transform.position.y < raisedHeight) {
			
		}
		base.HandleHover ();
	}

	protected override void HandleNormal ()
	{
		Debug.Log("NormalState");
		base.HandleNormal ();
	}

	protected override void HandlePressed ()
	{
		Debug.Log("PressedState");
		base.HandlePressed ();
	}
}
