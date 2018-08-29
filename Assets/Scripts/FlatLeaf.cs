using UnityEngine;

/// <summary>
/// Represents a flat leaf
/// </summary>
public class FlatLeaf : Leaf {

    /// <summary>
    /// Get the volume of this leaf
    /// </summary>
    /// <returns>The volume</returns>
    public override float GetVolume() {
        Vector3 scale = this.transform.localScale;
        return scale.x * scale.y * scale.z;
    }

    /// <summary>
    /// Set the thichness, width and length of the leaf also set the mass based on size
    /// </summary>
    /// <param name="thickness">The thickness of the leaf</param>
    /// <param name="width">The width of the leaf</param>
    /// <param name="length">The length of the leaf</param>
    public override void SetSize(float thickness, float width, float length) {
        this.transform.localScale = new Vector3(width, thickness, length);
        this.GetComponent<Rigidbody>().mass = width * thickness * length;
    }
}
