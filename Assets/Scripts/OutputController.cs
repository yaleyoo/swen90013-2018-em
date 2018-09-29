using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite; 
using System.Data;
using System;

public class OutputController : MonoBehaviour {

    // Path to file where output will be saved
    private string pathToOutputFile = "Assets/Resources/output.txt";

	// Path to database where output will be saved
	private string dbPath; 

	// Objects for connecting Sqlite database
	SqliteConnection sqlConn;
	SqliteCommand sqlCmd;

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
                Debug.Log("Prepare to write results to database ...");
				// Form the database location
				dbPath = "data source=" + Application.dataPath + "/database.db";

				try{
					// Create connection with database
					sqlConn = new SqliteConnection(dbPath);
					sqlConn.Open();
					Debug.Log("Database open successfully");
					sqlCmd = sqlConn.CreateCommand();
					Debug.Log("Writing results to database ...");
					// Write the results into database
					WriteResultsToDb();
					Debug.Log("Done. All results are saved in database");
				}
				catch(System.Exception e){
					Debug.LogError ("Failed to open database!" + e.ToString ());
				}

                WriteResultsToFile();
                Debug.Log("Done.");

                // Avoid the progress bar stop at 99%, inidiate the simulation done
                ProgressBarController.progressBar.gameObject.SetActive(true);
                ProgressBarController.progressBar.progressImg.fillAmount = 100;
                ProgressBarController.progressBar.proText.text = "DONE";   
               
            }                
        }
        
	}

    // Write the saved result to the output file specified in the sim settings
    private void WriteResultsToFile()
    {
        StreamWriter writer = new StreamWriter(pathToOutputFile, false);
        writer.WriteLine("Density average");

        // Write all results to file
		foreach (float result in Results.GetBatchRunAve())
        {
            writer.WriteLine(result);
        }

        writer.Close();
    }

	// Write the saved results to database
	private void WriteResultsToDb(){

		List<float> aveList = new List<float>();
		List<double> staDevList = new List<double>();
		List<float> medList = new List<float>();

		// Create lists for saving each type of data
		aveList = Results.GetBatchRunAve();
		staDevList = Results.GetBatchRunStaDev();
		medList = Results.GetBatchRunMed();

		// Form the query statement and write data into database
		for (int i = 0; i < aveList.Count; i++) {

			// Form the query and the command to be executed
			string val = "VALUES (" + aveList[i] + ", " + staDevList[i] + ", " + medList[i] + ")";
			sqlCmd.CommandText = "INSERT INTO ResultOut " + "(averageDensity, stddevDensity, median) " + val;

			// Execute query
			sqlCmd.ExecuteNonQuery();
		}

		// Release the lock and close the database
		sqlCmd.Dispose();
		sqlConn.Close();
	}
}
