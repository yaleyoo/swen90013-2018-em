using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OutputController : MonoBehaviour {

    // Path to file where output will be saved
    private string pathToOutputFile = "Assets/Resources/output.txt";

    // Use this for initialization
    void Start () {
		// first calculate the results
		// note: only average for single-run now
		//       lack standard-deviation and median
		Results.SetAverage ();

        // Print the results to the screen
		string result = "Volume density of leaf litter:\n(leaf volume)/(total volume) = " + System.Math.Round(Results.GetAverage(), 6).ToString();
        GameObject.FindGameObjectWithTag("OutputText").GetComponent<Text>().text = result;

        // Save the results to a file
        Debug.Log("Writing results to file ...");
        WriteResultsToFile();
        Debug.Log("Done.");
	}

    // Write the saved result to the output file specified in the sim settings
    private void WriteResultsToFile()
    {
        StreamWriter writer = new StreamWriter(pathToOutputFile, false);
        writer.WriteLine("Density");
		writer.WriteLine(Results.GetAverage());
        writer.Close();
    }

}
