/*
Contains all the classes of data used to write and extract data
*/
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        writer.WriteLine("{\"rounds\":[");
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

    public List<Round> ReadFileForRounds()
    {

        List<Round> tempsRoundsList = new List<Round>();
        Debug.Log(filePath + " If it crashed here probably the file does not exist or is not well formatted");
        StreamReader fs = new StreamReader(filePath, true);
        string jsonString = fs.ReadToEnd();
        fs.Close();
        JObject jsonRounds = JObject.Parse(jsonString);
        JToken round;
        int k = 0;
        while (true)
        {
            try
            {
                round = jsonRounds["rounds"][k];
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.Log("Reached end of file: loaded " + tempsRoundsList);
                break;
            }

            tempsRoundsList.Add(new Round(p_durationInSecond:(int)round["durationInSecond"],
                                          p_xAxisdegreesPerSecond:(float)round["xAxisdegreesPerSecond"],
                                          p_yAxisdegreesPerSecond:(float)round["yAxisdegreesPerSecond"],
                                          p_zAxisdegreesPerSecond:(float)round["zAxisdegreesPerSecond"]));
            k++;
        }
        return tempsRoundsList;
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