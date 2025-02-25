﻿using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	public class MinMaxSliderPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			return property.propertyType == SerializedPropertyType.Vector2 ||
			       property.propertyType == SerializedPropertyType.Vector2Int
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			var minMaxSliderAttribute = (MinMaxSliderAttribute) attribute;

			if (property.propertyType == SerializedPropertyType.Vector2 ||
			    property.propertyType == SerializedPropertyType.Vector2Int)
			{
				EditorGUI.BeginProperty(rect, label, property);

				var indentLength = NaughtyEditorGui.GetIndentLength(rect);
				var labelWidth = EditorGUIUtility.labelWidth + NaughtyEditorGui.HorizontalSpacing;
				var floatFieldWidth = EditorGUIUtility.fieldWidth;
				var sliderWidth = rect.width - labelWidth - 2.0f * floatFieldWidth;
				var sliderPadding = 5.0f;

				var labelRect = new Rect(
					rect.x,
					rect.y,
					labelWidth,
					rect.height);

				var sliderRect = new Rect(
					rect.x + labelWidth + floatFieldWidth + sliderPadding - indentLength,
					rect.y,
					sliderWidth - 2.0f * sliderPadding + indentLength,
					rect.height);

				var minFloatFieldRect = new Rect(
					rect.x + labelWidth - indentLength,
					rect.y,
					floatFieldWidth + indentLength,
					rect.height);

				var maxFloatFieldRect = new Rect(
					rect.x + labelWidth + floatFieldWidth + sliderWidth - indentLength,
					rect.y,
					floatFieldWidth + indentLength,
					rect.height);

				// Draw the label
				EditorGUI.LabelField(labelRect, label.text);

				// Draw the slider
				EditorGUI.BeginChangeCheck();

				switch (property.propertyType)
				{
					case SerializedPropertyType.Vector2:
					{
						var sliderValue = property.vector2Value;
						EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y,
							minMaxSliderAttribute.MinValue, minMaxSliderAttribute.MaxValue);

						sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
						sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue,
							Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

						sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
						sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x),
							minMaxSliderAttribute.MaxValue);

						if (EditorGUI.EndChangeCheck()) property.vector2Value = sliderValue;
						break;
					}
					case SerializedPropertyType.Vector2Int:
					{
						var sliderValue = property.vector2IntValue;
						float xValue = sliderValue.x;
						float yValue = sliderValue.y;
						EditorGUI.MinMaxSlider(sliderRect, ref xValue, ref yValue, minMaxSliderAttribute.MinValue,
							minMaxSliderAttribute.MaxValue);

						sliderValue.x = EditorGUI.IntField(minFloatFieldRect, (int) xValue);
						sliderValue.x = (int) Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue,
							Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

						sliderValue.y = EditorGUI.IntField(maxFloatFieldRect, (int) yValue);
						sliderValue.y = (int) Mathf.Clamp(sliderValue.y,
							Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x), minMaxSliderAttribute.MaxValue);

						if (EditorGUI.EndChangeCheck()) property.vector2IntValue = sliderValue;
						break;
					}
				}

				EditorGUI.EndProperty();
			}
			else
			{
				var message = minMaxSliderAttribute.GetType().Name +
				              " can be used only on Vector2 or Vector2Int fields";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}
	}
}