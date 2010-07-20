using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Besnik.Domain;
using Besnik.GenericRepository;

namespace Besnik.GenericRepository.Tests
{
	/// <summary>
	/// Base class for customer repository integration tests.
	/// Derive and implement abstract functionality with concrete unit of work implementation.
	/// Do not forget decorate derived class with [TestFixture] attribute in order to execute
	/// the below generic tests.
	/// </summary>
	public abstract class GenericCustomerRepositoryFixture : BaseUnitOfWorkFixture
	{
		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_ExplicitFlush()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);

				unitOfWork.Flush();
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_NoFlush()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_NoFlushWithException()
		{
			// arrange
			var customer = this.Factory.GetCustomer();
			Assert.That(customer.Id, Is.EqualTo(0));

			// act
			try
			{
				using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
				{
					ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
					cr.Insert(customer);

					throw new Exception("mocked ex");
				}
			}
			catch
			{
				// assert
				using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
				{
					var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

					Assert.That(customerFromDb, Is.Not.Null);
					Assert.That(customerFromDb, Is.Not.SameAs(customer));
					Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
					Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
				}
			}

			
		}

		/// <summary>
		/// Template test method for adding customer into repository in a transaction.
		/// </summary>
		[Test]
		public void AddItemsToRepositoryInTransaction()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);

				using ( var transaction = unitOfWork.BeginTransaction() )
				{
					cr.Insert(customer);

					transaction.Commit();
				}
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository in a transaction that rolls back
		/// after an exception.
		/// </summary>
		[Test]
		public void AddItemsToRepositoryInTransaction_Rollback()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				try
				{
					ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);

					using ( var transaction = unitOfWork.BeginTransaction() )
					{
						cr.Insert(customer);
						throw new Exception("mocked ex");
					}
				}
				catch ( Exception ex )
				{
					// assert
					Assert.That(ex.Message, Is.EqualTo("mocked ex"));

					using ( var innerUnitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
					{
						var customerFromDb = this.GetCustomer(customer.Id, innerUnitOfWork);

						Assert.That(customerFromDb, Is.Null);
					}

					return;
				}
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void GetUsingSpecifiedAge()
		{
			// arrange
			var customer = this.Factory.GetCustomer();
			
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);
			}

			Customer c = null;

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				c = cr.Specify<ICustomerSpecification>()
					.WithAge(Factory.DefaultCustomerAge)
					.ToResult()
					.Single();
			}

			// assert
			Assert.That(c, Is.Not.Null);
			Assert.That(c, Is.Not.SameAs(customer));
			Assert.That(c.Name, Is.EqualTo(customer.Name));
			Assert.That(c.Age, Is.EqualTo(customer.Age));
		}

		/// <summary>
		/// Creates unit of work implementation specific repository.
		/// </summary>
		protected abstract ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork);

		/// <summary>
		/// Loads customer from given data storage.
		/// </summary>
		protected abstract Customer GetCustomer(int customerId, IUnitOfWork unitOfWork);
	}
}
