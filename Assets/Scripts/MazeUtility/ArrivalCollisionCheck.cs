using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class ArrivalCollisionCheck : MonoBehaviour {

	MazeManager mazeManager;
    FadeManager fadeManager;
    InteractionManager interactionManager;

    [Header("Arrival Settings")]
    [Tooltip("Setup the arrival point name. It left empty, its object name will be used.")]
    [SerializeField] string ArrivalPointName = "";
    [Tooltip("Typically, we only allow one true target, which will result in a successful maze.")]
    [SerializeField] bool TrueTarget = false;
    [Tooltip("A starting object may not be the arrival target.")] 
    [SerializeField] bool StartingObject = false;

    [Header("Debug")]
    [Tooltip("For debug purpose.")] 
    [SerializeField] bool debug = false;

    private bool triggered = false;
    private bool controllerConfirm = false;
    private bool controllerTouching = false;
    private bool pinchClicked = false;
    void Start(){
        mazeManager = MazeManager.instance;
        fadeManager = FadeManager.instance;
        interactionManager = InteractionManager.instance;
        controllerConfirm = interactionManager.ControllerConfirm;

        //check if it has starting confirmation or not
        if (GetComponent<StartingConfirmation>())
        {
            StartingObject = true;
        }

        //event subscription from interaction manager
        if (controllerConfirm) interactionManager.PinchClicked += PinchClickDetected;
    }
    private void OnDestroy()
    {
        if (controllerConfirm) interactionManager.PinchClicked -= PinchClickDetected;
    }
    void Update(){
        if (controllerConfirm != interactionManager.ControllerConfirm){
            controllerConfirm = interactionManager.ControllerConfirm;
        }
        if (!mazeManager.LevelStartConfirmed)
        {
            return;
        }
        if (controllerTouching && pinchClicked && !StartingObject){
            //level start confirmation is required to be able to trigger arrival
            if (triggered == false) {
                //fadeManager.QuickFade();
                if (ArrivalPointName == "")
                {
                    ArrivalPointName = transform.name;
                }
                Debug.Log(ArrivalPointName + " arrival triggered");
                if (TrueTarget)
                {
                    mazeManager.CompleteMaze(true, ArrivalPointName);
                } else
                {
                    mazeManager.CompleteMaze(false, ArrivalPointName);
                }
                triggered = true; //extra safe to make sure if doesn't accidentally trigger twice
            }
        }
    }
	void OnTriggerEnter(Collider collider){

        if (!controllerConfirm){
            if (TrueTarget)
            {
                if (collider.tag == "Player")
                {
                    if (triggered == false)
                    {
                        //fadeManager.QuickFade();
                        mazeManager.CompleteMaze(true, ArrivalPointName);
                        triggered = true; //extra safe to make sure if doesn't accidentally trigger twice
                    }
                }
            }
        } else {
            if (collider.tag == "GameController"){
                controllerTouching = true;
            }
        }
    }
    void OnTriggerExit(Collider collider){
        if (controllerConfirm)
        {
            if (collider.tag == "GameController"){
                controllerTouching = false;
            }
        }
    }
    void PinchClickDetected(bool state)
    {
        pinchClicked = state;
    }
}
