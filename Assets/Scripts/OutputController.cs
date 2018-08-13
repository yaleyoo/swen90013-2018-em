using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string result = "Volume density of leaf litter (as ratio):\n" + System.Math.Round(Results.GetDensity(), 6).ToString();
        GameObject.FindGameObjectWithTag("OutputText").GetComponent<Text>().text = result;
	}

}
