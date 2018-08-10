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
using System;
using UnityEditor;

public class UIController : MonoBehaviour {

    public Toggle visualizeToggle;

    // Dropdown menu to select types of leaves
    public Dropdown leafDropdown;

    // The list to save the selected name
    private List<string> type = new List<string>();

    // The input field to set the ratio
    public InputField inputRatio;

    // Dictionary to save the type-ratio value pair
    private Dictionary<string, int> typeWithRatio;

    // Dictionary of leaf shapes and their ratio (used by LeafGenerator class)
    public static Dictionary<LeafShape, int> leavesAndRatios;

    // InputField on the canvas
    public InputField leafNumField;

    // Limit of leaf to be set
    private int leafNum;

    // The total number of selected leaves must be smaller than leafNum
    public static int totalRatio;

    // The flag whether the user click the un limited button
    private bool isUnlimited;


    // Component for message box
    // String to save the warning message
    private string message;
    public Image messageBox;
    public Text messageBoxConent;

    // Component for ListView
    public GameObject leafButton;
    public Transform listContent;
    private LeafButton leafButtonClicked;
   

    public Text OkButtonText;
    public Button DeleteButton;


    // Invoke when Start button clicked
    public void StartOnClick()
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
            if (leafNum >= 0 && totalRatio == 100)
            {
                Debug.Log("You selected " + leafNum + " leafs.");
                SimSettings.SetLeafLimit(leafNum);

                ChangeScene();
            }
            else if(leafNum >= 0 && totalRatio != 100)
            {
                Debug.Log("Wrong input, please click the REST button and input agian.\n" +
                    "The sume of ratios must be 100.\n");
                
                message = "Wrong input, please click the REST button and input agian.\n" +
                    "The sume of ratios must be 100.\n";
                DisplayMessage(message);
            }
            else 
            {
                Debug.Log("Invalid number.");

                message = "Invalid number. Please check the leaf quantity.";
                DisplayMessage(message);
            }
        }
        // Click the unlimited button, nothing in input field
        else if(isUnlimited == true)
        {
            if (totalRatio != 100)
            {
                Debug.Log("Wrong input, please click the REST button and input agian.\n" +
                   "The sume of ratios must be 100.\n");

                message = "Wrong input, please click the REST button and input agian.\n" +
                   "The sume of ratios must be 100.\n";
                DisplayMessage(message);
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
            DisplayMessage(message);
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
    public void QuitOnClick()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    private void Start()
    {
        typeWithRatio = new Dictionary<string, int>();

        totalRatio = 0;

        isUnlimited = false;

        messageBox.gameObject.SetActive(false);

        message = "";

        // Add the type to the dropdown menu
        InitializeLeafDropdown();
    }
 
    /* 
     * The response of clicking add button.
     * Display the selected type with ratio 
     *      and add to the dictionary which just save the name and ratio.
     */
    public void ConfirmOnClick()
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
                DisplayMessage(message);
            }
            else
            {
                string typeString = leafDropdown.captionText.text;

                if (typeWithRatio.ContainsKey(typeString))
                {
                    message = "The leaf has already been added.";
                    Debug.Log(message);                    
                    DisplayMessage(message);
                    return;
                }

                typeWithRatio.Add(typeString, ratioInt);

				// Add a leafButton
                GameObject newButton = Instantiate(leafButton) as GameObject;
                LeafButton button = newButton.GetComponent<LeafButton>();
                button.leafName.text = typeString;
                button.leafRatio.text = ratioInt.ToString() + "%";
                newButton.transform.SetParent(listContent);
                // Listen to the leaf button
                button.onClick.AddListener(
                        delegate ()
                        {
                            leafButtonClicked = button;
                            LeafButtonClick();
                        }

                    );
					
                totalRatio = 0;
                foreach (KeyValuePair<string, int> pair in typeWithRatio)
                {
                    totalRatio += pair.Value;
                }
            }
           
        }
        else
        {
            Debug.Log("Please check the ratio.");
            message = "Please check the ratio.";
            DisplayMessage(message);
        }
        
    }

    private void LeafButtonClick()
    {
        message = "Are you sure to delete?";
        OkButtonText.text = "Cancel";
        DeleteButton.gameObject.SetActive(true);
        DisplayMessage(message);
    }

    /*
     * The response of clicking reset button.
     * Reset all setting
     * Clear the dictionary typeWithRatio and the display text
     */
     public void ResetOnClick()
    {
        typeWithRatio.Clear();
        inputRatio.text = "";
        leafNumField.text = "";
        isUnlimited = false;
		GameObject[] leafButtons= GameObject.FindGameObjectsWithTag("LeafButton");
        if (leafButtons.Length > 0)
        {
            foreach (GameObject o in leafButtons)
            {
                Destroy(o);
            }
        }        
        totalRatio = 0;
        //leafNum = 0;
    }

    // Actions when click unlimited button
    public void UnlimitedOnClick()
    {
        isUnlimited = true;
        SimSettings.RemoveLeafLimit();
        Debug.Log("Leaf limit set to unlimited.");
        leafNumField.text = "Set as Unlimited";
    }

    // Read leaf name from csv and add them to the dropdown menu
    private void InitializeLeafDropdown()
    {
        // Read leaf trait csv
        CsvImporter.ReadCsv();

        foreach (LeafShape l in CsvImporter.Leaves)
        {
            type.Add(l.Name);
        }

        leafDropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < type.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = type[i];
            leafDropdown.options.Add(tempData);
        }
        // Update the name show on the label of dropdown
        leafDropdown.captionText.text = type[0];
    }

    public void OkOnClick()
    {
        messageBox.gameObject.SetActive(false);
        Debug.Log("Click OK button");
    }

    public void DeleteOnClick()
    {        
        OkButtonText.text = "OK";
        DeleteButton.gameObject.SetActive(false);
        messageBox.gameObject.SetActive(false);
        Destroy(leafButtonClicked.gameObject);
        typeWithRatio.Remove(leafButtonClicked.leafName.text);
        totalRatio = totalRatio - Int32.Parse(leafButtonClicked.leafRatio.text.Remove(leafButtonClicked.leafRatio.text.Length - 1, 1));
        Debug.Log("Delete Cancel button");
    }

    // Get the the selected LeafShape according to the name and saved as an dictionary
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
    private void DisplayMessage(string str)
    {
        messageBox.gameObject.SetActive(true);
        // Bring the components to front
        messageBox.gameObject.transform.SetAsLastSibling();
        messageBoxConent.text = str;
    }
}
