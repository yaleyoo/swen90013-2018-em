using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string result = "Volume density of leaf litter:\n" + System.Math.Round(Results.GetDensity()).ToString();
		GetComponent<Text> ().text = result;
	}

}
