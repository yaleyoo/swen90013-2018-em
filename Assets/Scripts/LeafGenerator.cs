/*
 * Created by Marko Ristic.
 * Modified by Michael Lumley
 * Generation and simulation of leaves dropping
 */

using System.Collections.Generic;
using UnityEngine;

public class LeafGenerator {

    // Sum of leaf ratios use when generating leaves
    private int totalRatioWeights;
    
	// Preset of Colors for differentiating leaves in various types
	List<Color> preColors = new List<Color>{Color.green, Color.red, Color.cyan, Color.yellow, 
		Color.magenta, Color.blue,Color.gray, Color.white, Color.black };

	// Dictionary of all kinds of leaf names and their corresponding colors
	private Dictionary<string, Color> nameAndColors = new Dictionary<string, Color>();

    public LeafGenerator(Dictionary<LeafData, int> leafShapes) {
        int sum = 0;
        foreach (LeafData ls in leafShapes.Keys) {
            sum += leafShapes[ls];
        }
        this.totalRatioWeights = sum;
    }

    // Generate a new leaf, and drop it in the simulation
    public GameObject GetNextLeaf()
    {
        GameObject leaf;

        // Get the next leaf type to drop based on their ratios, and then get the size of a single such leaf
        LeafData nextLeafShape = this.GetLeafData();
        Vector3 leafSize = this.GetConcreteLeafSize(nextLeafShape);

        // Make a new leaf object of correct type
        if (nextLeafShape.LeafForm.ToLower() == "round") {
            leaf = Resources.Load("RoundLeaf") as GameObject;
        }
        else {
            leaf = Resources.Load("FlatLeaf") as GameObject;
        }

        // Place the leaf object randomly within a circle defined by the dropping area
        Vector2 random2DPoint = Random.insideUnitCircle;
        leaf = GameObject.Instantiate(leaf, new Vector3(random2DPoint.x * (SimSettings.GetDropAreaX()/2), SimSettings.GetDropHeight(), random2DPoint.y * (SimSettings.GetDropAreaY()/2)), Quaternion.identity);

        leaf.GetComponent<Leaf>().SetName(nextLeafShape.Name);

        // Set the leaf object to have the calculated leaf size
        leaf.GetComponent<Leaf>().SetSize(leafSize.x, leafSize.y, leafSize.z);

        // Set colors for different types of leaves
        leaf.GetComponent<MeshRenderer>().material.color = this.GetColor(nextLeafShape);

        // Rotate the leaf object randomly after it's spawn
        leaf.transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
       
        // In case of no visualization, turn off the renderer for leaves
        if (!SimSettings.GetVisualize())
        {
            leaf.GetComponent<Renderer>().enabled = false;
        }

        return leaf;
    }

    // Using the ratio of leave to drop choose at random, in the right proportions, the next leaf to drop
    private LeafData GetLeafData()
    {
        // use the cumulative sum of the ratios, and a random number between 0 and the total ratio sum to choose the next leaf
        int cumulativeSum = 0;
        float randomNumber = Random.Range(0, this.totalRatioWeights);
        foreach (LeafData leafShape in SimSettings.GetLeafSizesAndRatios().Keys)
        {
            cumulativeSum += SimSettings.GetLeafSizesAndRatios()[leafShape];
            if (randomNumber < cumulativeSum)
            {
                // Return the chosen next leaf
                return leafShape;
            }
        }

        // Return the default next leaf
        return new LeafData();
    }

    // Given a leaf shape, returns a size of that leaf, taking the dimensions and their ranges into account
    private Vector3 GetConcreteLeafSize(LeafData leafShape)
    {
        // Three dimensions of the leaf
        float thickness = Random.Range(leafShape.ThicknessMean - leafShape.ThicknessRange / 2,
                                        leafShape.ThicknessMean + leafShape.ThicknessRange / 2);
        float width = Random.Range(leafShape.WidthMean - leafShape.WidthRange / 2,
                                        leafShape.WidthMean + leafShape.WidthRange / 2);
        float length = Random.Range(leafShape.LengthMean - leafShape.LengthRange / 2,
                                        leafShape.LengthMean + leafShape.LengthRange / 2);

        // Return as a vector for simplicity
        return new Vector3(thickness, width, length);
    }

	// Set colors according to types of leaves
	private Color GetColor(LeafData shape){
		
		string nameOfLeaf = shape.Name;

        // If the leaf is already be paired with a color, then use its paired color
        if (!nameAndColors.ContainsKey(nameOfLeaf)) {

            // If preset colors are not exhausted
            if (preColors.Count != 0) {

                // Pair the leaf name and a new preset color into dictionary
                nameAndColors.Add(nameOfLeaf, preColors[0]);

                // Remove the used color from the preset colors array
                Color selectedColor = preColors[0];
                preColors.Remove(selectedColor);
                return selectedColor;
            }
            else {
                while (true) {

                    // Random a new color 
                    float r = Random.Range(0f, 1f);
                    float g = Random.Range(0f, 1f);
                    float b = Random.Range(0f, 1f);
                    Color randomColor = new Color(r, g, b);

                    // If the random color is not used, change the color then break the loop
                    if (!nameAndColors.ContainsValue(randomColor)) {

                        // Pair the leaf name and the new random color
                        nameAndColors.Add(nameOfLeaf, randomColor);

                        return randomColor;
                    }
                }
            }
        }
        else
            // colorate the leaf by its corresponding color in dictionary
            return nameAndColors[nameOfLeaf];
	}
}
