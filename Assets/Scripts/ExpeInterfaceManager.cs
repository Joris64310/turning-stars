using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExpeInterfaceManager : MonoBehaviour
{
    // Interfaces
    GameObject expParametersPanel;
    GameObject roundParametersPanel;

    // Input fields
    TMP_InputField expeFolderInputField;
    InputField roundDurationInSecondInputField;
    InputField xAxisdegreesPerSecondInputField;
    InputField yAxisdegreesPerSecondInputField;
    InputField zAxisdegreesPerSecondInputField;

    // Buttons
    Button setExpeParametersButton;
    Button setRoundParametersButton;
    Button setExitButton;


    // Start is called before the first frame update
    void Start()
    {
        // Interfaces
        expParametersPanel = GameObject.Find("Expe Parameters Panel");
        roundParametersPanel = GameObject.Find("Round Parameters Panel");

        // Input fields
        GameObject a =  GameObject.Find("Expe Folder InputField");
        Debug.Log("here");
        Debug.Log(a);

        expeFolderInputField = GameObject.Find("expe Folder InputField").GetComponent<TMP_InputField>();
        roundDurationInSecondInputField = GameObject.Find("Round Duration In Second InputField").GetComponent<InputField>();
        xAxisdegreesPerSecondInputField = GameObject.Find("xAxisdegreesPerSecondInputField").GetComponent<InputField>();
        yAxisdegreesPerSecondInputField = GameObject.Find("yAxisdegreesPerSecondInputField").GetComponent<InputField>();
        zAxisdegreesPerSecondInputField = GameObject.Find("zAxisdegreesPerSecondInputField").GetComponent<InputField>();
        
        // Buttons
        setExpeParametersButton = GameObject.Find("setExpeParametersButton").GetComponent<Button>();
        setRoundParametersButton = GameObject.Find("setRoundParametersButton").GetComponent<Button>();
        setExitButton = GameObject.Find("setExitButton").GetComponent<Button>();

        // Add Listener to buttons
        setExpeParametersButton.onClick.AddListener(TaskOnSetExpeParametersButton);
        setRoundParametersButton.onClick.AddListener(TaskOnSetRoundParametersButton);
        setExitButton.onClick.AddListener(TaskOnSetExitButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void TaskOnSetExpeParametersButton()
    {
        expParametersPanel.SetActive(false);
        roundParametersPanel.SetActive(true);

    }
    private void TaskOnSetRoundParametersButton()
    {
        roundParametersPanel.SetActive(false);

    }
    private void TaskOnSetExitButton()
    {
         Application.Quit();
    }

}
