using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class LearningPoint : MonoBehaviour {
	TrackPlayer logManager;
	FadeManager fadeManager;
	MazeManager mazeManager;
    [Header("Arrow Setup")]
    [Tooltip("The arrow object that user will see")] 
    [SerializeField] GameObject ArrowAvatar;
    [Tooltip("The height of object")] [SerializeField] float ArrowHeight = 1.5f;
    [Header("Learning Points Setup")]
    [Tooltip("The waypoints of the learning route in consecutive order")] 
    [SerializeField] GameObject[] presetPoints;
    [Tooltip("How many times the user will need to complete the route")]
    [SerializeField] int LoopTimes = 2;
    [Header("Debug")]
    [Tooltip("For debug purpose, if true, a message will be recorded in txt file upon user arrival at arrow")]
    [SerializeField] bool isLogging = false;

    private int pointIndex = 0;
	private int loop = 0;
	private bool learningComplete = false;
	//sets black screen to transparent
	void Start () {
		logManager = TrackPlayer.instance;
		fadeManager = FadeManager.instance;
		mazeManager = MazeManager.instance;
		transform.position = new Vector3(presetPoints[pointIndex].transform.position.x, ArrowHeight, presetPoints[pointIndex].transform.position.z);
    }

	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		
		if (other.tag == "Player"){
			if (isLogging){
				string message = "learning point at idx = " + pointIndex + " loop = " + loop;
				logManager.WriteCustomInfo(message);
			}
            fadeManager.QuickFade();
			pointIndex ++;
			if (pointIndex < presetPoints.Length){
				NextPoint();
			} else {
				loop ++;
				if (loop < LoopTimes){
					pointIndex = 0;
					NextPoint();
				} else {
					if (!learningComplete){
						mazeManager.CompleteMaze(true, "Last Learning Point");
						learningComplete = true; //make sure it doesn't trigger twice;
					}
				}
			}
		}
	}
	void NextPoint(){
		transform.position = new Vector3(presetPoints[pointIndex].transform.position.x, ArrowHeight, presetPoints[pointIndex].transform.position.z);
		Debug.Log("Point Index = " + pointIndex + " loop No. " + (loop + 1));
	}


}

