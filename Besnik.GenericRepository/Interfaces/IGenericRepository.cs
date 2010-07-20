using System.Collections.Generic;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic repository interface (DDD) for reading and writing domain entities to a storage.
	/// </summary>
	/// <typeparam name="TEntity">Domain entity.</typeparam>
	/// <typeparam name="TPrimaryKey">Type of the primary key.</typeparam>
	public interface IGenericRepository<TEntity, TPrimaryKey> where TEntity : class
	{
		/// <summary>
		/// Inserts entity to the storage.
		/// </summary>
		void Insert(TEntity entity);

		/// <summary>
		/// Updates entity in the storage.
		/// </summary>
		void Update(TEntity entity);

		/// <summary>
		/// Deletes entity in the storage.
		/// </summary>
		void Delete(TEntity entity);

		/// <summary>
		/// Gets entity from the storage by it's Id.
		/// </summary>
		TEntity GetById(TPrimaryKey id);

		/// <summary>
		/// Gets all entities of the type from the storage. 
		/// </summary>
		IList<TEntity> GetAll();

		/// <summary>
		/// Gets specification interface for complex searching for an entity or entities.
		/// </summary>
		TSpecification Specify<TSpecification>() where TSpecification : class, ISpecification<TEntity>;
	}
}
