using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public enum MazeMode : int{tutorial, learning, testing, full};
public class MazeManager : MonoBehaviour
{
    [Header("Managers")]
    static public MazeManager instance;
    [SerializeField] LevelLauncher levelManager;
    [SerializeField] ArrowManager arrowManager;
    [SerializeField] TimeManager timeManager;
    [SerializeField] TrackPlayer logManager;
    [SerializeField] FadeManager fadeManager;
    [SerializeField] TargetManager targetManager;
    [SerializeField] SpaceManager spaceManager;

    [Header("Level Params")]
    [HideInInspector]public MazeMode CurrentMode;
    [HideInInspector]public int CurrentLevel;
    [HideInInspector] public bool LevelStartConfirmed = false;

    [Header("Random Level Loader")]
    private int levelcount = 22;
    private int testPhaseStartingIndex = 3;
    private int nextLevel;
    private bool missionComplete = false;
    // private int maxCount = 5;
    private HashSet<int> candidates = new HashSet<int>();
    System.Random random = new System.Random();

    // [Header("Space Visibility")]
    // public bool SpatialObjectOff = true;
    void Awake()
    {
        if (instance == null){
            instance = this;
        } else if (instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        //find all other manager instances
        if (fadeManager == null) fadeManager = FadeManager.instance;
        if (levelManager == null) levelManager = LevelLauncher.instance;
        if (arrowManager == null) arrowManager = ArrowManager.instance;
        if (timeManager == null) timeManager = TimeManager.instance;
        if (logManager == null) logManager = TrackPlayer.instance;
        if (targetManager == null) targetManager = TargetManager.instance;
        if (spaceManager == null) spaceManager = SpaceManager.instance;

        //the total level count depends on how many starting points
        levelcount = arrowManager.StartingPoints.Length + 1;
    }

    // Update is called once per frame
    void Update()
    {       

    }
    public void ResetManagers(){
        levelManager.LevelCheck();
        timeManager.Reset();
        logManager.Reset();
        arrowManager.Reset();
    }
    //steps of proceeding to next level: choose level -> prepare level -> loadlevel
    public void ChooseLevel(){
        GetRandomLevel(testPhaseStartingIndex);
        logManager.Reset();
    }

    public void PrepareLevel(){
        levelManager.LevelCheck(); //check level and unload existing level
        CurrentLevel = nextLevel; // 1 -> training scene
        LevelStartConfirmed = false; //prepared but not confirmed, need user active confirmation later
        arrowManager.Reset();
        targetManager.Reset();
        fadeManager.ResetFadeOut();
        if (missionComplete == false){
            if (CurrentLevel == 1){
                Debug.Log("Preparing tutorial level");
            } else if (CurrentLevel == 2){
                Debug.Log("Preparing learning level");
            } else {
                Debug.Log("Preparing testing level:" + (CurrentLevel - 2));
            }
            arrowManager.Activate(CurrentLevel - 1);
        } else {
            timeManager.WriteTimeDisplay("Mission Complete");
            arrowManager.SetTextContent("Mission Complete. Thank you!");
            arrowManager.InstructionText.transform.position = new Vector3(0,0.5f,0);
            arrowManager.InstructionText.SetActive(true);
        }
    }
    public void LoadLevel(){
        levelManager.SelectLevel(CurrentLevel);
        StartCoroutine(WaitInit());
    }
    //separate call to unload level, instead of PrepareLevel()
    public void UnloadLevel()
    {
        levelManager.LevelCheck();
    }
    public void ControlledInit()
    {
        //start confirmation check
        logManager.Run();
        timeManager.Run();
        spaceManager.Run();
        targetManager.Run();
        LevelStartConfirmed = true; // level official start upon user confirmation
    }
    private IEnumerator WaitInit()
    {
        while (levelManager.loading)
        {
            yield return null;
        }
        logManager.WriteLevelInfo();
        
        targetManager.Init();
        spaceManager.Init();

        if (CurrentLevel <= 2)
        {
            //if in tutorial and learning phase, no need for starting confirmation
            ControlledInit();
        }
    }

    //call made by arrival collision check object, once target is reached, CompleteMaze() is called
    public void CompleteMaze(bool success, string name){
        timeManager.Complete(success, name);
    }
    //called in main UI, which will change the parameters of maze mode
    public void MazeModeSelect(int mode){
        CurrentMode = (MazeMode)mode;
        Debug.Log("Selected Current Mode = " + CurrentMode);

        //if tutorial
        if (CurrentMode == MazeMode.tutorial || CurrentMode == MazeMode.full){
            nextLevel = 1; // be default tutorial level = 1
            timeManager.SetTimeLimit(int.MaxValue); // for learning purpose, timeCount goes infinite.
            PrepareLevel();
        }
        //if learning
        if (CurrentMode == MazeMode.learning){
            nextLevel = 2;
            timeManager.SetTimeLimit(int.MaxValue); // for learning purpose, timeCount goes infinite.
            PrepareLevel();
        }
        //if training
        if (CurrentMode == MazeMode.testing){
            ChooseLevel();
            PrepareLevel(); //to do: randomize prepare level
        }
    }
    //a string can be provided to skip certain levels, use comma to separate
    public void SkipLevels(string levels)
    {
        int[] skipLevels = Array.ConvertAll(levels.Split(','), int.Parse);
        for (int i = 0; i < skipLevels.Length; i++)
        {
            candidates.Add(skipLevels[i] + 1);
            Debug.Log("Level Skipped " + (skipLevels[i] + 1));
        }
        Debug.Log("Remaining " + (levelcount - testPhaseStartingIndex - candidates.Count));
    }
    private void GetRandomLevel(int min){
        //check if it's done
        if (candidates.Count == levelcount - min || CurrentMode == MazeMode.learning || CurrentMode == MazeMode.tutorial){
            missionComplete = true;
            ResetManagers(); // finishes
            Debug.Log("Mission Complete");
            return;
        }
        //check if its moving to learning phase
        if (CurrentMode == MazeMode.full && CurrentLevel == 1){
            nextLevel = 2;
            timeManager.SetTimeLimit(int.MaxValue);
            Debug.Log("Tutorial done. Move to next Learning phase");
            return;
        }
        while (candidates.Count != levelcount - min){
            int randNum = UnityEngine.Random.Range(min, levelcount-1);
            //Debug.Log(randNum + " = generated");
            if (candidates.Add(randNum))
            {
                nextLevel = randNum;
                Debug.Log("Level " + (nextLevel - 2) + " selected.");
                break;
            }
        }
    }

}
