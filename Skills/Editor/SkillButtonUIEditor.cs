using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ETK;

[CustomEditor(typeof(SkillButtonUI))]
public class MyScriptEditorSkillButtonUI : Editor
{
    Texture2D headerImage;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        var header = new Image() { image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RVRGaming/ETK2/RPG/Scripts/Skills/Editor/HeaderImageSkillButtonUI.png") };
        header.style.maxHeight = 34;
        header.style.maxWidth = 200;
        header.style.marginLeft = 0;
        root.Add(header);

        root.Add(new SpaceSmall());

        root.Add(new PropertyField(serializedObject.FindProperty("skillTitle")));
        root.Add(new PropertyField(serializedObject.FindProperty("icon")));
        root.Add(new PropertyField(serializedObject.FindProperty("bgPassive")));
        root.Add(new PropertyField(serializedObject.FindProperty("bgActive")));
        root.Add(new PropertyField(serializedObject.FindProperty("popupPrefab")));
        root.Add(new SpaceSmall());

        return root;
    }
}