export class UserHelper{
    constructor() {
    }

    public static getUserId(){
        return localStorage.getItem('userId');
    }
}