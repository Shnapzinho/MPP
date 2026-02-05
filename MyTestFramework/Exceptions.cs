using System;

namespace Framework
{
	public class AssertionException : Exception
	{
		public AssertionException(string message) : base(message) { }
	}
}