using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ShowNativePropertyTest : MonoBehaviour
	{
		[ShowNativeProperty] private Transform Transform => transform;

		[ShowNativeProperty] private Transform ParentTransform => transform.parent;

		[ShowNativeProperty] private ushort MyUShort => ushort.MaxValue;

		[ShowNativeProperty] private short MyShort => short.MaxValue;

		[ShowNativeProperty] private ulong MyULong => ulong.MaxValue;

		[ShowNativeProperty] private long MyLong => long.MaxValue;

		[ShowNativeProperty] private uint MyUInt => uint.MaxValue;

		[ShowNativeProperty] private int MyInt => int.MaxValue;
	}
}