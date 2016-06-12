using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RUI
{
    public static Color acceptGreen = new Color(0, 1, 0, 0.3f);

    public static void DrawArrow(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        Vector3 perp = Vector3.Cross(dir, Vector3.forward);

        Handles.DrawAAPolyLine(from, to);
        Handles.DrawAAPolyLine(to, to - dir * 0.1f - perp * 0.05f);
        Handles.DrawAAPolyLine(to, to - dir * 0.1f + perp * 0.05f);
    }

    public static void DrawOutlineRectXY(Vector3 a, Vector3 b, Color faceColor, Color outlineColor)
    {
        Vector3 c = new Vector3(a.x, b.y, 0);
        Vector3 d = new Vector3(b.x, a.y, 0);
        Handles.DrawSolidRectangleWithOutline(new Vector3[]{a, c, b, d}, faceColor, outlineColor);
    }

    /**
     * waits for Objects of type T being dropped into the specified area
     */
    public static T DropArea<T>(Rect dropArea) where T : UnityEngine.Object
    {
        // Cache References:
        Event currentEvent = Event.current;
        EventType eventType = currentEvent.type;

        // The DragExited event does not have the same mouse position data as the other events,
        // so it must be checked now:
        if (eventType == EventType.DragExited) DragAndDrop.PrepareStartDrag();// Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)

        if (dropArea.Contains(currentEvent.mousePosition))
        {
            object dropData = null;
            if (DragAndDrop.objectReferences.Length > 0)
            {
                dropData = DragAndDrop.objectReferences[0];
            }

            switch (eventType)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = (dropData is T) ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                    currentEvent.Use();
                    break;
                case EventType.Repaint:
                    if (
                    DragAndDrop.visualMode == DragAndDropVisualMode.None ||
                    DragAndDrop.visualMode == DragAndDropVisualMode.Rejected) break;

                    EditorGUI.DrawRect(dropArea, acceptGreen);
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();

                    currentEvent.Use();
                    return (T)dropData;
                case EventType.MouseUp:
                    // Clean up, in case MouseDrag never occurred:
                    DragAndDrop.PrepareStartDrag();
                    break;
            }
        }

        //return default(T);
        return null;
    }

    /**
     * returns true while the dragData Object is being dragged
     */
    public static bool DragArea(Rect dragArea, Object dragData)
    {
        // Cache References:
        Event currentEvent = Event.current;
        EventType eventType = currentEvent.type;

        // The DragExited event does not have the same mouse position data as the other events,
        // so it must be checked now:
        if (eventType == EventType.DragExited) DragAndDrop.PrepareStartDrag();// Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)

        switch (eventType)
        {
            case EventType.MouseDown:
                if (dragArea.Contains(currentEvent.mousePosition))
                {
                    if (dragData != null)
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.objectReferences = new Object[] {
                            dragData
                        };

                        currentEvent.Use();
                    }
                }

                return true;
        }

        return DragAndDrop.objectReferences.Length > 0 && DragAndDrop.objectReferences[0] == dragData;
    }

    /**
     * waits for Objects of type T being dropped into the specified area
     */
    public static T InternalDropArea<T>(Rect dropArea, ref object dragObj)
    {
        // Cache References:
        Event currentEvent = Event.current;
        EventType eventType = currentEvent.type;

        if (dropArea.Contains(currentEvent.mousePosition))
        {
            object dropData = dragObj;

            switch (eventType)
            {
                case EventType.Repaint:
                    if (!(dropData is T)) break;

                    EditorGUI.DrawRect(dropArea, acceptGreen);
                    break;
                case EventType.MouseUp:
                    dragObj = null;
                    currentEvent.Use();

                    if (dropData is T)
                    {
                        return (T)dropData;
                    }

                    break;
            }
        }

        return default(T);
    }

    /**
     * returns true while the dragData Object is being dragged
     */
    public static bool InternalDragArea(Rect dragArea, object dragData, ref object dragObj)
    {
        Event currentEvent = Event.current;
        EventType eventType = currentEvent.type;

        if (eventType == EventType.MouseDown)
        {
            if (dragArea.Contains(currentEvent.mousePosition))
            {
                if (dragData != null)
                {
                    dragObj = dragData;
                    currentEvent.Use();
                }
            }
        }

        return dragObj == dragData;
    }

}
