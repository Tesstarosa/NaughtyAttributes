using System;
using System.Collections.Generic;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public abstract class PropertyValidatorBase
	{
		public abstract void ValidateProperty(SerializedProperty property);
	}

	public static class ValidatorAttributeExtensions
	{
		private static readonly Dictionary<Type, PropertyValidatorBase> ValidatorsByAttributeType;

		static ValidatorAttributeExtensions()
		{
			ValidatorsByAttributeType = new Dictionary<Type, PropertyValidatorBase>();
			ValidatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();
			ValidatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
			ValidatorsByAttributeType[typeof(RequiredAttribute)] = new RequiredPropertyValidator();
			ValidatorsByAttributeType[typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator();
		}

		public static PropertyValidatorBase GetValidator(this ValidatorAttribute attr)
		{
			PropertyValidatorBase validator;
			if (ValidatorsByAttributeType.TryGetValue(attr.GetType(), out validator))
				return validator;
			return null;
		}
	}
}