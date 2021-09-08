using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class IntermissionPoint : MonoBehaviour {
    MazeManager mazeManager;
    IntermissionManager intermissionPointManager;
    private TextMeshPro instructionText;
	// Use this for initialization
	void Start () {
        mazeManager = MazeManager.instance;
        intermissionPointManager = IntermissionManager.instance;
        instructionText = GetComponentInChildren<TextMeshPro>();
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (intermissionPointManager.Approval)
            {
                mazeManager.PrepareLevel();
                intermissionPointManager.Approval = false;
                this.gameObject.SetActive(false);
            }
        }
    }

}
