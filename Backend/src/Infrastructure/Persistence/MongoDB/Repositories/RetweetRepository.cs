using Application.Retweets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.MongoDB.Repositories;
public class RetweetRepository : MongoRepository<Retweet>, IRetweetRepository
{
    public RetweetRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }
}
