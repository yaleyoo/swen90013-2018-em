using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Controls the running of the simulation
/// </summary>
public class SimulationController : MonoBehaviour {

    private const string OUTPUT_SCENE = "Output";

    private LeafGenerator leafGen;
    private DensityCalculator denCalc;
    private int numLeavesCreated = 0;
    private GameObject[] leaves;

    private float dropAreaX = SimSettings.GetDropAreaX() / 2;
    private float dropAreaY = SimSettings.GetDropAreaY() / 2;
    private float height = SimSettings.GetDropHeight();
    private float densityIgnoreBorder = SimSettings.GetDensityIgnoreBorder();

    // Parameters to update the progress bar
    private int targetValueOnce = 100;
    private int runCount = 0;
    private int totalProgress = 100;

    // Use this for initialization
    void Start() {
        this.leafGen = new LeafGenerator(SimSettings.GetLeafSizesAndRatios(), this.dropAreaX, this.dropAreaY, this.height);
        this.denCalc = new DensityCalculator();



        // Batuch Run
        if (SimSettings.GetBatchrun())
        {
            // Initialise the parameter for progres bar
            if (BatchRunCsvLoader.batchrunLeafAndRatio.Count != 0)
            {
                targetValueOnce = 100 / BatchRunCsvLoader.batchrunLeafAndRatio.Count;
            }
            runCount = BatchRunCsvLoader.batchrunLeafAndRatio.Count - SimSettings.GetRunTimeesLeft() + 1;
            totalProgress = targetValueOnce * runCount;

            // Show the progress bar
            ProgressBarController.progressBar.gameObject.SetActive(true);
        }
        else
        {
            ProgressBarController.progressBar.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update() {
        this.leaves = GameObject.FindGameObjectsWithTag("Leaf");
        if (this.CanCreateLeaf()) {
            this.CreateLeaf();
        }
        else if (this.hasEnded(this.leaves)) {
            this.CalculateDensity(this.leaves);
        }

        LoadingProgressBar();
    }

    // The method to update the porgress bar
    private void LoadingProgressBar()
    {
        if (ProgressBarController.progressBar.curProValue < totalProgress)
        {
            ProgressBarController.progressBar.curProValue++;
        }
        ProgressBarController.progressBar.progressImg.fillAmount = ProgressBarController.progressBar.curProValue / 100f;
        ProgressBarController.progressBar.proText.text = ProgressBarController.progressBar.curProValue + "%";
    }

    /// <summary>
    /// Creates a leaf gameobject
    /// </summary>
    /// <returns>A leaf</returns>
    private GameObject CreateLeaf() {
        GameObject leaf = this.leafGen.GetNextLeaf(SimSettings.GetVisualize());
        this.numLeavesCreated++;
        return leaf;
    }

    /// <summary>
    /// Check whether to create a leaf or not based on the
    /// number of leaves created or on the total leaf volume
    /// </summary>
    /// <returns>Can create a leaf</returns>
    public bool CanCreateLeaf() {
        // Limited
        if (SimSettings.GetUseLeafLimit() && this.numLeavesCreated < SimSettings.GetLeafLimit()) {
            return true;
        }

        // Unlimited
        if (!SimSettings.GetUseLeafLimit() && this.totalLeavesVolume(this.leaves) < SimSettings.GetLeafVolumeLimit()) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Calculates the total cumulative volume of all leaves
    /// in an array of leaves
    /// </summary>
    /// <param name="leaves">An array of leaf gameobjects</param>
    /// <returns>Total volume</returns>
    public float totalLeavesVolume(GameObject[] leaves) {
        float sum = 0f;

        foreach (GameObject leaf in leaves) {
            sum += leaf.GetComponent<Leaf>().GetVolume();
        }

        return sum;
    }

    /// <summary>
    /// Calculates the density of leaves and changes to the output scene
    /// to display the results
    /// </summary>
    private void CalculateDensity(GameObject[] leaves) {
        DensityCalculationCylinder calcArea = new DensityCalculationCylinder(
                                    leaves,
                                    (this.dropAreaX - this.densityIgnoreBorder),
                                    (this.dropAreaY - this.densityIgnoreBorder)
                                  );
        float density = denCalc.CalculateDensity(calcArea, SimSettings.GetMonteCarloNumIterations());
        Debug.Log("Density calculated as: " + density);
		Results.addResult (density);
        this.ChangeToOutputScene();
    }

    /// <summary>
    /// Returns true when all leaf objects are kinematic (frozen)
    /// </summary>
    /// <param name="leaves">List of all leaves in the world</param>
    /// <returns>Whether all leaves have been frozen</returns>
    public bool hasEnded(GameObject[] leaves) {
        foreach (GameObject leaf in leaves) {
            if (!leaf.GetComponent<Rigidbody>().isKinematic) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Change to the output scene
    /// </summary>
    public void ChangeToOutputScene() {
        SceneManager.LoadScene(OUTPUT_SCENE);
    }
}
