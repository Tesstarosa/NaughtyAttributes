using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class SceneTest : MonoBehaviour
	{
		public SceneNest1 nest1;

		[Scene] public string scene0;
	}

	[Serializable]
	public class SceneNest1
	{
		public SceneNest2 nest2;

		[Scene] public string scene1;
	}

	[Serializable]
	public struct SceneNest2
	{
		[Scene] public int scene2;
	}
}