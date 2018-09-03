using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BunchRunCsvLoader
{
    // holds leaves and rations for each run. 
    // key: run round id, the first run round is 1. value: Dictionary of this run's leaves and rations
    public static Dictionary<int, Dictionary<LeafData, int>> bunchrunLeafAndRatio = new Dictionary<int, Dictionary<LeafData, int>>();

    // load csv file. 
    // Path: file path.
    // string: error message. Null if no error.
    // return: 0 for normal. -1 for error.
    public static int LoadFile(string path, out string errorMsg)    {
        bunchrunLeafAndRatio.Clear();
        SimSettings.SetRunTimesLeft(0);
        // holds leaf types (by loading the first row of csv)
        List<LeafData> leafType = new List<LeafData>();

        StreamReader reader = new StreamReader(path, System.Text.Encoding.Default, false);
        int lineNum = 0;
        try
        {
            // read file line by line
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    Debug.Log(line);
                    // slit columns by ',' 
                    string[] parts = line.Split(',');
                    int columnNum = 0;
                    // leaves and ratios of a row
                    Dictionary<LeafData, int> leafAndRatio = new Dictionary<LeafData, int>();
                    foreach (string columnData in parts)
                    {
                        // get leaf types from first row
                        if (lineNum == 0)
                        {
                            // get leaf object by name.
                            LeafData shape = CSVImporter.Leaves.Find((LeafData l) => l.Name == columnData);
                            if (shape == null || shape.Name == "")
                            {
                                errorMsg = "Cannot find leaf with name " + columnData;
                                bunchrunLeafAndRatio.Clear();
                                return -1;
                            }
                            else
                            {
                                leafType.Add(shape);
                            }
                        }
                        // get ratios from following rows
                        else
                        {
                            int ratio = 0;
                            bool result = Int32.TryParse(columnData, out ratio);
                            if (!result)
                            {
                                errorMsg = "Cannot cast ration " + columnData;
                                bunchrunLeafAndRatio.Clear();
                                return -1;
                            }
                            else
                            {
                                leafAndRatio.Add(leafType[columnNum], ratio);
                                Debug.Log(leafType[columnNum].Name + ":" + ratio);
                            }

                        }
                        columnNum++;
                    }

                    if (columnNum != leafType.Count)
                    {
                        errorMsg = "Ration number can't match leaf type number.";
                        bunchrunLeafAndRatio.Clear();
                        return -1;
                    }
                    if (lineNum > 0)
                    {
                        // add this row data to the whole bunch run collection
                        bunchrunLeafAndRatio.Add(lineNum, leafAndRatio);
                    }
                }
                lineNum++;
            }
        }
        catch (Exception e) { Debug.Log(e); }
        finally
        {
            reader.Close();
        }

        if (lineNum < 1)
        {
            errorMsg = "No ration lines.";
            bunchrunLeafAndRatio.Clear();
            return -1;
        }
        // set runtimes
        SimSettings.SetRunTimesLeft(bunchrunLeafAndRatio.Count);
        Debug.Log("GetRunTimes = " + SimSettings.GetRunTimeesLeft());            
        errorMsg = "";
        return 0;
    }    
}
