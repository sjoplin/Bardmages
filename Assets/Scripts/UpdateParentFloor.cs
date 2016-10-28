using UnityEngine;
using System.Collections;

public class UpdateParentFloor : MonoBehaviour {


    protected void Update() {
        GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
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

        transform.parent = closestFloor.transform;
    }
}
