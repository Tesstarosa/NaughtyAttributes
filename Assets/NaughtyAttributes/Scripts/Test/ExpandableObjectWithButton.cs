using UnityEngine;

namespace NaughtyAttributes.Test
{

	[CreateAssetMenu(menuName = "Create ExpandableObjectWithButton", fileName = "ExpandableObjectWithButton", order = 0)]
	public class ExpandableObjectWithButton : ScriptableObject
	{

		[Button]
		public void Test()
		{

		}

		[Button]
		public void Test2()
		{

		}

	}
}