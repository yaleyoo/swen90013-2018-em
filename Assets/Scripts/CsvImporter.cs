/*
 * Created by Marko Ristic
 * Imports the leaf trait CSV and converts the leaves into the LeafData class for passing
 * to the leaf generation script
 */

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CSVImporter {

    // Name of the csv file; must be located in the Resources folder
    private static string CSV_PATH = "Data/LeafTraits";

    // Scaling factor of leaf sizes to our simulation units
    private static float SCALE = 0.1f;

    // Where to store csv data
    public static List<LeafData> Leaves { get; set; }

    // Method to read leaf trait csv into LeafData array, and return it
    public static List<LeafData> ReadCsv() {
        // Initialise leaf list, this will also reset the list if being called again to re-load from csv
        CSVImporter.Leaves = new List<LeafData>();

        // Read csv and split into lines
        TextAsset data = Resources.Load(CSV_PATH) as TextAsset;
        string[] lines = data.text.Split('\n');

        // For each line except first (header) parse individual sections, and add a new leaf shape to the list
        foreach (string line in lines.Skip(1)) {
            string[] parts = line.Split(',');
            // Last line may be treated as more info due to line ending encoding, if parts not right length, ignore it
            if (parts.Length != 8) {
                continue;
            }

            CSVImporter.Leaves.Add(new LeafData(
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
        return CSVImporter.Leaves;
    }

    // Debugging method to print all leaves in list on seperate lines
    public static void PrintLeaves() {

        foreach (LeafData l in CSVImporter.Leaves) {
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
