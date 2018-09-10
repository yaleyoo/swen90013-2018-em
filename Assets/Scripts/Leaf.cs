using UnityEngine;

/// <summary>
/// Represents a leaf object
/// </summary>
public abstract class Leaf : MonoBehaviour {

    // Minimum movement to be considered to be moving
    private const float MOVEMENT_MINIMUM = 0.5f;
    private const int MOVEMENT_CHECK_INTERVAL = 50;
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

    /// <summary>
    /// Set the name of a leaf
    /// </summary>
    /// <param name="leafName">The name</param>
    public void SetName(string leafName) {
        this.leafName = leafName;
    }

    /// <summary>
    /// Get the name of this leaf
    /// </summary>
    /// <returns>The name</returns>
    public string GetName() {
        return this.leafName;
    }

    /// <summary>
    /// Set the thichness, width and length of the leaf also set the mass based on size
    /// </summary>
    /// <param name="thickness">The thickness</param>
    /// <param name="width">The width</param>
    /// <param name="length">The length</param>
    public abstract void SetSize(float thickness, float width, float length);

    /// <summary>
    /// Get the size of this leaf
    /// </summary>
    /// <returns>The size</returns>
    public Vector3 GetSize() {
        return this.transform.localScale;
    }

    /// <summary>
    /// Get the volume of this leaf
    /// </summary>
    /// <returns>The volume</returns>
    public abstract float GetVolume();
}
