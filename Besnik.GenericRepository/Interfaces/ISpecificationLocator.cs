using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// The interface resolves specifications.
	/// In concrete implementation this is wrapper over IoC container.
	/// </summary>
	public interface ISpecificationLocator
	{
		/// <summary>
		/// Gets specification.
		/// </summary>
		TSpecification Resolve<TSpecification, TEntity>()
			where TSpecification : ISpecification<TEntity>
			where TEntity : class;
	}
}
