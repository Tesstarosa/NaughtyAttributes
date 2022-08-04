using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class GenericTargetTest : GenericTest
	{
		[Button]
		public override void Test()
		{
			Debug.Log("Work?");
		}

		[Button(text:"Test")]
		public void Test1()
		{
			Debug.Log("Absolutely");
		}
	}
}