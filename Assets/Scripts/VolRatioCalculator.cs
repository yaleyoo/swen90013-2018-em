/* 
 * Created by Jing Bi
 * Script of calculating the volume ratio of 
 * sum of leaf volume to ground volume
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolRatioCalculator : MonoBehaviour {

	// The result that we need
	public static float volumeRatio;

	// Number of generated leaf, the default value is 1000
	public int numOfLeaf = 1000;

	// List of leaf objects 
	protected List<GameObject> listOfLeaves;

	// Used for delay update() 
	private int tick = 0;

	// Sum of all ticks
	private int countTick = 0;

	// Tick interval for checking whether all leaves are stopped  
	private int checkInterval = 50;

	// Label for showing whether volume ratio has been calculated
	private bool isCalculated = false;

	// Start this script
	void Start () {
		
	}

	// Update once per frame
	void Update () {

		// Check if volume ratio has been calculated
		if (isCalculated == false) {
			
			// Initialise the value of leaf list
			this.listOfLeaves = GameObject.Find ("Ground").
				GetComponent<LeafGenerator> ().GetListOfLeaves ();

			// Check whether all leaves are stopped every checkInterval
			if (tick == checkInterval) {

				// Check if all leaves have been stopped, if they are,
				// calculate the volume ratio; if they are not, keep updating
				// unless it ticks more than 1500 times
				if (CheckIfStopped (listOfLeaves) || countTick >= 1500) {

					// Calculate the volume ratio now
					volumeRatio = 
						this.CalcVolRatio (CalcSumOfLeafVolume (listOfLeaves),
						CalcBulkVolume (listOfLeaves));

					// Set the isCalculated label as true
					isCalculated = true;

					// Log volume ratio as static value
					MenuSettings.SetVolumeRatio(volumeRatio);
				}
				tick = 0;
				countTick++;

				// Test output
				Debug.Log (volumeRatio);
			} else {
				tick++;	
				countTick++;
			}
		} 
		else {
			// Show the result
			SceneManager.LoadScene("Result");
		}
	}
		
	/// <summary>
	/// Checks if all leaves have been stopped
	/// </summary>
	/// <param name="listOfLeaves"> GameObject type list of all leaves </param>
	/// <returns> are all leaves stopped </returns>
	public bool CheckIfStopped(List<GameObject> listOfLeaves){

		foreach (var leaf in listOfLeaves) {
			
			// To avoid leaf being destroyed
			if (leaf) {
				// If there is a leaf which is still moving, return false
				if (leaf.GetComponent<Rigidbody> ().isKinematic == false)
					return false;

				// If the last leaf is stopped, return true
				else if (listOfLeaves.IndexOf (leaf) + 1 == listOfLeaves.Count)
					return true;
			}
		}
		// Default return true if there is destroyed leaf at last
		return true;
	}

	/// <summary>
	/// Calculate the sum of leaf volume 
	/// </summary>
	/// <param name="listOfLeaves"> GameObject type list of all leaves </param>
	/// <returns> sum of all leaves volume </returns>
	public float CalcSumOfLeafVolume(List<GameObject> listOfLeaves){

		// Initialise the sum of leaf volume
		float sumOfVolume = 0f;

		// sizeOfLeaf is a temporary variable to store current leaf size
		Vector3 sizeOfLeaf = new Vector3();
		foreach (var leaf in this.listOfLeaves) {
			
			// To avoid leaf being destroyed
			if (leaf) {
				// For each leaf, get its length, width and thickness 
				sizeOfLeaf = leaf.GetComponent<Leaf> ().GetSize ();
				sumOfVolume += sizeOfLeaf.x * sizeOfLeaf.y * sizeOfLeaf.z;
			} 
		}
		return sumOfVolume;
	}

	/// <summary> Considering the area as a cylinder, approximately calculate 
	/// the volume by Volume = h(average height of Leaves) * 
	/// S (surface area of ground), S(for square) = length * width 
	/// </summary>
	/// <param name="listOfLeaves"> GameObject type list of all leaves </param>
	/// <returns> bulk volume </returns>
	public float CalcBulkVolume(List<GameObject> listOfLeaves){

		// Initialise the sum of leaf height
		float sumOfHeight = 0f;

		// Set a temporary variable to store current leaf position
		Vector3 positionOfLeaf = new Vector3 ();
		foreach (var leaf in this.listOfLeaves){
			
			// To avoid leaf being destroyed
			if (leaf) {
				positionOfLeaf = 
					leaf.GetComponent<Leaf> ().transform.position;

				// Height of leaf can be calculated as
				// height of leaf - height of ground(=0)
				sumOfHeight = sumOfHeight + positionOfLeaf.y;
			}
		}
		if (listOfLeaves.Count == 0)
			return 0;
		// Calculate the average height of all leaves
		float averHeight = sumOfHeight / listOfLeaves.Count;

		// Calculate the surface area of ground
		Vector3 scaleOfGround = GameObject.Find("Ground").transform.localScale;
		float surArea = scaleOfGround.x * scaleOfGround.z;

		// Return the surface area of ground
		return averHeight * surArea;
	}

	/// <summary> Calculate the volume ratio by 
	/// (sum of leaf volume) / (bulk volume) 
	/// </summary>
	/// <param name="sumOfLeafVolume"> sum of all leaves volume </param>
	/// <param name="bulkVolume"> bulk volume </param>
	/// <returns> leaf volume ratio </returns>
	public float CalcVolRatio(float sumOfLeafVolume, float bulkVolume){

		if (bulkVolume == 0)
			return 0;
		float volumeRatio = sumOfLeafVolume/bulkVolume;
		return volumeRatio;
	}

	/// <summary>
	/// Get the current value of volume ratio
	/// </summary>
	public static float GetVolumeRatio(){
		
		return volumeRatio;
	}
}
