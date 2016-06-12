using UnityEngine;
using System.Collections;

public class MouthController : MonoBehaviour {

    public bool open = false;
    public Texture closed, opened;

    private Material material;

    public float openZoom = 3.0f;
    private float zoom;

    void Start()
    {
        //GetComponent<MeshRenderer>().sortingOrder = 1;
        transform.position += Vector3.back * 0.1f;
        material = GetComponent<MeshRenderer>().material;

        zoom = transform.localScale.x;

        SetState(open);
    }

    public void SetState(bool open)
    {
        this.open = open;

        material.mainTexture = open ? opened : closed;
        transform.localScale = Vector3.one * (open ? openZoom * zoom : zoom);
    }

}
