/*
 * Created by Jing Bi
 * Class for database operation, 
 * including open, close, insert and read functions.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using System.Data;
using System;

public class DatabaseOperator {

	// Objects for connecting Sqlite database
	private static SqliteConnection sqlConn;
	private static SqliteCommand sqlCmd;
	private static SqliteDataReader dbReader;

	public static void ConnAndOpenDB(string dbPath){
		
		try{
			sqlConn = new SqliteConnection(dbPath);
			sqlConn.Open();
			Debug.Log("Database open successfully \n");
		}
		catch(Exception e){
			Debug.Log ("Failed to open database. \n" + e.ToString ());
		}
	}

	public static void CloseConnection(){

		if (sqlCmd != null) {
			sqlCmd.Cancel();
			sqlCmd = null;
		}

		if (dbReader != null) {
			dbReader.Close();
			dbReader = null;
		}

		if (sqlConn != null) {
			sqlConn.Close();
			sqlConn = null;
		}
	}

	public static SqliteDataReader ExecutQuery(string query){

		sqlCmd = sqlConn.CreateCommand();
		sqlCmd.CommandText = query;
		dbReader = sqlCmd.ExecuteReader();
		return dbReader;
	}

	public static SqliteDataReader ReadLeafTraits(string tableName){

		string query = "SELECT name, leafForm, thickness, thicknessRange, width, " +
						"widthRange, len, lenRange From " + tableName;
		return ExecutQuery (query);
	}

	public static void InsertResults(string tableName, 
									List<float> aveList, 
									List<double> staDevList, 
									List<float> medList, 
									int numOfRuns){

		Debug.Log("Writing results to database ... \n");

		for (int i = 0; i < aveList.Count; i++) {
			//
			string query = "INSERT INTO " +
			               tableName +
			               "(averageDensity, stddevDensity, median, numbersRuns) VALUES (" +
			               aveList[i] + ", " +
						   staDevList[i] + ", " +
						   medList[i] + ", " +
						   numOfRuns + ")";

			ExecutQuery (query);
		}
	}

}

