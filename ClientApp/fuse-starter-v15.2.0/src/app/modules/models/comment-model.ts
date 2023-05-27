export interface CreateComment {
   tweetId: string,
   content: string
   tweetCreatorId: string
}

export interface CommentViewModel {
   id: string,
   content: string,
   createdTime: Date,
   tweetId: string,
   userId: string,
   userName: string,
   image: string,
   canDelete: boolean
}

export interface CommentResponse {
   totalComments: number
}