/*
 * Created by Michael Lumley.
 * Reprsents the volume that the density calculator compares with the 
 * volume of leaves.
 */
 
using UnityEngine;

public class DensityCalculationCylinder {

    GameObject[] objects;
    GameObject highestLeaf;
    float height;

    float dropAreaX;
    float dropAreaY;

    public DensityCalculationCylinder(GameObject[] objects, float dropAreaX, float dropAreaY) {
        this.objects = objects;
        this.highestLeaf = GetHighestObject();
        this.height = CalcHeight(highestLeaf);
        this.dropAreaX = dropAreaX;
        this.dropAreaY = dropAreaY;
        Debug.Log("Highest leaf position: " + highestLeaf.transform.position);
        Debug.Log("Top of cylinder is at height: " + height);
    }

    // Finds the highest object from a list
    public GameObject GetHighestObject() {
        GameObject highestObj = null;
        // Check every object against saved, and replace it if new object is higher than it
        foreach (GameObject leaf in this.objects) {
            // On first object, choose it as the saved on regardless
            if (highestObj == null) {
                highestObj = leaf;
            }
            else if (leaf.transform.position.y >= highestObj.transform.position.y) {
                highestObj = leaf;
            }
        }
        // Return the object that was the highest
        return highestObj;
    }

    private float CalcHeight(GameObject obj) {
        float height = highestLeaf.GetComponent<Collider>().bounds.min.y;

        if (height > 0) {
            return height;
        }

        return 0f;
    }

    // Generates a random point inside an eliptic cylinder of passed height, and
    public Vector3 RandomPointInCylinder() {
        Vector2 UnitCirclePoint = Random.insideUnitCircle;
        float height = Random.Range(0, this.height);

        // unit circle point values are multiplied by the area dimensions that are where the density is calculated
        return new Vector3(UnitCirclePoint.x * dropAreaX,
                            UnitCirclePoint.y * dropAreaY,
                            height);
    }

    // Checks whether or not a given point is inside any object in the given object array
    public bool IsPointInObjects(Vector3 point) {
        // For every object, check if the point is contained within it (using the collider bounds class)
        foreach (GameObject objs in this.objects) {
            // If point in any object, finish and return true
            if (objs.GetComponent<Collider>().bounds.Contains(point)) {
                return true;
            }
        }

        // Point must not have been in any of the objects in the array, return false
        return false;
    }

}
