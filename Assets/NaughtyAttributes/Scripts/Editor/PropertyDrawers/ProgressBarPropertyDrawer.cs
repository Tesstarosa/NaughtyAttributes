using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ProgressBarAttribute))]
	public class ProgressBarPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			var progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
			var maxValue = GetMaxValue(property, progressBarAttribute);

			return IsNumber(property) && IsNumber(maxValue)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (!IsNumber(property))
			{
				var message = $"Field {property.name} is not a number";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				return;
			}

			var progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
			var value = property.propertyType == SerializedPropertyType.Integer
				? property.intValue
				: property.floatValue;
			var valueFormatted = property.propertyType == SerializedPropertyType.Integer
				? value.ToString(CultureInfo.InvariantCulture)
				: $"{value:0.00}";
			var maxValue = GetMaxValue(property, progressBarAttribute);

			if (maxValue != null && IsNumber(maxValue))
			{
				var fillPercentage = value / ReturnAsFloat(maxValue);
				var barLabel = $"{(!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "")}{valueFormatted}/{maxValue}";
				var barColor = progressBarAttribute.Color.GetColor();
				var labelColor = Color.white;

				var indentLength = NaughtyEditorGui.GetIndentLength(rect);
				var barRect = new Rect
				{
					x = rect.x + indentLength,
					y = rect.y,
					width = rect.width - indentLength,
					height = EditorGUIUtility.singleLineHeight
				};

				DrawBar(barRect, Mathf.Clamp01(fillPercentage), barLabel, barColor, labelColor);
			}
			else
			{
				var message =
					$"The provided dynamic max value for the progress bar is not correct. Please check if the '{nameof(progressBarAttribute.MaxValueName)}' is correct, or the return type is float/int";

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private static object GetMaxValue(SerializedProperty property, ProgressBarAttribute progressBarAttribute)
		{
			if (string.IsNullOrEmpty(progressBarAttribute.MaxValueName))
			{
				return progressBarAttribute.MaxValue;
			}

			var target = PropertyUtility.GetTargetObjectWithProperty(property);

			var valuesFieldInfo = ReflectionUtility.GetField(target, progressBarAttribute.MaxValueName);
			if (valuesFieldInfo != null) return valuesFieldInfo.GetValue(target);

			var valuesPropertyInfo = ReflectionUtility.GetProperty(target, progressBarAttribute.MaxValueName);
			if (valuesPropertyInfo != null) return valuesPropertyInfo.GetValue(target);

			var methodValuesInfo = ReflectionUtility.GetMethod(target, progressBarAttribute.MaxValueName);
			if (methodValuesInfo != null && (TypeIsNumber(methodValuesInfo.ReturnType)) && methodValuesInfo.GetParameters().Length == 0)
				return methodValuesInfo.Invoke(target, null);

			return null;
		}



		private static void DrawBar(Rect rect, float fillPercent, string label, Color barColor, Color labelColor)
		{
			if (Event.current.type != EventType.Repaint) return;

			var fillRect = new Rect(rect.x, rect.y, rect.width * fillPercent, rect.height);

			EditorGUI.DrawRect(rect, new Color(0.13f, 0.13f, 0.13f));
			EditorGUI.DrawRect(fillRect, barColor);

			// set alignment and cache the default
			var align = GUI.skin.label.alignment;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;

			// set the color and cache the default
			var c = GUI.contentColor;
			GUI.contentColor = labelColor;

			// calculate the position
			var labelRect = new Rect(rect.x, rect.y - 2, rect.width, rect.height);

			// draw~
			EditorGUI.DropShadowLabel(labelRect, label);

			// reset color and alignment
			GUI.contentColor = c;
			GUI.skin.label.alignment = align;
		}

		private static bool IsNumber(SerializedProperty property)
		{
			var isNumber = property.propertyType is SerializedPropertyType.Float or SerializedPropertyType.Integer;
			return isNumber;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsNumber(object value)
		{
			return value 
				is sbyte
				or byte
				or short
				or ushort
				or int
				or uint
				or long
				or ulong
				or float;
		}

		private static bool TypeIsNumber(Type type)
		{
			return type == typeof(sbyte)
			       || type == typeof(byte)
			       || type == typeof(short)
			       || type == typeof(ushort)
			       || type == typeof(int)
			       || type == typeof(uint)
			       || type == typeof(long)
			       || type == typeof(float);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float ReturnAsFloat(object obj)
		{
			return obj switch
			{
				int		 i => i,
				uint	 ui => ui,
				sbyte	 sb => sb,
				byte	 b => b,
				short	 s => s,
				ushort	 us => us,
				long	 l => l,
				ulong	 ul => ul,
				_ => (float) obj
			};
		}
	}
}