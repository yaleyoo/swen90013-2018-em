/* Created by Jing Bi
   Script of calculating the volume ratio of 
   sum of leaf volume to ground volume
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolRatioCalculator : MonoBehaviour {


	// List of leaf objects 
	protected List<GameObject> listOfLeaves;

	// The result that we need
	public float volumeRatio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	// Calculate the leaf volume ratio when click the button
	public void Click(){

		// Initialise the value of leaf list
		this.listOfLeaves = GameObject.Find ("Ground").
			GetComponent<LeafGenerator> ().GetListOfLeaves();

		// Ratio should be 0 if there is no leaf in simulation
		if (listOfLeaves.Count == 0) {

			volumeRatio = 0f;
		} else {

			volumeRatio = 
				this.CalcVolRatio (CalcSumOfLeafVolume (listOfLeaves), 
				CalcGroundVolume (listOfLeaves));
		}
	}
	// Calculate the sum of leaf volume
	public float CalcSumOfLeafVolume(List<GameObject> listOfLeaves){

		// Initialise the sum of leaf volume
		float sumOfVolume = 0f;
		foreach (var leaf in this.listOfLeaves) {

			// For each leaf, get the size info from the list
			Vector3 sizeOfLeaf = leaf.GetComponent<Leaf> ().GetSize();
			sumOfVolume += sizeOfLeaf.x * sizeOfLeaf.y * sizeOfLeaf.z;
		}
		return sumOfVolume;
	}

	/* Considering the area as a cylinder, approximately calculate 
	   the volume by 
	   V = h(Average height of Leaves) * S (Surface area of ground),
	   S(for square) = length * width
	*/
	public float CalcGroundVolume(List<GameObject> listOfLeaves){

		// Initialise the sum of leaf height
		float sumOfHeight = 0f;
		foreach (var leaf in this.listOfLeaves){

			Vector3 positionOfLeaf = 
				leaf.GetComponent<Leaf> ().transform.position;

			// Height of leaf can be calculated as
			// height of leaf - height of ground(=0)
			sumOfHeight = sumOfHeight + positionOfLeaf.y;
		}
		// Calculate the average height of all leaves
		float averHeight = sumOfHeight / listOfLeaves.Count;

		// Calculate the surface area of ground
		float surArea = 200f * 200f;

		// Return the surface area of ground
		return averHeight * surArea;
	}

	// Calculate the volume ratio by
	// (sum of leaf volume) / (ground volume)
	public float CalcVolRatio(float sumOfLeafVolume, float groundVolume){

		float volumeRatio = sumOfLeafVolume/groundVolume;
		return volumeRatio;
	}

	// Get the current value of volume ratio
	public float GetVolumeRatio(){
		
		return this.volumeRatio;
	}
}
