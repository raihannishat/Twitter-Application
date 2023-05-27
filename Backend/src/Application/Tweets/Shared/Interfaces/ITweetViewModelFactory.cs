using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tweets.Shared.Interfaces;
public interface ITweetViewModelFactory
{
    Task<TweetViewModel> CreateTweetViewModelAsync(Tweet tweet, User user);
}
