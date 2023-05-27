export interface TweetViewModel {
    id: string;
    userId: string;
    userName: string;
    image: string;
    tweetCreatorId: string;
    tweetCreatorName: string;
    tweetCreatorImage: string;
    content: string;
    likes: number;
    retweets: number;
    createdAt: Date;
    edited: boolean;
    canDelete: boolean;
    comments: number;
    isLikedByCurrentUser: boolean;
    isRetweetedByCurrentUser: boolean;
    isRetweeted: boolean;
}

export interface Tweet {
    content: string;
}

export interface Commnet {
    tweetId: string;
    tweetOwnerId?: string;
    tweetCreatorId?: string;
    content: string;
}
