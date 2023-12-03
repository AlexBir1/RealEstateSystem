export class ErrorModel{
    title: string = '';
    messages: string[] = [];

    constructor(title: string, messages: string[]){
        this.title = title;
        this.messages = messages;
    }
}