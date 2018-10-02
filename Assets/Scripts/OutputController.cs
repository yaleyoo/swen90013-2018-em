using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutputController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        if (SimSettings.DecreaseSimulationTimesLeft())
        {
            int runRound = BatchRunCsvLoader.batchrunLeafAndRatio.Keys.Count - SimSettings.GetRunTimeesLeft() + 1;
            SceneManager.LoadScene("Simulation");
        }
        else
        {
            // first calculate the average, standard deviation and median
            Results.SetAverage();
			Results.SetSD();
			Results.SetMedian();

            // Print the average density result to the screen
		    string result = "Volume density of leaf litter:\n(leaf volume)/(total volume) = " + System.Math.Round(Results.GetAverage(), 6).ToString();
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
                // Save the results to database
				WriteResultsToDb();

                // Avoid the progress bar stop at 99%, inidiate the simulation done
                ProgressBarController.progressBar.gameObject.SetActive(true);
                ProgressBarController.progressBar.progressImg.fillAmount = 100;
                ProgressBarController.progressBar.proText.text = "DONE";   
               
            }                
        }
        
	}
		
	// Write the saved results to database
	private void WriteResultsToDb(){

		// Create lists for saving each type of data
		List<float> aveList = new List<float>();
		List<double> staDevList = new List<double>();
		List<float> medList = new List<float>();

		// Get average, standard deviation and median from Results class
		aveList = Results.GetBatchRunAve();
		staDevList = Results.GetBatchRunStaDev();
		medList = Results.GetBatchRunMed();

		// Get the time that each line will run
		int numOfRuns = SimSettings.GetSimulationTimes();

		// Form the database location: both works on Mac or Windows PC
		string dbPath = "data source=" + Application.dataPath + "/database.db";

		// Create database connection and open database
		DatabaseOperator.ConnAndOpenDB (dbPath);
		// Insert the results into table ResultOut 
		DatabaseOperator.InsertResults ("ResultOut", aveList, staDevList, medList, numOfRuns);
		// Close the database
		DatabaseOperator.CloseConnection ();

		Debug.Log("Done. All results are saved in database. \n" +
			"The location is : " + Application.dataPath + "/database.db");
	}
}
