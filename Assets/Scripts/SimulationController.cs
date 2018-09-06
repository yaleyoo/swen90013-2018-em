﻿using UnityEngine;
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

    private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    // Use this for initialization
    void Start() {
        this.leafGen = new LeafGenerator(SimSettings.GetLeafSizesAndRatios(), this.dropAreaX, this.dropAreaY, this.height);
        this.denCalc = new DensityCalculator();
    }

    // Update is called once per frame
    void Update() {
        this.leaves = GameObject.FindGameObjectsWithTag("Leaf");
        if (this.CanCreateLeaf()) {
            this.CreateLeaf();
        }
        else if (this.HasEnded()) {
            this.CalculateDensity(this.leaves);
        }
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
        // Time how long it takes for the density to be computed (for optimisation use)
        stopWatch.Start();

        DensityCalculationCylinder calcArea = new DensityCalculationCylinder(
                                    leaves,
                                    (this.dropAreaX - this.densityIgnoreBorder),
                                    (this.dropAreaY - this.densityIgnoreBorder)
                                  );
        float density = denCalc.CalculateDensity(calcArea, SimSettings.GetMonteCarloNumIterations());

        // Console log the density and the time it took to compute
        stopWatch.Stop();
        Debug.Log(string.Format("Density calculated as {0} in {1} seconds.",
                                System.Math.Round(density, 6),
                                System.Math.Round(stopWatch.ElapsedMilliseconds / 1000.0, 6)));

        Results.addResult (density);
        this.ChangeToOutputScene();
    }

    /// <summary>
    /// Returns true when all leaf objects are kinematic (frozen)
    /// </summary>
    /// <param name="leaves">List of all leaves in the world</param>
    /// <returns>Whether all leaves have been frozen</returns>
    public bool HasEnded(GameObject[] leaves) {
        foreach (GameObject leaf in leaves) {
            if (!leaf.GetComponent<Rigidbody>().isKinematic) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Alternative stopping check. Returns true when the time since it's first call is longer than
    /// twice the time a leaf takes to fall to teh ground from the dropping height. Twice the time is used
    /// as a safe measure
    /// </summary>
    /// <returns>
    /// Whether or not the time between the first call and the current call of this method exceeds twice the
    /// time it takes a single leaf to fall from the dropping height to the ground
    /// </returns>
    public bool HasEnded()
    {
        // On fist call of the method, start running teh stopwatch, and return false
        if (!this.stopWatch.IsRunning)
        {
            this.stopWatch.Start();
            return false;
        }

        // Not the first time method run, get time for a leaf to fall, and time since first call of method
        double timeToFall = System.Math.Sqrt(SimSettings.GetDropHeight() / System.Math.Abs(Physics.gravity.y));
        double secondsSinceLastLeaf = this.stopWatch.ElapsedMilliseconds / 1000.0;

        // Reset the stopwatch if enough time has elapsed, and return true
        if (secondsSinceLastLeaf > timeToFall*2)
        {
            stopWatch.Stop();
            stopWatch.Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Change to the output scene
    /// </summary>
    public void ChangeToOutputScene() {
        SceneManager.LoadScene(OUTPUT_SCENE);
    }
}
