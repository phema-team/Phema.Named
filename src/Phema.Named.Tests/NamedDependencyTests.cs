using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.Named.Tests
{
	public interface IDependency
	{
	}

	public class DependencyA : IDependency
	{
	}
	
	public class DependencyB : IDependency
	{
	}

	public class DependencyC : IDependency
	{
	}
	
	public class NamedDependencyTests
	{
		[Fact]
		public void ServiceCollectionDependencies()
		{
			var services = new ServiceCollection()
				.AddNamedSingleton<IDependency, DependencyA>("A")
				.AddNamedTransient<IDependency, DependencyB>("B")
				.AddNamedScoped<IDependency, DependencyC>("C");

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
				.AddNamedSingleton<IDependency, DependencyA>("A")
				.AddNamedSingleton<IDependency, DependencyB>("B")
				.AddNamedSingleton<IDependency, DependencyC>("C");

			var provider = services.BuildServiceProvider();

			Assert.Equal(3, provider.GetServices<IDependency>().Count());

			Assert.IsType<DependencyA>(provider.GetRequiredNamedService<IDependency>("A"));
			Assert.IsType<DependencyB>(provider.GetRequiredNamedService<IDependency>("B"));

			Assert.IsType<DependencyC>(provider.GetNamedService<IDependency>("C"));
			Assert.Null(provider.GetNamedService<IDependency>("D"));
		}

		[Fact]
		public void PopulateOptions()
		{
			var services = new ServiceCollection()
				.AddNamedSingleton<IDependency, DependencyA>("A");

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<NamedOptions>>().Value;

			var ((name, service), implementation) = Assert.Single(options.NamedDependencies);

			Assert.Equal("A", name);
			Assert.Equal(typeof(IDependency), service);
			Assert.Equal(typeof(DependencyA), implementation);
		}
	}
}