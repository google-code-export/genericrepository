namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic base interface for the domain specifications (domain oriented queries).
	/// </summary>
	/// <remarks>
	/// Specification pattern belongs to the domain patterns family. It is used in
	/// Domain Driven Design (DDD) world. Name of the methods of derivate interfaces
	/// are matching the domain and it's context. It is domain oriented.
	///
	/// In comparison the Query pattern belogs to the (generic) design patterns family. 
	/// The pattern is focused more on generic and technical side of the problem.
	/// Example is NHibernate and it's criterion query objects pattern implementation.
	/// </remarks>
	public interface ISpecification<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Initializes specification from given unit of work instance.
		/// </summary>
		void Initialize(IUnitOfWork unitOfWork);

		/// <summary>
		/// Gets specification result wrapped into <see cref="ISpecificationResult"/> interface.
		/// </summary>
		ISpecificationResult<TEntity> ToResult();
	}
}
