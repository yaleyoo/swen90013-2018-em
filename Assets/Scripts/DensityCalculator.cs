/* 
 * Created by Jing Bi
 * Modified by Marko Ristic, Michael Lumley
 * Script of calculating the density of leaf litter
 * as the volume ratio of leaves to air
 */

using UnityEngine;

public class DensityCalculator {

    // Calculates the density of leaf litter as a volume ratio
    public float CalculateDensity(DensityCalculationCylinder calcArea, int numIterations)
    {
        // To calculate the volume ratio, the intersection of the cylinder and all dropped leaves is calculated
        // Use the Monte Carlo method for computing the 3D integration problem of object intersection
        int numPointsInAir = 0;
        int numPointsInLeaves = 0;

        // The number of iterations is a trade off between accuracy and time taken to compute, the constant is set in the simulation settings
        for (int i = 0; i < numIterations; i++)
        {
            // Each random point inside the cylinder is either also inside some leaf, or not
            Vector3 pointInCylinder = calcArea.RandomPointInCylinder();

            // Update the counters appropriately
            if (calcArea.IsPointInObjects(pointInCylinder))
            {
                numPointsInLeaves++;
            }
            else
            {
                numPointsInAir++;
            }
        }

        // The density is the ratio between the two counters, this is saved to the results static class for displaying and saving in the output scene
        return (float)numPointsInLeaves / (float)numPointsInAir;
    }






}
