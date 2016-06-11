using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimationController : MonoBehaviour {

    public GameObject slicePrefab;

    public List<SliceInfo> Slice(Vector3 from, Vector3 to)
    {
        List<SliceInfo> info = SpriteSlicer.SliceAll(from, to);

        if (slicePrefab)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(slicePrefab);
            obj.transform.localPosition = from;
            obj.transform.localRotation = Quaternion.FromToRotation(Vector3.right, to - from);
        }

        return info;
    }

}
