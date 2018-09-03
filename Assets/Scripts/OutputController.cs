using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutputController : MonoBehaviour {

    // Path to file where output will be saved
    private string pathToOutputFile = "Assets/Resources/output.txt";

    // Use this for initialization
    void Start () {
        if (SimSettings.GetRunTimeesLeft() > 0)
        {
            // Print the results to the screen
            string result = "Volume density of leaf litter (as ratio):\n" + System.Math.Round(Results.GetDensity(), 6).ToString();
            GameObject.FindGameObjectWithTag("OutputText").GetComponent<Text>().text = result;

            //decrease remaining run times 
            SimSettings.SetRunTimesLeft(SimSettings.GetRunTimeesLeft() - 1);
            // has remaining run times 
            if (SimSettings.GetRunTimeesLeft() > 0)                
            {
                // get next round number
                int runRound = BunchRunCsvLoader.bunchrunLeafAndRatio.Keys.Count - SimSettings.GetRunTimeesLeft() + 1;
                Debug.Log("current round = " + runRound);
                Dictionary<LeafData, int> leafSizesAndRatios;
                // get next round leaves and ratios from bunch run dictionary by run round number
                BunchRunCsvLoader.bunchrunLeafAndRatio.TryGetValue(runRound, out leafSizesAndRatios);
                // set next round leaves and ratios to settings for loading by simulation 
                SimSettings.SetLeafSizesAndRatios(leafSizesAndRatios);
                // go to simulate next run
                SceneManager.LoadScene("Simulation");                
            }
            else
            {
                // Save the results to a file
                Debug.Log("Writing results to file ...");
                WriteResultsToFile();
                Debug.Log("Done.");
            }                
        }
        
	}

    // Write the saved result to the output file specified in the sim settings
    private void WriteResultsToFile()
    {
        StreamWriter writer = new StreamWriter(pathToOutputFile, false);
        writer.WriteLine("Density");
        // write all density result to file
        foreach (float density in Results.GetDensityList())
        {
            writer.WriteLine(density);
        }
        writer.Close();
    }

}
