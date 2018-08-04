/*
 * Created by Yuanyu Guo.
 * Class to store static variables.
 */


using System.Collections.Generic;


public class SimSettings {
    // Visual simulation settings
    private static bool visualize;

    // Spacial simulation settings
    private float dropHeight = 100;
    private float dropAreaX = 100;
    private float dropAreaY = 100;

    // Leaf Simulation settings
    private Dictionary<LeafShape, int> sizesAndRatios;
    private static bool useLeafLimit = true;
    private static int leafLimit = 1000;

    // Get visualization flag
    public static bool GetVisualize()
    {
        return SimSettings.visualize;
    }

    // Set visualization flag
    public static void SetVisualize(bool isVisualize)
    {
        SimSettings.visualize = isVisualize;
    }

    // Check if a leaf limit exists
    public static bool GetUseLeafLimit()
    {
        return useLeafLimit;
    }

    // Get the leaf limit is (regardless of whether or not if it's in use)
    public static int GetLeafLimit()
    {
        return leafLimit;
    }

    // Set a maximum number of leaves for the simulation to stop at
    public static void SetLeafLimit(int numberLimit)
    {
        leafLimit = numberLimit;
        useLeafLimit = true;
    }

    // Remove a maximum number of leaves limit if it existed
    public static void RemoveLeafLimit()
    {
        useLeafLimit = false;
    }

    // Get the drop height of the simulation
    public static int GetDropheight()
    {
        return SimSettings.
    }
}
