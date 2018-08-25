/*
 * Created by Michael Lumley.
 * Controls the running of the simulation
 */
 
 using UnityEngine;

public class SimulationController : MonoBehaviour {

    private LeafGenerator leafGen;

	// Use this for initialization
	void Start () {
        leafGen = new LeafGenerator(SimSettings.GetLeafSizesAndRatios());
    }
	
	// Update is called once per frame
	void Update () {
        if (!(SimSettings.GetUseLeafLimit() && SimSettings.GetNumLeavesDropped() >= SimSettings.GetLeafLimit())) {
            GameObject leaf = leafGen.GetNextLeaf();
            SimSettings.SetNumLeavesDropped(SimSettings.GetNumLeavesDropped() + 1);
        }
        
	}

    
}
