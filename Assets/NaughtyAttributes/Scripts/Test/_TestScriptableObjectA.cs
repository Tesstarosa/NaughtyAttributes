using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "TestScriptableObjectA", menuName = "NaughtyAttributes/TestScriptableObjectA")]
	public class TestScriptableObjectA : ScriptableObject
	{
		[Expandable] public List<TestScriptableObjectB> listB;
	}
}