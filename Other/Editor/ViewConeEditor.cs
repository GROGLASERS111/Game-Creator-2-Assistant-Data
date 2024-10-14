using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ETK;

[CustomEditor(typeof(ViewCone))]
public class MyScriptEditorView : Editor
{
    Texture2D headerImage;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        var header = new Image() { image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RVRGaming/ETK2/RPG/Scripts/Other/Editor/HeaderView.png") };
        header.style.maxHeight = 34;
        header.style.maxWidth = 200;
        header.style.marginLeft = 0;
        root.Add(header);

        root.Add(new SpaceSmall());

        root.Add(new PropertyField(serializedObject.FindProperty("viewConeHeight")));
        root.Add(new PropertyField(serializedObject.FindProperty("viewConeWidth")));
        root.Add(new PropertyField(serializedObject.FindProperty("viewConeLength")));
        root.Add(new PropertyField(serializedObject.FindProperty("detectionUI")));
        root.Add(new PropertyField(serializedObject.FindProperty("detectionProgressImage")));
        root.Add(new PropertyField(serializedObject.FindProperty("spottedUIGroup")));
        root.Add(new SpaceSmall());

        var playerInteractLabel = new Label("On Detect");
        root.Add(playerInteractLabel);
        root.Add(new SpaceSmall());
        root.Add(new PropertyField(serializedObject.FindProperty("buildUpTimeToDetect")));
        root.Add(new SpaceSmall());
        root.Add(new PropertyField(serializedObject.FindProperty("onView")));
        root.Add(new SpaceSmall());

        var playerStopViewLabel = new Label("Stop Detect");
        root.Add(playerStopViewLabel);
        root.Add(new SpaceSmall());
        root.Add(new PropertyField(serializedObject.FindProperty("buildUpTimeToLose")));
        root.Add(new SpaceSmall());
        root.Add(new PropertyField(serializedObject.FindProperty("stopView")));
        root.Add(new SpaceSmall());

        return root;
    }
}