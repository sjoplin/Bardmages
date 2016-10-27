using UnityEngine;
using System.Collections;
using Bardmages.AI;

public class MainMenuManager : MonoBehaviour {

	[SerializeField]
	private MainMenuAI[] mainMenuAI;

	/// <summary>
	/// The blocks that animate when you add a new player
	/// </summary>
	public GameObject[] playerBlocks;

	/// <summary>
	/// A list of all the tunes in the game.
	/// </summary>
	public Tune[] tunes;

	/// <summary>
	/// The array of the boxes that hold the tunes for each player
	/// </summary>
	private GameObject[,] playerTunes;

	/// <summary>
	/// The various visuals to communicate the tunes to the players
	/// </summary>
	private GameObject[] playerTuneDescriptions, playerTuneName, upIcons, downIcons, acceptIcons, rejectIcons;

	/// <summary>
	/// The selected tunes for each player.
	/// </summary>
	private int[,] selectedTune;

	/// <summary>
	/// Which tune each player is setting next
	/// </summary>
	private int[] nextTune;

	/// <summary>
	/// Silly state variable
	/// </summary>
	private bool inPlayerSelect;

	/// <summary>
	/// Ensures repeat input doesn't occurr accidentally
	/// </summary>
	private float[] inputDelay;


	#region State_Changing
	public void GoToMainMenu() {
		GetComponent<Animator>().SetInteger("State",0);
	}

	public void GoToPlayMenu() {
		GetComponent<Animator>().SetInteger("State",1);
	}

	public void GoToPlayerSelect() {
		inPlayerSelect = true;
		GetComponent<Animator>().SetInteger("State",2);
	}
	#endregion
		

	#region Unity_Code
	void Start() {
		selectedTune = new int[4,3];
		nextTune = new int[4];
		for(int i = 0; i < 4; i++) {
			nextTune[i] = 0;
			for(int j = 0; j < 3; j++) {
				selectedTune[i,j] = -1;
			}
		}

		inputDelay = new float[4];

		playerTunes = new GameObject[4,3];
		playerTuneDescriptions = new GameObject[4];
		upIcons = new GameObject[4];
		downIcons = new GameObject[4];
		acceptIcons = new GameObject[4];
		rejectIcons = new GameObject[4];

		for(int i = 0; i < 4; i++) {
			for(int j = 0; j < 3; j++) {
				playerTunes[i,j] = playerBlocks[i].transform.FindChild("Tune"+(j+1)).gameObject;
			}
		}

		for(int i = 0; i < 4; i++) {
			playerTuneDescriptions[i] = playerBlocks[i].transform.FindChild("TuneDescription").gameObject;
			upIcons[i] = playerBlocks[i].transform.FindChild("UpIcon").gameObject;
			downIcons[i] = playerBlocks[i].transform.FindChild("DownIcon").gameObject;
			acceptIcons[i] = playerBlocks[i].transform.FindChild("ControlDescription").FindChild("Button_A").gameObject;
			rejectIcons[i] = playerBlocks[i].transform.FindChild("ControlDescription").FindChild("Button_B").gameObject;
		}
	}

