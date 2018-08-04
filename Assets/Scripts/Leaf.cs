/** Created by Chao Li 
 * Script for leaf object
﻿/** Created by Chao Li 
 * Script for leaf object
**/

using UnityEngine;

public class Leaf : MonoBehaviour {
    // default gravity for leaf mass calculation
    private const float GRAVITY = 9.8f;
    // height to start detection of leaf's velocity
    private const float HEIGHT_TO_START_DETECTION = 50f;
    // Minimum movement to be considered to be moving
    private const float MOVEMENT_MINIMUM = 0.5f;
    // Number of updates to wait before checking movement
    private const int MOVEMENT_CHECK_INTERVAL = 50;

    // name of leaf
    private string leafName;

    private int tick = 0;

    // height of the top leaf
    private static float height = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Every MOVEMENT_CHECK_INTERVAL updates check if the leaf is moving
        if (tick == MOVEMENT_CHECK_INTERVAL) {
            float speed = this.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            float angularVelocity = this.GetComponent<Rigidbody>().angularVelocity.sqrMagnitude;

            // If the leaf is not moving disable physics movement
            if (!CheckIfMoving(speed, angularVelocity)) {
                this.GetComponent<Rigidbody>().isKinematic = true;
                // If this object is highest, log the y value when turn on the Kinematic
                if (transform.position.y > Leaf.height) {
                    Leaf.height = transform.position.y;
                }
            }
            tick = 0;
        }
        else {
            tick++;
        }

    }

    /// <summary>
    /// Check if a leaf is moving or not based on it's speed and angular velocity
    /// is above a minimum threshold.
    /// </summary>
    /// <param name="speed">The speed of the object</param>
    /// <param name="angularVelocity">The anglar velocity of the object</param>
    /// <returns>Is object moving</returns>
    public bool CheckIfMoving(float speed, float angularVelocity) {

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

    // Set the thichness, width and lenth of a leaf, set the mass based on size
    public void SetSize(float thickness, float width, float length) {
        this.transform.localScale = new Vector3(width, thickness, length);

        // Calculate the mass of the leaf
        // assume density is 1 for all leaves

        // The leaf has a cylinder shape
        if (this.GetComponent<CapsuleCollider>()) {
            this.GetComponent<Rigidbody>().mass = Mathf.PI * width * thickness * length * 1000 / GRAVITY;
        }
        else {
            this.GetComponent<Rigidbody>().mass = width * thickness * length * 1000 / GRAVITY;
        }        
    }

    // Set the rotation of a leaf
    public void SetRotation(float x, float y, float z) {
        this.transform.rotation = Quaternion.Euler(x, y, z);
    }

    // Get the name of this leaf
    public string GetName() {
        return leafName;
    }

    // Get the size of this leaf
    public Vector3 GetSize() {
        return this.transform.localScale;
    }

    // Get the current position of this leaf
    public Vector3 GetPosition() {
        return this.transform.position;
    }

    // Get the height of the top leaf
    public float GetHeight()
    {
        return Leaf.height;
    }
}
