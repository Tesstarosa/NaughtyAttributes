using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public static class ReflectionUtility
	{
		public static bool IsOverride(this MethodInfo m)
		{
			return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
		}

		public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
		{
			if (target == null)
			{
				Debug.LogError("The target object is null. Check for missing scripts.");
				yield break;
			}

			var types = GetSelfAndBaseTypes(target);

			for (var i = types.Count - 1; i >= 0; i--)
			{
				var fieldInfos = types[i]
					.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
					           BindingFlags.Public | BindingFlags.DeclaredOnly)
					.Where(predicate);

				foreach (var fieldInfo in fieldInfos) yield return fieldInfo;
			}
		}

		public static IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
		{
			if (target == null)
			{
				Debug.LogError("The target object is null. Check for missing scripts.");
				yield break;
			}

			var types = GetSelfAndBaseTypes(target);

			for (var i = types.Count - 1; i >= 0; i--)
			{
				var propertyInfos = types[i]
					.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
					               BindingFlags.Public | BindingFlags.DeclaredOnly)
					.Where(predicate);

				foreach (var propertyInfo in propertyInfos) yield return propertyInfo;
			}
		}

		private static bool HasOverride(MethodInfo target, IEnumerable<MethodInfo> methods)
		{
			foreach (var method in methods)
			{
				if (method == target) continue;
				if (target.Name != method.Name) continue;

				var currentBaseDef = target.GetBaseDefinition();
				var findedBaseDef = method.GetBaseDefinition();

				if (currentBaseDef == null) continue;

				if (findedBaseDef == null) continue;

				if (currentBaseDef != findedBaseDef) continue;

				if (currentBaseDef.DeclaringType != findedBaseDef.DeclaringType) continue;

				if (target.IsOverride() || method.IsOverride())
				{
					if(target.DeclaringType.IsSubclassOf(method.DeclaringType))
					{
						continue;
					}
					else
					{
						return true;
					}
				}
			}

			return false;
		}

		public static IEnumerable<MethodInfo> GetAllUniqueMethods(object target, Func<MethodInfo, bool> predicate)
		{
			if (target == null)
			{
				Debug.LogError("The target object is null. Check for missing scripts.");
				yield break;
			}

			var types = GetSelfAndBaseTypes(target);

			var methods = (from type in types
				from method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
				                               BindingFlags.Public | BindingFlags.DeclaredOnly)
				select method).Where(predicate).ToList();

			foreach (var method in methods)
			{
				if (HasOverride(method, methods))
				{
					continue;
				}

				yield return method;
			}
		}

		public static FieldInfo GetField(object target, string fieldName)
		{
			return GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.Ordinal)).FirstOrDefault();
		}

		public static PropertyInfo GetProperty(object target, string propertyName)
		{
			return GetAllProperties(target, p => p.Name.Equals(propertyName, StringComparison.Ordinal))
				.FirstOrDefault();
		}

		public static MethodInfo GetMethod(object target, string methodName)
		{
			return GetAllUniqueMethods(target, m => m.Name.Equals(methodName, StringComparison.Ordinal)).FirstOrDefault();
		}

		public static Type GetListElementType(Type listType)
		{
			if (listType.IsGenericType)
				return listType.GetGenericArguments()[0];
			return listType.GetElementType();
		}

		/// <summary>
		///     Get type and all base types of target, sorted as following:
		///     <para />
		///     [target's type, base type, base's base type, ...]
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private static List<Type> GetSelfAndBaseTypes(object target)
		{
			var types = new List<Type>
			{
				target.GetType()
			};

			while (types.Last().BaseType != null) types.Add(types.Last().BaseType);

			return types;
		}
	}
}