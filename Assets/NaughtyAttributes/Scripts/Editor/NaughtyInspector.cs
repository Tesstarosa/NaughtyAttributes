using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NaughtyAttributes.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Object), true)]
	public class NaughtyInspector : UnityEditor.Editor
	{
		private readonly Dictionary<string, SavedBool> _foldouts = new();
		private IEnumerable<MethodInfo> _methods;
		private IEnumerable<PropertyInfo> _nativeProperties;
		private IEnumerable<FieldInfo> _nonSerializedFields;
		private List<SerializedProperty> _serializedProperties = new();

		protected virtual void OnEnable()
		{
			_nonSerializedFields = ReflectionUtility.GetAllFields(
				target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

			_nativeProperties = ReflectionUtility.GetAllProperties(
				target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

			_methods = ReflectionUtility.GetAllMethods(
				target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
		}

		protected virtual void OnDisable()
		{
			ReorderableListPropertyDrawer.Instance.ClearCache();
		}

		public override void OnInspectorGUI()
		{
			GetSerializedProperties(ref _serializedProperties);

			var anyNaughtyAttribute =
				_serializedProperties.Any(p => PropertyUtility.GetAttribute<INaughtyAttribute>(p) != null);
			if (!anyNaughtyAttribute)
				DrawDefaultInspector();
			else
				DrawSerializedProperties();

			DrawNonSerializedFields();
			DrawNativeProperties();
			DrawButtons();
		}

		protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
		{
			outSerializedProperties.Clear();
			using (var iterator = serializedObject.GetIterator())
			{
				if (iterator.NextVisible(true))
					do
					{
						outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
					} while (iterator.NextVisible(false));
			}
		}

		protected void DrawSerializedProperties()
		{
			serializedObject.Update();

			// Draw non-grouped serialized properties
			foreach (var property in GetNonGroupedProperties(_serializedProperties))
				if (property.name.Equals("m_Script", StringComparison.Ordinal))
					using (new EditorGUI.DisabledScope(true))
					{
						EditorGUILayout.PropertyField(property);
					}
				else
					NaughtyEditorGui.PropertyField_Layout(property, true);

			// Draw grouped serialized properties
			foreach (var group in GetGroupedProperties(_serializedProperties))
			{
				var visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
				if (!visibleProperties.Any()) continue;

				NaughtyEditorGui.BeginBoxGroup_Layout(group.Key);
				foreach (var property in visibleProperties) NaughtyEditorGui.PropertyField_Layout(property, true);

				NaughtyEditorGui.EndBoxGroup_Layout();
			}

			// Draw foldout serialized properties
			foreach (var group in GetFoldoutProperties(_serializedProperties))
			{
				var visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
				if (!visibleProperties.Any()) continue;

				if (!_foldouts.ContainsKey(group.Key))
					_foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);

				_foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
				if (_foldouts[group.Key].Value)
					foreach (var property in visibleProperties)
						NaughtyEditorGui.PropertyField_Layout(property, true);
			}

			serializedObject.ApplyModifiedProperties();
		}

		protected void DrawNonSerializedFields(bool drawHeader = false)
		{
			if (_nonSerializedFields.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGuiStyle());
					NaughtyEditorGui.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
						HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var field in _nonSerializedFields)
					NaughtyEditorGui.NonSerializedField_Layout(serializedObject.targetObject, field);
			}
		}

		protected void DrawNativeProperties(bool drawHeader = false)
		{
			if (_nativeProperties.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Native Properties", GetHeaderGuiStyle());
					NaughtyEditorGui.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
						HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var property in _nativeProperties)
					NaughtyEditorGui.NativeProperty_Layout(serializedObject.targetObject, property);
			}
		}

		protected void DrawButtons(bool drawHeader = false)
		{
			if (_methods.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Buttons", GetHeaderGuiStyle());
					NaughtyEditorGui.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
						HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var method in _methods) NaughtyEditorGui.Button(serializedObject.targetObject, method);
			}
		}

		private static IEnumerable<SerializedProperty> GetNonGroupedProperties(
			IEnumerable<SerializedProperty> properties)
		{
			return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p) == null);
		}

		private static IEnumerable<IGrouping<string, SerializedProperty>> GetGroupedProperties(
			IEnumerable<SerializedProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name);
		}

		private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(
			IEnumerable<SerializedProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name);
		}

		private static GUIStyle GetHeaderGuiStyle()
		{
			var style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.UpperCenter;

			return style;
		}
	}
}