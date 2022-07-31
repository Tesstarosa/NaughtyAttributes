using System;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ReorderableListTest : MonoBehaviour
	{
		[ReorderableList] public GameObject[] gameObjectsList;

		[ReorderableList] public int[] intArray;

		[ReorderableList] public List<MonoBehaviour> monoBehavioursList;

		[ReorderableList] public List<SomeStruct> structList;

		[ReorderableList] public List<Transform> transformsList;

		[ReorderableList] public List<Vector3> vectorList;
	}

	[Serializable]
	public struct SomeStruct
	{
		public int @int;
		public float @float;
		public Vector3 vector;
	}
}