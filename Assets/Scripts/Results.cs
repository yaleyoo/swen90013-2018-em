/*
 * Created by Marko Ristic.
 * Class to store static variables between simulation and output scenes.
 */
using System.Collections.Generic;

public class Results {
    // Single calculated density from the simulation
    private static float density;

    // Store density result for each run
    private static List<float> densityList = new List<float>();

    // Get the density of the simulation
    public static float GetDensity()
    {
        return density;
    }

    // Set the density of the simulation
    public static void SetDensity(float density)
    {
        Results.density = density;

        // Add result to density list
        Results.densityList.Add(density);
    }

    // Get the density of the batch run
    public static List<float> GetDensityList()
    {
        return densityList;
    }

}
