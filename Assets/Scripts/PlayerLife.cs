using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

	/// <summary>
	/// This players' health.
	/// This is normalized (0-1);
	/// </summary>
	protected float health;

	protected Vector3 positionOfDeath;

	/// <summary>
	/// The ui elements to be animated when the player is damaged
	/// </summary>
	private Image greenHealthBar, freshRedHealthBar;

	/// <summary>
	/// Handles offsetting the respawn, if the player is allowed to respawn
	/// </summary>
	private float respawnTimer;

	/// <summary>
	/// How much time should the player remain dead?
	/// Use -1 for game modes like elimination where players don't respawn automatically
	/// </summary>
	public float respawnTime;

    /// <summary> The movement component of the player. </summary>
    private BaseControl control;

    /// <summary> The controller for this player's UI health bar. </summary>
    private PlayerUIController uiController;

	/// <summary>
	/// Sets the player health to 1 and finds the appropriate UI elements
	/// </summary>
	protected virtual void Start() {
        control = GetComponent<BaseControl>();
        uiController = LevelManager.instance.GetPlayerUI(control.player);
		health = 1f;
		greenHealthBar = transform.FindChild("Canvas").FindChild("HealthBarRed").FindChild("HealthBarGreen").GetComponent<Image>();
		freshRedHealthBar = transform.FindChild("Canvas").FindChild("HealthBarRed").FindChild("HealthBarFreshRed").GetComponent<Image>();
	}

	/// <summary>
	/// Deals damage to this player.
	/// </summary>
	/// <param name="amount">The amount of damage done. All damage is normalized (1 = instakill)</param>
	public void DealDamage(float amount) {
		health -= amount;
		bool died = false;
		if(health <= 0) {
			control.ClearMomentum();
			EffectManager.instance.SpawnDeathEffect(transform.position);
			respawnTimer = respawnTime;
            control.enabled = false;
			positionOfDeath = transform.position;
			transform.position = Vector3.up*100f;
			died = true;
		}
        if(health > 1) {
            health = 1f;
        }

        updateHealthBar();

        if(uiController != null) {
            uiController.UpdateHealth(health, died);
		}
	}

    /// <summary> Updates the health bar around the player with the player's current health. </summary>
    private void updateHealthBar() {
        greenHealthBar.fillAmount = health/2f + 0.5f;
        StartCoroutine(HealthBarCatchup());
    }

	void Update() {
		if(health <= 0f && respawnTimer > 0f) {
			respawnTimer -= Time.deltaTime;
			if(respawnTimer <= 0f) {
				transform.position = Vector3.up*10f;
				health = 1f;
                GetComponent<BaseControl>().enabled = true;
                uiController.UpdateHealth(health, false);
                updateHealthBar();
			}
		}
	}

	/// <summary>
	/// Raises the controller collider hit event.
	/// </summary>
	/// <param name="hit">Hit.</param>
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.gameObject.tag.Equals("Kill")) {
			DealDamage(1f);
		}
	}

	public bool Alive {
		get {
			return health > 0;
		}
	}

	/// <summary>
	/// A coroutine that handles animating the health bar
	/// </summary>
	/// <returns>Coroutine.</returns>
	private IEnumerator HealthBarCatchup() {
		float timer = 0f;

		while (timer < 1f) {
			timer += Time.deltaTime;

			freshRedHealthBar.fillAmount = Mathf.Lerp(freshRedHealthBar.fillAmount, greenHealthBar.fillAmount, timer);

			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

	public float Health {
		get {
			return health;
		}
	}

	public Vector3 PositionOfDeath {
		get {
			return positionOfDeath;
		}
	}

}
