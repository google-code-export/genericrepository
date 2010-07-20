using System.Collections.Generic;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic domain specification result interface that 
	/// contains methods common for filtering the result.
	/// </summary>
	/// <remarks>
	/// If the specification model uses lazy loading using <see cref="IQueryable<T>"/>, 
	/// this is the place where the query is translated and executed by calling for example
	/// <see cref="ToList"/> or <see cref="Single"/> methods.
	/// </remarks>
	public interface ISpecificationResult<TEntity> where TEntity : class
	{
		/// <summary>
		/// Takes given count of domain objects.
		/// </summary>
		ISpecificationResult<TEntity> Take(int count);

		/// <summary>
		/// Gets the list of domain objects the query represents.
		/// </summary>
		IList<TEntity> ToList();

		/// <summary>
		/// Gets single domain object the query represents.
		/// </summary>
		TEntity Single();
	}
}
