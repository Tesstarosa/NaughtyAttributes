using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public class ValidateInputPropertyValidator : PropertyValidatorBase
	{
		public override void ValidateProperty(SerializedProperty property)
		{
			var validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
			var target = PropertyUtility.GetTargetObjectWithProperty(property);

			var validationCallback = ReflectionUtility.GetMethod(target, validateInputAttribute.CallbackName);

			if (validationCallback != null &&
			    validationCallback.ReturnType == typeof(bool))
			{
				var callbackParameters = validationCallback.GetParameters();

				if (callbackParameters.Length == 0)
				{
					if (!(bool) validationCallback.Invoke(target, null))
					{
						if (string.IsNullOrEmpty(validateInputAttribute.Message))
							NaughtyEditorGui.HelpBox_Layout(
								property.name + " is not valid", MessageType.Error,
								property.serializedObject.targetObject);
						else
							NaughtyEditorGui.HelpBox_Layout(
								validateInputAttribute.Message, MessageType.Error,
								property.serializedObject.targetObject);
					}
				}
				else if (callbackParameters.Length == 1)
				{
					var fieldInfo = ReflectionUtility.GetField(target, property.name);
					var fieldType = fieldInfo.FieldType;
					var parameterType = callbackParameters[0].ParameterType;

					if (fieldType == parameterType)
					{
						if (!(bool) validationCallback.Invoke(target, new[] {fieldInfo.GetValue(target)}))
						{
							if (string.IsNullOrEmpty(validateInputAttribute.Message))
								NaughtyEditorGui.HelpBox_Layout(
									property.name + " is not valid", MessageType.Error,
									property.serializedObject.targetObject);
							else
								NaughtyEditorGui.HelpBox_Layout(
									validateInputAttribute.Message, MessageType.Error,
									property.serializedObject.targetObject);
						}
					}
					else
					{
						var warning = "The field type is not the same as the callback's parameter type";
						NaughtyEditorGui.HelpBox_Layout(warning, MessageType.Warning,
							property.serializedObject.targetObject);
					}
				}
				else
				{
					var warning =
						validateInputAttribute.GetType().Name +
						" needs a callback with boolean return type and an optional single parameter of the same type as the field";

					NaughtyEditorGui.HelpBox_Layout(warning, MessageType.Warning,
						property.serializedObject.targetObject);
				}
			}
		}
	}
}