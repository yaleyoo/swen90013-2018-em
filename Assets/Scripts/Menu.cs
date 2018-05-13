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

public class Menu : MonoBehaviour {

    public Toggle toggle;

    // Dropdown menu to select types of leaves
    private Dropdown selectedType;

    // The list to save the selected name
    private List<string> type;

    // The input field to set the ratio
    private InputField inputRatio;

    // The string to save the type and ratio
    private string tempText;

    // Button to confirm the type and ratio
    private Button addButton;

    // Dictionary to save the type-ratio value pair
    private Dictionary<string, int> typeWithRatio;

    // Dictionary of leaf shapes and their ratio (used by LeafGenerator class)
    public static Dictionary<LeafShape, int> leavesAndRatios;

    // Text to show the selected type and ratio
    private Text showText;

    // Button to reset the leaves and ratio
    private Button resetButton;

    // InputField on the canvas
    public InputField leafNumField;
    // Limit of leaf to be set
    private int leafNum;

    private Button unlimitedButton;

    // The total number of selected leaves must be smaller than leafNum
    int total_num;

    // Invoke when Start button clicked
    public void ClickStart()
    {
        // To pass the dictionary leavesAndRatios to the LeafGenerator
        // Get the LeafShap based on the leaf name
        GetLeafShape(typeWithRatio);

      

        // Actions to submit the number of leaves
        // Check if input leaf limit is valid
        if (System.Int32.TryParse(leafNumField.text, out leafNum))
        {
            // Check if inputed leaf number is greater than 0
            if (leafNum >= 0 && total_num == leafNum)
            {
                Debug.Log("You selected " + leafNum + " leafs.");
                // Change limit
                //this.GetComponent<LeafGenerator>().SetLeafNumberLimit(leafNum);
                LeafLimit.SetLeafNumberLimit(leafNum);
                //LeafLimit.FindGenerator().SetLeafNumberLimit(leafNum);
                //SetLeafNumberLimit(leafNum);
                //SceneManager.LoadScene("Main");

               

                // If visualization toggle is choosen
                if (toggle.isOn)
                {
                    MenuSettings.SetIsVisualize(true);
                    SceneManager.LoadScene("Main");
                }
                // If visualization toggle is not choosen
                else
                {
                    MenuSettings.SetIsVisualize(false);
                    SceneManager.LoadScene("Main");

                }
            }
            else if(total_num != leafNum)
            {
                Debug.Log("Wrong input, please check the ratios.\n"
                    + "The ratio will be delete, please input again.");
                ResetButtonClick();
            }
            else
            {
                Debug.Log("Invalid number.");
            }
        }
        else
        {
            Debug.Log("Invalid input.");
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

        // Get the input field to input the ratio
        inputRatio = GameObject.Find("RatioInput").GetComponent<InputField>();

        // Get the component - AddButton
        addButton = GameObject.Find("AddButton").GetComponent<Button>();

        // Get the component - DisplayText
        showText = GameObject.Find("DisplayText").GetComponent<Text>();

        // Get the component - ResetButton
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();

        typeWithRatio = new Dictionary<string, int>();

        leafNumField = GameObject.Find("QuantityInput").GetComponent<InputField>();

        unlimitedButton = GameObject.Find("Unlimited").GetComponent<Button>();

        total_num = 0;
    }

    private void Start()
    {
        // Listen to the add Button
        addButton.onClick.AddListener(
            delegate ()
            {
                AddButtonClick();
            }    
        );

        // Listen to the reset Button
        resetButton.onClick.AddListener(
            delegate ()
            {
                ResetButtonClick();
            }
        );

        // Listen to the unlimited button
        unlimitedButton.onClick.AddListener(
            delegate ()
            {
                UnlimitedButtonClick();
            }    
        );

        // Add the type to the dropdown menu
        AddType();
        UpdateDropdownView(type);
    }

    
    /* 
     * The response of clicking add button.
     * Deisplay the selected type with ratio 
     *      and add to the dictionary which just save the name and ratio.
     */
    private void AddButtonClick()
    {
        // Type conversion, string to int
        int ratioInt = int.Parse(inputRatio.text);
        string typeString = selectedType.captionText.text;

        typeWithRatio.Add(typeString, ratioInt);

        tempText = "";
        total_num = 0;
        foreach (KeyValuePair<string, int> pair in typeWithRatio)
        {
            tempText = tempText + pair.Key + ", " + pair.Value.ToString() + "\n";
            total_num += pair.Value;
        }

        showText.text = "The Type of Leaves, Ratio\n" + tempText;  
    }

    /*
     * The response of clicking reset button.
     * Clear the dictionary typeWithRatio and the display text
     */
     private void ResetButtonClick()
    {
        typeWithRatio.Clear();
        showText.text = "The Type of Leaves, Ratio\n";
    }

    // Actions when click unlimited button
    private void UnlimitedButtonClick()
    {
        LeafLimit.RemoveLeafNumberLimit();
        //RemoveLeafNumberLimit();
        Debug.Log("Leaf limit set to unlimited.");
        //SceneManager.LoadScene("Main");
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
        }
    }
}
