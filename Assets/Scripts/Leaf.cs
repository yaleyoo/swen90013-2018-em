/** Created by Chao Li 
 * Script for leaf object
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    // default gravity for leaf mass calculation
    private const float GRAVITY = 9.8f;
    // height to start detection of leaf's velocity
    private const float HEIGHT_TO_START_DETECTION = 50f;

    // name of leaf
    private string leafName;

    private int tick = 0;
    private Vector3 position_i;
    private Quaternion rotation_i;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tick > 0 && tick < 50)
        {
            tick++;
        }
        else if (tick == 50)
        {
            float delta = Mathf.Abs(transform.rotation.x - rotation_i.x) + Mathf.Abs(transform.rotation.y - rotation_i.y)
                + Mathf.Abs(transform.rotation.z - rotation_i.z);
            if ((transform.position - position_i).sqrMagnitude < 0.5 && delta < 0.5f)
            {
                this.GetComponent<Rigidbody>().isKinematic = true;
                //this.GetComponent<Renderer>().material.color = Color.blue;
                tick++;
            }
            else
            {
                tick = 0;
            }
        }

    }

    public void OnCollisionEnter(Collision collision)
    {

        if (tick == 0 && this.transform.position.y < HEIGHT_TO_START_DETECTION)
        {
            float speed = this.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            float radians = this.GetComponent<Rigidbody>().angularVelocity.sqrMagnitude;            
            if (collision.gameObject.tag.Equals("Ground") || (!this.GetComponent<Rigidbody>().isKinematic && speed < 0.2 && radians < 1))
            {
                position_i = transform.position;
                rotation_i = transform.rotation;
                tick++;
            }

        }
    }

    // Set the name of a leaf
    public void SetName(string leafName)
    {
        this.leafName = leafName;
    }

    // Set the thichness, width and lenth of a leaf, set the mass based on size
    public void SetSize(float thickness, float width, float length)
    {
        this.transform.localScale = new Vector3(width, thickness, length);
        // assume density is 1 for all leaves
        this.GetComponent<Rigidbody>().mass = width * thickness * length * 1000 / GRAVITY;
    }

    // Set the rotation of a leaf
    public void SetRotation(float x, float y, float z)
    {
        this.transform.rotation = Quaternion.Euler(x, y, z);
    }

    // Get the name of this leaf
    public string GetName()
    {
        return leafName;
    }

    // Get the size of this leaf
    public Vector3 GetSize()
    {
        return this.transform.localScale;
    }

    // Get the current position of this leaf
    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
}
