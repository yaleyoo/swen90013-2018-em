using UnityEngine;

/// <summary>
/// Script of calculating the density of leaf litter
/// as the volume ratio of leaves to air.
/// </summary>
public class DensityCalculator {

    /// <summary>
    /// Calculates the density of leaf litter as a volume ratio by randomly sampling
    /// the area to check if a point exists in a leaf or not
    /// </summary>
    /// <param name="calcArea">The area to calculate the density for</param>
    /// <param name="sampleSize">The number of points to sample</param>
    /// <returns></returns>
    public float CalculateDensity(DensityCalculationCylinder calcArea, int sampleSize) {
        float numPointsInAir = 0;
        float numPointsInLeaves = 0;

        for (int i = 0; i < sampleSize; i++) {
            Vector3 pointInCylinder = calcArea.RandomPointInCylinder();

            if (calcArea.IsPointInObjects(pointInCylinder)) {
                numPointsInLeaves++;
            }
            else {
                numPointsInAir++;
            }
        }

        if (numPointsInAir > 0) {
            return numPointsInLeaves / numPointsInAir;
        }
        else {
            return 0;
        }
    }
}
