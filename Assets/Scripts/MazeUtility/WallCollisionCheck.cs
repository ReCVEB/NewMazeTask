using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class WallCollisionCheck : MonoBehaviour {
	TrackPlayer logManager;
	FadeManager fadeManager;
    TargetManager targetManager;
    MazeManager mazeManager;
	[SerializeField] bool IsLogging = true;
    [SerializeField] string DetectName = "Wall";
	//sets black screen to transparent
	void Start () {
		if (logManager == null) logManager = TrackPlayer.instance;
		if (fadeManager == null) fadeManager = FadeManager.instance;
        if (targetManager == null) targetManager = TargetManager.instance;
        if (mazeManager == null) mazeManager = MazeManager.instance;
    }
    private void Update()
    {
        
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        if (mazeManager.CurrentLevel >= 3)
        {
            //TO DO: need more test
            if (other.gameObject.tag == DetectName)
            {
                fadeManager.FadeIn();

                targetManager.SetTargetStatus(false);

                if (IsLogging)
                {
                    string message = "Collision with " + other.gameObject.name + " at time: " + Time.time;
                    logManager.WriteCustomInfo(message);
                }

            }
        }
	}
	void OnTriggerStay(Collider other){
	}
	void OnTriggerExit(Collider other){
        if (mazeManager.CurrentLevel >= 3)
        {
            if (other.gameObject.tag == DetectName)
            {
                fadeManager.StopAllCoroutines();
                fadeManager.ResetFadingStatus();
                fadeManager.FadeOut();

                if (IsLogging)
                {
                    logManager.WriteCustomInfo("Collision with " + other.gameObject.name + " ends at time: " + Time.time);
                }
                targetManager.SetTargetStatus(true);
            }
        }
	}
   
}
