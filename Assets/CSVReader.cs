using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public string fileName;
    public List<double> datax;
    public List<double> datay;
    public List<double> dataz;
    public List<double> times;

    void Start()
    {
        datax = new List<double>();
        datay = new List<double>();
        dataz = new List<double>();
        times = new List<double>();
        ReadCSV();
    }

    void ReadCSV()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string[] rawData = File.ReadAllLines(filePath);
           
            for (int i = 1; i < rawData.Length; i++)
            {
                string[] row = rawData[i].Split(',');

                float time = float.Parse(row[0]);
                float x = float.Parse(row[1]);
                float y = float.Parse(row[2]);
                float z = float.Parse(row[3]);

                times.Add(time);

                if (Mathf.Abs(x) < 0.3) { x = 0; }
                datax.Add(x);
                if (Mathf.Abs(y) < 0.3) { y = 0; }
                datay.Add(y);
                if (Mathf.Abs(z) < 0.3) { z = 0; }
                dataz.Add(z);
            }
            Debug.Log(datax);
            Debug.Log(datay);
            Debug.Log(dataz);
        }
        else
        {
            Debug.LogError("File not found!");
        }

        Debug.Log("Number of elements in times list: " + times.Count);

    }
}