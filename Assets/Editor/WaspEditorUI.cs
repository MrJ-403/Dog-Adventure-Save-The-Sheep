using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(WaspController))]
public class WaspEditorUI : Editor
{
    public VisualTreeAsset visualTreeAsset;
    //public override VisualElement CreateInspectorGUI()
    //{
    //    VisualElement root = new VisualElement();

    //    //add in all the UI builder stuff
    //    visualTreeAsset.CloneTree(root);

    //    return root;
    //}
}
