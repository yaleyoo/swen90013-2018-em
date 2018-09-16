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
        if (SimSettings.DecreaseSimulationTimesLeft())
        {
            int runRound = BatchRunCsvLoader.batchrunLeafAndRatio.Keys.Count - SimSettings.GetRunTimeesLeft() + 1;
            SceneManager.LoadScene("Simulation");
        }
        else
        {
            // first calculate the results
            // note: only average for single-run now
            //       lack standard-deviation and median
            Results.SetAverage();

            // Print the results to the screen
            string result = "Volume density of leaf litter (as ratio):\n" + System.Math.Round(Results.GetAverage(), 6).ToString();
            GameObject.FindGameObjectWithTag("OutputText").GetComponent<Text>().text = result;
            Results.ClearResultSet();

            //decrease remaining run times 
            SimSettings.SetRunTimesLeft(SimSettings.GetRunTimeesLeft() - 1);
            // has remaining run times 
            if (SimSettings.GetRunTimeesLeft() > 0)                
            {
                // get next round number
                int runRound = BatchRunCsvLoader.batchrunLeafAndRatio.Keys.Count - SimSettings.GetRunTimeesLeft() + 1;
                Debug.Log("current round = " + runRound);
                Dictionary<LeafData, int> leafSizesAndRatios;
                // get next round leaves and ratios from batch run dictionary by run round number
                BatchRunCsvLoader.batchrunLeafAndRatio.TryGetValue(runRound, out leafSizesAndRatios);
                // set next round leaves and ratios to settings for loading by simulation 
                SimSettings.SetLeafSizesAndRatios(leafSizesAndRatios);

                SimSettings.ResetSimulationTimesLeft();
                // go to simulate next run
                SceneManager.LoadScene("Simulation");                
            }
            else
            {
                // Save the results to a file
                Debug.Log("Writing results to file ...");
                WriteResultsToFile();
                Debug.Log("Done.");

                // Avoid the progress bar stop at 99%, inidiate the simulation done
                ProgressBarController.progressBar.gameObject.SetActive(true);
                ProgressBarController.progressBar.progressImg.fillAmount = 100;
                ProgressBarController.progressBar.proText.text = "DONE";   

                // Change the title of the single run
                if (!SimSettings.GetBatchrun())
                {
                    ProgressBarController.progressBar.proTitle.text = "Simulations Finished.";
                }
               
            }                
        }
        
	}

    // Write the saved result to the output file specified in the sim settings
    private void WriteResultsToFile()
    {
        StreamWriter writer = new StreamWriter(pathToOutputFile, false);
        writer.WriteLine("Density average");

        // Write all results to file
        foreach (float result in Results.GetBatchRunResults())
        {
            writer.WriteLine(result);
        }

        writer.Close();
    }

}
