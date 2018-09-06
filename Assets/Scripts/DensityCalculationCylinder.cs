using UnityEngine;

/// <summary>
/// Reprsents the volume that the density calculator compares with the 
/// volume of leaves.
/// </summary>
public class DensityCalculationCylinder {

    private GameObject[] objectsInWorld;
    private float cylinderAreaX;
    private float cylinderAreaY;

    /// <summary>
    /// Creates a DensityCalculationCylinder
    /// </summary>
    /// <param name="objects">Objects in the world</param>
    /// <param name="cylinderAreaX">The X size of the cylinder</param>
    /// <param name="cylinderAreaY">The Y size of the cylinder</param>
    public DensityCalculationCylinder(GameObject[] objects, float cylinderAreaX, float cylinderAreaY) {
        this.objectsInWorld = objects;
        this.cylinderAreaX = cylinderAreaX;
        this.cylinderAreaY = cylinderAreaY;
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
    private float CalcHeight(GameObject obj) {
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
        float z = UnitCirclePoint.y * cylinderAreaY;

        // unit circle point values are multiplied by the area dimensions that are where the density is calculated
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Checks if the given point is in any of the world objects. Same as above function but
    /// uses the ray cast method to check whether or not point is inside objects.
    /// Method adapted from https://answers.unity.com/questions/163864/test-if-point-is-in-collider.html
    /// </summary>
    /// <param name="point">The point which may be inside a world object</param>
    /// <returns>True if the point given is inside any world objects</returns>
    public bool IsPointInObjectsRayCast(Vector3 point)
    {
        // Chose a point which will definitely not be inside an object as raycast origin. This is
        // chosen as a point just above the leaf dropping height
        Vector3 distantPoint = new Vector3(0, SimSettings.GetDropHeight() + 10, 0);

        // Gets the direction and distance from raycast origin, to the point we are checking
        Vector3 directionToPoint = point - distantPoint;
        float distance = directionToPoint.magnitude;
        directionToPoint.Normalize();

        // Get the number of collisions from the distant point to the point we are checking.
        // Note this has to be done in both directions due to colliders not being hit from the inside (back face)
        // and this also RELIES on convex objects in the world, as raycastting will only collide with the same
        // collider once
        int hits = 0;
        hits += Physics.RaycastAll(distantPoint, directionToPoint, distance).Length;
        hits += Physics.RaycastAll(point, -directionToPoint, distance).Length;
        // If odd number of hits, then point is in object
        return (hits % 2) == 1;
    }

    /// <summary>
    /// Checks if the given point is in any of the world objects. Uses raycasting as above, but performs the
    /// cast manually. This allows hitting the same collider more than once, and ensures concave objects
    /// will be detected correctly as well as convex ones.
    /// Method adapted from https://answers.unity.com/questions/163864/test-if-point-is-in-collider.html
    /// </summary>
    /// <param name="point">The point which may be inside a world object</param>
    /// <returns>True if the point given is inside any world objects</returns>
    public bool IsPointInObjectsRayCastConcave(Vector3 point)
    {
        // Chose a point which will definitely not be inside an object as raycast origin. This is
        // chosen as a point just above the leaf dropping height
        Vector3 distantPoint = new Vector3(0, SimSettings.GetDropHeight() + 10, 0);

        // Gets the direction from raycast origin, to the point we are checking
        Vector3 direction = point - distantPoint;
        direction.Normalize();

        // Keeps track of the number of collisions made by the ray cast
        int collisions = 0;

        // Start ray cast at the distant point, and iterate over collisions towards the point we are checking
        Vector3 currPoint = distantPoint;
        while (currPoint != point)
        {
            // If there is a ray cast collision, increase the counter appropriately
            RaycastHit hit;
            if (Physics.Linecast(currPoint, point, out hit))
            {
                collisions++;
                // Move the current point to *just* after the colision point, to avoid hitting the same collision repeatedly
                currPoint = hit.point + (direction / 1000.0f);
            }
            // No more collisions, current point can be set as the end of ray cast (the point being checked)
            else
            {
                currPoint = point;
            }
        }

        // Repeat the raycast process in the other direction. As colliders are only hit when tracing "from the outside in", this ensures
        // that all collisions are detected correctly (back faces also detected)
        while (currPoint != distantPoint)
        {
            RaycastHit hit;
            if (Physics.Linecast(currPoint, distantPoint, out hit))
            {
                collisions++;
                currPoint = hit.point + (-direction / 100.0f);
            }
            else
            {
                currPoint = distantPoint;
            }
        }

        // If the number of collisions is odd, then point was in an object
        if (collisions % 2 == 1)
        {
            return true;
        }
        // If number of collisions is even, then point not in any object
        else
        {
            return false;
        }
    }
}
