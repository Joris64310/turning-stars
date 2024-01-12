using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpeInterfaceManager : MonoBehaviour
{
    // Constant
    string pathRefProject = "C:\\Users\\joris\\Documents\\turning_stars_data";
    int initNumberParticipant = 0;

    int roundDuration = 60

    List<Round> roundsList = new List<Round>()
    {
        new Round(durationInSecondParam = roundDuration,
                  xAxisdegreesPerSecondParam = 10,
                  yAxisdegreesPerSecondParam = 0,
                  zAxisdegreesPerSecond = 0),
        new Round(durationInSecondParam = roundDuration,
                  xAxisdegreesPerSecondParam = 0,
                  yAxisdegreesPerSecondParam = 10,
                  zAxisdegreesPerSecond = 0),
        new Round(durationInSecondParam = roundDuration,
                  xAxisdegreesPerSecondParam = 0,
                  yAxisdegreesPerSecondParam = 0,
                  zAxisdegreesPerSecond = 10)
                  };

    // Path
    string participantFilePath;
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

        //Proposed a participant number and path to save data
        participantNumberInputField.text = initNumberParticipant.ToString();
        expeFolderInputField.text = pathRefProject;

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

        // Create expe folder
        participantFilePath = expeFolderInputField.text;
        if (!Directory.Exists(participantFilePath))
        {
            Directory.CreateDirectory(participantFilePath);
        }
        // Create participant folder
        participantFilePath += String.Format("/s{0}_{1}/", participantNumberInputField.text, DateTime.Now.ToString(@"dd-MM-yy_HH\hmm"));
        if (!Directory.Exists(participantFilePath))
        {
            Directory.CreateDirectory(participantFilePath);
        }


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
class Round
{
  public int durationInSecond;
  public int xAxisdegreesPerSecond;
  public int yAxisdegreesPerSecond;
  public int zAxisdegreesPerSecond;

  // Create a class constructor for the Car class
  public Round(int durationInSecondParam, int xAxisdegreesPerSecondParam, int yAxisdegreesPerSecondParam, int zAxisdegreesPerSecondParam)
  {
    durationInSecond = durationInSecondParam;
    xAxisdegreesPerSecond = xAxisdegreesPerSecondParam;
    yAxisdegreesPerSecond = yAxisdegreesPerSecondParam;
    zAxisdegreesPerSecond = zAxisdegreesPerSecondParam;
  }
}
