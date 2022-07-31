using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class InfoBoxTest : MonoBehaviour
	{
		public InfoBoxNest1 nest1;

		[InfoBox("Normal")] public int normal;
	}

	[Serializable]
	public class InfoBoxNest1
	{
		public InfoBoxNest2 nest2;

		[InfoBox("Warning", EInfoBoxType.Warning)]
		public int warning;
	}

	[Serializable]
	public class InfoBoxNest2
	{
		[InfoBox("Error", EInfoBoxType.Error)] public int error;
	}
}