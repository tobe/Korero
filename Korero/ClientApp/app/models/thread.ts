import { Reply } from './reply'
import { Tag } from './tag'
import { User } from './user'

export class Thread {
    public id: number;
    public title: string;
    public dateCreated: Date;
    public views: number;
    public tag: Tag;
    public replies: Reply[];
    public author: User;

    public show: boolean = true; // Filtering purposes ;)
}

export class ThreadData {
    public total: number;
    public data: Thread[];
}