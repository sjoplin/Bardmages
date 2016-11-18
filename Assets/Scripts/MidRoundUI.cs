using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MidRoundUI : MonoBehaviour {

	public RectTransform[] playerScoreBoxes;

	private static int playerWinner;
	private static bool updateUI;

	void Start() {
		for(int i = 0; i < 4; i++) {
			for(int j = 0; j < 10; j++) { 
				playerScoreBoxes[i].GetChild(j).GetChild(0).GetComponent<Image>().CrossFadeAlpha(0f,2f,true);
			}
		}
		Assets.Scripts.Data.RoundHandler.Instance.RegisterUI(ShowUI);
		Assets.Scripts.Data.RoundHandler.Instance.RegisterUI(ColiseumSpectacter.Reset);
	}

	void Update() {
//		for(int i = 1; i < 5; i++) {
//			if(Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode),"Alpha"+i))) {
//				Assets.Scripts.Data.RoundHandler.Instance.AddScore((PlayerID)i);
//				AddScore(i-1);
//			}
//		}
		if(updateUI && playerWinner != -1) {
			StartCoroutine(OpenUIAnim());
			updateUI = false;
		}
	}

	public static void ShowUI(int player) {
		playerWinner = player;
		updateUI = true;
	}

	public void AddScore(int player) {
		StartCoroutine(AddScoreAnim(player));
	}

	private IEnumerator OpenUIAnim() {
		float timer = 0f;

		while(timer < 1f) {
			timer += Time.deltaTime;

//			GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(1024f,0f), new Vector2(1024f,576f),timer);
			GetComponent<CanvasGroup>().alpha = timer;

			yield return new WaitForEndOfFrame();
		}

		AddScore(playerWinner);

		yield return null;
	}

	private IEnumerator CloseUIAnim() {
		float timer = 0f;

		while(timer < 1f) {
			timer += Time.deltaTime;

//			GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(1024f,576f), new Vector2(1024f,0f),timer);
			GetComponent<CanvasGroup>().alpha = 1-timer;

			yield return new WaitForEndOfFrame();
		}

		Assets.Scripts.Data.RoundHandler.Instance.BeginRound();

		yield return null;
	}

	private IEnumerator AddScoreAnim(int player) {
		float timer = 0f;
		playerScoreBoxes[player].GetChild(Assets.Scripts.Data.RoundHandler.Instance.Scores[player]-1).GetChild(0).GetComponent<Image>().CrossFadeAlpha(1f,2f,true);
		RectTransform tempRect = playerScoreBoxes[player].GetChild(Assets.Scripts.Data.RoundHandler.Instance.Scores[player]-1).GetChild(0).GetComponent<RectTransform>();
		while (timer < 1f) {
			timer += Time.deltaTime;

			tempRect.localScale = Vector3.Lerp(Vector3.one*2f,Vector3.one,timer);
			tempRect.localEulerAngles = new Vector3(0f,0f,Mathf.Lerp(360f, 0f, timer));

			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(2f);

		StartCoroutine(CloseUIAnim());

		yield return null;
	}
}