	void Update() {
		if(inPlayerSelect) {
			if(ControllerManager.instance.AddPlayer(ControllerInputWrapper.Buttons.Start)) {
				StopAllCoroutines();
				StartCoroutine(AddPlayerAnim());
			}

			for(int i = 0; i < ControllerManager.instance.NumPlayers; i++) {
				if(inputDelay[i] > 0f) inputDelay[i] -= Time.deltaTime;
				if(nextTune[i] < 3) {
					if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, (PlayerID)(i+1)) > 0
						&& inputDelay[i] <= 0f) {
						inputDelay[i] = 0.25f;
						selectedTune[i,nextTune[i]] = (selectedTune[i,nextTune[i]]+1) % tunes.Length;
						StartCoroutine(UpdateSelectedTune(selectedTune[i,nextTune[i]], i, true));
					}
					if(ControllerManager.instance.GetAxis(ControllerInputWrapper.Axis.DPadY, (PlayerID)(i+1)) < 0
						&& inputDelay[i] <= 0f) {
						inputDelay[i] = 0.25f;
						if(selectedTune[i,nextTune[i]] <= 0) selectedTune[i,nextTune[i]] = tunes.Length-1;
						else selectedTune[i,nextTune[i]]--;
						StartCoroutine(UpdateSelectedTune(selectedTune[i,nextTune[i]], i, false));
					}
					if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.A, (PlayerID)(i+1))) {
						nextTune[i]++;
						StartCoroutine(UpdateNextTune(i));
					}
				}
				if(ControllerManager.instance.GetButtonDown(ControllerInputWrapper.Buttons.B, (PlayerID)(i+1))) {
					if(nextTune[i] > 0) {
						nextTune[i]--;
						StartCoroutine(UpdateNextTune(i));
					}
					else {
						if(ControllerManager.instance.AllowPlayerRemoval(ControllerInputWrapper.Buttons.B) != 0) {
							StopAllCoroutines();
							StartCoroutine(AddPlayerAnim());
						}
					}
				}
			}
		}
	}
	#endregion


	#region Animation_Coroutines
	private IEnumerator UpdateNextTune(int player) {
		float timer = 0f;

		while(timer < 1f) {
			timer += Time.deltaTime*8f;
			playerTunes[player,nextTune[player]].transform.localPosition = Vector3.Lerp(playerTunes[player,nextTune[player]].transform.localPosition,
				new Vector3(0, -3f, -4.3f + 3*nextTune[player]), timer);
			upIcons[player].transform.localPosition = 
				Vector3.Lerp(upIcons[player].transform.localPosition,
					new Vector3(0f, -4f, playerTunes[player,nextTune[player]].transform.localPosition.z - 1.2f), timer);
			downIcons[player].transform.localPosition = 
				Vector3.Lerp(downIcons[player].transform.localPosition,
					new Vector3(0f, -4f, playerTunes[player,nextTune[player]].transform.localPosition.z + 0.7f), timer);
			for(int i = 0; i < 3; i++) {
				if(i != nextTune[player]) {
					playerTunes[player,i].transform.localPosition = Vector3.Lerp(playerTunes[player,i].transform.localPosition,
						new Vector3(0, -2.1f, -4.3f + 3*i), timer);
				}
			}
			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

	private IEnumerator UpdateSelectedTune(int tuneIndex, int player, bool increase) {
		float timer = 0f;

		while (timer < 1f) {
			timer += Time.deltaTime*8f;

			playerTunes[player, nextTune[player]].transform.localRotation =
				Quaternion.Lerp(playerTunes[player, nextTune[player]].transform.localRotation,
					Quaternion.Euler(increase ? -180f : 180f,0f,0f),
					timer);
			playerTuneDescriptions[player].transform.localRotation =
				Quaternion.Lerp(playerTunes[player, nextTune[player]].transform.localRotation,
					Quaternion.Euler(increase ? 270f : -270f,0f,0f),
					timer);
			yield return new WaitForEndOfFrame();
		}

		timer = 0f;
		playerTunes[player, nextTune[player]].transform.GetChild(0).GetComponent<TextMesh>().text = tunes[tuneIndex].tuneName;
		playerTuneDescriptions[player].GetComponent<TextMesh>().text = tunes[tuneIndex].tuneDescription.Replace("\\n","\n");

		while (timer < 1f) {
			timer += Time.deltaTime*8f;

			playerTunes[player, nextTune[player]].transform.localRotation =
				Quaternion.Lerp(playerTunes[player, nextTune[player]].transform.localRotation,
					Quaternion.Euler(0f,0f,0f),
					timer);
			playerTuneDescriptions[player].transform.localRotation =
				Quaternion.Lerp(playerTunes[player, nextTune[player]].transform.localRotation,
					Quaternion.Euler(-90f,0f,0f),
					timer);

			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}

	private IEnumerator AddPlayerAnim() {
		float timer = 0f;

		while(timer < 1f) {
			timer += Time.deltaTime;
			int i = 0;
			for(; i < ControllerManager.instance.NumPlayers; i++) {
				playerBlocks[i].transform.localRotation = Quaternion.Lerp(playerBlocks[i].transform.localRotation,
					Quaternion.Euler(new Vector3(-180f,0f,0f)),
					timer);
				playerTuneDescriptions[i].GetComponent<Renderer>().enabled = true;
			}
			for(; i < 4; i++) {
				playerBlocks[i].transform.localRotation = Quaternion.Lerp(playerBlocks[i].transform.localRotation,
					Quaternion.Euler(new Vector3(0f,0f,0f)),
					timer);
				playerTuneDescriptions[i].GetComponent<Renderer>().enabled = false;
			}
			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}
	#endregion

}
