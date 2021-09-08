using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;
    [Header("Target Setup")]
    [Tooltip("Set up the tag for target objects. Make sure the tag name here matches the tag used on Target objects. " +
        "Target Manager will find the objects with defined tags")]
    [SerializeField] string TargetTag = "Target";
    
    [Header("Place Holder Setup")]
    [Tooltip("Switch object display mode between dummy objects or regular targets")]
    public bool UseDummies = false;
    [Tooltip("Set up the placeholder object. Drag and drop prefabs from assets")]
    [SerializeField] GameObject TargetPlaceHolder;

    [HideInInspector] public List<GameObject> Targets = new List<GameObject>();
    [HideInInspector] public bool TargetReady = false;
    

    private List<GameObject> placeHolders = new List<GameObject>();
    private Renderer[] targetRenderers;
    private Renderer[] placeHolderRenderers;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
    }

    public void Reset()
    {
        Targets.Clear();
        placeHolders.Clear();
        //Debug.Log(Targets.Count + " Reset ");
        TargetReady = false;
    }
    public void Init()
    {
        TargetSearch();
    }
    public void Run()
    {
        SetTargetRendering(!UseDummies);
        SetDummyRendering(UseDummies);
        TargetReady = true;


        //to do:: need to turn off objects based on their status
    }
    public void SwitchDummyMode(bool state)
    {
        UseDummies = state;
        SetDummyRendering(state);
        SetTargetRendering(!state);
        //SetRenderingStatus(false);
        //UseDummies = state;
        //SetRenderingStatus(true);
    }

    //call by visibility
    public void SetRenderingStatus(bool state)
    {
        //if currentlevel == 2 (learning mode), it will ignore dummy target bool
        if (!UseDummies || MazeManager.instance.CurrentLevel == 2)
        {
            SetTargetRendering(state);
        } else
        {
            SetDummyRendering(state);
        }
        
    }
    //call by wallcollisioncheck script
    public void SetTargetStatus(bool state)
    {
        if (TargetReady)
        {
            for (int i = 0; i < Targets.Count; i++)
            {
                Targets[i].SetActive(state);
            }
        }
    }
    //call by visibility distance check
    public void SetRenderingStatus(int index, bool state)
    {
        //if currentlevel == 2 (learning mode), it will ignore dummy target bool
        if (!UseDummies || MazeManager.instance.CurrentLevel == 2)
        {
            if (index < targetRenderers.Length)
            {
                if (targetRenderers[index] != null)
                {
                    if (targetRenderers[index].enabled != state)
                    {
                        targetRenderers[index].enabled = state;
                    }
                }
            }
        } else
        {
            if (index < placeHolderRenderers.Length)
            {
                if (placeHolderRenderers[index] != null)
                {
                    if (placeHolderRenderers[index].enabled != state)
                    {
                        placeHolderRenderers[index].enabled = state;
                    }
                }
            }
        }
    }

    private void TargetSearch()
    {
        TargetReady = false;
        //make sure everything is cleared
        //Debug.Log(Targets.Count + " search reset ");
        Targets.Clear();
        placeHolders.Clear();

        //find targets
        FindTargetsAndRenderers();
        //generate dummy targets at where targets are placed
        InstantiateDummyTargets();
        //set dummy renderers
        FindDummyRenderers();

        //Set Rendering status, by default show normal targets first
        SetTargetRendering(true);
        SetDummyRendering(false);
    }
    private void SetTargetRendering(bool state)
    {
        if (targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                {
                    targetRenderers[i].enabled = state;
                }
            }
        }
    }
    private void SetDummyRendering(bool state)
    {
        if (placeHolderRenderers != null)
        {
            for (int i = 0; i < placeHolderRenderers.Length; i++)
            {
                if (placeHolderRenderers[i] != null)
                {
                    placeHolderRenderers[i].enabled = state;
                }
            }
        }
    }
    private void DestroyPlaceHolders()
    {
        //TO DO : make sure it is gone
        foreach(var o in placeHolders)
        {
            Destroy(o);
        }
    }
    private void FindTargetsAndRenderers()
    {
        //find Targets
        var objs = GameObject.FindGameObjectsWithTag(TargetTag);
        foreach (var o in objs)
        {
            Targets.Add(o);
        }
        //set renderers
        targetRenderers = new Renderer[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            targetRenderers[i] = Targets[i].GetComponent<Renderer>();
            //targetRenderers[i].enabled = true;
        }
    }
    private void InstantiateDummyTargets()
    {
        //create dummies
        foreach (var o in Targets)
        {
            var ph = Instantiate(TargetPlaceHolder, o.transform.position, Quaternion.identity);
            ph.transform.name = o.transform.name + "Dummy";
            //ph.transform.parent = placeHolderContainer.transform;\
            ph.transform.parent = o.transform;
            placeHolders.Add(ph);
        }
    }
    private void FindDummyRenderers()
    {
        placeHolderRenderers = new Renderer[placeHolders.Count];
        for (int i = 0; i < placeHolders.Count; i++)
        {
            placeHolderRenderers[i] = placeHolders[i].GetComponent<Renderer>();
            //placeHolderRenderers[i].enabled = false;
        }
    }

}
