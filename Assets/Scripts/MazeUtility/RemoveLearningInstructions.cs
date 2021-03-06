using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RemoveLearningInstructions : MonoBehaviour
//Author: Carol He, 2021
//For questions: carol.hcxy@gmail.com
{
    MazeManager mazeManager;
    FadeManager fadeManager;
    TrackPlayer logManager;
    InteractionManager interactionManager;
    [Header("Setup")]
    [SerializeField] TextMeshPro InstructionText;
//    [Tooltip("Setup starting point name. It will appear on txt file recording.")]
//    [SerializeField] string StartingPointName = "";
//    [SerializeField] string TargetObjectName = "";
    


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
                InstructionText.text = ""; 
                triggered = true; //extra safe to make sure if doesn't accidentally trigger twice
            }
            // if (triggered == true)
            // {
            //     fadeManager.QuickFade();
            //     InstructionText.text = "Please explore the space and remember where all the objects are located. You have 3 mins to explore."; 
            //     triggered = false; //extra safe to make sure if doesn't accidentally trigger twice
            // }
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