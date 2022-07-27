using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;
using static GPSLocationData;  

public class GPSLocation : MonoBehaviour
{
    public TextMeshProUGUI GPSStatus;
    public TextMeshProUGUI latitudeValue;
    public TextMeshProUGUI longitudeValue;
    public TextMeshProUGUI altitudeValue;
    public TextMeshProUGUI timestampValue;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
        UpdateGPSData();
        CreateText();
    }

    List<GPSLocationData> GPSDataList = new List<GPSLocationData>();
    
    void setLocationData()
    {
        GPSLocationData obj= new GPSLocationData(float.Parse(latitudeValue.text),float.Parse(longitudeValue.text) );
        GPSDataList.Add(obj);
    }

    void CreateText()
    {
        string path = Application.dataPath + "/Log.txt";
            // create file if it doesn't exist
            if (!File.Exists(path)) {
                File.WriteAllText(path, "Location tracker \n\n");
            }
        foreach (var i in GPSDataList) 
        {
            string content = latitudeValue + "," + longitudeValue + "\n" + timestampValue + "\n"; 
            File.AppendAllText(path, content);      
        }

        string[] textFromFile = File.ReadAllLines(path);
            foreach (string line in textFromFile)
            {   
                Console.WriteLine(line);
            }   
    }
    

    IEnumerator GPSLoc()
    {

        // check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;
        // start the location service
        Input.location.Start();

        // waits until location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // if service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            GPSStatus.text = "Time out";
            yield break;
        }

        // connection failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            yield break;
        }
        else 
        {
            // access granted
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }

        } // end of GPSlocation

        void UpdateGPSData()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                // access granted to GPS values and it has been initialized
                GPSStatus.text = "Running";
                latitudeValue.text = Input.location.lastData.latitude.ToString();
                longitudeValue.text = Input.location.lastData.longitude.ToString();
                altitudeValue.text = Input.location.lastData.altitude.ToString();
                timestampValue.text = Input.location.lastData.timestamp.ToString();
                setLocationData();
            }
            else 
            {
                // service is stopped
                GPSStatus.text = "Stop";
            }
        } // end of UpdateGPSData

    
    public void OnButtonClick() 
    {
        GPSLoc();
    }

    void Update() {
        UpdateGPSData();
    }

}
