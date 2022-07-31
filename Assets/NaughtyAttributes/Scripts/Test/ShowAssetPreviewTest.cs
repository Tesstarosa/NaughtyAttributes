using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ShowAssetPreviewTest : MonoBehaviour
	{
		public ShowAssetPreviewNest1 nest1;

		[ShowAssetPreview(96, 96)] public GameObject prefab0;

		[ShowAssetPreview] public Sprite sprite0;
	}

	[Serializable]
	public class ShowAssetPreviewNest1
	{
		public ShowAssetPreviewNest2 nest2;

		[ShowAssetPreview(96, 96)] public GameObject prefab1;

		[ShowAssetPreview] public Sprite sprite1;
	}

	[Serializable]
	public class ShowAssetPreviewNest2
	{
		[ShowAssetPreview(96, 96)] public GameObject prefab2;

		[ShowAssetPreview] public Sprite sprite2;
	}
}