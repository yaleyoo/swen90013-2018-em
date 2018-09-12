/*
 * Created by Yuanyu Guo.
 * Class to store static variables.
 */


using System.Collections.Generic;


public class SimSettings {
    // Visual simulation settings
    private static bool visualize;

    // Spacial simulation settings
    private static float dropHeight = 100;
    private static float dropAreaX = 100;
    private static float dropAreaY = 100;

    // Leaf simulation settings
    private static Dictionary<LeafData, int> leafSizesAndRatios;
    private static bool useLeafLimit = true;
    private static int leafLimit = 1000;
    private static float leafVolumeLimit = 15;

    // Density calculation settings
    private static float densityIgnoreBorder = 10;
    private static int monteCarloNumIterations = 1000;
    private static int numCylinderSlices = 10;

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

    // Get leaf volume limit
    public static float GetLeafVolumeLimit()
    {
        return leafVolumeLimit;
    }

    // Get the drop height of the simulation
    public static float GetDropHeight()
    {
        return dropHeight;
    }

    // Set the drop height of the simulation
    public static void SetDropHeight(float dropHeight)
    {
        SimSettings.dropHeight = dropHeight;
    }

    // Get the leaf dropping area X-axis of the simulation
    public static float GetDropAreaX()
    {
        return dropAreaX;
    }

    // Get the leaf dropping area Y-axis of the simulation
    public static float GetDropAreaY()
    {
        return dropAreaY;
    }

    // Set the drop height of the simulation
    public static void SetDropArea(float dropAreaX, float dropAreaY)
    {
        SimSettings.dropAreaX = dropAreaX;
        SimSettings.dropAreaY = dropAreaY;
    }

    // Get the list of leaves and their relative ratios to use in the simulation
    public static Dictionary<LeafData, int> GetLeafSizesAndRatios()
    {
        return leafSizesAndRatios;
    }

    // Set the list of leaves and their relative ratios to use in the simulation
    public static void SetLeafSizesAndRatios(Dictionary<LeafData, int> leafSizesAndRatios)
    {
        SimSettings.leafSizesAndRatios = leafSizesAndRatios;
    }

    // Get the distance from the edge of the dropping area to ignore when calculating density
    // This avoid edge effects where the density may be different when leaves slope to the ground
    public static float GetDensityIgnoreBorder()
    {
        return densityIgnoreBorder;
    }

    // Get the number of iterations to run the monte carlo method when calculating the leaf litter density (uses volume intersaction)
    // This is the number of points sampled in EACH horizontal slice of the cylinder (numCylinderSlices)
    public static int GetMonteCarloNumIterations()
    {
        return monteCarloNumIterations;
    }

    // Get the number of horizontal slices to split the density calculation cylinder into and average the result over
    public static int GetNumCylinderSlices()
    {
        return numCylinderSlices;
    }
}
