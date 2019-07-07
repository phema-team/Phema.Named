using System;
using System.Collections.Generic;

namespace Phema.Named
{
	internal class NamedOptions
	{
		public NamedOptions()
		{
			NamedDependencies = new Dictionary<(string, Type), Type>();
		}

		public IDictionary<(string, Type), Type> NamedDependencies { get; }
	}
}