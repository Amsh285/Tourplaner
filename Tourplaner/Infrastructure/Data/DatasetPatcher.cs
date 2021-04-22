using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tourplaner.Infrastructure.Data
{
    public static class DatasetPatcher
    {
        public static DatasetUnitOfWork<TRow> PatchRows<TRow, TIdentifier>(IEnumerable<TRow> newState, IEnumerable<TRow> oldState)
            where TIdentifier : IEquatable<TIdentifier>
            where TRow : IIdentity<TIdentifier>
        {
            IEnumerable<TRow> rowsToInsert = newState
                .Where(n => !oldState.Contains(n, new IdentityEqualityComparer<TRow, TIdentifier>()));

            IEnumerable<TRow> rowsToUpdate = newState
                .Where(n => oldState.Contains(n, new IdentityEqualityComparer<TRow, TIdentifier>()));

            IEnumerable<TRow> rowsToDelete = oldState
                .Where(o => !newState.Contains(o, new IdentityEqualityComparer<TRow, TIdentifier>()));

            return new DatasetUnitOfWork<TRow>()
            {
                RowsToInsert = rowsToInsert,
                RowsToUpdate = rowsToUpdate,
                RowsToDelete = rowsToDelete
            };
        }
    }
}
