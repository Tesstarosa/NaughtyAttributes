using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ProgressBarAttribute : DrawerAttribute
	{
		public ProgressBarAttribute(string name, float maxValue, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValue = maxValue;
			Color = color;
		}

		public ProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValueName = maxValueName;
			Color = color;
		}

		public ProgressBarAttribute(float maxValue, EColor color = EColor.Blue)
			: this("", maxValue, color)
		{
		}

		public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
			: this("", maxValueName, color)
		{
		}

		public string Name { get; }
		public float MaxValue { get; set; }
		public string MaxValueName { get; }
		public EColor Color { get; }
	}
}