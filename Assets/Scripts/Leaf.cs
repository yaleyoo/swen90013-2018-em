/** Created by Chao Li 
 * Script for leaf object
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {

    private const float GRAVITY = 9.8f;
    private string leafName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    // Set the name of a leaf
    public void SetName(string leafName) {
        this.leafName = leafName;
    }

    // Set the thichness, width and lenth of a leaf, set the mass based on size
    public void SetSize(float thickness, float width, float length) {
        this.transform.localScale = new Vector3(width, thickness, length);
        // assume density is 1 for all leaves
        this.GetComponent<Rigidbody>().mass = width * thickness * length / GRAVITY;
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
}
