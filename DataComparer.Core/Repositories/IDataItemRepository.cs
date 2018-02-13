using DataComparer.Models.Entities;

namespace DataComparer.Core.Repositories
{
    /// <summary>
    ///  Repository that operates with the <see cref="DataItem"/> entity.
    /// </summary>
    /// <seealso cref="IRepository{DataItem}" />
    public interface IDataItemRepository : IRepository<DataItem>
    {
    }
}
