/* Created by Jing Bi
 * Script of calculating the bulk density
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityCalculator : MonoBehaviour {

	// List of leaf objects 
	public List<GameObject> listOfLeaves;

	// The result that we need
	public float bulkDensity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Calculate the bulk density when click the button
	public void Click(){

		// Initialise the value of leaf list
		this.listOfLeaves = GameObject.Find ("Ground").
			GetComponent<LeafGenerator> ().GetListOfLeaves();
		
		// Density should be 0 if there is no leaf in simulation
		if (listOfLeaves.Count == 0) {
			
			bulkDensity = 0f;
		} else {
			
			bulkDensity = this.CalcDensity (CalcMass (listOfLeaves), 
				CalcVolume (listOfLeaves));
		}
	}
	// Calculate the total mass of leaves
	public float CalcMass(List<GameObject> listOfLeaves){

		// Initialise the sum of leaf mass
		float sumOfMass = 0f;
		foreach (var leaf in this.listOfLeaves) {
			
			Vector3 sizeOfLeaf = leaf.GetComponent<Leaf> ().GetSize();

			// Assume that density of leaf is 1
			sumOfMass += sizeOfLeaf.x * sizeOfLeaf.y * sizeOfLeaf.z;
		}
		return sumOfMass;
	}

	/* Considering the area as a cylinder, approximately calculate 
	   the volume by V = h(Average height) * S (Surface area)
	*/
	public float CalcVolume(List<GameObject> listOfLeaves){

		// Get the average high
		float sumOfHeight = 0f;
		foreach (var leaf in this.listOfLeaves){
			
			Vector3 positionOfLeaf = 
				leaf.GetComponent<Leaf> ().transform.position;
			
			// Height of leaf can be calculated as
			// height of leaf - height of ground(=0)
			sumOfHeight = sumOfHeight + positionOfLeaf.y;
		}
		float averHeight = sumOfHeight / listOfLeaves.Count;

		// Calculate the surface area of ground
		float surArea = 200f * 200f;

		// Return the surface area of ground
		return averHeight * surArea;
	}

	// Calculate the bulk density according to the formula: d = m/v
	public float CalcDensity(float sumOfMass, float surVolume){

		float bulkDensity = sumOfMass/surVolume;
		return bulkDensity;
	}

	// Get the current value of bulk density
	public float GetBulkDensity(){
		return bulkDensity;
	}
}
