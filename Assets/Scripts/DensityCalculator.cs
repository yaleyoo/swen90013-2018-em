/* 
 * Created by Jing Bi
 * Script of calculating the volume ratio of 
 * sum of leaf volume to ground volume
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DensityCalculator : MonoBehaviour {

	// The result that we need
	public static float volumeRatio;

	// List of leaf objects 
	protected GameObject[] listOfLeaves;

	// Number of all leaves
	private int numOfLeaf;

	// Falling height
	private float fallHeight;

	// The time that calculation should wait for,
	// it based on falling height and number of leaves
	private float waitTime;

	// Label for showing whether volume ratio has been calculated
	private bool isCalculated = false;

	// Start this script
	void Start () {
		
	}

	// Update once per frame
	void Update () {

		// Check if volume ratio has been calculated
		// the density will only be calculated once
		if (isCalculated == false) {

			this.isCalculated = true;

			// Get the falling height and the number of all leaves
			this.fallHeight = SimSettings.GetDropHeight();
            this.numOfLeaf = GameObject.FindGameObjectsWithTag("Leaf").Length;

			// Calculate the wait time
			this.waitTime = CalcWaitTime (fallHeight, numOfLeaf);

			// Delay the calculation
			StartCoroutine (ExecCalcAfterSometime (waitTime));
		}
	}
		
	/// <summary>
	/// Calculates the wait time.
	/// </summary>
	/// <returns>The wait time.</returns>
	/// <param name="height">Falling Height.</param>
	/// <param name="numOfLeaf">Number of all leaves.</param>
	public float CalcWaitTime(float height, int numOfLeaf){

		// Time of all leaves are generated = (number of leaves) * 0.01
		// Time of leaves falling down = sqrt(1/2 * height * g)
		// Give extra 5s to ensure that almost all leaves stop moving
		return (Mathf.Sqrt (height / 4.9f) + numOfLeaf * 0.01f) + 5f;
	}

	/// <summary>
	/// Execute the calculation after the wait time.
	/// </summary>
	/// <param name="waitTime">Wait time.</param>
	public IEnumerator ExecCalcAfterSometime(float waitTime){

		yield return new WaitForSeconds (waitTime);

		// Get the list of leaves from leaf generator script
		this.listOfLeaves = GameObject.FindGameObjectsWithTag("Leaf");

		// Calculate the volume ratio
		volumeRatio = 
			this.CalcVolRatio (CalcSumOfLeafVolume (listOfLeaves),
				CalcBulkVolume (listOfLeaves));

		// Only for test output
		Debug.Log (volumeRatio);
	}

	/// <summary>
	/// Calculate the sum of leaf volume 
	/// </summary>
	/// <param name="listOfLeaves"> GameObject type list of all leaves </param>
	/// <returns> sum of all leaves volume </returns>
	public float CalcSumOfLeafVolume(GameObject[] listOfLeaves){

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
	public float CalcBulkVolume(GameObject[] listOfLeaves){

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
		if (listOfLeaves.Length == 0)
			return 0;
		// Calculate the average height of all leaves
		float averHeight = sumOfHeight / listOfLeaves.Length;

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
