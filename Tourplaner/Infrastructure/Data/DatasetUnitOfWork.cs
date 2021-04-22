using System.Collections.Generic;

namespace Tourplaner.Infrastructure.Data
{
    public sealed class DatasetUnitOfWork<T>
    {
        public IEnumerable<T> RowsToInsert { get; set; }

        public IEnumerable<T> RowsToUpdate { get; set; }

        public IEnumerable<T> RowsToDelete { get; set; }
    }
}
