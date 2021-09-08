using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class VisibilityManager : MonoBehaviour
{
    public static VisibilityManager instance;
    [Header("Distance Mode Parameters")]
    [Tooltip("Distance Mode: if ON, the target objects will display only when user is close enough within defined range")]
    public bool DistanceMode = true;
    [Tooltip("Range value in distance mode. Object will only show up when user is within the radius range around the object")]
    public float VisibilityDistance = 1.0f;
    [Header("Fog Mode Parameters")]
    [Tooltip("Fog Mode: if ON, fog will appear to prevent user from seeing the environment beyond certain distance")] 
    public bool FogMode = true;
    [Tooltip("Anything within Fog Near Distance will be 100% clear.")]
    public float FogNearDistance = 1.0f;
    [Tooltip("Anything beyond Fog Far Distance will be impossible to see. Anything in between Near and Far will linear fade out")]
    public float FogFarDistance = 2.5f;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Update()
    {
        //make sure that far distance is always larger than near distance
        if (FogFarDistance < FogNearDistance)
        {
            FogFarDistance = FogNearDistance + 0.1f;
        }
    }
}
