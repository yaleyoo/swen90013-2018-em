using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangularLeaf : Leaf
{
    public override float GetVolume()
    {
        Vector3 scale = this.transform.localScale;
        return scale.x * scale.y * scale.z;
    }

    public override void SetSize(float thickness, float width, float length)
    {
        this.transform.localScale = new Vector3(width, thickness, length);

        this.GetComponent<Rigidbody>().mass = width * thickness * length * 1000 / GRAVITY;
    }
}
