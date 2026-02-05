using System;

namespace Framework
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TestClassAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method)]
	public class TestMethodAttribute : Attribute
	{
		public string Description { get; set; }
		public int Priority { get; set; } = 0;
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class IgnoreAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TestCaseAttribute : Attribute
	{
		public object[] Params { get; }
		public TestCaseAttribute(params object[] parameters) => Params = parameters;
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class BeforeEachAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method)]
	public class AfterEachAttribute : Attribute { }
}