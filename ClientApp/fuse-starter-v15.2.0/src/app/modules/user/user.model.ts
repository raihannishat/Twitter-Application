export interface UserViewModel {
    id: string,
    name: string,
    email: string,
    image: string,
    coverImage: string,
    bio: string,
    location: string,
    verifiedAt: Date,
    followers: number,
    followings: number,
    isFollowing: boolean,
    isCurrentUserProfile: boolean,
    isBlockedByCurrentUser: boolean
}