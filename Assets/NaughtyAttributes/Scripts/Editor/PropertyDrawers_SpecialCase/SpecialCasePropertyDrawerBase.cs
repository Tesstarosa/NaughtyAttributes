﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public abstract class SpecialCasePropertyDrawerBase
	{
		public void OnGUI(Rect rect, SerializedProperty property)
		{
			// Check if visible
			var visible = PropertyUtility.IsVisible(property);
			if (!visible) return;

			// Validate
			var validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
			foreach (var validatorAttribute in validatorAttributes)
				validatorAttribute.GetValidator().ValidateProperty(property);

			// Check if enabled and draw
			EditorGUI.BeginChangeCheck();
			var enabled = PropertyUtility.IsEnabled(property);

			using (new EditorGUI.DisabledScope(!enabled))
			{
				OnGUI_Internal(rect, property, PropertyUtility.GetLabel(property));
			}

			// Call OnValueChanged callbacks
			if (EditorGUI.EndChangeCheck()) PropertyUtility.CallOnValueChangedCallbacks(property);
		}

		public float GetPropertyHeight(SerializedProperty property)
		{
			return GetPropertyHeight_Internal(property);
		}

		protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
		protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
	}

	public static class SpecialCaseDrawerAttributeExtensions
	{
		private static readonly Dictionary<Type, SpecialCasePropertyDrawerBase> DrawersByAttributeType;

		static SpecialCaseDrawerAttributeExtensions()
		{
			DrawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>();
			DrawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;
		}

		public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr)
		{
			SpecialCasePropertyDrawerBase drawer;
			if (DrawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
				return drawer;
			return null;
		}
	}
}