/* tslint:disable:max-line-length */
import { FuseNavigationItem } from '@fuse/components/navigation';

export const defaultNavigation: FuseNavigationItem[] = [

];
export const compactNavigation: FuseNavigationItem[] = [
    {
        id: 'feed',
        title: 'Home',
        type: 'basic',
        icon: 'heroicons_outline:home',
        link: '/feed'
    },
    {
        id: 'people',
        title: 'People',
        type: 'basic',
        icon: 'people_outline',
        link: '/people'
    },
    {
        id: 'notificationKey',  ///  uses it for notification badge
        title: 'Notifications',
        type: 'basic',
        icon: 'heroicons_outline:bell',
        link: '/notifications'
    },

    {
        id: 'profile',
        title: 'Profile',
        type: 'basic',
        icon: 'heroicons_outline:user',
        link: '/profile'
    },
];
export const futuristicNavigation: FuseNavigationItem[] = [

];
export const horizontalNavigation: FuseNavigationItem[] = [
    {
        id: 'feed',
        title: 'Home',
        type: 'basic',
        icon: 'heroicons_outline:home',
        link: '/feed'
    },
    {
        id: 'explore',
        title: 'Explore',
        type: 'basic',
        icon: 'heroicons_outline:hashtag',
        link: '/explore'
    },
    {
        id: 'notifications',
        title: 'Notifications',
        type: 'basic',
        icon: 'heroicons_outline:bell',
        link: '/notifications'
    },
    {
        id: 'profile',
        title: 'Profile',
        type: 'basic',
        icon: 'heroicons_outline:user',
        link: '/profile'
    }
];

