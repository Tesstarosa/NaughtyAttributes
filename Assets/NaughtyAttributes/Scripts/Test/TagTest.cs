using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class TagTest : MonoBehaviour
	{
		public TagNest1 nest1;

		[Tag] public string tag0;

		[Button]
		private void LogTag0()
		{
			Debug.Log(tag0);
		}
	}

	[Serializable]
	public class TagNest1
	{
		public TagNest2 nest2;

		[Tag] public string tag1;
	}

	[Serializable]
	public struct TagNest2
	{
		[Tag] public string tag2;
	}
}