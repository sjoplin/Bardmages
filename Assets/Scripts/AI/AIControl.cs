using UnityEngine;

/// <summary>
/// Allows a player to move a bardmage.
/// </summary>
public class AIControl : BaseControl {

    /// <summary> The direction that the bardmage is moving in. </summary>
    [HideInInspector]
    public Vector2 currentDirection = Vector2.zero;

    /// <summary> Whether the bardmage is currently turning to face a position. </summary>
    private bool _isTurning;
    public bool isTurning {
        get { return _isTurning; }
    }

    /// <summary>
    /// Gets the directional input to move the bardmage with.
    /// </summary>
    /// <returns>The directional input to move the bardmage with.</returns>
    protected override Vector2 GetDirectionInput() {
        return currentDirection;
    }

    /// <summary>
    /// Checks if the bardmage turns gradually.
    /// </summary>
    /// <returns>Whether the bardmage turns gradually.</returns>
    protected override bool GetGradualTurn() {
        return true;
    }

    /// <summary>
    /// Updates control aspects of the AI.
    /// </summary>
    public void UpdateControl() {
        if (_isTurning && Vector3.Angle(GetVector2Direction(transform.forward), currentDirection) < 0.1f) {
            _isTurning = false;
            currentDirection = Vector3.zero;
        }
    }

    /// <summary>
    /// Turns to face a position.
    /// </summary>
    /// <param name="position">Position.</param>
    public void FacePosition(Vector3 position) {
        currentDirection = GetVector2Direction(position - transform.position);
        currentDirection.Normalize();
        _isTurning = true;
    }

    /// <summary>
    /// Converts a 3D vector to a 2D xz vector.
    /// </summary>
    /// <returns>The converted xz vector.</returns>
    /// <param name="vector">The 3D vector to convert.</param>
    private Vector2 GetVector2Direction(Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }
}