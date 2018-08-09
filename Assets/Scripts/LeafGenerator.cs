/*
 * Created by Marko Ristic.
 * Generation and simulation of leaves dropping
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeafGenerator : MonoBehaviour {

    // List of all the leaves that have been generated
    private List<GameObject> listOfLeaves;

    // Sum of leaf ratios use when generating leaves
    private int totalRatioWeights;
    
	// Preset of Colors for differentiating leaves in various types
	List<Color> preColors = new List<Color>{Color.green, Color.red, Color.cyan, Color.yellow, 
		Color.magenta, Color.blue,Color.gray, Color.white, Color.black };

	// Dictionary of all kinds of leaf names and their corresponding colors
	private Dictionary<string, Color> nameAndColors = new Dictionary<string, Color>();
    
    // Use this for initialization
    void Start () {
        // Initialize the list of leaves in the simulation
        //TODO remove this and associated methods, should use the fact leaves are tagged 
        //(since leaves with -y value will be destroyed corrupting this list)
        this.listOfLeaves = new List<GameObject>();

        // Update the ratio weights sum, used for selecting the next leaf with right probability
        calcTotalRatioWeight();

        // Automatically begin the simulation, using settings in static SimSettings class
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
        GameObject leaf;

        // Get the next leaf type to drop based on their ratios, and then get the size of a single such leaf
        LeafShape nextLeafShape = getLeafSize();
        Vector3 leafSize = getConcreteLeafSize(nextLeafShape);

        // Make a new leaf object of correct type
        if (nextLeafShape.LeafForm.ToLower() == "round") {
            leaf = Resources.Load("RoundLeaf") as GameObject;
        }
        else {
            leaf = Resources.Load("FlatLeaf") as GameObject;
        }

        // Place the leaf object randomly within a circle defined by the dropping area
        Vector2 random2DPoint = Random.insideUnitCircle;
        GameObject leafCopy = Instantiate(leaf, new Vector3(random2DPoint.x * (SimSettings.GetDropAreaX()/2), SimSettings.GetDropHeight(), random2DPoint.y * (SimSettings.GetDropAreaY()/2)), Quaternion.identity);

		// Set colors for different types of leaves
		setColor (nextLeafShape, leafCopy);

		// Set the leaf object to have the calculated leaf size
		leaf.GetComponent<Leaf>().SetSize(leafSize.x, leafSize.y, leafSize.z);

        // Rotate the leaf object randomly after it's spawn
        leafCopy.GetComponent<Leaf>().SetRotation(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        // In case of no visualization, turn off the renderer for leaves
        if (!SimSettings.GetVisualize())
        {
            leafCopy.GetComponent<Renderer>().enabled = false;
        }

        // Add the new leaf to the list of existing leaves
        this.listOfLeaves.Add(leafCopy);
        
        // If terminating at leaf limit, and limit is reached, end the simulation
        if (SimSettings.GetUseLeafLimit() && this.listOfLeaves.Count >= SimSettings.GetLeafLimit()){
            EndSim();
        }
    }


    // Using the ratio of leave to drop choose at random, in the right proportions, the next leaf to drop
    private LeafShape getLeafSize()
    {
        // use the cumulative sum of the ratios, and a random number between 0 and the total ratio sum to choose the next leaf
        int cumulativeSum = 0;
        float randomNumber = Random.Range(0, this.totalRatioWeights);
        foreach (LeafShape leafShape in SimSettings.GetLeafSizesAndRatios().Keys)
        {
            cumulativeSum += SimSettings.GetLeafSizesAndRatios()[leafShape];
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
        foreach(LeafShape ls in SimSettings.GetLeafSizesAndRatios().Keys)
        {
            sum += SimSettings.GetLeafSizesAndRatios()[ls];
        }
        this.totalRatioWeights = sum;
    }

	// Set colors according to types of leaves
	private void setColor(LeafShape shape, GameObject leaf){
		
		string nameOfLeaf = shape.Name;

		// If the leaf is already be paired with a color, then use its paired color
		if (!nameAndColors.ContainsKey (nameOfLeaf)) {

			// If preset colors are not exhausted
			if (preColors.Count != 0) {
				
				// Pair the leaf name and a new preset color into dictionary
				nameAndColors.Add (nameOfLeaf, preColors [0]);

				// Change the color of this leaf
				leaf.GetComponent<MeshRenderer> ().material.color = preColors [0];

				preColors.Remove (preColors [0]);
			} 
			else {
				while (true) {
					
					// Random a new color 
					float r = Random.Range (0f, 1f);
					float g = Random.Range (0f, 1f);
					float b = Random.Range (0f, 1f);
					Color randomColor = new Color(r, g, b);

					// If the random color is not used, change the color then break the loop
					if (!nameAndColors.ContainsValue (randomColor)) {
						
						// Pair the leaf name and the new random color
						nameAndColors.Add (nameOfLeaf, randomColor);
						leaf.GetComponent<MeshRenderer> ().material.color = randomColor;
						break;
					}
				}
			}
		}
		else
			// colorate the leaf by its corresponding color in dictionary
			leaf.GetComponent<MeshRenderer> ().material.color = nameAndColors [nameOfLeaf];

	}
}
