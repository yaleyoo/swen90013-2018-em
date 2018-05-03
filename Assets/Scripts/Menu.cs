/*
 * Created by Yuanyu Guo.
 * User interface for select visualization
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Toggle toggle;

    // Invoke when Start button clicked
    public void ClickStart()
    {
        // If visualization toggle is choosen
        if (toggle.isOn)
        {
            StaticValue.SetIsVisualize(true);
            SceneManager.LoadScene("Main");
        }
        // If visualization toggle is not choosen
        else
        {
            StaticValue.SetIsVisualize(false);
            SceneManager.LoadScene("Main");

        }
    }

    // Invoke when Quit button clicked
    public void ClickQuit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
