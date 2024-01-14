using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpeInterfaceManager : MonoBehaviour
{
    //Utility objects to write data in a JSON file
    DataWriter expDataWriter;

    // Constant
    int initNumberParticipant = 0;

    public static int roundDuration = 10;

    List<Round> roundsList = new List<Round>()
        {
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond:0.1f,
                      yAxisdegreesPerSecond:0,
                      zAxisdegreesPerSecond:0),
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond:-0.1f,
                      yAxisdegreesPerSecond:0,
                      zAxisdegreesPerSecond:0),
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond: 0,
                      yAxisdegreesPerSecond:0.1f,
                      zAxisdegreesPerSecond:0),
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond:0,
                      yAxisdegreesPerSecond:-0.1f,
                      zAxisdegreesPerSecond:0),
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond:0,
                      yAxisdegreesPerSecond:0,
                      zAxisdegreesPerSecond:0.1f),
            new Round(durationInSecond:roundDuration,
                      xAxisdegreesPerSecond:0,
                      yAxisdegreesPerSecond:0,
                      zAxisdegreesPerSecond:-0.1f)
                    };


    // Save data
    CommonExperimentToSave data;
    // Path
    string participantFilePath;
    // Interfaces
    public GameObject expParametersPanel;
    public GameObject roundParametersPanel;
    public TMP_Text roundMessage;

    // Camera
    public GameObject cameraHolder;
    SkyboxCamera skyboxCamera;

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
 
    float xAxisdegreesPerSecondValue;
    float yAxisdegreesPerSecondValue;
    float zAxisdegreesPerSecondValue;

    // Start is called before the first frame update
    void Start()
    {
        // Get camera script
        skyboxCamera = cameraHolder.GetComponent<SkyboxCamera>();
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
        expeFolderInputField.text = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "turning_stars");

        // Set inactive the round panel
        roundParametersPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRoundBool)
        {
            roundElapsedTime = System.DateTime.UtcNow - roundStartTime;
            Debug.Log(roundElapsedTime);
            data = new CommonExperimentToSave{timestamp = (float)roundElapsedTime.TotalSeconds + roundElapsedTime.Milliseconds / 1000};
            if (roundElapsedTime < roundDurationTime)
            {
                expDataWriter.WriteSample(data);
            }
            else
            {
                expDataWriter.WriteSample(data, finalSample: true);
                roundParametersPanel.SetActive(true);
                roundNumber += 1;
                inRoundBool = false;
                skyboxCamera.SetSkyBoxRotation(new Vector3(0, 0, 0));
                SetNextRoundParameters();
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

        // Set first round value
        SetNextRoundParameters();

    }

    private void TaskOnClickSetRoundParametersButton()
    {
        // Get round duration
        roundStartTime = System.DateTime.UtcNow;
        roundDurationTime = System.TimeSpan.FromSeconds(int.Parse(roundDurationInSecondInputField.text));

        // Get round rotation
        xAxisdegreesPerSecondValue = float.Parse(xAxisdegreesPerSecondInputField.text);
        yAxisdegreesPerSecondValue = float.Parse(yAxisdegreesPerSecondInputField.text);
        zAxisdegreesPerSecondValue = float.Parse(zAxisdegreesPerSecondInputField.text);

        // Prepare file to save 
        expDataWriter = new DataWriter(participantFilePath, "round_" + roundNumber.ToString());

        //Update rotation value of the camera
        skyboxCamera.SetSkyBoxRotation(new Vector3(xAxisdegreesPerSecondValue, yAxisdegreesPerSecondValue, zAxisdegreesPerSecondValue));


        roundParametersPanel.SetActive(false);
        inRoundBool = true;

    }
    private void TaskOnClickSetExitButton()
    {
         Application.Quit();
    }
    private void SetNextRoundParameters()
    {
        roundMessage.text =  string.Format("Round number {0}:", roundNumber + 1);
        roundDurationInSecondInputField.text = roundsList[roundNumber].DurationInSecond.ToString();
        xAxisdegreesPerSecondInputField.text = roundsList[roundNumber].XAxisdegreesPerSecond.ToString();
        yAxisdegreesPerSecondInputField.text = roundsList[roundNumber].YAxisdegreesPerSecond.ToString();
        zAxisdegreesPerSecondInputField.text = roundsList[roundNumber].ZAxisdegreesPerSecond.ToString();
    }

}
public class Round{
    private int _durationInSecond;
    private float _xAxisdegreesPerSecond;
    private float _yAxisdegreesPerSecond;
    private float _zAxisdegreesPerSecond;

    public Round(int durationInSecond, float xAxisdegreesPerSecond, float yAxisdegreesPerSecond, float zAxisdegreesPerSecond)
    {
        _durationInSecond = durationInSecond;
        _xAxisdegreesPerSecond = xAxisdegreesPerSecond;
        _yAxisdegreesPerSecond = yAxisdegreesPerSecond;
        _zAxisdegreesPerSecond = zAxisdegreesPerSecond;
    }

    public int DurationInSecond
    {
        get => _durationInSecond;
        set => _durationInSecond = value;
    }
    public float XAxisdegreesPerSecond
    {
        get => _xAxisdegreesPerSecond;
        set => _xAxisdegreesPerSecond = value;
    }

    public float YAxisdegreesPerSecond
    {
        get => _yAxisdegreesPerSecond;
        set => _yAxisdegreesPerSecond = value;
    }

    public float ZAxisdegreesPerSecond
    {
        get => _zAxisdegreesPerSecond;
        set => _zAxisdegreesPerSecond = value;
    }
}