export interface Chatroom {
    id: number;
    name: string;
    messages: Message[];
}

export interface Message {
    id: number;
    messageText: string;
    timestamp: Date;
    sender: UserResponse;
    chatroomId: number;
}

export interface UserResponse {
    username: string;
    color: string;
}    

export interface User {
    id: number;
    color: string;
    username: string;
    password: string;
    chatrooms: number[];
}