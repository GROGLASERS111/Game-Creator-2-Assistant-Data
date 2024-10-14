using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ETK;

[CustomEditor(typeof(HandIKControl))]
public class MyScriptEditorHandIK : Editor
{
    Texture2D headerImage;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        var header = new Image() { image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RVRGaming/ETK2/RPG/Scripts/Other/Editor/HeaderIK.png") };
        header.style.maxHeight = 34;
        header.style.maxWidth = 200;
        header.style.marginLeft = 0;
        root.Add(header);

        root.Add(new SpaceSmall());

        root.Add(new PropertyField(serializedObject.FindProperty("ikActive")));
        root.Add(new PropertyField(serializedObject.FindProperty("rightHandObj")));
        root.Add(new PropertyField(serializedObject.FindProperty("leftHandObj")));
        root.Add(new SpaceSmall());

        return root;
    }
}