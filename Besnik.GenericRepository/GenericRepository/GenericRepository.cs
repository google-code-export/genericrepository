using System;
using System.Collections.Generic;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic repository wraps given <see cref="IUnitOfWork"/> implementation
	/// and provides unified access to the entities stored in underlying data storage.
	/// </summary>
	/// <remarks>
	/// Additionally to <see cref="IUnitOfWork"/>, the repository supports 
	/// fluently initialized specifications. See also <see cref="Specify"/> method.
	/// 
	/// All commands have to be executed over started unit of work session.
	///
	/// Flushing of entities to underlying data storage is in competence of 
	/// given unit of work. In other words, synchronization between in-memory repository
	/// and data storage (e.g. database) is done via unit of work. This way the client
	/// has complete control over calling data storage and can optimize the way the entities
	/// are managed.
	/// </remarks>
	public class GenericRepository<TEntity, TPrimaryKey> : IGenericRepository<TEntity, TPrimaryKey>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public GenericRepository(
			IUnitOfWork unitOfWork
			, ISpecificationLocator specificationLocator
			)
		{
			this.EnsureNotNull(specificationLocator);
			this.EnsureNotNull(unitOfWork);

			this.SpecificationLocator = specificationLocator;
			this.UnitOfWork = unitOfWork;
		}

		/// <summary>
		/// Checks if given instance is not null. Use the method to validate input parameters.
		/// </summary>
		protected void EnsureNotNull(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o", "Argument can not be null.");
			}
		}

		/// <summary>
		/// Gets specification locator for the repository to resolve specifications.
		/// </summary>
		protected ISpecificationLocator SpecificationLocator { get; private set; }

		/// <summary>
		/// Gets unit of work the repository operates on.
		/// </summary>
		protected IUnitOfWork UnitOfWork { get; private set; }

		/// <summary>
		/// Inserts entity to the repository.
		/// </summary>
		public virtual void Insert(TEntity entity)
		{
			this.UnitOfWork.Insert<TEntity>(entity);
		}

		/// <summary>
		/// Updates entity in the repository.
		/// </summary>
		public virtual void Update(TEntity entity)
		{
			this.UnitOfWork.Update<TEntity>(entity);
		}

		/// <summary>
		/// Deletes entity from the repository.
		/// </summary>
		public virtual void Delete(TEntity entity)
		{
			this.UnitOfWork.Delete<TEntity>(entity);
		}

		/// <summary>
		/// Gets entity from the repository by given id.
		/// </summary>
		public virtual TEntity GetById(TPrimaryKey id)
		{
			return this.UnitOfWork.GetById<TEntity, TPrimaryKey>(id);
		}

		/// <summary>
		/// Gets all entities from the repository.
		/// </summary>
		public virtual IList<TEntity> GetAll()
		{
			return this.UnitOfWork.GetAll<TEntity>();
		}

		/// <summary>
		/// Gets specification that allows to filter only requested entities
		/// from the repository.
		/// </summary>
		public virtual TSpecification Specify<TSpecification>()
			where TSpecification : class, ISpecification<TEntity>
		{
			var specification = this.SpecificationLocator.Resolve<TSpecification, TEntity>();
			specification.Initialize(UnitOfWork);
			return specification;
		}
	}
}
