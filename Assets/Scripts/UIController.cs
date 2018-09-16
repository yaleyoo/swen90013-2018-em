/* 
 * User interface for select:
 *      number of leaves to simulate
 *      single run & batch run
 *      select the type of leaf with ratio
 *      visualization
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Crosstales.FB;

public class UIController : MonoBehaviour
{
    // One of these two toggles must be on but cannot be on at the same time    
    public Toggle batchrunToggle;
    public Toggle singlerunToggle;

    public Toggle visualizeToggle;

    public bool batchrunFileLoadSuccess = false;

    // Dropdown menu to select types of leaves
    public Dropdown leafDropdown;

    // The list to save the selected name
    private List<string> type = new List<string>();

    // The input field and slider to set the ratio
    public Slider inputRatioSlider;
    public Text inputRatioText;

    // Dictionary to save the type-ratio value pair
    private Dictionary<string, int> typeWithRatio;

    // Dictionary of leaf shapes and their ratio (used by LeafGenerator class)
    public static Dictionary<LeafData, int> leavesAndRatios;

    // InputField on the canvas
    public InputField leafNumField;

    // Limit of leaf to be set
    private int leafNum;

    // The flag whether the user click the un limited button
    private bool isUnlimited;

    // InputField for simulation times of mulitrun with same parameters
    public InputField simulationTimesField;

    // Simulation times for multirun
    private int SimulationTimes;

    // Component for message box
    // String to save the warning message
    private string message;
    public Image messageBox;
    public Text messageBoxConent;

    // Component for ListView
    public GameObject leafButton;
    public Transform listContent;
    private LeafButton leafButtonClicked;


    public Text okButtonText;
    public Button deleteButton;

   
    // Initialisation
    private void Start()
    {
        typeWithRatio = new Dictionary<string, int>();

        isUnlimited = false;

        messageBox.gameObject.SetActive(false);

        message = "";

        // Add the type to the dropdown menu
        InitializeLeafDropdown();

        // Hide the progress bar canvas
        ProgressBarController.progressBar.gameObject.SetActive(false);

        // Set the default input value
        leafNumField.text = "5000";
        simulationTimesField.text = "10";
    }


    // Invoke when Start button clicked
    public void StartOnClick()
    {      
        // Actions to submit the number of leaves
        // Check if input leaf limit is valid
        if (System.Int32.TryParse(leafNumField.text, out leafNum))
        {
            // Check if inputed leaf number is greater than 0
            if (leafNum >= 0)
            {
                Debug.Log("You selected " + leafNum + " leafs.");
                SimSettings.SetLeafLimit(leafNum);

                ChangeScene();
            }
            else
            {
                Debug.Log("Invalid number.");

                message = "Invalid number. Please check the leaf quantity.";
                DisplayMessage(message);
            }
        }
        // Click the unlimited button, nothing in input field
        else if (isUnlimited == true)
        {
           
            ChangeScene();
            
        }
        else
        {
            Debug.Log("Invalid input.\n"
                + "Please check the leaf quantity. ");

            message = "Invalid number. Please check the leaf quantity.";
            DisplayMessage(message);
        }
    }

    // Click the button to choose the file
    public void LoadBatchRunCsvClick()
    {
        ClearAddedLeafBox();
        string extensions = "csv";
        string path = FileBrowser.OpenSingleFile("Open File", "", extensions);
        Debug.Log("Selected file: " + path);
        batchrunToggle.isOn = true;
        // Click cancel or didn't choose file
        if (path == "") {
            batchrunFileLoadSuccess = false;
            return;
        }
        string errorMsg = "";
        if (BatchRunCsvLoader.LoadFile(path, out errorMsg) != 0)
        {
            batchrunFileLoadSuccess = false;
            DisplayMessage(errorMsg);
        }
        else
        {
            batchrunFileLoadSuccess = true;
        }                
    }

    // Simulate several times with the same ratios
    private void MultiRun()
    {
        if (!System.Int32.TryParse(simulationTimesField.text, out SimulationTimes))
        {
            message = "Invalid simulation number. Please enter an interger.";
            DisplayMessage(message);
            return;
        }
        else
        {
            SimSettings.SetSimulationTimes(SimulationTimes);
            SimSettings.ResetSimulationTimesLeft();
        }
    }

    // Load the simulation
    private void ChangeScene()
    {
        // If single run toggle is choosen
        if (singlerunToggle.isOn)
        {            
            // To pass the dictionary leavesAndRatios to the LeafGenerator
            // Get the LeafShap based on the leaf name
            GetLeafShape(typeWithRatio);
            if (leavesAndRatios.Count == 0)
            {
                message = "Please input leaves and ratios.";
                DisplayMessage(message);
                return;
            }

            // Multirun
            MultiRun();

            //SimSettings.SetSimulationTimes(1);
            //SimSettings.ResetSimulationTimesLeft();

            SimSettings.SetLeafSizesAndRatios(leavesAndRatios);
            // set visualize flag according to visualizeToggle's status
            SimSettings.SetVisualize(visualizeToggle.isOn);            
            SimSettings.SetRunTimesLeft(1);
            SceneManager.LoadScene("Simulation");
        }
        // If batch run toggle is choosen
        else if (batchrunToggle.isOn)
        {
            ClearAddedLeafBox();

            MultiRun();

            if (!batchrunFileLoadSuccess)
            {
                message = "Load batch run data error.";
                DisplayMessage(message);
                return;
            }
            SimSettings.SetVisualize(false);
            SimSettings.SetBatchrun(true);
            int runRound = BatchRunCsvLoader.batchrunLeafAndRatio.Keys.Count - SimSettings.GetRunTimeesLeft() + 1;
            Debug.Log("current round = " + runRound);
            Dictionary<LeafData, int> leafSizesAndRatios;
            BatchRunCsvLoader.batchrunLeafAndRatio.TryGetValue(runRound, out leafSizesAndRatios);
            SimSettings.SetLeafSizesAndRatios(leafSizesAndRatios);
            SceneManager.LoadScene("Simulation");
        }
    }

    // Invoke when Quit button clicked
    public void QuitOnClick()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    // Set the ratio of the slider
    public void UpdateRatio()
    {
        //inputRatioText = GetComponent<Text>();
        inputRatioText.text = Mathf.Round(inputRatioSlider.value).ToString();
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
        if (System.Int32.TryParse(Mathf.Round(inputRatioSlider.value).ToString(), out ratioInt))
        {
            
            string typeString = leafDropdown.captionText.text;

            // Check if the same leaf type is selected
            if (typeWithRatio.ContainsKey(typeString))
            {
                message = "You have already chosen this type of leaf.\n" +
                    "Please check your selection.";
                DisplayMessage(message);
                return;
            }

            typeWithRatio.Add(typeString, ratioInt);

            // Add a leafButton
            GameObject newButton = Instantiate(leafButton) as GameObject;
            LeafButton button = newButton.GetComponent<LeafButton>();
            button.leafName.text = typeString ;
            button.leafRatio.text = ratioInt.ToString();
            
            newButton.transform.SetParent(listContent);

            // Listen to the leaf button
            button.onClick.AddListener(
                    delegate ()
                    {
                        leafButtonClicked = button;
                        LeafButtonClick();
                    }

                );               
        }
        else
        {
            Debug.Log("Please check the ratio.");
            message = "Please check the ratio.";
            DisplayMessage(message);
        }

    }

    // Click the buttion to delete the selection 
    private void LeafButtonClick()
    {
        message = "Are you sure you want to delete?";
        okButtonText.text = "Cancel";
        deleteButton.gameObject.SetActive(true);
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
        leafNumField.text = "";
        isUnlimited = false;
        GameObject[] leafButtons = GameObject.FindGameObjectsWithTag("LeafButton");
        if (leafButtons.Length > 0)
        {
            foreach (GameObject o in leafButtons)
            {
                Destroy(o);
            }
        }
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
        CSVImporter.ReadCsv();

        foreach (LeafData l in CSVImporter.Leaves)
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

    // Cancel the deletion
    public void OkOnClick()
    {
        messageBox.gameObject.SetActive(false);
        Debug.Log("Click Cancel button");
    }

    // Confirm the deletion 
    public void DeleteOnClick()
    {
        okButtonText.text = "OK";
        deleteButton.gameObject.SetActive(false);
        messageBox.gameObject.SetActive(false);
        Destroy(leafButtonClicked.gameObject);
        typeWithRatio.Remove(leafButtonClicked.leafName.text);
        Debug.Log("Delete Delete button");
    }

    // Get the the selected LeafData according to the name and saved as an dictionary
    private void GetLeafShape(Dictionary<string, int> nameDictionary)
    {
        leavesAndRatios = new Dictionary<LeafData, int>();
        LeafData temp;

        foreach (KeyValuePair<string, int> pair in typeWithRatio)
        {
            temp = CSVImporter.Leaves.Find((LeafData l) => l.Name == pair.Key);
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

    public void OnBatchrunToggleChanged(bool check)
    {
        ClearAddedLeafBox();
    }

    private void ClearAddedLeafBox()
    {
        typeWithRatio.Clear();
        GameObject[] leafButtons = GameObject.FindGameObjectsWithTag("LeafButton");
        if (leafButtons.Length > 0)
        {
            foreach (GameObject o in leafButtons)
            {
                Destroy(o);
            }
        }
    }
}
