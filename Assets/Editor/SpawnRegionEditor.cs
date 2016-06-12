using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SpawnRegion))]
public class SpawnRegionEditor : Editor {

    private SpawnRegion region;
    private SpawnRegion.SpawnPath nextPath = null;

    private Vector3 point;
    private List<Vector3> points;

    void OnEnable()
    {
        region = this.target as SpawnRegion;
        points = new List<Vector3>();
        nextPath = null;
    }

    Vector2 WindowToScreenPoint(Vector2 position)
    {
        return new Vector2(position.x, Screen.height - position.y);
    }

    void OnSceneGUI()
    {
        if (nextPath == null)
        {
            Handles.BeginGUI();
            Rect dropArea = new Rect(Screen.width - 70, 20, 50, 50);

            GUI.Box(dropArea, "Drop List");
            EnemyDataStore data = RUI.DropArea<EnemyDataStore>(dropArea);
            if (data)
            {
                nextPath = new SpawnRegion.SpawnPath();
                nextPath.receipt = data;
                Tools.current = Tool.None;

                points.Clear();
                SceneView.currentDrawingSceneView.Repaint();
            }

            Handles.EndGUI();
        }

        if (nextPath != null)
        {
            int control = GUIUtility.GetControlID(new GUIContent("NEXTPATH"), FocusType.Passive);
            
            bool finished = false;
            Camera cam = SceneView.currentDrawingSceneView.camera;
            Ray r = cam.ScreenPointToRay(WindowToScreenPoint(Event.current.mousePosition));

            Vector3 scenePosition = r.origin - r.direction / r.direction.z * r.origin.z;

            switch (Event.current.type)
            {
                case EventType.mouseUp:
                    finished = true;
                    GUIUtility.hotControl = 0;
                    point = scenePosition;
                    Event.current.Use();
                    SceneView.currentDrawingSceneView.Repaint();
                    break;
                case EventType.mouseDown:
                    point = scenePosition;
                    GUIUtility.hotControl = control;
                    SceneView.currentDrawingSceneView.Repaint();
                    break;
                case EventType.mouseDrag:
                case EventType.mouseMove:
                    point = scenePosition;
                    Event.current.Use();
                    SceneView.currentDrawingSceneView.Repaint();
                    break;
                case EventType.Layout:
                case EventType.Repaint:
                    Handles.CircleCap(control, point, Quaternion.identity, HandleUtility.GetHandleSize(point) * 0.2f);
                    break;
            }

            if (finished)
            {
                points.Add(point);
                if (points.Count == nextPath.receipt.enemyTypes.Count)
                {
                    nextPath.points = points.ToArray();
                    region.paths.Add(nextPath);
                    nextPath = null;
                }
            }
        }

        foreach (SpawnRegion.SpawnPath path in region.paths)
        {
            Handles.Label(path.points[0], path.receipt.name);

            for (int i=0; i<path.points.Length; i++)
            {
                path.points[i] = Handles.FreeMoveHandle(path.points[i], Quaternion.identity, HandleUtility.GetHandleSize(path.points[i]) * 0.2f, Vector3.zero, Handles.CircleCap);
            }
            Handles.DrawAAPolyLine(path.points);
        }
    }

}
