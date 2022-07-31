using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ShowIfTest : MonoBehaviour
	{
		public ShowIfEnum enum1;
		[EnumFlags] public ShowIfEnumFlag enum2;

		public ShowIfNest1 nest1;
		public bool show1;
		public bool show2;

		[ShowIf(EConditionOperator.And, "show1", "show2")] [ReorderableList]
		public int[] showIfAll;

		[ShowIf(EConditionOperator.Or, "show1", "show2")] [ReorderableList]
		public int[] showIfAny;

		[ShowIf("enum1", ShowIfEnum.Case0)] [ReorderableList]
		public int[] showIfEnum;

		[ShowIf("enum2", ShowIfEnumFlag.Flag0)] [ReorderableList]
		public int[] showIfEnumFlag;

		[ShowIf("enum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)] [ReorderableList]
		public int[] showIfEnumFlagMulti;
	}

	[Serializable]
	public class ShowIfNest1
	{
		public ShowIfEnum enum1;
		[EnumFlags] public ShowIfEnumFlag enum2;

		public ShowIfNest2 nest2;
		public bool show1;
		public bool show2;

		[ShowIf(EConditionOperator.And, "Show1", "Show2")]
		[AllowNesting]
		// Because it's nested we need to explicitly allow nesting
		public int showIfAll;

		[ShowIf(EConditionOperator.Or, "Show1", "Show2")]
		[AllowNesting]
		// Because it's nested we need to explicitly allow nesting
		public int showIfAny;

		[ShowIf("Enum1", ShowIfEnum.Case1)] [AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int showIfEnum;

		[ShowIf("Enum2", ShowIfEnumFlag.Flag0)] [AllowNesting]
		public int showIfEnumFlag;

		[ShowIf("Enum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)] [AllowNesting]
		public int showIfEnumFlagMulti;

		public bool Show1 => show1;
		public bool Show2 => show2;
		public ShowIfEnum Enum1 => enum1;
		public ShowIfEnumFlag Enum2 => enum2;
	}

	[Serializable]
	public class ShowIfNest2
	{
		public ShowIfEnum enum1;
		[EnumFlags] public ShowIfEnumFlag enum2;
		public bool show1;
		public bool show2;

		[ShowIf(EConditionOperator.And, "GetShow1", "GetShow2")]
		[MinMaxSlider(0.0f, 1.0f)]
		// AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfAll = new(0.25f, 0.75f);

		[ShowIf(EConditionOperator.Or, "GetShow1", "GetShow2")]
		[MinMaxSlider(0.0f, 1.0f)]
		// AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfAny = new(0.25f, 0.75f);

		[ShowIf("GetEnum1", ShowIfEnum.Case2)]
		[MinMaxSlider(0.0f, 1.0f)]
		// AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfEnum = new(0.25f, 0.75f);

		[ShowIf("GetEnum2", ShowIfEnumFlag.Flag0)]
		[MinMaxSlider(0.0f, 1.0f)]
		// AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfEnumFlag;

		[ShowIf("GetEnum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)]
		[MinMaxSlider(0.0f, 1.0f)]
		// AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfEnumFlagMulti;

		public bool GetShow1()
		{
			return show1;
		}

		public bool GetShow2()
		{
			return show2;
		}

		public ShowIfEnum GetEnum1()
		{
			return enum1;
		}

		public ShowIfEnumFlag GetEnum2()
		{
			return enum2;
		}
	}

	public enum ShowIfEnum
	{
		Case0,
		Case1,
		Case2
	}

	[Flags]
	public enum ShowIfEnumFlag
	{
		Flag0 = 1,
		Flag1 = 2,
		Flag2 = 4,
		Flag3 = 8
	}
}