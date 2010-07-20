using System.Collections.Generic;
using NHibernate;

namespace Besnik.GenericRepository.NHibernate
{
	/// <summary>
	/// The class represents criteria specification result with 
	/// generic functionality for all specifications.
	/// </summary>
	public class CriteriaSpecificationResult<TEntity> : ISpecificationResult<TEntity> where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public CriteriaSpecificationResult(ICriteria criteria)
		{
			this.Criteria = criteria;
		}

		/// <summary>
		/// Gets criteria specification.
		/// </summary>
		protected ICriteria Criteria { get; private set; }

		/// <summary>
		/// Takes only given amount of entities.
		/// </summary>
		public ISpecificationResult<TEntity> Take(int count)
		{
			this.Criteria.SetMaxResults(count);
			return this;
		}

		/// <summary>
		/// Gets list of entities represented by the specification.
		/// </summary>
		public IList<TEntity> ToList()
		{
			return this.Criteria.List<TEntity>();
		}

		/// <summary>
		/// Gets single result represented by the specification.
		/// </summary>
		public TEntity Single()
		{
			return this.Criteria.UniqueResult<TEntity>();
		}
	}
}
