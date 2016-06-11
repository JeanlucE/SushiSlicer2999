using UnityEngine;
using System.Collections;

public class SliceInfo : MonoBehaviour {

    public GameObject slicePrefab;
    public Sprite insideSprite;
    
    public void copyFrom(SliceInfo info)
    {
        this.slicePrefab = info.slicePrefab;
        this.insideSprite = info.insideSprite;
    }
}
