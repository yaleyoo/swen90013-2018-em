using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Toggle toggle;

    public void ClickStart()
    {
        if (toggle.isOn)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            //TODO
            Debug.Log("Invoke simulator directly from here");
            
        }
    }

    public void ClickQuit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
