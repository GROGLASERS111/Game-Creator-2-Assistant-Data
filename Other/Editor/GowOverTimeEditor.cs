using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ETK;

[CustomEditor(typeof(GrowOvertime))]
public class MyScriptEditorGrow : Editor
{
    Texture2D headerImage;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        var header = new Image() { image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RVRGaming/ETK2/RPG/Scripts/Other/Editor/HeaderOvertime.png") };
        header.style.maxHeight = 34;
        header.style.maxWidth = 200;
        header.style.marginLeft = 0;
        root.Add(header);

        root.Add(new SpaceSmall());

        root.Add(new PropertyField(serializedObject.FindProperty("minSize")));
        root.Add(new PropertyField(serializedObject.FindProperty("maxSize")));
        root.Add(new PropertyField(serializedObject.FindProperty("resizeDuration")));
        root.Add(new PropertyField(serializedObject.FindProperty("canvas")));
        root.Add(new SpaceSmall());

        var playerInteractLabel = new Label("Player Interact");
        root.Add(playerInteractLabel);
        root.Add(new SpaceSmall());
        root.Add(new PropertyField(serializedObject.FindProperty("onRun")));
        root.Add(new SpaceSmall());

        return root;
    }
}