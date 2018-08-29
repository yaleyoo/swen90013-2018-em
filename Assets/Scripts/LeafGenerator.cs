using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates leaves is the drop area at the set height
/// </summary>
public class LeafGenerator {

    private const string ROUND_LEAF = "round";

    private int totalRatioWeights;
    private float dropAreaX;
    private float dropAreaY;
    private float height;
    private LeafColorer leafColorer;
    private Dictionary<LeafData, int> leafShapes;

    /// <summary>
    /// Creates a LeafGenerator
    /// </summary>
    /// <param name="leafRatios">
    ///     A dictionary of leafData to be used and
    ///     the percentage of that type of leaf to create
    /// </param>
    /// <param name="dropAreaX">The X size of the drop area</param>
    /// <param name="dropAreaY">The Y size of the drop area</param>
    /// <param name="height">The height of the drop area</param>
    public LeafGenerator(Dictionary<LeafData, int> leafRatios, float dropAreaX, float dropAreaY, float height) {
        int sum = 0;

        foreach (LeafData ls in leafRatios.Keys) {
            sum += leafRatios[ls];
        }

        this.totalRatioWeights = sum;
        this.dropAreaX = dropAreaX;
        this.dropAreaY = dropAreaY;
        this.height = height;
        this.leafColorer = new LeafColorer();
        this.leafShapes = leafRatios;
    }

    /// <summary>
    /// Instantiates a leaf in the world
    /// </summary>
    /// <param name="visualize">Should the leaf be visable</param>
    /// <returns>The leaf instantiated</returns>
    public GameObject GetNextLeaf(bool visualize) {
        GameObject leaf = null;

        LeafData nextLeafShape = this.GetLeafData(this.leafShapes);
        Vector3 leafData = nextLeafShape.GetConcreteLeafSize();

        switch (nextLeafShape.LeafForm.ToLower()) {
            case ROUND_LEAF:
                leaf = Resources.Load("RoundLeaf") as GameObject;
                break;
            default:
                leaf = Resources.Load("FlatLeaf") as GameObject;
                break;
        }

        Vector3 randomPoint = this.GetRandomPointInDropArea(this.dropAreaX, this.dropAreaY, this.height);
        leaf = GameObject.Instantiate(leaf, randomPoint, Quaternion.identity);

        leaf.GetComponent<Leaf>().SetName(nextLeafShape.Name);
        leaf.GetComponent<Leaf>().SetSize(leafData.x, leafData.y, leafData.z);
        leaf.GetComponent<MeshRenderer>().material.color = this.leafColorer.GetColor(nextLeafShape);
        leaf.transform.eulerAngles = this.GetRandomAngle();

        if (!visualize) {
            leaf.GetComponent<Renderer>().enabled = false;
        }

        return leaf;
    }

    /// <summary>
    /// Returns a random euler angle
    /// </summary>
    /// <returns>The angle</returns>
    private Vector3 GetRandomAngle() {
        float x = Random.Range(0f, 360f);
        float y = Random.Range(0f, 360f);
        float z = Random.Range(0f, 360f);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Return a random point in the drop area
    /// </summary>
    /// <param name="dropAreaX">The X size of the drop area</param>
    /// <param name="dropAreaY">THe Y size of the drop area</param>
    /// <param name="height">The height of the drop area</param>
    /// <returns>The point</returns>
    public Vector3 GetRandomPointInDropArea(float dropAreaX, float dropAreaY, float height) {
        Vector2 random2DPoint = Random.insideUnitCircle;
        return new Vector3(random2DPoint.x * dropAreaX, height, random2DPoint.y * dropAreaY);
    }

    /// <summary>
    /// Return the LeafData of a leaf using the ratio of leaves to drop
    /// </summary>
    /// <param name="leafData">The dictionary of LeafData for the simulation</param>
    /// <returns>The selected LeafData</returns>
    private LeafData GetLeafData(Dictionary<LeafData, int> leafData) {
        int cumulativeSum = 0;
        float randomNumber = Random.Range(0, this.totalRatioWeights);

        foreach (LeafData leafShape in leafData.Keys) {
            cumulativeSum += leafData[leafShape];
            if (randomNumber < cumulativeSum) {
                return leafShape;
            }
        }

        return new LeafData();
    }
}
