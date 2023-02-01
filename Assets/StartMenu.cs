using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;


public class StartMenu : MonoBehaviour
{
    // Strings for filepaths to be entered in the startup UI
    public static string json;
    public static string config;
    public static string pos;
    string assets = "Assets/SampleFiles/";
    
    // input objects to receive text input
    public TMP_InputField JsonPathInput;
    public TMP_InputField ConfigPathInput;
    public TMP_InputField PositionPathInput;

    // //Use these for adding options to the Dropdown List
    // Dropdown.OptionData m_NewData, m_NewData2;
    // //The list of messages for the Dropdown
    // List<Dropdown.OptionData> m_Messages = new List<Dropdown.OptionData>();

// unity docs example
    //This is the Dropdown
    public TMP_Dropdown jsonDropDown;
    public TMP_Dropdown configDropDown;
    public TMP_Dropdown positionDropDown;
    
    List<string> jsonFiles = new List<string>();
    List<string> configFiles = new List<string>();
    List<string> positionFiles = new List<string>();

    void Start()
    {

        string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/SampleFiles";
        setSampleFiles(path);
        jsonDropDown = jsonDropDown.GetComponent<TMP_Dropdown>();
        jsonDropDown.ClearOptions();
        
        configDropDown = configDropDown.GetComponent<TMP_Dropdown>();
        configDropDown.ClearOptions();

        positionDropDown = positionDropDown.GetComponent<TMP_Dropdown>();
        positionDropDown.ClearOptions();

        foreach (string json in jsonFiles) {
            jsonDropDown.options.Add(new TMP_Dropdown.OptionData() { text = json });
        }
        foreach (string conf in configFiles) {
            configDropDown.options.Add(new TMP_Dropdown.OptionData() { text = conf });
        }
        foreach (string pos in positionFiles) {
            positionDropDown.options.Add(new TMP_Dropdown.OptionData() { text = pos });
        }
    }

    public void setDropDownStrings() {
        json = assets + dropDownSelected(jsonDropDown);
        config = assets + dropDownSelected(configDropDown);
        pos = assets + dropDownSelected(positionDropDown);

        if (json.Length == 19) {
            // send message to screen
            Debug.Log("Default json");
            json = "Assets/SampleFiles/robot.json";
        }
        
        if (config.Length == 19) {
            // send message to screen
            Debug.Log("Default config");
            config = "Assets/SampleFiles/config.txt";
        }

        if (pos.Length == 19) {
            // send message to screen
            Debug.Log("Default position");
            pos = "Assets/SampleFiles/robotPositions.txt";
        }
    }


    public string dropDownSelected(TMP_Dropdown dropdown) {
        int i = dropdown.value;
        string s = dropdown.options[i].text;
        return s;
    }


    private void setSampleFiles(string folderPath) {
        var allFiles = System.IO.Directory.EnumerateFiles(folderPath);
        foreach (string file in allFiles) {
            string filename = file.Substring(folderPath.Length + 1);
            if (!filename.Contains(".meta")) {
                if (filename.ToLower().Contains(".json")) {
                    jsonFiles.Add(filename);
                } else if (filename.ToLower().Contains("config")) {
                    configFiles.Add(filename);
                } else if (filename.ToLower().Contains("position")) {
                    positionFiles.Add(filename);
                }
            }
        }
    }


    public void setWrittenText() {
        json = assets + JsonPathInput.GetComponent<TMP_InputField>().text;
        config = assets + ConfigPathInput.GetComponent<TMP_InputField>().text;
        pos = assets + PositionPathInput.GetComponent<TMP_InputField>().text;

        if (json.Length == 19) {
            // send message to screen
            Debug.Log("Default json");
            json = "Assets/SampleFiles/robot.json";
        }
        
        if (config.Length == 19) {
            // send message to screen
            Debug.Log("Default config");
            config = "Assets/SampleFiles/config.txt";
        }

        if (pos.Length == 19) {
            // send message to screen
            Debug.Log("Default position");
            pos = "Assets/SampleFiles/robotPositions.txt";
        }
    }

    public void Enter3D() 
    {
        setDropDownStrings();
       
        try {
            System.IO.File.ReadAllText(json);
            System.IO.File.ReadAllText(config);
            System.IO.File.ReadAllText(pos);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } catch {
            Debug.Log("invlid file path");
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
