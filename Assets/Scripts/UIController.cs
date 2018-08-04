/*
 * Created by Yuanyu Guo.
 * User interface for select visualization
 * 
 * User interface to select the type of leaf
 *      and set the ratio of leaves
 *      
 *  User interface to select number of leaves to drop
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

    public Toggle visualizeToggle;

    // Dropdown menu to select types of leaves
    private Dropdown selectedType;

    // The list to save the selected name
    private List<string> type;

    // The input field to set the ratio
    public InputField inputRatio;

    // The string to save the type and ratio
    private string tempText;

    // Dictionary to save the type-ratio value pair
    private Dictionary<string, int> typeWithRatio;

    // Dictionary of leaf shapes and their ratio (used by LeafGenerator class)
    public static Dictionary<LeafShape, int> leavesAndRatios;

    // Text to show the selected type and ratio
    public Text showText;

    // InputField on the canvas
    public InputField leafNumField;

    // Limit of leaf to be set
    private int leafNum;

    // The total number of selected leaves must be smaller than leafNum
    public static int total_ratio;

    // The flag whether the user click the un limited button
    private bool flag_unlimited;


    // Component for message box
    // String to save the warning message
    private string message;
    public Image messageBox;
    public Text messageBoxConent;

    // Invoke when Start button clicked
    public void ClickStart()
    {
        // To pass the dictionary leavesAndRatios to the LeafGenerator
        // Get the LeafShap based on the leaf name
        GetLeafShape(typeWithRatio);
        SimSettings.SetLeafSizesAndRatios(leavesAndRatios);

        // Actions to submit the number of leaves
        // Check if input leaf limit is valid
        if (System.Int32.TryParse(leafNumField.text, out leafNum))
        {
            // Check if inputed leaf number is greater than 0
            if (leafNum >= 0 && total_ratio == 100)
            {
                Debug.Log("You selected " + leafNum + " leafs.");
                SimSettings.SetLeafLimit(leafNum);

                ChangeScene();
            }
            else if(leafNum >= 0 && total_ratio != 100)
            {
                Debug.Log("Wrong input, please click the REST button and input agian.\n" +
                    "The sume of ratios must be 100.\n");
                
                message = "Wrong input, please click the REST button and input agian.\n" +
                    "The sume of ratios must be 100.\n";
                MessageBox(message);
            }
            else 
            {
                Debug.Log("Invalid number.");

                message = "Invalid number. Please check the leaf quantity.";
                MessageBox(message);
            }
        }
        // Click the unlimited button, nothing in input field
        else if(flag_unlimited == true)
        {
            if (total_ratio != 100)
            {
                Debug.Log("Wrong input, please click the REST button and input agian.\n" +
                   "The sume of ratios must be 100.\n");

                message = "Wrong input, please click the REST button and input agian.\n" +
                   "The sume of ratios must be 100.\n";
                MessageBox(message);
            }
            else
            {
                ChangeScene();
            }
        }
        else
        {
            Debug.Log("Invalid input.\n"
                + "Please check the leaf quantity. ");

            message = "Invalid number. Please check the leaf quantity.";
            MessageBox(message);
        }
    }

    // Load the simulation
    private void ChangeScene()
    {
        // If visualization toggle is choosen
        if (visualizeToggle.isOn)
        {
            SimSettings.SetVisualize(true);
            SceneManager.LoadScene("Simulation");
        }
        // If visualization toggle is not choosen
        else
        {
            SimSettings.SetVisualize(false);
            SceneManager.LoadScene("Simulation");

        }
    }

    // Invoke when Quit button clicked
    public void ClickQuit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    // Initialization
    private void Awake()
    {
        // Dropdown menu for selecting the leaf type
        // Just one dropdown UI, so use FindObjectOfType to get the UI
        selectedType = Dropdown.FindObjectOfType<Dropdown>();
        type = new List<string>();

        typeWithRatio = new Dictionary<string, int>();

        total_ratio = 0;

        flag_unlimited = false;

        messageBox.gameObject.SetActive(false);

        message = "";
    }

    private void Start()
    {
        // Add the type to the dropdown menu
        AddType();
        UpdateDropdownView(type);
    }
 
    /* 
     * The response of clicking add button.
     * Deisplay the selected type with ratio 
     *      and add to the dictionary which just save the name and ratio.
     */
    public void ConfirmButtonClick()
    {
        // The int number to save the ratio of each type
        int ratioInt = 0;
        // Type conversion, string to int
        if (System.Int32.TryParse(inputRatio.text, out ratioInt))
        {
            if (ratioInt > 100)
            {
                Debug.Log("The ratio cannot be larger than 100.");
                message = "The ratio cannot be larger than 100.";
                MessageBox(message);
            }
            else
            {
                string typeString = selectedType.captionText.text;

                typeWithRatio.Add(typeString, ratioInt);

                tempText = "";
                total_ratio = 0;
                foreach (KeyValuePair<string, int> pair in typeWithRatio)
                {
                    tempText = tempText + pair.Key + ", " + pair.Value.ToString() + "%\n";
                    total_ratio += pair.Value;
                }

                showText.text = "The Type of Leaves, Ratio\n" + tempText;
            }
           
        }
        else
        {
            Debug.Log("Please check the ratio.");
            message = "Please check the ratio.";
            MessageBox(message);
        }
        
    }

    /*
     * The response of clicking reset button.
     * Reset all setting
     * Clear the dictionary typeWithRatio and the display text
     */
     public void ResetButtonClick()
    {
        typeWithRatio.Clear();
        showText.text = "The Type of Leaves, Ratio\n";
        inputRatio.text = "";
        leafNumField.text = "";
        flag_unlimited = false;
        //leafNum = 0;
    }

    // Actions when click unlimited button
    public void UnlimitedButtonClick()
    {
        flag_unlimited = true;
        SimSettings.RemoveLeafLimit();
        Debug.Log("Leaf limit set to unlimited.");
        leafNumField.text = "Set as Unlimited";
    }

    // Read leaf name from csv and add them to the dropdown menu
    private void AddType()
    {
        // Read leaf trait csv
        CsvImporter.ReadCsv();

        foreach (LeafShape l in CsvImporter.Leaves)
        {
            type.Add(l.Name);
        }
    }

    public void OKButtonClick()
    {
        messageBox.gameObject.SetActive(false);
        Debug.Log("Click OK button");
    }

    // Display the name on the dropdown menu
    private void UpdateDropdownView(List<string> showType)
    {
        selectedType.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < showType.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = showType[i];
            selectedType.options.Add(tempData);
        }
        // Update the name show on the label of dropdown
        selectedType.captionText.text = showType[0];
    }

    // Get the the selected LeafShape according to the name
    // and saved as an dictionary
    private void GetLeafShape(Dictionary<string, int> nameDictionary)
    {
        leavesAndRatios = new Dictionary<LeafShape, int>();
        LeafShape temp;

        foreach (KeyValuePair<string, int> pair in typeWithRatio)
        {
            temp = CsvImporter.Leaves.Find((LeafShape l) => l.Name == pair.Key);
            leavesAndRatios.Add(temp, pair.Value);
            Debug.Log(temp.Name + ":" + pair.Value);
        }
    }

    // The method to disaplay the message box
    private void MessageBox(string str)
    {
        messageBox.gameObject.SetActive(true);
        // Bring the components to front
        messageBox.gameObject.transform.SetAsLastSibling();
        messageBoxConent.text = str;
    }
}
