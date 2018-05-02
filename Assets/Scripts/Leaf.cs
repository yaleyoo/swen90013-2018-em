/** Created by Chao Li 
 * Script for leaf object
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {
    // default gravity for leaf mass calculation
    private const float GRAVITY = 9.8f;
    // height to start detection of leaf's velocity
    private const float HEIGHT_TO_START_DETECTION = 50f;

    // name of leaf
    private string leafName;
    // current speed of the leaf
    private float speed;
    // current radius of rotation
    private float radians;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    // If leaf's velocity becomes really slow and almost stop rotating, stop the auto calculation of rigidbody
    public void OnCollisionStay(Collision collision)
    {
        speed = this.GetComponent<Rigidbody>().velocity.sqrMagnitude;
        radians = this.GetComponent<Rigidbody>().angularVelocity.magnitude;
        if (!this.GetComponent<Rigidbody>().isKinematic && speed < 0.1 && radians < 1 && this.transform.position.y < HEIGHT_TO_START_DETECTION)
        {
            if (Mathf.Abs(this.transform.rotation.eulerAngles.x) < 50 || Mathf.Abs(this.transform.rotation.eulerAngles.x) > 130)
            {
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        //if (!this.GetComponent<Rigidbody>().isKinematic)
        //{
        //    Debug.Log("Radius: " + radians + ", Speed: " + speed + ", Angle: " + this.transform.rotation.eulerAngles.x);
        //}
    }

    // Set the name of a leaf
    public void SetName(string leafName) {
        this.leafName = leafName;
    }

    // Set the thichness, width and lenth of a leaf, set the mass based on size
    public void SetSize(float thickness, float width, float length) {
        this.transform.localScale = new Vector3(width, thickness, length);
        // assume density is 1 for all leaves
        this.GetComponent<Rigidbody>().mass = width * thickness * length * 1000 / GRAVITY;
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
