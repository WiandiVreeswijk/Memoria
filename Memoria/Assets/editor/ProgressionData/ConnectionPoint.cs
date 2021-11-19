using System;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint {
    public static int POINTHEIGHT = 20;
    public static int POINTWIDTH = 10;
    public static int LABELWIDTH = 100;
    public static int OUTPADDING = 5;

    public Rect rect;
    public Rect labelRect;

    public string name = "";

    public ConnectionPointType type;

    public EditorNode node;

    public Action<ConnectionPoint> OnClickConnectionPoint;
    public Action<ConnectionPoint> DeleteOutNode;
    public Action<ConnectionPoint> OnClickRemoveConnection;

    public ConnectionPoint(EditorNode node, string name, ConnectionPointType type,
        Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionPoint> DeleteOutNode) {
        this.node = node;
        this.name = name;
        this.type = type;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.DeleteOutNode = DeleteOutNode;
        rect = new Rect(0, 0, POINTWIDTH, POINTHEIGHT);
        labelRect = new Rect(0, 0, LABELWIDTH, 20f);
    }

    public void Draw(int index) {
        //rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
        rect.y = node.rect.y + ProgressionDataEditorStyles.TOOLBARHEIGHT + rect.height * index + OUTPADDING * index + 6;

        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width;
                break;
        }

        if (GUI.Button(rect, "", type == ConnectionPointType.In ? ProgressionDataEditorStyles.INPOINT : ProgressionDataEditorStyles.OUTPOINT)) {
            if (OnClickConnectionPoint != null) {
                OnClickConnectionPoint(this);
            }
        }

        if (type != ConnectionPointType.Out) return;
        labelRect.y = node.rect.y + ProgressionDataEditorStyles.TOOLBARHEIGHT + labelRect.height * index + OUTPADDING * index - 2 + 6;
        //labelRect.x = rect.x - LABELWIDTH - 5;
        labelRect.x = rect.x - node.rect.width / 2f + 5f;
        labelRect.width = node.rect.width / 2f - 10f;
        GUILayout.BeginArea(labelRect);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(ProgressionDataEditorStyles.DELETEOUTPUTNODE, ProgressionDataEditorStyles.FOOTERBUTTON)) {
            DeleteOutNode(this);
        }
        if (name.Length == 0) GUI.backgroundColor = ProgressionDataEditorStyles.ERROR;
        name = EditorGUILayout.TextField(name);
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}