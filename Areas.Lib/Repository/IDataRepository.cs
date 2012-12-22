// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataRepository.cs" company="Radmade">
//   2012
// </copyright>
// <summary>
//   The DataRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAreas.Lib.Repository
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IDataRepository
    {
        T Create<T>(T entity) where T : class;

        void Create<T>(IEnumerable<T> entities) where T : class;

        IQueryable<T> Read<T>() where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
    }
}