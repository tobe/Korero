import { User } from './user';

export class Reply {
    public id: number;
    public dateCreated: Date;
    public dateUpdated: Date;
    public body: string;

    public author: User;
}
export class ReplyData {
    public total: number;
    public data: Reply[];
}
