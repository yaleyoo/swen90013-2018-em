using UnityEngine;

/// <summary>
/// Reprsents the volume that the density calculator compares with the 
/// volume of leaves.
/// </summary>
public class DensityCalculationCylinder {

    private GameObject[] objectsInWorld;
    private float cylinderAreaX;
    private float cylinderAreaZ;

    /// <summary>
    /// Creates a DensityCalculationCylinder
    /// </summary>
    /// <param name="objects">Objects in the world</param>
    /// <param name="cylinderAreaX">The X value of the cylinder</param>
    /// <param name="cylinderAreaZ">The Z value of the cylinder</param>
    public DensityCalculationCylinder(GameObject[] objects, float cylinderAreaX, float cylinderAreaZ) {
        this.objectsInWorld = objects;
        this.cylinderAreaX = cylinderAreaX;
        this.cylinderAreaZ = cylinderAreaZ;
    }

    /// <summary>
    /// Returns the object with the largest y value
    /// </summary>
    /// <returns>The object</returns>
    public GameObject GetHighestObject() {
        GameObject highestObj = null;

        foreach (GameObject leaf in this.objectsInWorld) {
            if (highestObj == null) {
                highestObj = leaf;
            }
            else if (leaf.transform.position.y >= highestObj.transform.position.y) {
                highestObj = leaf;
            }
        }

        return highestObj;
    }

    /// <summary>
    /// Returns the y value of lowest point of the object.
    /// Returns 0 is the lowest point is negative.
    /// </summary>
    /// <param name="obj">The object</param>
    /// <returns>The y value of the lowest point</returns>
    public float CalcHeight(GameObject obj) {
        float height = obj.GetComponent<Collider>().bounds.min.y;

        if (height > 0) {
            return height;
        }

        return 0f;
    }

    /// <summary>
    /// Returns a random point within the cylinder
    /// </summary>
    /// <returns>The point</returns>
    public Vector3 RandomPointInCylinder() {
        float height = this.CalcHeight(this.GetHighestObject());
        Vector2 UnitCirclePoint = Random.insideUnitCircle;

        float x = UnitCirclePoint.x * this.cylinderAreaX;
        float y = Random.Range(0, height);
        float z = UnitCirclePoint.y * this.cylinderAreaZ;

        // unit circle point values are multiplied by the area dimensions that are where the density is calculated
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Checks if the point is in any of the objects
    /// </summary>
    /// <param name="point">The point</param>
    /// <returns>Whether the point is in an object</returns>
    public bool IsPointInObjects(Vector3 point) {
        foreach (GameObject obj in this.objectsInWorld) {
            if (obj.GetComponent<Collider>().bounds.Contains(point)) {
                return true;
            }
        }

        return false;
    }

}
