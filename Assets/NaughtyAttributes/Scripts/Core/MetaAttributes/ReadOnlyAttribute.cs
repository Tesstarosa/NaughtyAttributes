﻿using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ReadOnlyAttribute : MetaAttribute
	{
	}
}