using UnityEngine;

namespace Bardmages.AI {

    /// <summary>
    /// Aims towards the nearest player and shoots.
    /// </summary>
    class AimAI : AIController {

        /// <summary> The y coordinate that the AI wants to be under. </summary>
        private float targetY;

        /// <summary>
        /// Changes any needed settings for the AI.
        /// </summary>
        protected override void InitializeAI() {
            targetY = transform.position.y + GetComponent<Collider>().bounds.extents.y;
        }

        /// <summary>
        /// Updates the AI's actions.
        /// </summary>
        protected override void UpdateAI() {
            if (transform.position.y > targetY) {
                // Prevent pile-up at the spawn point by moving forward.
				control.currentDirection = VectorUtil.GetDirection2D(transform.forward);
            } else {
                // Check if the current move has distance constraints.
                Vector3 targetPosition = GetClosestPlayer().transform.position;
                float moveDistance = 0;
                if (bard.isPlayingTune) {
                    float targetDistance = control.GetDistance2D(targetPosition);
                    if (targetDistance > bard.currentTune.maxDistance) {
                        moveDistance = bard.currentTune.maxDistance;
                    } else if (targetDistance < bard.currentTune.minDistance) {
                        moveDistance = bard.currentTune.minDistance;
                    }
                }
                if (moveDistance != 0) {
                    // Approach if too far away from the target for the current attack.
                    Vector3 targetOffset = transform.position - targetPosition;
                    targetOffset.Normalize();
                    targetOffset *= moveDistance;
                    control.MoveToPosition(targetPosition + targetOffset);
                } else {
                    control.FacePosition(targetPosition, true);
					control.MoveToPosition(targetPosition);
                }

            }

            if (!bard.isPlayingTune) {
                bard.StartTune(ChooseTune(), false, enabledRhythms[Random.Range(0, enabledRhythms.Count)]);
            }

        }

        /// <summary>
        /// Chooses a tune to start playing.
        /// </summary>
        /// <returns>The index of the tune to play next.</returns>
        protected virtual int ChooseTune() {
            return Random.Range(0, bard.tunes.Length);
        }
    }
}