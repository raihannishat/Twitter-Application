export interface FollowResponse {
   isFollowing: boolean,
   followers: number
}

export interface FollowersViewModel {
   id: string,
   name: string,
   image: string,
   isCurrentUser: boolean,
   isFollowing: boolean,
   isBlocked: boolean
}


export interface FollowingsViewModel {
   id: string,
   name: string,
   image: string,
   isCurrentUser: boolean,
   isFollowing: boolean,
   isBlocked: boolean
}
