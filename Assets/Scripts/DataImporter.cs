﻿/*
 * Created by Marko Ristic, modified by Jing Bi
 * Imports the leaf trait database table and converts the leaves into the LeafData class for passing
 * to the leaf generation script
 */

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data;
using System;

public class DataImporter {

//    // Name of the csv file; must be located in the Resources folder
//    private static string CSV_PATH = "Data/LeafTraits";

	// Path of database location
	private static string dbPath;

	// Database reader for reading data
	private static SqliteDataReader dbReader;

	// Objects for connecting Sqlite database
	private static SqliteConnection sqlConn;

	// Objects for database operation command
	private static SqliteCommand sqlCmd;

    // Scaling factor of leaf sizes to our simulation units
    private static float SCALE = 0.1f;

    // Where to store database data
    public static List<LeafData> Leaves { get; set; }

    // Method to read leaf trait database table into LeafData array, and return it
    public static List<LeafData> ReadDatabase() {
        // Initialise leaf list, this will also reset the list if being called again to re-load from database
		DataImporter.Leaves = new List<LeafData>();

		// Lines for saving data in each row
		string[] lines;

		// Temporary List for dealing with different types between SqliteDataReader and list
		List<string> lineList = new List<string>();

		// Add the first row into list
		lineList.Add ("Name,Leaf Form,Thickness,Thickness_Range,Width,Width_Range,Length,Length_Range");

		try{
			// Form the path of database
			dbPath = "data source=" + Application.dataPath + "/database.db";

			// Create the database connection
			sqlConn = new SqliteConnection (dbPath);
			sqlConn.Open();
			Debug.Log("Database is opened");

			// Initialise the sql command
			sqlCmd = sqlConn.CreateCommand();

			// Form the sql command
			string cmdText = "SELECT name, leafForm, thickness, thicknessRange, width, widthRange, len, lenRange From LeafType";
			sqlCmd.CommandText = cmdText;

			// Execute query statement and read the data from database
			Debug.Log("Reading leaf trait from database ...");
			dbReader = sqlCmd.ExecuteReader();
			while (dbReader.Read ()) {
				// Form a record becomes a line
				string line = "";
				for (int i = 0; i < dbReader.FieldCount - 1; i++) {
					// Directly transfer the first two columns to string 
					if(i < 2)						
						line += dbReader[i].ToString () + ",";
					else
						// Keep the precision of the double type 
						line += dbReader.GetDouble(i).ToString("0.#########") + ",";
				}
				// Add the last data row without ","
				line += dbReader.GetDouble(dbReader.FieldCount - 1).ToString("0.#########");

				// Save each record into lineList
				lineList.Add (line);
			}
			// Release the lock and close the database
			dbReader.Close();
			dbReader = null;
			sqlCmd.Dispose();
			sqlCmd = null;
			sqlConn.Close();
			sqlConn = null;
			Debug.Log("Database is closed");
		}
		catch(System.Exception e){
			Debug.LogError ("Failed to read data from database! \n" + e.ToString ());
		}

		// Keep the null row for line ending encoding and transfer list into string[]
		lineList.Add ("");
		lines = lineList.ToArray ();

        // For each line except first (header) parse individual sections, and add a new leaf shape to the list
        foreach (string line in lines.Skip(1)) {
            string[] parts = line.Split(',');
            // Last line may be treated as more info due to line ending encoding, if parts not right length, ignore it
            if (parts.Length != 8) {
                continue;
            }

			DataImporter.Leaves.Add(new LeafData(
                                        // Name
                                        parts[0].Trim(),
                                        // Leaf form (lower case for consistency)
                                        parts[1].Trim().ToLower(),
                                        // Thickness mean
                                        float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE,
                                        // Thickness range
                                        float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE,
                                        // Width mean
                                        float.Parse(parts[4].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE,
                                        // Width range
                                        float.Parse(parts[5].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE,
                                        // Length mean
                                        float.Parse(parts[6].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE,
                                        // Length range
                                        float.Parse(parts[7].Trim(), CultureInfo.InvariantCulture.NumberFormat) * SCALE)
                            );
        }

        // Return the list of leaves
		return DataImporter.Leaves;
    }

    // Debugging method to print all leaves in list on seperate lines
    public static void PrintLeaves() {

		foreach (LeafData l in DataImporter.Leaves) {
            Debug.Log(string.Format("{0} - {1} - {2} {3} {4} {5} {6} {7}", 
                                    l.Name, 
                                    l.LeafForm, 
                                    l.ThicknessMean, 
                                    l.ThicknessRange, 
                                    l.WidthMean, 
                                    l.WidthRange, 
                                    l.LengthMean, 
                                    l.LengthRange));
        }
    }
}
