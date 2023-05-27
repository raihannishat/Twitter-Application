using Application.Notifications.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.MongoDB.Repositories;
internal class NotificationRepository : MongoRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public async Task<IEnumerable<Notification>> GetNotifications(Expression<Func<Notification, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<Notification>.Filter.Where(filterExpression);

        var res = await _collection.Aggregate().Match(filter).SortByDescending(x =>
            x.Time).Skip((pageNumber - 1) * 5).Limit(5).ToListAsync();

        return res;
    }
}
