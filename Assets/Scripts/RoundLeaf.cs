using UnityEngine;

/// <summary>
/// Represents a round leaf
/// </summary>
public class RoundLeaf : Leaf {

    /// <summary>
    /// Get the volume of this leaf
    /// </summary>
    /// <returns>The volume</returns>
    public override float GetVolume() {
        Vector3 scale = this.transform.localScale;
        return Mathf.PI * (scale.x / 2f) * (scale.z / 2f) * scale.y;
    }

    /// <summary>
    /// Set the thichness, width and length of the leaf also set the mass based on size
    /// </summary>
    /// <param name="longAxis">The longAxis of the cylinder</param>
    /// <param name="height">The height of the cylinder</param>
    /// <param name="minorAxis">The minorAxis of the cylinder</param>
    public override void SetSize(float longAxis, float height, float minorAxis) {	
		this.transform.localScale = new Vector3(longAxis, height, minorAxis);
		this.GetComponent<Rigidbody>().mass = Mathf.PI * (longAxis / 2f) * (minorAxis / 2f) * height;
    }
}
