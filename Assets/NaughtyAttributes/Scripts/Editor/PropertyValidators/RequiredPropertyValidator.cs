﻿using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public class RequiredPropertyValidator : PropertyValidatorBase
	{
		public override void ValidateProperty(SerializedProperty property)
		{
			var requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				if (property.objectReferenceValue == null)
				{
					var errorMessage = property.name + " is required";
					if (!string.IsNullOrEmpty(requiredAttribute.Message)) errorMessage = requiredAttribute.Message;

					NaughtyEditorGui.HelpBox_Layout(errorMessage, MessageType.Error,
						property.serializedObject.targetObject);
				}
			}
			else
			{
				var warning = requiredAttribute.GetType().Name + " works only on reference types";
				NaughtyEditorGui.HelpBox_Layout(warning, MessageType.Warning, property.serializedObject.targetObject);
			}
		}
	}
}