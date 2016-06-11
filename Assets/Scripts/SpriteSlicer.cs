using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteSlicer : MonoBehaviour {

    public SliceInfo sliceInfo;

    public Vector3 direction;
    public Vector3 start;
    public float punch;

    private Vector3 normal;

    private ClipperLib.ClipType clipType;
    private ClipperLib.Clipper clip = new ClipperLib.Clipper();

    public static List<SliceInfo> SliceAll(Vector3 from, Vector3 to, float punch = 1.0f)
    {
        List<SliceInfo> hits = new List<SliceInfo>();

        Vector3 direction = to - from;
        foreach (RaycastHit2D hit in Physics2D.LinecastAll(from, to, ~0))
        {
            SliceInfo info = hit.collider.GetComponent<SliceInfo>();

            if (info)
            {
                hits.Add(info);
                Slice(info, from, direction, punch);
            }
        }

        return hits;
    }

    public static void Slice(SliceInfo sliceInfo, Vector3 start, Vector3 direction, float punch = 1.0f)
    {
        SpriteSlicer slicer = sliceInfo.gameObject.AddComponent<SpriteSlicer>();
        slicer.sliceInfo = sliceInfo;

        Transform t = sliceInfo.transform;
        slicer.start = t.InverseTransformPoint(start);
        slicer.direction = t.InverseTransformDirection(direction);
        slicer.punch = punch;
    }

    void Start()
    {
        direction = direction.normalized;
        normal = Vector3.Cross(direction, new Vector3(0, 0, 1));

        GameObject obj;
        Rigidbody2D rb;

        Rigidbody2D rbbase = GetComponent<Rigidbody2D>();

        CreateBaseMesh();
        DoSlice();
        obj = SpawnSegment(false);
        if (obj)
        {
            obj.AddComponent<SliceInfo>().copyFrom(this.sliceInfo);
            rb = obj.GetComponent<Rigidbody2D>();
            rb.velocity = rbbase.velocity + (Vector2) (direction - 0.5f * normal) * punch;
            rb.angularVelocity = rbbase.angularVelocity + 40.0f * punch;
            rb.gravityScale = 0;
        }

        DoSlice();
        obj = SpawnSegment(true);
        if (obj)
        {
            obj.AddComponent<SliceInfo>().copyFrom(this.sliceInfo);
            rb = obj.GetComponent<Rigidbody2D>();
            rb.velocity = rbbase.velocity + (Vector2)(direction + 0.5f * normal) * punch;
            rb.angularVelocity = rbbase.angularVelocity - 40.0f * punch;
            rb.gravityScale = 0;
        }

        Destroy(gameObject);
    }

    void CreateBaseMesh()
    {
        clip.Clear();

        /*quad.Add(new Vector2(-0.5f, -0.5f));
        quad.Add(new Vector2(-0.5f, 0.5f));
        quad.Add(new Vector2( 0.5f,  0.5f));
        quad.Add(new Vector2(0.5f, -0.5f));*/


        Mesh mesh = GetComponent<MeshFilter>().mesh;

        int n = 0;
        List<Vector2> quad = new List<Vector2>();
        foreach (int i in mesh.triangles)
        {
            quad.Add(mesh.vertices[i]);
            n++;
            if ((n%3) == 0)
            {
                clip.AddPath(quad.ConvertAll<ClipperLib.IntPoint>(CP), ClipperLib.PolyType.ptSubject, true);
                quad.Clear();
            }
        }
    }

    void DoSlice()
    {
        List<Vector2> quad = new List<Vector2>();

        Vector2 center = (Vector2)start;
        Vector2 dir = (Vector2)direction;
        Vector2 norm = (Vector2)normal;
        quad.Add(center - 10 * dir + 10 * norm);
        quad.Add(center - 5*dir);
        // here is the cut
        quad.Add(center + 5*dir);
        quad.Add(center + 10 * dir + 10 * norm);

        clip.AddPath(quad.ConvertAll<ClipperLib.IntPoint>(CP), ClipperLib.PolyType.ptClip, true);
    }

    GameObject SpawnSegment(bool leftSide)
    {
        clipType = leftSide ? ClipperLib.ClipType.ctIntersection : ClipperLib.ClipType.ctDifference;
        ClipperLib.PolyTree tree = new ClipperLib.PolyTree();
        clip.Execute(clipType, tree, ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> norms = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> faces = new List<int>();
        int idx = 0;
        foreach (ClipperLib.PolyNode child in tree.Childs)
        {
            if (!child.IsHole)
            {
                renderTree(child, verts, norms, uvs, faces, ref idx);
            }
        }

        if (verts.Count == 0)
        {
            return null;
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = norms.ToArray();
        mesh.triangles = faces.ToArray();

        GameObject obj = GameObject.Instantiate<GameObject>(sliceInfo.slicePrefab);
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;

        Transform t = obj.transform;
        t.localPosition = transform.localPosition;
        t.localRotation = transform.localRotation;
        t.localScale = transform.localScale;

        Vector3 min = new Vector3(1000, 1000, 0);
        Vector3 max = new Vector3(-1000, -1000, 0);
        foreach (Vector3 v in verts)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        BoxCollider2D c = obj.GetComponent<BoxCollider2D>();
        c.offset = (max + min) / 2;
        c.size = max - min;

        return obj;
    }

    public static void renderTree(ClipperLib.PolyNode node, List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<int> faces, ref int idx)
    {
        Poly2Tri.Polygon p = new Poly2Tri.Polygon(node.Contour.ConvertAll<Poly2Tri.PolygonPoint>(PP));
        //p.RemoveDuplicateNeighborPoints();
        //p.MergeParallelEdges(1e-3);

        foreach (ClipperLib.PolyNode child in node.Childs)
        {
            if (child.IsHole)
            {
                Poly2Tri.Polygon h = new Poly2Tri.Polygon(child.Contour.ConvertAll<Poly2Tri.PolygonPoint>(PP));
                h.RemoveDuplicateNeighborPoints();
                h.MergeParallelEdges(1e-3);

                p.AddHole(h);
            }
            else
            {
                renderTree(child, verts, norms, uvs, faces, ref idx);
            }
        }

        Poly2Tri.DTSweepContext ctx = new Poly2Tri.DTSweepContext();
        ctx.PrepareTriangulation(p);
        Poly2Tri.DTSweep.Triangulate(ctx);

        foreach (Poly2Tri.DelaunayTriangle dt in p.Triangles)
        {
            faces.Add(idx); uvs.Add(UV(dt.Points[2])); norms.Add(Vector3.back); verts.Add(V3(dt.Points[2])); idx++;
            faces.Add(idx); uvs.Add(UV(dt.Points[1])); norms.Add(Vector3.back); verts.Add(V3(dt.Points[1])); idx++;
            faces.Add(idx); uvs.Add(UV(dt.Points[0])); norms.Add(Vector3.back); verts.Add(V3(dt.Points[0])); idx++;
        }
    }

    public static Poly2Tri.PolygonPoint PP(ClipperLib.IntPoint v)
    {
        return new Poly2Tri.PolygonPoint(v.X / 16384.0, v.Y / 16384.0);
    }

    public static ClipperLib.IntPoint CP(Vector2 v)
    {
        return new ClipperLib.IntPoint((double)v.x * 16384.0, (double)v.y * 16384.0);
    }
    public static Vector2 V2(ClipperLib.IntPoint v)
    {
        return new Vector2((float)(v.X / 16384.0), (float)(v.Y / 16384.0));
    }

    public static Vector3 V3(Poly2Tri.TriangulationPoint tp)
    {
        return new Vector3((float)tp.X, (float)tp.Y, 0f);
    }

    public static Vector2 UV(Poly2Tri.TriangulationPoint tp, float scale = 1.0f, float offset = 0.5f)
    {
        return new Vector2((float)(tp.X+offset) * scale, (float)(tp.Y+offset) * scale);
    }

    void OnDrawGizmos()
    {
        if (transform)
        {
            direction = direction.normalized;

            Matrix4x4 matrix = Gizmos.matrix;
            Gizmos.matrix *= transform.localToWorldMatrix;

            Gizmos.DrawLine(start, start + 5f * direction);

            Gizmos.matrix = matrix;
        }
    }

}
