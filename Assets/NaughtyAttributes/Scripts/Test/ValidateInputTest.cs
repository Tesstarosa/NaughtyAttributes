﻿using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ValidateInputTest : MonoBehaviour
	{
		public ValidateInputInheritedNest inheritedNest;

		[ValidateInput("NotZero0", "int0 must not be zero")]
		public int int0;

		public ValidateInputNest1 nest1;

		private bool NotZero0(int value)
		{
			return value != 0;
		}
	}

	[Serializable]
	public class ValidateInputNest1
	{
		[ValidateInput("NotZero1")] [AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int int1;

		public ValidateInputNest2 nest2;

		private bool NotZero1(int value)
		{
			return value != 0;
		}
	}

	[Serializable]
	public class ValidateInputNest2
	{
		[ValidateInput("NotZero2")] [AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int int2;

		private bool NotZero2(int value)
		{
			return value != 0;
		}
	}

	[Serializable]
	public class ValidateInputInheritedNest : ValidateInputNest1
	{
	}
}