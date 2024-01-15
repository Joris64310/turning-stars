/*
Contains all the classes of data used to write and extract data
*/
using System;
using System.IO;
using UnityEngine;


public class RoundWriter
{
    public string filePath;
    private StreamWriter writer;
    private bool firstSample;
    public RoundWriter(string filePath)
    {
        this.filePath = filePath;
        // Create file if needed and write its header
        writer = new StreamWriter(this.filePath, false);  // Erase file if it already exists
        writer.WriteLine("{\"round\":[");
        firstSample = true;
    }
    public RoundWriter(string dstFolderPath, string dstFilename) : this(string.Concat(dstFolderPath, dstFilename)) { }

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


[System.Serializable]
public class Round : SampleToJson{
    public int durationInSecond;
    public float xAxisdegreesPerSecond;
    public float yAxisdegreesPerSecond;
    public float zAxisdegreesPerSecond;

    public Round(int p_durationInSecond, float p_xAxisdegreesPerSecond, float p_yAxisdegreesPerSecond, float p_zAxisdegreesPerSecond)
    {
        durationInSecond = p_durationInSecond;
        xAxisdegreesPerSecond = p_xAxisdegreesPerSecond;
        yAxisdegreesPerSecond = p_yAxisdegreesPerSecond;
        zAxisdegreesPerSecond = p_zAxisdegreesPerSecond;
    }
}

