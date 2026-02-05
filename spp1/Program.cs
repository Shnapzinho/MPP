using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Framework;

namespace Runner
{
	class Program
	{
		static async Task Main()
		{
			var assembly = typeof(Tests.LibraryTests).Assembly;
			var testClasses = assembly.GetTypes().Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

			int passed = 0, failed = 0, ignored = 0;

			foreach (var type in testClasses)
			{
				Console.WriteLine($"TESTS: {type.Name}");
				var instance = Activator.CreateInstance(type);
				var methods = type.GetMethods();
				var setup = methods.FirstOrDefault(m => m.GetCustomAttribute<BeforeEachAttribute>() != null);
				var cleanup = methods.FirstOrDefault(m => m.GetCustomAttribute<AfterEachAttribute>() != null);
				var testMethods = methods
					.Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null)
					.OrderByDescending(m => m.GetCustomAttribute<TestMethodAttribute>().Priority);

				foreach (var method in testMethods)
				{
					var testAttr = method.GetCustomAttribute<TestMethodAttribute>();
					var ignoreAttr = method.GetCustomAttribute<IgnoreAttribute>();

					if (ignoreAttr != null)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write("[SKIP] ");
						Console.ResetColor();
						Console.WriteLine(testAttr.Description ?? method.Name);
						ignored++; 
						Console.ResetColor(); 
						continue;
					}

					var cases = method.GetCustomAttributes<TestCaseAttribute>().ToList();
					if (!cases.Any()) cases.Add(new TestCaseAttribute(null));

					foreach (var tc in cases)
					{
						try
						{
							setup?.Invoke(instance, null);
							object result = (tc.Params == null) ? method.Invoke(instance, null) : method.Invoke(instance, tc.Params);
							if (result is Task t) await t;

							Console.ForegroundColor = ConsoleColor.Green;
							Console.Write("[PASS] ");
							Console.ResetColor();
							Console.WriteLine(testAttr.Description ?? method.Name);
							passed++;
						}
						catch (Exception ex)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.Write("[FAIL] ");
							Console.ResetColor();
							var msg = ex.InnerException?.Message ?? ex.Message;
							Console.WriteLine($"{testAttr.Description ?? method.Name}. {msg}");
							failed++;
						}
						finally { cleanup?.Invoke(instance, null); Console.ResetColor(); }
					}
				}
			}
			Console.WriteLine("\n==============================");
			Console.WriteLine($"TOTALS: Passed: {passed}, Failed: {failed}, Ignored: {ignored}");
			Console.WriteLine("==============================");
			Console.ReadKey();
		}
	}
}