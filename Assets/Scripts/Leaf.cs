﻿/* Created by Chao Li
 * Modified by Michael Lumley
 * Script for leaf object
 */

using UnityEngine;

public abstract class Leaf : MonoBehaviour {
    // Minimum movement to be considered to be moving
    private const float MOVEMENT_MINIMUM = 0.5f;
    // Number of updates to wait before checking movement
    private const int MOVEMENT_CHECK_INTERVAL = 50;

    // name of leaf
    private string leafName;

    private int tick = 0;

    // Update is called once per frame
    void Update() {
        // Every MOVEMENT_CHECK_INTERVAL updates check if the leaf is moving
        if (tick == MOVEMENT_CHECK_INTERVAL) {
            float speed = this.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            float angularVelocity = this.GetComponent<Rigidbody>().angularVelocity.sqrMagnitude;

            // If the leaf is not moving disable physics movement
            if (!IsMoving(speed, angularVelocity)) {
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
            tick = 0;
        }
        else {
            tick++;
        }

        // If the leaf has fallen below the ground delete itself
        if (this.transform.position.y < -10) {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Check if a leaf is moving or not based on it's speed and angular velocity
    /// is above a minimum threshold.
    /// </summary>
    /// <param name="speed">The speed of the object</param>
    /// <param name="angularVelocity">The anglar velocity of the object</param>
    /// <returns>Is object moving</returns>
    public bool IsMoving(float speed, float angularVelocity) {

        if (speed < MOVEMENT_MINIMUM && angularVelocity < MOVEMENT_MINIMUM) {
            return false;
        }
        else {
            return true;
        }
    }

    // Set the name of a leaf
    public void SetName(string leafName) {
        this.leafName = leafName;
    }

    // Get the name of this leaf
    public string GetName() {
        return this.leafName;
    }

    // Set the thichness, width and lenth of a leaf, set the mass based on size
    public abstract void SetSize(float thickness, float width, float length);

    // Get the size of this leaf
    public Vector3 GetSize() {
        return this.transform.localScale;
    }

    // Get the volume of this leaf
    public abstract float GetVolume();
}
