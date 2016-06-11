using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimationController : MonoBehaviour {

    public GameObject slicePrefab;
    public Animator animator;
    public TrailRenderer swordTrail;

    public List<SliceInfo> Slice(Vector3 from, Vector3 to)
    {
        List<SliceInfo> info = SpriteSlicer.SliceAll(from, to);

        if (slicePrefab)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(slicePrefab);
            obj.transform.localPosition = from;
            obj.transform.localRotation = Quaternion.FromToRotation(Vector3.right, to - from);
        }

        SetSlice(true);

        return info;
    }

    public void SetRunning(bool running)
    {
        animator.SetBool("running", running);
    }

    public void SetSlice(bool slice)
    {
        animator.SetBool("slice", slice);

        //enable/disable sword trail
        if (slice) { swordTrail.enabled = true; }
        else { StartCoroutine(disableSwordTrail(0.1f)); }
    }

    private IEnumerator disableSwordTrail(float delay)
    {
        yield return new WaitForSeconds(delay);
        swordTrail.enabled = false;
    }
}
