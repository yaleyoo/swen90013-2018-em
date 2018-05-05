/*
 * Created by Marko Ristic
 * Imports the leaf trait CSV and converts the leaves into the LeafShape class for passing
 * to the leaf generation script
 */

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CsvImporter {

    // Name of the csv file; must be located in the Resources folder
    private static string CSV_NAME = "LeafTraits";

    // Where to store csv data
    public static List<LeafShape> Leaves { get; set; }

    // Method to read leaf trait csv into LeafShape array, and return it
    public static List<LeafShape> ReadCsv()
    {
        // Initialise leaf list, this will also reset the list if being called again to re-load from csv
        CsvImporter.Leaves = new List<LeafShape>();

        // Read csv and split into lines
        TextAsset data = Resources.Load(CSV_NAME) as TextAsset;
        string[] lines = data.text.Split('\n');

        // For each line except first (header) parse individual sections, and add a new leaf shape to the list
        foreach (string line in lines.Skip(1))
        {
            string[] parts = line.Split(',');
            // Last line may be treated as more info due to line ending encoding, if parts not right length, ignore it
            if (parts.Length != 8)
            {
                continue;
            }
            CsvImporter.Leaves.Add(new LeafShape(
                                        // Name
                                        parts[0].Trim(),
                                        // Leaf form (lower case for consistency)
                                        parts[1].Trim().ToLower(),
                                        // Thickness mean
                                        float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        // Thickness range
                                        float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        // Width mean
                                        float.Parse(parts[4].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        // Width range
                                        float.Parse(parts[5].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        // Length mean
                                        float.Parse(parts[6].Trim(), CultureInfo.InvariantCulture.NumberFormat),
                                        // Length range
                                        float.Parse(parts[7].Trim(), CultureInfo.InvariantCulture.NumberFormat))
                            );
        }

        // Return the list of leaves
        return CsvImporter.Leaves;
    }

    // Debugging method to print all leaves in list on seperate lines
    public static void PrintLeaves()
    {
        foreach (LeafShape l in CsvImporter.Leaves)
        {
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
