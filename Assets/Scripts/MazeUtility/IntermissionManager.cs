using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class IntermissionManager : MonoBehaviour {

    public static IntermissionManager instance;
    [Header("Setups")]
    [Tooltip("Set up the intermission point objects and they will be used during maze transition phase. Click to see the referenced objects")] 
    public IntermissionPoint[] Points;
    [HideInInspector] public bool Approval = false;

    MazeManager mazeManager;
    ArrowManager arrowManager;
    
	void Awake () {
        if (instance == null) instance = this;
	}
    void Start()
    {
        mazeManager = MazeManager.instance;
        arrowManager = ArrowManager.instance;

        if (Points == null)
        {
            Points = GameObject.FindObjectsOfType<IntermissionPoint>();
        }
        for(int i = 0; i < Points.Length; i++)
        {
            Points[i].gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ActivatePoints()
    {
        float maxDist = 0;
        int maxID = 0;
        for (int i = 0; i < Points.Length; i++)
        {
            var dist = Vector3.Distance(Points[i].transform.position, arrowManager.StartingPoints[mazeManager.CurrentLevel - 1].transform.position      );
            if (dist > maxDist)
            {
                maxDist = dist;
                maxID = i;
            }
        }
        Points[maxID].gameObject.SetActive(true);
        //Points[maxID].SetInstructionText();
        Approval = true;
    }
}
