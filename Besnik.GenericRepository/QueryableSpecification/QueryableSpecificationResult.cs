using System.Collections.Generic;
using System.Linq;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Specification result class contains common functionality for filtering result.
	/// </summary>
	/// <typeparam name="TEntity">Domain entity.</typeparam>
	public class QueryableSpecificationResult<TEntity> : ISpecificationResult<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public QueryableSpecificationResult(IQueryable<TEntity> queryable)
		{
			this.Queryable = queryable;
		}

		/// <summary>
		/// Gets or sets IQueryable interface for domain entity.
		/// </summary>
		protected IQueryable<TEntity> Queryable { get; set; }

		/// <summary>
		/// Takes given count of the records represented by the specification.
		/// </summary>
		public ISpecificationResult<TEntity> Take(int count)
		{
			this.Queryable = this.Queryable.Take(count);
			return this;
		}

		/// <summary>
		/// Executes the specification and query behind it and returns list of records
		/// that matches criteria.
		/// </summary>
		public IList<TEntity> ToList()
		{
			return Queryable.ToList();
		}

		/// <summary>
		/// Executes the specification and query behind it and returns the only record
		/// of a sequence. Throws if there is not exactly one element in the sequence.
		/// </summary>
		public TEntity Single()
		{
			return Queryable.Single();
		}
	}
}
