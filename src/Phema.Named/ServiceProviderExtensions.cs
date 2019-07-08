using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phema.Named
{
	public static class ServiceProviderExtensions
	{
		/// <summary>
		/// Get named service of type <typeparamref name="TService" />
		/// </summary>
		public static TService GetService<TService>(this IServiceProvider serviceProvider, string name)
			where TService : class
		{
			var options = serviceProvider.GetRequiredService<IOptions<NamedOptions>>().Value;

			return options.NamedDependencies.TryGetValue((name, typeof(TService)), out var type)
				? (TService) serviceProvider.GetService(type)
				: null;
		}

		/// <summary>
		/// Get required named service of type <typeparamref name="TService" />
		/// </summary>
		public static TService GetRequiredService<TService>(this IServiceProvider serviceProvider, string name)
			where TService : class
		{
			var options = serviceProvider.GetRequiredService<IOptions<NamedOptions>>().Value;

			return options.NamedDependencies.TryGetValue((name, typeof(TService)), out var type)
				? (TService) serviceProvider.GetRequiredService(type)
				: null;
		}
	}
}