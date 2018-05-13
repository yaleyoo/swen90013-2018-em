using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeafSelection : MonoBehaviour {
    // InputField on the canvas
    public InputField leafNumField;
    // Limit of leaf to be set
    private int leafNum;
    // Whether or not to stop generating leaves at some maximum number
    //private bool stopAtLeafLimit = true;
    //private int leafNumberLimit = 1000;

    // Actions when click submit button
    public void Submit() {
        // Check if input leaf limit is valid
        if (System.Int32.TryParse(leafNumField.text, out leafNum)) {
            // Check if inputed leaf number is greater than 0
            if (leafNum >= 0) {
                Debug.Log("You selected " + leafNum + " leafs.");
                // Change limit
                //this.GetComponent<LeafGenerator>().SetLeafNumberLimit(leafNum);
                LeafLimit.SetLeafNumberLimit(leafNum);
                //LeafLimit.FindGenerator().SetLeafNumberLimit(leafNum);
                //SetLeafNumberLimit(leafNum);
                SceneManager.LoadScene("Main");
            } else {
                Debug.Log("Invalid number.");
            }
        } else {
            Debug.Log("Invalid input.");
        }
    }

    // Actions when click unlimited button
    public void Unlimited() {
        //this.GetComponent<LeafGenerator>().RemoveLeafNumberLimit();
        LeafLimit.RemoveLeafNumberLimit();
        //RemoveLeafNumberLimit();
        Debug.Log("Leaf limit set to unlimited.");
        SceneManager.LoadScene("Main");
    }

    public int GetLeafLimit() {
        return leafNum;
    }

}
