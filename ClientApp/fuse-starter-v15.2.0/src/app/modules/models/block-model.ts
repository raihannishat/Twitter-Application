export interface BlockResponse {
   isBlocked: boolean
}


export interface BlockedUsersViewModel {
   id: string,
   name: string,
   image: string,
   isCurrentUser: boolean,
   isFollowing: boolean,
   isBlocked: boolean
}