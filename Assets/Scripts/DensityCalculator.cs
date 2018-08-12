/* 
 * Created by Jing Bi
 * Modified by Marko Ristic
 * Script of calculating the density of leaf litter
 * as the volume ratio of leaves to air
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DensityCalculator : MonoBehaviour {
    // Variables for periodic end checking. Saves computational time by not checking continuously. All values are in seconds.
    private bool isWaiting = false;
    private float timeWaited = 0;
    private float timeBetweenChecks = 3;

    // Variables for waiting the time it takes for a leaf to drop, after having deduced that the simulation has ended
    private bool readyToCalculateDensity = false;
    //TODO make gravity be adjusted for the units of height, as this may not be in meters
    private float timeToFall = (float) System.Math.Sqrt(SimSettings.GetDropHeight() / Physics.gravity.magnitude);

    // Run once at start of scene
    void Start()
    {

    }

    // Run once per frame
    void Update()
    {
        //========================// Waiting //========================//

        // Wait either between checks to see if simulation ended, or after ending wait the time it takes for a leaf to fall from the drop height
        if (isWaiting)
        {
            timeWaited += Time.deltaTime;
            // If waited long enough, stop waiting, and perform the check on next Update call
            if (!readyToCalculateDensity && timeWaited >= timeBetweenChecks)
            {
                isWaiting = false;
                timeWaited = 0;
            }
            // If ready to calculate, wait the time it takes for a leaf to drop before doing so
            else if (readyToCalculateDensity && timeWaited >= timeToFall)
            {
                // When waited long enough, calculate the density and change scene to the Output scene
                CalculateDensity();
                ChangeToOutputScene();
            }
        }

        //========================// Checking if simulation is done //========================//
        else
        {
            // Simulation ends depending on whether or not there is a leaf limit set
            if (SimSettings.GetUseLeafLimit())
            {
                // If there is a leaf limit, generation will have stopped automatically
                if (SimSettings.GetNumLeavesDropped() >= SimSettings.GetLeafLimit())
                {
                    // Ready to compute density flag, marks that density will be computed after the time it takes for a leaf to fall from it's dropped height
                    readyToCalculateDensity = true;
                }
            }
            else
            {
                // If no limit set, end the simulation manually when the leaves dropped exceed a volume threshold
                float leafVolumeSum = 0;
                foreach (GameObject leaf in GameObject.FindGameObjectsWithTag("Leaf"))
                {
                    leafVolumeSum += leaf.GetComponent<Leaf>().GetVolume();
                }
                // If computed volume sum exceeds the threshold, then ready to computed density
                if (leafVolumeSum >= SimSettings.GetLeafVolumeLimit())
                {
                    GetComponent<LeafGenerator>().EndSim();
                    // Ready to compute density flag, marks that density will be computed after the time it takes for a leaf to fall from it's dropped height
                    readyToCalculateDensity = true;
                }
            }

            // After checking if simulation complete, wait the set timeout before checking again
            isWaiting = true;
        }
    }

    // Calculates the density of leaf litter as a volume ratio
    private void CalculateDensity()
    {
        // To get the cylinder in which to calculate the volume ratio, use the lowest point of the highest leaf as the height of the cylinder
        GameObject[] leaves = GameObject.FindGameObjectsWithTag("Leaf");
        GameObject highestLeaf = GetHighestObject(leaves);
        float cylinderHeight = GetHeightOfLowestPointOfObject(highestLeaf);

        //========================// Calculating density //========================//

        // To calculate the volume ratio, the intersection of the cylinder and all dropped leaves is calculated
        // Use the Monte Carlo method for computing the 3D integration problem of object intersection
        int numPointsInAir = 0;
        int numPointsInLeaves = 0;

        // The number of iterations is a trade off between accuracy and time taken to compute, the constant is set in the simulation settings
        for (int i=0; i<SimSettings.GetMonteCarloNumIterations(); i++)
        {
            // Each random point inside the cylinder is either also inside some leaf, or not
            Vector3 pointInCylinder = RandomPointInCylinder(cylinderHeight);

            // Update the counters appropriately
            if (IsPointInObjects(pointInCylinder, leaves))
            {
                numPointsInLeaves++;
            }
            else
            {
                numPointsInAir++;
            }
        }

        // The density is the ratio between the two counters, this is saved to the results static class for displaying and saving in the output scene
        Results.SetDensity(numPointsInLeaves / numPointsInAir);
    }

    // Finds the highest object from a list
    private GameObject GetHighestObject(GameObject[] objs)
    {
        GameObject highestObj = null;
        // Check every object against saved, and replace it if new object is higher than it
        foreach(GameObject obj in objs)
        {
            // On first object, choose it as the saved on regardless
            if (highestObj == null)
            {
                highestObj = obj;
            }
            else if (obj.GetComponent<Leaf>().GetPosition().y >= highestObj.GetComponent<Leaf>().GetPosition().y)
            {
                highestObj = obj;
            }
        }
        // Return the object that was the highest
        return highestObj;
    }

    // Finds the y-value of the lowest point in an object
    private float GetHeightOfLowestPointOfObject(GameObject obj)
    {
        // All object points are found using the Mesh component
        float lowestPoint = SimSettings.GetDropHeight();
        Vector3[] pointsInObj = obj.GetComponent<Mesh>().vertices;

        // Check every point's y value against saved y value and replace it if new one's is lower
        foreach (Vector3 point in pointsInObj)
        {
            if (point.y < lowestPoint)
            {
                lowestPoint = point.y;
            }
        }

        // Return the lowest y value that was found
        return lowestPoint;
    }

    // Generates a random point inside an eliptic cylinder of passed height, and
    private Vector3 RandomPointInCylinder(float cylinderHeight)
    {
        Vector2 UnitCirclePoint = Random.insideUnitCircle;
        float height = Random.Range(0, cylinderHeight);

        // unit circle point values are multiplied by the area dimensions that are where the density is calculated
        return new Vector3(UnitCirclePoint.x * ((SimSettings.GetDropAreaX()/2) - SimSettings.GetDensityIgnoreBorder()), 
                            UnitCirclePoint.y * ((SimSettings.GetDropAreaY()/2) - SimSettings.GetDensityIgnoreBorder()), 
                            height);
    }

    // Checks whether or not a given point is inside any object in the given object array
    private bool IsPointInObjects(Vector3 point, GameObject[] objects)
    {
        return false;
    }

    // Changes the Unity scene to the output scene, where results calculated here are displayed and saved
    private void ChangeToOutputScene()
    {
        SceneManager.LoadScene("Output");
    }
}
