using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class StartingConfirmation : MonoBehaviour
{
    MazeManager mazeManager;
    FadeManager fadeManager;
    TrackPlayer logManager;
    InteractionManager interactionManager;
    [Header("Setup")]
    [Tooltip("Setup starting point name. It will appear on txt file recording.")]
    [SerializeField] string StartingPointName = "";

    private bool triggered = false;
    private bool controllerTouching = false;
    private bool pinchClicked = false;
    void Start()
    {
        mazeManager = MazeManager.instance;
        fadeManager = FadeManager.instance;
        logManager = TrackPlayer.instance;
        interactionManager = InteractionManager.instance; 

        interactionManager.PinchClicked += PinchClickDetected;
    }
    private void OnDestroy()
    {
        interactionManager.PinchClicked -= PinchClickDetected;
    }
    private void Update()
    {
        if (controllerTouching && pinchClicked)
        {
            //level start confirmation is required to be able to trigger arrival
            if (triggered == false)
            {
                fadeManager.QuickFade();

                mazeManager.ControlledInit();
                if (StartingPointName == "")
                {
                    StartingPointName = transform.name;
                }
                logManager.WriteLevelStartingInfo(StartingPointName);

                triggered = true; //extra safe to make sure if doesn't accidentally trigger twice
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "GameController")
        {
            controllerTouching = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "GameController")
        {
            controllerTouching = false;
        }
    }
    void PinchClickDetected(bool state)
    {
        pinchClicked = state;
    }
}
