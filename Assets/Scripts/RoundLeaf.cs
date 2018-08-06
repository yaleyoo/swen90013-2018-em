/*
 * Created by Michael Lumley
 * Subclass for a round leaf
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLeaf : Leaf {

    public override float GetVolume()
    {
        Vector3 scale = this.transform.localScale;
        return Mathf.PI * (scale.x / 2f) * (scale.y / 2f) * scale.z;
    }

    public override void SetSize(float thickness, float width, float length)
    {
        this.transform.localScale = new Vector3(width, thickness, length);

        this.GetComponent<Rigidbody>().mass = Mathf.PI * (width / 2f) * (thickness / 2f) * length * 1000;
    }
}
