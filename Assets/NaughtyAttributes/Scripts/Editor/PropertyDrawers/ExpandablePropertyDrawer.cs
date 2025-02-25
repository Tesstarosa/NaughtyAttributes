﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ExpandableAttribute))]
	public class ExpandablePropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			if (property.objectReferenceValue == null) return GetPropertyHeight(property);

			var propertyType = PropertyUtility.GetPropertyType(property);
			if (typeof(ScriptableObject).IsAssignableFrom(propertyType))
			{
				var scriptableObject = property.objectReferenceValue as ScriptableObject;
				if (scriptableObject == null) return GetPropertyHeight(property);

				if (property.isExpanded)
					using (var serializedObject = new SerializedObject(scriptableObject))
					{
						var totalHeight = EditorGUIUtility.singleLineHeight;

						using (var iterator = serializedObject.GetIterator())
						{
							if (iterator.NextVisible(true))
								do
								{
									var childProperty = serializedObject.FindProperty(iterator.name);
									if (childProperty.name.Equals("m_Script", StringComparison.Ordinal)) continue;

									var visible = PropertyUtility.IsVisible(childProperty);
									if (!visible) continue;

									var height = GetPropertyHeight(childProperty);
									totalHeight += height;
								} while (iterator.NextVisible(false));
						}

						totalHeight += EditorGUIUtility.standardVerticalSpacing;
						return totalHeight;
					}

				return GetPropertyHeight(property);
			}

			return GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (property.objectReferenceValue == null)
			{
				EditorGUI.PropertyField(rect, property, label, false);
			}
			else
			{
				var propertyType = PropertyUtility.GetPropertyType(property);
				if (typeof(ScriptableObject).IsAssignableFrom(propertyType))
				{
					var scriptableObject = property.objectReferenceValue as ScriptableObject;
					if (scriptableObject == null)
					{
						EditorGUI.PropertyField(rect, property, label, false);
					}
					else
					{
						// Draw a foldout
						var foldoutRect = new Rect
						{
							x = rect.x,
							y = rect.y,
							width = EditorGUIUtility.labelWidth,
							height = EditorGUIUtility.singleLineHeight
						};

						property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

						// Draw the scriptable object field
						var propertyRect = new Rect
						{
							x = rect.x,
							y = rect.y,
							width = rect.width,
							height = EditorGUIUtility.singleLineHeight
						};

						EditorGUI.PropertyField(propertyRect, property, label, false);

						// Draw the child properties
						if (property.isExpanded)
						{
							DrawChildProperties(rect, property);
							DrawButtons(property);
						}
					}
				}
				else
				{
					var message = $"{nameof(ExpandableAttribute)} can only be used on scriptable objects";
					DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				}
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();
		}

		private static void DrawButtons(SerializedProperty property)
		{
			if (property.objectReferenceValue == null) return;
			foreach (var methodInfo in ReflectionUtility.GetAllUniqueMethods(
				         property.objectReferenceValue,
				         m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0))
			{
				NaughtyEditorGui.Button(property.objectReferenceValue, methodInfo);
			}
		}

		private void DrawChildProperties(Rect rect, SerializedProperty property)
		{
			var scriptableObject = property.objectReferenceValue as ScriptableObject;
			if (scriptableObject == null) return;

			var boxRect = new Rect
			{
				x = 0.0f,
				y = rect.y + EditorGUIUtility.singleLineHeight,
				width = rect.width * 2.0f,
				height = rect.height - EditorGUIUtility.singleLineHeight
			};

			GUI.Box(boxRect, GUIContent.none);

			using (new EditorGUI.IndentLevelScope())
			{
				var serializedObject = new SerializedObject(scriptableObject);
				serializedObject.Update();

				using (var iterator = serializedObject.GetIterator())
				{
					var yOffset = EditorGUIUtility.singleLineHeight;

					if (iterator.NextVisible(true))
						do
						{
							var childProperty = serializedObject.FindProperty(iterator.name);
							if (childProperty.name.Equals("m_Script", StringComparison.Ordinal)) continue;

							var visible = PropertyUtility.IsVisible(childProperty);
							if (!visible) continue;

							var childHeight = GetPropertyHeight(childProperty);
							var childRect = new Rect
							{
								x = rect.x,
								y = rect.y + yOffset,
								width = rect.width,
								height = childHeight
							};

							NaughtyEditorGui.PropertyField(childRect, childProperty, true);

							yOffset += childHeight;
						} while (iterator.NextVisible(false));
				}

				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}