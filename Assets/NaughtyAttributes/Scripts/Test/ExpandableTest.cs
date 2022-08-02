using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ExpandableTest : MonoBehaviour
	{
		[Expandable]  public ExpandableObjectWithButton test;
		public ExpandableScriptableObjectNest1 nest1;

		[Expandable] public ScriptableObject obj0;

		// See #294
		public int precedingField = 5;
	}

	[Serializable]
	public class ExpandableScriptableObjectNest1
	{
		public ExpandableScriptableObjectNest2 nest2;

		[Expandable] public ScriptableObject obj1;
	}

	[Serializable]
	public class ExpandableScriptableObjectNest2
	{
		[Expandable] public ScriptableObject obj2;

		
	}
}