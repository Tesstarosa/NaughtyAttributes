using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(CurveRangeAttribute))]
	public class CurveRangePropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			var propertyHeight = property.propertyType == SerializedPropertyType.AnimationCurve
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			// Check user error
			if (property.propertyType != SerializedPropertyType.AnimationCurve)
			{
				var message = $"Field {property.name} is not an AnimationCurve";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				return;
			}

			var curveRangeAttribute = (CurveRangeAttribute) attribute;
			var curveRanges = new Rect(
				curveRangeAttribute.Min.x,
				curveRangeAttribute.Min.y,
				curveRangeAttribute.Max.x - curveRangeAttribute.Min.x,
				curveRangeAttribute.Max.y - curveRangeAttribute.Min.y);

			EditorGUI.CurveField(
				rect,
				property,
				curveRangeAttribute.Color == EColor.Clear ? Color.green : curveRangeAttribute.Color.GetColor(),
				curveRanges,
				label);

			EditorGUI.EndProperty();
		}
	}
}