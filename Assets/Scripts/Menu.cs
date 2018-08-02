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

public class Menu : MonoBehaviour {

    public Toggle toggle;

    // Dropdown menu to select types of leaves
    private Dropdown selectedType;

    // The list to save the selected name
    private List<string> type;

    // The input field to set the ratio
    private InputField inputRatio;

    // Button to confirm the type and ratio
    private Button confirmButton;

    // Dictionary to save the type-ratio value pair
    private Dictionary<string, int> typeWithRatio;

    // Dictionary of leaf shapes and their ratio (used by LeafGenerator class)
    public static Dictionary<LeafShape, int> leavesAndRatios;

    // Button to reset the leaves and ratio
    private Button resetButton;

    // InputField on the canvas
    public InputField leafNumField;
    // Limit of leaf to be set
    private int leafNum;

    private Button unlimitedButton;

    // The total number of selected leaves must be 100%
    public static int total_ratio;

    // The flag whether the user click the un limited button
    private bool flag_unlimited;

    // Component for message box
    // String to save the warning message
    private string message;
    private Image box;
    private Button okButton;
    private Text boxConent;
    //private bool flag_box;    // whether the message box is shown

    // Component for ListView
    public GameObject leafButton;
    public Transform listContent;
    


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
            if (leafNum >= 0 && total_ratio == 100)
            {
                Debug.Log("You selected " + leafNum + " leafs.");
                LeafLimit.SetLeafNumberLimit(leafNum);

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

    // Load Main scene
    private void ChangeScene()
    {
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

        // Get the component - ConfirmButton
        confirmButton = GameObject.Find("ConfirmButton").GetComponent<Button>();

        // Get the component - ResetButton
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();

        typeWithRatio = new Dictionary<string, int>();

        leafNumField = GameObject.Find("QuantityInput").GetComponent<InputField>();

        unlimitedButton = GameObject.Find("Unlimited").GetComponent<Button>();

        total_ratio = 0;

        flag_unlimited = false;

        box = GameObject.Find("MessageBox").GetComponent<Image>();
        okButton = GameObject.Find("OKButton").GetComponent<Button>();
        boxConent = GameObject.Find("content_box").GetComponent<Text>();
        box.gameObject.SetActive(false);
        //flag_box = false;
        message = "";
    }

    private void Start()
    {
        // Listen to the add Button
        confirmButton.onClick.AddListener(
            delegate ()
            {
                ConfirmButtonClick();
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

        // There is a message box, listen to the ok button
        okButton.onClick.AddListener(
                delegate ()
                {
                    OKButtonClick();
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
    private void ConfirmButtonClick()
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
                            LeafButtonClick(newButton, typeString, ratioInt);
                        }

                    );


                total_ratio = 0;
                foreach (KeyValuePair<string, int> pair in typeWithRatio)
                {
                     total_ratio += pair.Value;
                }


            }
           
        }
        else
        {
            Debug.Log("Please check the ratio.");
            message = "Please check the ratio.";
            MessageBox(message);
        }
        
    }

    private void LeafButtonClick(GameObject button, String typeString, int ratioInt)
    {
        if(EditorUtility.DisplayDialog("Delete warning", "Are you sure to delete?", "Yes", "No"))
        {
            Destroy(button);
            typeWithRatio.Remove(typeString);
            total_ratio = total_ratio - ratioInt;
        }               
    }

    /*
     * The response of clicking reset button.
     * Reset all setting
     * Clear the dictionary typeWithRatio and the display text
     */
    private void ResetButtonClick()
    {
        typeWithRatio.Clear();
        inputRatio.text = "";
        leafNumField.text = "";
        flag_unlimited = false;
        GameObject[] leafButtons= GameObject.FindGameObjectsWithTag("LeafButton");
        if (leafButtons.Length > 0)
        {
            foreach (GameObject o in leafButtons)
            {
                Destroy(o);
            }
        }        
        total_ratio = 0;
        //leafNum = 0;
    }

    // Actions when click unlimited button
    private void UnlimitedButtonClick()
    {
        flag_unlimited = true;
        LeafLimit.RemoveLeafNumberLimit();
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

    private void OKButtonClick()
    {
        box.gameObject.SetActive(false);
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
        box.gameObject.SetActive(true);
        // Bring the components to front
        box.gameObject.transform.SetAsLastSibling();
        boxConent.text = str;
    }
}
