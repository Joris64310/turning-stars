using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpeInterfaceManager : MonoBehaviour
{
    // Interfaces
    public GameObject expParametersPanel;
    public GameObject roundParametersPanel;

    // Input fields
    public TMP_InputField expeFolderInputField;
    public TMP_InputField participantNumberInputField;
    public TMP_InputField roundDurationInSecondInputField;
    public TMP_InputField xAxisdegreesPerSecondInputField;
    public TMP_InputField yAxisdegreesPerSecondInputField;
    public TMP_InputField zAxisdegreesPerSecondInputField;

    // Buttons
    Button setExpeParametersButton;
    Button setRoundParametersButton;
    Button setExitButton;

    // Initialize round variables
    bool inRoundBool = false;
    int roundNumber = 0;

    System.DateTime roundStartTime;
    System.TimeSpan roundElapsedTime;
    System.TimeSpan roundDurationTime;
 
    int xAxisdegreesPerSecondValue;
    int yAxisdegreesPerSecondValue;
    int zAxisdegreesPerSecondValue;

    // Start is called before the first frame update
    void Start()
    {
        // Set active the round panel
        roundParametersPanel.SetActive(true);
        // Buttons
        setExpeParametersButton = GameObject.Find("Set Expe Parameters Button").GetComponent<Button>();
        setRoundParametersButton = GameObject.Find("Set Round Parameters Button").GetComponent<Button>();
        setExitButton = GameObject.Find("Exit Button").GetComponent<Button>();

        // Add Listener to buttons
        setExpeParametersButton.onClick.AddListener(TaskOnClickSetExpeParametersButton);
        setRoundParametersButton.onClick.AddListener(TaskOnClickSetRoundParametersButton);
        setExitButton.onClick.AddListener(TaskOnClickSetExitButton);

        // Restrict input fields
        participantNumberInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        roundDurationInSecondInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        xAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        yAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        zAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.IntegerNumber;

        //TODO: proposed a participant number


        // Set inactive the round panel
        roundParametersPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRoundBool)
        {
            roundElapsedTime = System.DateTime.UtcNow - roundStartTime;

            if (roundElapsedTime < roundDurationTime)
            {
                //TODO: save data
            }
            else
            {
                roundParametersPanel.SetActive(true);
                roundNumber += 1;
                //TODO: Update text
                inRoundBool = false;
                //TODO: Save and close last data
            }
        }

        
    }
    private void TaskOnClickSetExpeParametersButton()
    {
        expParametersPanel.SetActive(false);
        roundParametersPanel.SetActive(true);

        //TODO: Create the participant folder

    }
    private void TaskOnClickSetRoundParametersButton()
    {
        // Get round duration
        roundStartTime = System.DateTime.UtcNow;
        roundDurationTime = System.TimeSpan.FromSeconds(int.Parse(roundDurationInSecondInputField.text));

        // Get round rotation
        xAxisdegreesPerSecondValue = int.Parse(xAxisdegreesPerSecondInputField.text);
        yAxisdegreesPerSecondValue = int.Parse(yAxisdegreesPerSecondInputField.text);
        zAxisdegreesPerSecondValue = int.Parse(zAxisdegreesPerSecondInputField.text);

        //TODO: Update rotation value of the camera

        roundParametersPanel.SetActive(false);

    }
    private void TaskOnClickSetExitButton()
    {
         Application.Quit();
    }

}
