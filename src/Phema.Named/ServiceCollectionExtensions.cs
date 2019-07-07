using Microsoft.Extensions.DependencyInjection;

namespace Phema.Named
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds named service of the type specified in <typeparamref name="TService" /> with an
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// </summary>
		public static IServiceCollection AddNamed<TService, TImplementation>(
			this IServiceCollection services,
			ServiceLifetime serviceLifetime,
			string name)
			where TService : class
			where TImplementation : class, TService
		{
			services.Add(ServiceDescriptor.Describe(
				typeof(TImplementation),
				typeof(TImplementation),
				serviceLifetime));

			services.Add(ServiceDescriptor.Describe(
				typeof(TService),
				sp => sp.GetRequiredService<TImplementation>(),
				serviceLifetime));

			services.Configure<NamedOptions>(options =>
				options.NamedDependencies
					.Add((name, typeof(TService)), typeof(TImplementation)));

			return services;
		}

		/// <summary>
		/// Adds singleton named service of the type specified in <typeparamref name="TService" /> with an
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// </summary>
		public static IServiceCollection AddNamedSingleton<TService, TImplementation>(
			this IServiceCollection services,
			string name)
			where TService : class
			where TImplementation : class, TService
		{
			return services.AddNamed<TService, TImplementation>(ServiceLifetime.Singleton, name);
		}

		/// <summary>
		/// Adds transient named service of the type specified in <typeparamref name="TService" /> with an
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// </summary>
		public static IServiceCollection AddNamedTransient<TService, TImplementation>(
			this IServiceCollection services,
			string name)
			where TService : class
			where TImplementation : class, TService
		{
			return services.AddNamed<TService, TImplementation>(ServiceLifetime.Transient, name);
		}

		/// <summary>
		/// Adds scoped named service of the type specified in <typeparamref name="TService" /> with an
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// </summary>
		public static IServiceCollection AddNamedScoped<TService, TImplementation>(
			this IServiceCollection services,
			string name)
			where TService : class
			where TImplementation : class, TService
		{
			return services.AddNamed<TService, TImplementation>(ServiceLifetime.Scoped, name);
		}
	}
}