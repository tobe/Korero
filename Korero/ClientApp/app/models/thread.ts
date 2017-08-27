import { Reply } from './reply'
import { Tag } from './tag'

export class Thread {
    public id: number;
    public title: string;
    public dateCreated: Date;
    public tag: Tag[];
    public replies: Reply[];
}

export class ThreadData {
    public total: number;
    public data: Thread[];
}