using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Restrict the precision of the result to 6 bits. To make sure it would not be too long for screen.
		string result = "The density is: " + MenuSettings.GetVolumeRatio().ToString("f6");
		GetComponent<Text> ().text = result;
	}

}
