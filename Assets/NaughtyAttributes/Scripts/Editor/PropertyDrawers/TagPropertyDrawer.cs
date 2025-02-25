﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TagAttribute))]
	public class TagPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			return property.propertyType == SerializedPropertyType.String
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (property.propertyType == SerializedPropertyType.String)
			{
				// generate the taglist + custom tags
				var tagList = new List<string>();
				tagList.Add("(None)");
				tagList.Add("Untagged");
				tagList.AddRange(InternalEditorUtility.tags);

				var propertyString = property.stringValue;
				var index = 0;
				// check if there is an entry that matches the entry and get the index
				// we skip index 0 as that is a special custom case
				for (var i = 1; i < tagList.Count; i++)
					if (tagList[i].Equals(propertyString, StringComparison.Ordinal))
					{
						index = i;
						break;
					}

				// Draw the popup box with the current selected index
				var newIndex = EditorGUI.Popup(rect, label.text, index, tagList.ToArray());

				// Adjust the actual string value of the property based on the selection
				var newValue = newIndex > 0 ? tagList[newIndex] : string.Empty;

				if (!property.stringValue.Equals(newValue, StringComparison.Ordinal)) property.stringValue = newValue;
			}
			else
			{
				var message = $"{nameof(TagAttribute)} supports only string fields";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}
	}
}