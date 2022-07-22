using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

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
    }

    List<string> GPSDataList = new List<string>();
    class GPSLocationData {
        string Coordinates{get; set;}
        int Id{get; set;}
        void GPSDataList(string Coordinates, int Id)
        {}
    }

    void setLocationData(string coordinates_pass, int id_pass)
    {
        string Coordinates A = new Coordinates(coordinates_pass, id_pass);
        GPSDataList.add(A);
    }

    /* void GPSLoc() 
    {
        setLocationData(Coordinates, id);
    } */
    //stop 
    void stop()
    {
        foreach (var i in GPSDataList) 
        {
            string path = Application.dataPath + "/Log.txt";
            // create file if it doesn't exist
            if (!File.Exists(path)) {
                File.WriteAllText(path, "Location tracker \n\n");
            }
                string content = i.Coordinates + i.id + "\n"; 
                File.AppendAllText(path, content);
        }
    }
    

    IEnumerator GPSLoc()
    {

        setLocationData(coordinates, id);

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
            }
            else 
            {
                // service is stopped
                GPSStatus.text = "Stop";
            }
        } // end of UpdateGPSData

    // create and write to text file ********
    /* void CreateText() {
        // path of the file
        string path = Application.dataPath + "/Log.txt";
        // create file if it doesn't exist
        if (!File.Exists(path)) {
            File.WriteAllText(path, "Location tracker \n\n");
        }
        // content of file
        string content = "Login date: " + System.DateTime.Now + "\n" + "Location data: " + latitudeValue.text + "," + longitudeValue.text + "\n";
        // add some text to ot
        File.AppendAllText(path, content);

    } */


    public void OnButtonClick() 
    {
        GPSLoc();
    }

    void Update() {
        UpdateGPSData();
    }

}
