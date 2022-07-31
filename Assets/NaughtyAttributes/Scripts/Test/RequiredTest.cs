using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class RequiredTest : MonoBehaviour
	{
		public RequiredNest1 nest1;

		[Required] public Transform trans0;
	}

	[Serializable]
	public class RequiredNest1
	{
		public RequiredNest2 nest2;

		[Required] [AllowNesting] // Because it's nested we need to explicitly allow nesting
		public Transform trans1;
	}

	[Serializable]
	public class RequiredNest2
	{
		[Required("trans2 is invalid custom message - hohoho")]
		[AllowNesting]
		// Because it's nested we need to explicitly allow nesting
		public Transform trans2;
	}
}