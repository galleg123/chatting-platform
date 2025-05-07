export interface Chatroom {
    id: number;
    name: string;
    messages: Message[];
}

export interface Message {
    id: number;
    senderName: string;
    messageText: string;
    timestamp: Date;
    chatroomId: number;
}

export interface User {
    id: number;
    username: string;
    password: string;
    chatrooms: number[];
}