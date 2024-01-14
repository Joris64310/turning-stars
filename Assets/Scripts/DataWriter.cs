/*
Contains all the classes of data used to write and extract data
*/
using System;
using System.IO;
using UnityEngine;


public class DataWriter
{
    public string filePath;
    private StreamWriter writer;
    private bool firstSample;
    public DataWriter(string filePath)
    {
        this.filePath = filePath;
        // Create file if needed and write its header
        writer = new StreamWriter(this.filePath, false);  // Erase file if it already exists
        writer.WriteLine("{\"samples\":[");
        firstSample = true;
    }
    public DataWriter(string dstFolderPath, string dstFilename) : this(string.Concat(dstFolderPath, dstFilename)) { }

    //write a line in the json file
    public void WriteSample(SampleToJson sample, bool finalSample = false)
    {
        if (firstSample)
        {
            firstSample = false;
        }
        else
        {
            this.writer.WriteLine(",");
        }

        this.writer.Write(String.Concat("  ", sample.ToJson()));

        if (finalSample)
        {
            EndFile();
        }
    }

    public void EndFile()
    {
        if (this.writer.BaseStream != null)
        {
            this.writer.WriteLine("\n]}");
            this.writer.Close();
        }
    }
}


public class SampleToJson
{
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
        // return JsonConvert.SerializeObject(this, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore});
    }
}


[System.Serializable]
public class CommonExperimentToSave : SampleToJson
{
    public float timestamp;
    //For Eyes
    public Vector3 headSenPos;
    public Quaternion headSenQuat;
    public Vector3 leftEyeOriginPos;
    public Vector3 rightEyeOriginPos;
    public Vector3 leftEyeDirectionPos;
    public Vector3 rightEyeDirectionPos;
    public Vector3 gazeFocusPos;  // Legacy
    public Vector3 gazeFocusInWorldPos;
    public Vector3 gazeFocusInHeadsetPos;
    public ulong leftEyeValidity;
    public ulong rightEyeValidity;
    public float timeElapsedEyes;
    public Vector3 gazeFocusPosRefHeadSenPixOnZedMiniCameraLeft;
    public Vector3 hardCodedTgtPosPixOnZedMiniCameraLeft;
    public Vector3 leftEyeOriginPosRefHead;
    public Vector3 leftEyeDirectionPosRefHead;
    public Vector3 rightEyeOriginPosRefHead;
    public Vector3 rightEyeDirectionPosRefHead;
    public Vector3 combineEyeOriginPosRefHead;
    public Vector3 combineEyeDirectionPosRefHead;
    public ulong combinedEyeValidity;
    public float combinedEyeConvDist;
    public bool combinedEyeConvDistValidity;
    public Vector3 leftEyeFocusPos;
    public Vector3 rightEyeFocusPos;
}

