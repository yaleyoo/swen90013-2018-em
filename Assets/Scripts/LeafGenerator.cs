/*
 * Created by Marko Ristic.
 * Modified by Michael Lumley
 * Generation and simulation of leaves dropping
 */

using System.Collections.Generic;
using UnityEngine;

public class LeafGenerator {

    private int totalRatioWeights;
    private float dropAreaX;
    private float dropAreaY;
    private LeafColorer leafColorer;
    private Dictionary<LeafData, int> leafShapes;

    public LeafGenerator(Dictionary<LeafData, int> leafShapes, float dropAreaX, float dropAreaY) {
        int sum = 0;

        foreach (LeafData ls in leafShapes.Keys) {
            sum += leafShapes[ls];
        }

        this.totalRatioWeights = sum;
        this.dropAreaX = dropAreaX;
        this.dropAreaY = dropAreaY;
        this.leafColorer = new LeafColorer();
        this.leafShapes = leafShapes;
    }

    public GameObject GetNextLeaf() {
        GameObject leaf = null;

        // Get the next leaf type to drop based on their ratios, and then get the size of a single such leaf
        LeafData nextLeafShape = this.GetLeafData(this.leafShapes);
        Vector3 leafData = nextLeafShape.GetConcreteLeafSize();

        // Make a new leaf object of correct type
        if (nextLeafShape.LeafForm.ToLower() == "round") {
            leaf = Resources.Load("RoundLeaf") as GameObject;
        }
        else {
            leaf = Resources.Load("FlatLeaf") as GameObject;
        }

        // Place the leaf object randomly within a circle defined by the dropping area
        Vector3 randomPoint = this.GetRandomPointInDropArea();
        leaf = GameObject.Instantiate(leaf, randomPoint, Quaternion.identity);

        leaf.GetComponent<Leaf>().SetName(nextLeafShape.Name);

        // Set the leaf object to have the calculated leaf size
        leaf.GetComponent<Leaf>().SetSize(leafData.x, leafData.y, leafData.z);

        // Set colors for different types of leaves
        leaf.GetComponent<MeshRenderer>().material.color = this.leafColorer.GetColor(nextLeafShape);

        // Rotate the leaf object randomly after it's spawn
        leaf.transform.eulerAngles = this.GetRandomAngle();

        // In case of no visualization, turn off the renderer for leaves
        if (!SimSettings.GetVisualize()) {
            leaf.GetComponent<Renderer>().enabled = false;
        }

        return leaf;
    }

    private Vector3 GetRandomAngle() {
        float x = Random.Range(0f, 360f);
        float y = Random.Range(0f, 360f);
        float z = Random.Range(0f, 360f);

        return new Vector3(x, y, z);
    }

    public Vector3 GetRandomPointInDropArea() {
        Vector2 random2DPoint = Random.insideUnitCircle;
        return new Vector3(random2DPoint.x * dropAreaX, SimSettings.GetDropHeight(), random2DPoint.y * dropAreaY);
    }

    // Using the ratio of leaf to drop choose at random, in the right proportions, the next leaf to drop
    private LeafData GetLeafData(Dictionary<LeafData, int> leafShapes) {
        // use the cumulative sum of the ratios, and a random number between 0 and the total ratio sum to choose the next leaf
        int cumulativeSum = 0;
        float randomNumber = Random.Range(0, this.totalRatioWeights);
        foreach (LeafData leafShape in leafShapes.Keys) {
            cumulativeSum += leafShapes[leafShape];
            if (randomNumber < cumulativeSum) {
                // Return the chosen next leaf
                return leafShape;
            }
        }

        // Return the default next leaf
        return new LeafData();
    }


}
