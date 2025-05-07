import React, { useState, useEffect } from 'react';
import { Chatroom, Message, User } from '../types/index';
import styles from '../styles/Chat.module.css';
import * as signalR from '@microsoft/signalr';

interface ChatProps {
    selectedChatroom: Chatroom | null; // Chatroom can be null if no chatroom is selected
    hubConnection: signalR.HubConnection | null; // SignalR connection passed from App.tsx
    user: User; // User object to get the sender's name
}

const Chat: React.FC<ChatProps> = ({ selectedChatroom, hubConnection, user}) => {
    const [messages, setMessages] = useState<Message[]>([]);

    const fetchMessages = async (chatroomId: number) => {
        try {
            const response = await fetch(`http://localhost:5062/api/Message/chatroom/${chatroomId}`);
            if (!response.ok) {
                throw new Error('Failed to fetch messages');
            }
            const data: { $values: Message[]} = await response.json();
            const values: Message[] = data.$values;
            setMessages(values);
        } catch (error) {
            console.error('Error fetching messages:', error);
        }
    };

    const handleSendMessage = async (messageText: string) => {
        if (!selectedChatroom || !hubConnection) return;

        try {
            const response = await fetch(`http://localhost:5062/api/Message`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    chatroomId: selectedChatroom.id, // Send the chatroom ID
                    senderId: user.id, // The sender's username
                    messageText, // The message text
                }),
            });
            if (!response.ok) {
                throw new Error('Failed to send message');
            }
        } catch (error) {
            console.error('Error sending message:', error);
        }
    };

    useEffect(() => {
        if (selectedChatroom && hubConnection) {
            fetchMessages(selectedChatroom.id);

            hubConnection.invoke('JoinChatroom', selectedChatroom.id.toString());

            hubConnection.on('ReceiveMessage', (newMessage: Message) => {
                setMessages((prevMessages) => [...prevMessages, newMessage]);
            });

            hubConnection.onreconnected(() => {
                hubConnection.invoke('JoinChatroom', selectedChatroom.id.toString());
            });

            return () => {
                hubConnection.invoke('LeaveChatroom', selectedChatroom.id.toString());
                hubConnection.off('ReceiveMessage');
            };
        }
    }, [selectedChatroom, hubConnection]);

    return (
        <div className={styles.chat}>
            <div className={styles.chatName}>{selectedChatroom ? selectedChatroom.name : 'Select a chatroom'}</div>
            <div className={styles.messages}>
                {messages.map((message) => (
                    <div key={message.id} className={`${styles.message} ${message.sender.username === user.username ? styles.myMessage : ''}`}>
                        <span 
                            className={styles.username} 
                            style={{color: message.sender.color}}>
                            {message.sender.username}:
                        </span>
                        <span className={styles.text} style={{backgroundColor: message.sender.color}}>{message.messageText}</span>
                    </div>
                ))}
            </div>
            <div className={styles.inputContainer}>
                <input
                    type="text"
                    placeholder="Type a message..."
                    onKeyDown={(e) => {
                        if (e.key === 'Enter') {
                            handleSendMessage((e.target as HTMLInputElement).value);
                            (e.target as HTMLInputElement).value = '';
                        }
                    }}
                />
                <button
                    onClick={() => {
                        const input = document.querySelector(`.${styles.inputContainer} input`) as HTMLInputElement;
                        if (input) {
                            handleSendMessage(input.value);
                            input.value = '';
                        }
                    }}
                >
                    Send
                </button>
            </div>
        </div>
    );
};

export default Chat;