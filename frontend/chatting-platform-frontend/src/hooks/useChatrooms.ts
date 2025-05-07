import { useState, useEffect } from 'react';
import { Chatroom, User } from '../types/index';

export const useChatrooms = (user: User | null) => {
    const [chatrooms, setChatrooms] = useState<Chatroom[]>([]);

    const fetchChatrooms = async () => {
        if (!user) return;
        try {
            const response = await fetch(`http://localhost:5062/api/Chat/user/${user.id}`);
            if (!response.ok) {
                throw new Error('Failed to fetch chatrooms');
            }
            const data: { $values: Chatroom[] } = await response.json();
            setChatrooms(data.$values);
        } catch (error) {
            console.error('Error fetching chatrooms:', error);
        }
    };

    const createChatroom = async (name: string) => {
        if (!user) return;
        try {
            const response = await fetch('http://localhost:5062/api/Chat', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ chat: { name }, userId: user.id }),
            });
            if (!response.ok) {
                throw new Error('Failed to create chatroom');
            }
            const newChatroom: Chatroom = await response.json();
            setChatrooms((prev) => [...prev, newChatroom]);
        } catch (error) {
            console.error('Error creating chatroom:', error);
        }
    }

    const joinChatroom = async (chatroomName: string) => {
        if(!user) return;
        try {
            const response = await fetch(`http://localhost:5062/api/Chat/join`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userId: user.id, name: chatroomName }),
            });
            if (!response.ok) {
                throw new Error('Failed to join chatroom');
            }
            const chatroom: Chatroom = await response.json();
            setChatrooms((prev) => [...prev, chatroom]);
        } catch (error) {
            console.error('Error joining chatroom:', error);
        }
    };

    useEffect(() => {
        fetchChatrooms();
    }, [user]);

    return { chatrooms, createChatroom, joinChatroom };

}