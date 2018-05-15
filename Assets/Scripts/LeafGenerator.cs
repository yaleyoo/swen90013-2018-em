/*
 * Created by Marko Ristic.
 * Generation and simulation of leaves dropping
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGenerator : MonoBehaviour {

    // Area to generate leaves in
    private float height;
    private float dropAreaX;
    private float dropAreaY;

    // Whether or not to stop generating leaves at some maximum number
    private bool stopAtLeafLimit;
    private int leafNumberLimit;

    // List of all the leaves that have been generated
    private List<GameObject> listOfLeaves;

    // Dictionary of leaf shapes, and their ratio in the current leaves to be dropped
    private Dictionary<LeafShape, int> sizesAndRatios;
    private int totalRatioWeights;

    // Is visualization?
    private bool isVisualize;
    
    // Use this for initialization
    void Start () {
        // Initial values for simulation
        this.height = 100;
        this.dropAreaX = 100;
        this.dropAreaY = 100;
        this.stopAtLeafLimit = true;
        this.leafNumberLimit = 1000;
        this.listOfLeaves = new List<GameObject>();
        this.isVisualize = MenuSettings.GetIsVisualize(); // Set visualization for simulation

        // TEMP Default leaves defined here - remove when simulation is not started automatically
        LeafShape AcaciaMelanoxylon = new LeafShape(
            "Acacia Melanoxylon", 
            "flat",
            0.021f,
            0.01f,
            1.8f,
            1.2f,
            10f,
            4f);

        LeafShape BurchardiaUmbellata = new LeafShape(
            "Burchardia Umbellata", 
            "flat",
            0.02f,
            0.008f,
            0.9f,
            0.3f,
            20f,
            10f);

        // TEMP Add the default leaves and set their ratios (irrelevant in the case on one leaf)
        this.sizesAndRatios = new Dictionary<LeafShape, int>();
        this.sizesAndRatios.Add(AcaciaMelanoxylon, 1);
        this.sizesAndRatios.Add(BurchardiaUmbellata, 1);

        // TEMP Update the ratio weights sum, used for selecting the next leaf with right probability
        calcTotalRatioWeight();

        // TEMP Automatically begin the simulation on start
        BeginSim(0.01f);
    }

    // Update is called once per frame
    void Update () {
		
	}


    // Begin the simulation, generating a new leaf every timeBetweenDrops seconds
    public void BeginSim(float timeBetweenDrops)
    {
        // End an ongoing simulation if there is one, erase all leaves in scene
        EndSim();
        RemoveAllLeaves();

        // Start the generation of leaves
        InvokeRepeating("DropLeaf", 0f, timeBetweenDrops);
    }


    // End simulation by stopping the generation of new leaves
    public void EndSim()
    {
        CancelInvoke("DropLeaf");
    }


    // Remove all the currently added leaves in the simulation
    public void RemoveAllLeaves()
    {
        // Destroys the game objects
        foreach (var leaf in this.listOfLeaves)
        {
            Destroy(leaf);
        }

        // Empties the list of leaves
        this.listOfLeaves.Clear();
    }


    // Generate a new leaf, and drop it in the simulation
    private void DropLeaf()
    {
        // Make a new leaf object
        GameObject leaf = Resources.Load("Leaf") as GameObject;

        // Get the next leaf type to drop based on their ratios, and then get the size of a single such leaf
        LeafShape nextLeafShape = getLeafSize();
        Vector3 leafSize = getConcreteLeafSize(nextLeafShape);

        // Set the leaf object to have the calculated leaf size
        leaf.GetComponent<Leaf>().SetSize(leafSize.x, leafSize.y, leafSize.z);

        // Place the leaf object randomly within a circle defined by the dropping area
        Vector2 random2DPoint = Random.insideUnitCircle;
        GameObject leafCopy = Instantiate(leaf, new Vector3(random2DPoint.x * (this.dropAreaX/2), this.height, random2DPoint.y * (this.dropAreaY/2)), Quaternion.identity);

        // Rotate the leaf object randomly after it's spawn
        leafCopy.GetComponent<Leaf>().SetRotation(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        // In case of no visualization, turn off the renderer for leaves
        if (!this.isVisualize)
        {
            leafCopy.GetComponent<Renderer>().enabled = false;
        }

        // Add the new leaf to the list of existing leaves
        this.listOfLeaves.Add(leafCopy);
        
        // If terminating at leaf limit, and limit is reached, end the simulation
        if (this.stopAtLeafLimit && this.listOfLeaves.Count >= this.leafNumberLimit){
            EndSim();
        }
    }


    // Using the ratio of leave to drop choose at random, in the right proportions, the next leaf to drop
    private LeafShape getLeafSize()
    {
        // use the cumulative sum of the ratios, and a random number between 0 and the total ratio sum to choose the next leaf
        int cumulativeSum = 0;
        float randomNumber = Random.Range(0, this.totalRatioWeights);
        foreach (LeafShape leafShape in this.sizesAndRatios.Keys)
        {
            cumulativeSum += this.sizesAndRatios[leafShape];
            if (randomNumber < cumulativeSum)
            {
                // Return the chosen next leaf
                return leafShape;
            }
        }

        // Return the default next leaf
        return new LeafShape();
    }


    // Given a leaf shape, returns a size of that leaf, taking the dimensions and their ranges into account
    private Vector3 getConcreteLeafSize(LeafShape leafShape)
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


    // Calculates the sum of all leaf ratios, to use when choosing which next leaf to drop. Only needs to be run when sizesAndRatios dict is updated
    private void calcTotalRatioWeight()
    {
        int sum = 0;
        foreach(LeafShape ls in this.sizesAndRatios.Keys)
        {
            sum += this.sizesAndRatios[ls];
        }
        this.totalRatioWeights = sum;
    }


    // Set new set of leaf sizes and leaf ratios
    public void SetRatiosAndSizes(Dictionary<LeafShape, int> sizesAndRatios)
    {
        this.sizesAndRatios = sizesAndRatios;
        calcTotalRatioWeight();
    }


    // Set the number of maximum leaves for the simulation to stop at
    public void SetLeafNumberLimit(int leafNumberLimit)
    {
        this.leafNumberLimit = leafNumberLimit;
        this.stopAtLeafLimit = true;
    }


    // Remove the maximum number of leaves limit, and let the simulation run until call to stop method
    public void RemoveLeafNumberLimit()
    {
        this.stopAtLeafLimit = false;
    }


    // Set a new drop height for the lewaves to be dropped from
    public void SetDropHeight(float height)
    {
        this.height = height;
    }


    // Set a new size of area that the leaves are dropped from
    public void SetDropArea(float x, float y)
    {
        this.dropAreaX = x;
        this.dropAreaY = y;
    }

    // Returns the list of all leaves that are spawned at the moment
    public List<GameObject> GetListOfLeaves()
    {
        return this.listOfLeaves;
    }
}
