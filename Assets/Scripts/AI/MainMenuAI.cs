using UnityEngine;

namespace Bardmages.AI {
    /// <summary>
    /// Randomly moving AI for testing purposes/easy opponent.
    /// </summary>
    class MainMenuAI : AIController {

		public bool leaveArena;

		private float startFighting = 5f;

        /// <summary>
        /// Updates the AI's actions.
        /// </summary>
        protected override void UpdateAI() {
            if (!bard.isPlayingTune) {

				if(startFighting > 0f) {
					startFighting -= Time.deltaTime;
					return;
				}

				if(leaveArena) {
					control.currentDirection = new Vector2(0f, -1f);
				} else {
	                bard.StartTune(Random.Range(0, 3), false, enabledRhythms[Random.Range(0, enabledRhythms.Count)]);

					control.currentDirection = (new Vector2(-transform.position.x, -transform.position.z)).normalized;
				}
            }
        }
    }
}