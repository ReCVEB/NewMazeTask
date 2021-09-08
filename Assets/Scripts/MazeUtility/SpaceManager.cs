using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Mengyu Chen, 2019
//For questions: mengyuchenmat@gmail.com
public class SpaceManager : MonoBehaviour
{
    public static SpaceManager instance;
    [Header("Mode Switch")]
    [Tooltip("Switch between different spatial object environments")] public bool SpatialObjectOff = false;
    [Header("Space Object Setup")]
    [Tooltip("Set up group A object tags. Space manager will find objects of these tags and be able to control their renderers")] 
    [SerializeField] string[] GroupAObjectTags = new string[3] { "Wall", "Floor", "Boundary" };
    [Tooltip("Set up group B object tags. Space manager will find objects of these tags and be able to control their renderers")] 
    [SerializeField] string[] GroupBObjectTags = new string[1] { "GroundPlane" };
    [Header("Special Feature")]
    public bool WallColliderOff = true;
    [HideInInspector] public GameObject[][] GroupAObjects;
    [HideInInspector] public GameObject[][] GroupBObjects;
    [HideInInspector] public bool GroupAObjectReady = false;
    [HideInInspector] public bool GroupBObjectReady = false;

    FadeManager fadeManager;
    
    private Renderer[][] GroupARenderers;
    private Renderer[][] GroupBRenderers;
    private Collider[] colliders; //temporary containers
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        if (fadeManager == null) fadeManager = FadeManager.instance;
    }
    public void SwitchDisplayMode(bool state)
    {
        SpatialObjectOff = !state;
        SetRendererStatus(GroupARenderers, state);
        SetRendererStatus(GroupBRenderers, !state);
        if (WallColliderOff)
        {
            SetColliderStatus(GroupAObjects, 0, state);
        }
        fadeManager.ResetFadeOut();
    }
    public void Init()
    {
        //find and init gourp A objs and renderers
        GroupAObjects = FindObjectsWithTags(GroupAObjectTags);
        GroupARenderers = InitRenderers(GroupAObjects);
        GroupAObjectReady = true;

        //do same with group B
        GroupBObjects = FindObjectsWithTags(GroupBObjectTags);
        GroupBRenderers = InitRenderers(GroupBObjects);
        GroupBObjectReady = true;
        //turn off group b renderers
        SetRendererStatus(GroupBRenderers, false);
    }
    public void Run()
    {
        if (SpatialObjectOff && MazeManager.instance.CurrentLevel > 2)
        {
            SwitchDisplayMode(false);
        }
    }
    #region private methods
    private void SetRendererStatus(Renderer[][] renderers, bool state)
    {
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                for (int j = 0; j < renderers[i].Length; j++)
                {
                    if (renderers[i][j] != null)
                    {
                        renderers[i][j].enabled = state;
                    }
                }
            }
        }
    }
    private GameObject[][] FindObjectsWithTags(string[] tags)
    {
        //find targets
        GameObject[][] objects = new GameObject[tags.Length][];
        for (int i = 0; i < tags.Length; i++)
        {
            var objs = GameObject.FindGameObjectsWithTag(tags[i]);
            objects[i] = new GameObject[objs.Length];
            for (int j = 0; j < objs.Length; j++)
            {
                objects[i][j] = objs[j];
            }
        }
        return objects;
    }

    private void SetColliderStatus(GameObject[][] objects, int index, bool state)
    {
        if (objects != null)
        {
            colliders = new Collider[objects[index].Length];
            for (int i = 0; i < objects[index].Length; i++)
            {
                colliders[i] = objects[index][i].GetComponent<Collider>();
                colliders[i].enabled = state;
            }
        }
    }

    private Renderer[][] InitRenderers(GameObject[][] objects)
    {
        Renderer[][] renderers = new Renderer[objects.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i] = new Renderer[objects[i].Length];
            for (int j = 0; j < renderers[i].Length; j++)
            {
                renderers[i][j] = objects[i][j].GetComponent<Renderer>();
            }
        }
        return renderers;
    }
    #endregion private methods
}