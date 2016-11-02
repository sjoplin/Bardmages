using UnityEngine;
using System.Collections;

/// <summary>
/// Keeps track of the part of the floor that the object is standing on.
/// </summary>
public class UpdateParentFloor : MonoBehaviour {

    /// <summary> All floor objects in the scene. </summary>
    private static GameObject[] allFloors;

    /// <summary>
    /// Finds all floors in the scene.
    /// </summary>
    private void Start() {
        if (allFloors == null) {
            allFloors = GameObject.FindGameObjectsWithTag("Floor");
        }
    }

    /// <summary>
    /// Keeps track of the part of the floor that the object is standing on.
    /// </summary>
    protected void Update() {
        GameObject closestFloor = null;
        float nearestDistanceSquared = Mathf.Infinity;
        foreach (GameObject floor in allFloors) {
            var floorPosition = floor.transform.position;
            var currDistanceSquared = (floorPosition - transform.position).sqrMagnitude;
            if (currDistanceSquared < nearestDistanceSquared) {
                closestFloor = floor;
                nearestDistanceSquared = currDistanceSquared;
            }
        }

        if (closestFloor != null) {
            transform.parent = closestFloor.transform;
        }
    }
}
