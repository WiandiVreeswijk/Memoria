using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProgressionDataEditorStyles : MonoBehaviour {
    public static GUIStyle FOOTERBUTTON;

    public static GUIStyle LINE;

    public static GUIStyle NODE;
    public static GUIStyle SELECTEDNODE;
    public static GUIStyle INPOINT;
    public static GUIStyle OUTPOINT;

    public static GUIStyle FOLDOUT;

    public static GUIContent iconToolbarPlus;
    public static GUIContent ADDCOMPONENT;
    public static GUIContent ADDOUTPUTNODE;
    public static GUIContent DELETEOUTPUTNODE;

    public static Color ERROR = new Color(1.5f, 0f, 0f);
    public static Color ACTIVENODE = new Color(0f, 2f, 0f);
    public static Color BASENODE = new Color(0f, 1f, 0f);
    public static Color GREY = new Color(0.11f, 0.11f, 0.11f);

    public static int TOOLBARHEIGHT = 21;

    private static bool initialized = false;
    public static Color DARKGREY = new Color(0.7f, 0.7f, 0.7f);

    public static void InitStyles() {
        if (initialized) return;
        FOOTERBUTTON = "RL FooterButton";

        LINE = new GUIStyle();
        LINE.normal.background = MakeTex(1, 1, Color.white);

        NODE = new GUIStyle();
        NODE.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        NODE.border = new RectOffset(12, 12, 12, 12);

        SELECTEDNODE = new GUIStyle();
        SELECTEDNODE.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        SELECTEDNODE.border = new RectOffset(12, 12, 12, 12);

        INPOINT = new GUIStyle();
        INPOINT.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        INPOINT.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        INPOINT.border = new RectOffset(4, 4, 12, 12);

        OUTPOINT = new GUIStyle();
        OUTPOINT.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        OUTPOINT.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        OUTPOINT.border = new RectOffset(4, 4, 12, 12);

        FOLDOUT = EditorStyles.foldoutHeader;
        FOLDOUT.alignment = TextAnchor.MiddleCenter;
        FOLDOUT.focused.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.normal.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.hover.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.onNormal.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.onHover.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.onActive.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.focused.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.onFocused.textColor = new Color(0.768f, 0.768f, 0.768f);
        FOLDOUT.margin = new RectOffset(0, 0, 0, 0);

        FOOTERBUTTON.fixedHeight = TOOLBARHEIGHT - 1;
        ADDCOMPONENT = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add Component");
        ADDOUTPUTNODE = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add Output Node");
        DELETEOUTPUTNODE = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove Output Node");

        initialized = true;
    }

    private static Texture2D MakeTex(int width, int height, Color col) {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

}
