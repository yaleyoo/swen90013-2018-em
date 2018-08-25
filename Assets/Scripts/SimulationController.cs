/*
 * Created by Michael Lumley.
 * Controls the running of the simulation
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour {

    private LeafGenerator leafGen;
    private DensityCalculator denCalc;
    private int numLeavesCreated = 0;
    private GameObject[] leaves;

    // Use this for initialization
    void Start() {
        leafGen = new LeafGenerator(SimSettings.GetLeafSizesAndRatios());
        denCalc = new DensityCalculator();
    }

    // Update is called once per frame
    void Update() {
        this.leaves = GameObject.FindGameObjectsWithTag("Leaf");
        if (this.CanCreateLeaf()) {
            this.CreateLeaf();
        }

        if (this.hasEnded()) {
            this.CalculateDensity(this.leaves);
        }
    }

    private void CreateLeaf() {
        leafGen.GetNextLeaf();
        this.numLeavesCreated++;
    }

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
        DensityCalculationCylinder calcArea = new DensityCalculationCylinder(leaves,
                                    ((SimSettings.GetDropAreaX() / 2) - SimSettings.GetDensityIgnoreBorder()),
                                    ((SimSettings.GetDropAreaY() / 2) - SimSettings.GetDensityIgnoreBorder())
                                  );
        float density = denCalc.CalculateDensity(calcArea, SimSettings.GetMonteCarloNumIterations());
        Debug.Log("Density calculated as: " + density);
        Results.SetDensity(density);
        this.ChangeToOutputScene();
    }

    /// <summary>
    /// Returns true when all leaf objects are kinematic (frozen)
    /// </summary>
    public bool hasEnded() {
        GameObject[] leaves = GameObject.FindGameObjectsWithTag("Leaf");

        foreach (GameObject leaf in leaves) {
            if (!leaf.GetComponent<Rigidbody>().isKinematic) {
                return false;
            }
        }
        return true;
    }

    // Changes the Unity scene to the output scene, where results calculated here are displayed and saved
    public void ChangeToOutputScene() {
        SceneManager.LoadScene("Output");
    }

}
