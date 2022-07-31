using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ResizableTextAreaTest : MonoBehaviour
	{
		public ResizableTextAreaNest1 nest1;

		[ResizableTextArea] public string text0;
	}

	[Serializable]
	public class ResizableTextAreaNest1
	{
		public ResizableTextAreaNest2 nest2;

		[ResizableTextArea] public string text1;
	}

	[Serializable]
	public class ResizableTextAreaNest2
	{
		[ResizableTextArea] public string text2;
	}
}