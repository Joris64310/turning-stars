using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class ExpeInterfaceManager : MonoBehaviour
{
    //Utility objects to write data in a JSON file
    DataWriter expDataWriter;

    RoundWriter roundsWriter;

    RoundWriter roundWriter;

    // Constant
    int initNumberParticipant = 0;

    public static int roundDuration = 10;

    List<Round> roundsList = new List<Round>()
        {
            new Round(p_durationInSecond:roundDuration,
                      p_xAxisdegreesPerSecond:0.1f,
                      p_yAxisdegreesPerSecond:0,
                      p_zAxisdegreesPerSecond:0),
            new Round(p_durationInSecond:roundDuration,
                      p_xAxisdegreesPerSecond: 0,
                      p_yAxisdegreesPerSecond:0.1f,
                      p_zAxisdegreesPerSecond:0),
            new Round(p_durationInSecond:roundDuration,
                      p_xAxisdegreesPerSecond:0,
                      p_yAxisdegreesPerSecond:0,
                      p_zAxisdegreesPerSecond:0.1f)
                    };
    Round currentRound;


    // Save data
    CommonExperimentToSave data;
    // Path
    string participantFilePath;
    // Interfaces
    public GameObject expParametersPanel;
    public GameObject roundParametersPanel;
    public GameObject timeElapsePanel;

    public TMP_Text roundMessage;
    public TMP_Text timeElapseMessage;

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


    // Start is called before the first frame update
    void Start()
    {
        // Make a new set of rounds
        string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "TurningStarsConfigFiles");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            

        }

        string roundsFilePath = Path.Combine(folderPath, "rounds_list_info.json");
        roundsWriter = new RoundWriter(roundsFilePath);

        if (!File.Exists(roundsFilePath))
        {
            roundsWriter.InitializeFile();
            for (int i = 0; i < roundsList.Count - 1; i++)
            {
                roundsWriter.WriteSample(roundsList[i]);
            
            }
            roundsWriter.WriteSample(roundsList.Last(), finalSample: true);
        }
        else
        {
            roundsList = roundsWriter.ReadFileForRounds();
        }

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
        xAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        yAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        zAxisdegreesPerSecondInputField.contentType = TMP_InputField.ContentType.DecimalNumber;


        //Proposed a participant number and path to save data
        string expeFolder =  Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "turning_stars");
        expeFolderInputField.text = expeFolder;

        if (Directory.Exists(expeFolder))
        {
            string[] dirNames = Directory.GetDirectories(expeFolder);
            Debug.Log(dirNames);
            for (int i = 0; i < dirNames.Count(); i++)
            {

                int lastParticipantNumber = Convert.ToInt32(dirNames[i].Split('s').Last().Split('_')[0]);
                if (lastParticipantNumber >= initNumberParticipant)
                {
                     initNumberParticipant = lastParticipantNumber + 1;
                }
            }
            
        }
        participantNumberInputField.text = initNumberParticipant.ToString();

        // Set inactive the round panel
        roundParametersPanel.SetActive(false);
        timeElapsePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRoundBool)
        {
            roundElapsedTime = System.DateTime.Now - roundStartTime;
            timeElapseMessage.text = ((int)roundElapsedTime.TotalSeconds).ToString();
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
                timeElapsePanel.SetActive(false);
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
        currentRound.durationInSecond = int.Parse(roundDurationInSecondInputField.text);
        roundStartTime = System.DateTime.Now;
        roundDurationTime = System.TimeSpan.FromSeconds(currentRound.durationInSecond);

        // Get round rotation
        currentRound.xAxisdegreesPerSecond  = float.Parse(xAxisdegreesPerSecondInputField.text);
        currentRound.yAxisdegreesPerSecond  = float.Parse(yAxisdegreesPerSecondInputField.text);
        currentRound.zAxisdegreesPerSecond  = float.Parse(zAxisdegreesPerSecondInputField.text);

        // Prepare file to save 
        expDataWriter = new DataWriter(participantFilePath, "round_" + (roundNumber + 1).ToString() + "_data");

        // Save round parameters
        roundWriter = new RoundWriter(string.Concat(participantFilePath, "round_" + (roundNumber + 1).ToString() + "_info"));
        roundWriter.InitializeFile();
        roundWriter.WriteSample(currentRound, finalSample: true);

        //Update rotation value of the camera
        skyboxCamera.SetSkyBoxRotation(new Vector3(currentRound.xAxisdegreesPerSecond,
                                                   currentRound.yAxisdegreesPerSecond,
                                                   currentRound.zAxisdegreesPerSecond));


        roundParametersPanel.SetActive(false);
        inRoundBool = true;
        timeElapsePanel.SetActive(true);

    }
    private void TaskOnClickSetExitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void SetNextRoundParameters()
    {
        if (roundNumber < roundsList.Count)
        {
            currentRound = roundsList[roundNumber];
            roundMessage.text =  string.Format("Round number {0}:", roundNumber + 1);
            roundDurationInSecondInputField.text = currentRound.durationInSecond.ToString();
            xAxisdegreesPerSecondInputField.text = currentRound.xAxisdegreesPerSecond.ToString();
            yAxisdegreesPerSecondInputField.text = currentRound.yAxisdegreesPerSecond.ToString();
            zAxisdegreesPerSecondInputField.text = currentRound.zAxisdegreesPerSecond.ToString();
        }
        else
        {
            roundMessage.text =  string.Format("Round number {0} (extra):", roundNumber + 1);
        }

    }

}
