using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliceInfo : MonoBehaviour {

    public GameObject slicePrefab;
    public Sprite insideSprite;
    public MeshRenderer renderObject;

    public List<MonoBehaviour> actions = new List<MonoBehaviour>();

    void Start()
    {
        if (!renderObject)
        {
            renderObject = GetComponent<MeshRenderer>();
        }
        if (!renderObject)
        {
            Destroy(this);
        }
    }
    
    public void copyFrom(SliceInfo info)
    {
        this.slicePrefab = info.slicePrefab;
        this.insideSprite = info.insideSprite;
    }

    public void DoSlice()
    {
        foreach (MonoBehaviour action in this.actions)
        {
            if (action)
            {
                action.SendMessage("Start");
            }
        }
    }

}
