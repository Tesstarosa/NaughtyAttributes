using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ReadOnlyTest : MonoBehaviour
	{
		public ReadOnlyNest1 nest1;

		[ReadOnly] public int readOnlyInt = 5;
	}

	[Serializable]
	public class ReadOnlyNest1
	{
		public ReadOnlyNest2 nest2;

		[ReadOnly] [AllowNesting] public float readOnlyFloat = 3.14f;
	}

	[Serializable]
	public struct ReadOnlyNest2
	{
		[ReadOnly] [AllowNesting] public string readOnlyString;
	}
}