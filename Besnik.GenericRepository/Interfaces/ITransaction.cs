using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Represents transaction in the repository.
	/// </summary>
	public interface ITransaction : IDisposable
	{
		void Commit();
		void Rollback();
	}
}
