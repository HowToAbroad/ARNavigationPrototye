using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLocationData : MonoBehaviour
{

    List<GPSLocationData> GPSDataList = new List<GPSLocationData>();

        public float latitudeValue{get; set;}
        public float longitudeValue{get;set;}
        int id;
        int count =0;
        public GPSLocationData(float _lat, float _long)
        {
            latitudeValue=_lat;
            longitudeValue=_long;
            id=count;
            count= count +1;

        }


}
