﻿using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class LayerTest : MonoBehaviour
	{
		[Layer] public string layerName0;

		[Layer] public int layerNumber0;

		public LayerNest1 nest1;

		[Button]
		public void DebugLog()
		{
			Debug.LogFormat("{0} = {1}", nameof(layerNumber0), layerNumber0);
			Debug.LogFormat("{0} = {1}", nameof(layerName0), layerName0);
			Debug.LogFormat("LayerToName({0}) = {1}", layerNumber0, LayerMask.LayerToName(layerNumber0));
			Debug.LogFormat("NameToLayer({0}) = {1}", layerName0, LayerMask.NameToLayer(layerName0));
		}
	}

	[Serializable]
	public class LayerNest1
	{
		[Layer] public string layerName1;

		[Layer] public int layerNumber1;

		public LayerNest2 nest2;
	}

	[Serializable]
	public struct LayerNest2
	{
		[Layer] public int layerNumber2;

		[Layer] public string layerName2;
	}
}