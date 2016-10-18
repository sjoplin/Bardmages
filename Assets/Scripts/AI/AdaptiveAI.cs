using UnityEngine;

namespace Bardmages.AI {

    /// <summary>
    /// Adapts to how well the player is doing and which attacks are doing better.
    /// </summary>
    class AdaptiveAI : AimAI {

        /// <summary>
        /// Updates the AI's actions.
        /// </summary>
        protected override void UpdateAI() {
            bard.timingAccuracy = LevelManager.instance.perfectNoteChance;
            base.UpdateAI();
        }
    }
}