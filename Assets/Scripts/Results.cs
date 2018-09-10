/*
 * Created by Marko Ristic.
 * Modified by Yudong Gao.
 * Class to store static variables between simulation and output scenes.
 * also for other error calculations
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;

public class Results {

	private static float average;

	private static double standard_deviation;

	private static float median;

    // Store run results, one element for one run
    private static List<float> runResults = new List<float>();

	/// <summary>
	/// a resultset used to store all density results
	/// </summary>
	private static List<float> resultset = new List<float>();

	/// <summary>
	/// Gets the value of average density.
	/// </summary>
	/// <returns>The average density(float).</returns>
	public static float GetAverage(){        
		return average;
	}

	/// <summary>
	/// Calculate the average density,
	/// then set it to the static variable "average".
	/// </summary>
	public static void SetAverage(){
		float sum = 0;

		for (int i = 0; i < resultset.Count; i++) 
		{
			sum += resultset [i];
		}

		average = sum / resultset.Count;
        // Add result to run result
        Results.runResults.Add(average);
    }

	/// <summary>
	/// Gets the Standard Deviation.
	/// </summary>
	/// <returns>The Standard Deviation(double).</returns>
	public static double GetSD(){
		return standard_deviation;
	}

	/// <summary>
	/// Calculate the Standard Deviation,
	/// then set it to the static variable "standard_deviation".
	/// </summary>
	public static void SetSD(){
		float sum = 0;

		foreach (int i in resultset) {
			sum = sum + (i - average);
		}

		// calculate the standard deviation 
		// then set the value
		standard_deviation = System.Math.Sqrt (sum / resultset.Count);

	}

	/// <summary>
	/// Gets the median.
	/// </summary>
	/// <returns>The median(float).</returns>
	public static float GetMedian(){
		return median;
	}

	/// <summary>
	/// Sets the median, then set it to the static variable "median".
	/// </summary>
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

	/// <summary>
	/// Adds the result to the "resultset" list
	/// </summary>
	/// <param name="rs">one density result(float)</param>
	public static void addResult(float rs){
		resultset.Add (rs);
	}

    // Get the results of batch run
    public static List<float> GetRunResultst()
    {
        return runResults;
    }

    public static void ClearResultset()
    {
        resultset.Clear();
    }
}
