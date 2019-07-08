using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.Named.Tests
{
	public class NamedDependencyTests
	{
		[Fact]
		public void ServiceCollectionDependencies()
		{
			var services = new ServiceCollection()
				.AddSingleton<IDependency, DependencyA>("A")
				.AddTransient<IDependency, DependencyB>("B")
				.AddScoped<IDependency, DependencyC>("C");

			Assert.Equal(3, services.Count(s => s.ServiceType == typeof(IDependency)));
			var dependencyA = Assert.Single(services.Where(s => s.ServiceType == typeof(DependencyA)));
			Assert.Equal(ServiceLifetime.Singleton, dependencyA.Lifetime);

			var dependencyB = Assert.Single(services.Where(s => s.ServiceType == typeof(DependencyB)));
			Assert.Equal(ServiceLifetime.Transient, dependencyB.Lifetime);

			var dependencyC = Assert.Single(services.Where(s => s.ServiceType == typeof(DependencyC)));
			Assert.Equal(ServiceLifetime.Scoped, dependencyC.Lifetime);
		}

		[Fact]
		public void RegisterResolve()
		{
			var services = new ServiceCollection()
				.AddSingleton<IDependency, DependencyA>("A")
				.AddSingleton<IDependency, DependencyB>("B")
				.AddSingleton<IDependency, DependencyC>("C");

			var provider = services.BuildServiceProvider();

			Assert.Equal(3, provider.GetServices<IDependency>().Count());

			Assert.IsType<DependencyA>(provider.GetRequiredService<IDependency>("A"));
			Assert.IsType<DependencyB>(provider.GetRequiredService<IDependency>("B"));

			Assert.IsType<DependencyC>(provider.GetService<IDependency>("C"));
			Assert.Null(provider.GetService<IDependency>("D"));
		}

		[Fact]
		public void PopulateOptions()
		{
			var services = new ServiceCollection()
				.AddSingleton<IDependency, DependencyA>("A");

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<NamedOptions>>().Value;

			var ((name, service), implementation) = Assert.Single(options.NamedDependencies);

			Assert.Equal("A", name);
			Assert.Equal(typeof(IDependency), service);
			Assert.Equal(typeof(DependencyA), implementation);
		}
	}
}