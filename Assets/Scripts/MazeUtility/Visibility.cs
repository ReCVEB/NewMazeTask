using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class Visibility : MonoBehaviour {

    [Header("Visibility Mode")]
    private bool fogMode = false;
    private bool distanceMode = false;
    private float visibilityDistance = 1.0f;
    private float fogNearDist = 1.0f;
    private float fogFarDist = 2.0f;
    MazeManager mazeManager;
    CameraMarker cameraMarker;
    TargetManager targetManager;
    VisibilityManager visibilityManager;
	void Start () {
        if (cameraMarker == null) cameraMarker = CameraMarker.instance;
        if (visibilityManager == null) visibilityManager = VisibilityManager.instance;
        if (targetManager == null) targetManager = TargetManager.instance;
        if (mazeManager == null) mazeManager = MazeManager.instance;

        //Debug.Log(targets.Length);
        InitializeVisibilityMode(visibilityManager.FogMode, visibilityManager.DistanceMode);
    }
	// Update is called once per frame
	void Update () {
        
        if (distanceMode){
            
            if (mazeManager.CurrentLevel >= 3 && targetManager.TargetReady)
            {
                TargetDistanceChecking();
            }		
        } 
        if (fogMode) { 

            if (RenderSettings.fogStartDistance != fogNearDist){
                RenderSettings.fogStartDistance = fogNearDist;
            }
            if (RenderSettings.fogEndDistance != fogFarDist){
                RenderSettings.fogEndDistance = fogFarDist;
            }
        }
	}
    public void SwitchDistanceMode(bool state)
    {
        distanceMode = state;
        if (distanceMode == false)
        {
            targetManager.SetRenderingStatus(true);
        } 
    }
    public void SwitchFogMode(bool state)
    {
        fogMode = state;
        if (fogMode)
        {
            fogNearDist = visibilityManager.FogNearDistance;
            fogFarDist = visibilityManager.FogFarDistance;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = RenderSettings.skybox.GetColor("_Tint");
            RenderSettings.fogStartDistance = fogNearDist;
            RenderSettings.fogEndDistance = fogFarDist;
        } else
        {
            RenderSettings.fog = false;
        }
    }
    public void InitializeVisibilityMode(bool fog, bool distance)
    {
        fogMode = fog;
        distanceMode = distance;

        visibilityDistance = visibilityManager.VisibilityDistance;

        fogNearDist = visibilityManager.FogNearDistance;
        fogFarDist = visibilityManager.FogFarDistance;
        if (fogMode)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = RenderSettings.skybox.GetColor("_Tint");
            RenderSettings.fogStartDistance = fogNearDist;
            RenderSettings.fogEndDistance = fogFarDist;
        }
    }
    private void TargetDistanceChecking()
    {
        for (int i = 0; i < targetManager.Targets.Count; i++)
        {
            var dist = Vector3.Distance(cameraMarker.transform.position, targetManager.Targets[i].transform.position);
            if (dist > visibilityDistance)
            {
                targetManager.SetRenderingStatus(i, false);
            }
            else
            {
                targetManager.SetRenderingStatus(i, true);
            }
        }
    }

}
