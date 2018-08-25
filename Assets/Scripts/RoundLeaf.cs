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
        return Mathf.PI * (scale.x / 2f) * (scale.z / 2f) * scale.y;
    }

    public override void SetSize(float longAxis, float height, float minorAxis)
    {	
		// it is an Elliptical cylinder
		// first parametre is x, corresponding to long axis
		// second is y, corresponding to height
		// third is z, corresponding to minor axis
		this.transform.localScale = new Vector3(longAxis, height, minorAxis);

		this.GetComponent<Rigidbody>().mass = Mathf.PI * (longAxis / 2f) * (minorAxis / 2f) * height;
    }
}
