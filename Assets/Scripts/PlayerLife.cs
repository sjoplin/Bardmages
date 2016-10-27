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
    /// (DEPRECATED)Handles offsetting the respawn, if the player is allowed to respawn
    /// </summary>
    private float respawnTimer;

    /// <summary>
    /// (DEPRECATED)How much time should the player remain dead?
    /// Use -1 for game modes like elimination where players don't respawn automatically
    /// </summary>
    public float respawnTime;

    /// <summary>
    /// Sets the player health to 1 and finds the appropriate UI elements
    /// </summary>

    public bool fellOffMap;
    public bool roundHandler;
    protected virtual void Start() {
        roundHandler = Assets.Scripts.Data.RoundHandler.Instance != null;
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
			GetComponent<BaseControl>().ClearMomentum();
			if (this.GetComponent<BaseControl>().player != PlayerID.None) {
				EffectManager.instance.SpawnDeathEffect (transform.position);
			}
			GetComponent<BaseControl>().enabled = false;
			positionOfDeath = transform.position;
			transform.position = Vector3.up*100f;
			died = true;
            if(roundHandler)
                Assets.Scripts.Data.RoundHandler.Instance.AddDeath();
            else
                respawnTimer = respawnTime;
        }
        if (health > 1) {
            health = 1f;
        }

		greenHealthBar.fillAmount = health/2f + 0.5f;
		StartCoroutine(HealthBarCatchup());

        PlayerUIController uiController = LevelManager.instance.GetPlayerUI(GetComponent<BaseControl>().player);
        if(uiController != null) {
            uiController.UpdateHealth(health, died);
		}
	}

    void Update()
    {
        if (!roundHandler && (health <= 0f && respawnTimer > 0f))
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                transform.position = Vector3.up * 10f;
                health = 1f;
                GetComponent<BaseControl>().enabled = true;
            }
        }
    }

    /// <summary> Re-initialize the player health and re-enable control. </summary>
    public void Respawn()
    {
        health = 1f;
        PlayerUIController uiController = LevelManager.instance.GetPlayerUI(GetComponent<BaseControl>().player);
        if (uiController != null)
        {
            uiController.UpdateHealth(health, false);
        }
    }

	/// <summary>
	/// Raises the controller collider hit event.
	/// </summary>
	/// <param name="hit">Hit.</param>
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.gameObject.tag.Equals("Kill")) {
			fellOffMap = true;
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

	public bool FellOffMap {
		get {
			return fellOffMap;
		}
		set {
			fellOffMap = value;
		}
	}
}
