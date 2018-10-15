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

    // InputField on the canvas
    public InputField leafNumField;

    // Limit of leaf to be set
    private int leafNum;

    // The flag whether the user click the un limited button
    private bool isUnlimited;
   
    // Component for message box
    // String to save the warning message
    private string message;
    public Image messageBox;
    public Text messageBoxConent;
    public Text okButtonText;
    public Button deleteButton;

    private BatchRunUIController batchRunUIController;
    private SingleRunUIController singleRunUIController;

    public void setBatchRunUIController(BatchRunUIController batchRunUIController)
    {
        this.batchRunUIController = batchRunUIController;
    }

    public void setSingleRunUIController(SingleRunUIController singleRunUIController)
    {
        this.singleRunUIController = singleRunUIController;
    }


    // Initialisation
    private void Start()
    {     
        isUnlimited = false;

        messageBox.gameObject.SetActive(false);

        message = "";

        // Hide the progress bar canvas
        ProgressBarController.progressBar.gameObject.SetActive(false);

        // Set the default input value
        leafNumField.text = "5000";
      
        // Reset the progress bar 
        ProgressBarController.progressBar.curProValue = 0;
        ProgressBarController.progressBar.progressImg.fillAmount = 0;
        ProgressBarController.progressBar.proText.text = ProgressBarController.progressBar.curProValue + "%";
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

    // Load the simulation
    private void ChangeScene()
    {
        // If single run toggle is choosen
        if (singlerunToggle.isOn)
        {
            singleRunUIController.run();
        }
        // If batch run toggle is choosen
        else if (batchrunToggle.isOn)
        {
            batchRunUIController.Run();
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

    // Invoke when Quit button clicked
    public void QuitOnClick()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    /************************
     * Message box part start
     * ********************
    **/
    // The method to disaplay the message box
    public void DisplayMessage(string str)
    {
        messageBox.gameObject.SetActive(true);
        // Bring the components to front
        messageBox.gameObject.transform.SetAsLastSibling();
        messageBoxConent.text = str;
    }

    // Cancel the deletion
    public void OkOnClick()
    {
        messageBox.gameObject.SetActive(false);
        Debug.Log("Click Cancel button");
    }
    /************************
     * Message box part end
     * *********************
    **/

    /*
     * The response of clicking reset button.
     * Reset all setting
     * Clear the dictionary typeWithRatio and the display text
     */
    public void ResetOnClick()
    {        
        leafNumField.text = "";
        isUnlimited = false;
        singleRunUIController.Reset();
    }

    // Clear the dictionary typeWithRatio and selected leaf types
    public void ResetSingleRun()
    {
        singleRunUIController.Reset();
    }
}
