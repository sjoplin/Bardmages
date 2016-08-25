using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public PlayerID player;
	public float speed;

	private Vector2 move;
	private CharacterController charControl;

	// Use this for initialization
	void Start () {
		charControl = GetComponent<CharacterController>();
		move = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 rawInput = new Vector2(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickX, player),
			ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.LeftStickY, player));
		move = Vector2.MoveTowards(move, rawInput, Time.deltaTime);
		
		if(move.magnitude > 1) move.Normalize();

		if(charControl) charControl.Move(new Vector3(move.x,-1f,move.y)	* Time.deltaTime * speed);


		if(rawInput != Vector2.zero) {
			float targetRotation = Vector2.Angle(Vector2.left, new Vector2(-rawInput.y,rawInput.x));
			float dir = Mathf.Sign(rawInput.x);
			if(ControllerManager.instance.PlayerControlType(player) == ControllerManager.ControlType.Keyboard) {
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation*dir, Time.deltaTime*200f), transform.eulerAngles.z);
			} else {
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotation*dir, transform.eulerAngles.z);
			}
		}
	}
}
