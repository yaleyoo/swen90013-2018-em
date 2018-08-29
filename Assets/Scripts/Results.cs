/*
 * Created by Marko Ristic.
 * Modified by Yudong Gao.
 * Class to store static variables between simulation and output scenes.
 * also for other margin of errors calculation
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;

public class Results {

	private static float average;

	private static double standard_deviation;

	private static float median;

	// a resultset used to store all density results
	private static List<float> resultset = new List<float>();

	// return the average value
	public static float GetAverage(){
		return average;
	}

	// calculate and set the average
	public static void SetAverage(){
		float sum = 0;

		for (int i = 0; i < resultset.Count; i++) 
		{
			sum += resultset [i];
		}

		average = sum / resultset.Count;
	}

	// get the value of standard deviation
	public static double GetSD(){
		return standard_deviation;
	}

	// set the value of standard deviation
	public static void SetSD(){
		float sum = 0;

		foreach (int i in resultset) {
			sum = sum + (i - average);
		}

		// calculate the standard deviation 
		// then set the value
		standard_deviation = System.Math.Sqrt (sum / resultset.Count);

	}

	// get the median value
	public static float GetMedian(){
		return median;
	}

	// set the median value
	public static void SetMedian(){
		// index for 
		int index;
		// sort the result array
		resultset.Sort();

		// get the correct index in accordance with the length of the result array
		if (resultset.Count % 2 == 0) {
			index = resultset.Count / 2 - 1;
		} else {
			index = (resultset.Count - 1) / 2;
		}

		// set the value of median
		median = resultset[index];
	}

	// add one result to the result set
	public static void addResult(float rs){
		resultset.Add (rs);
	}

}
