using UnityEngine;
using UnityEditor;
using System.Collections;



[CustomPropertyDrawer (typeof(NamedArrayAttribute))]public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
        //EditorGUI.LabelField(rect, "lol");
        //base.OnGUI(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
        EditorGUI.PropertyField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
        //EditorGUI.prop(rect, property, label);
       // try {
        //    int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
       //     EditorGUI.ObjectField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
       // } catch {
       //     EditorGUI.ObjectField(rect, property, label);
       // }
    }
}