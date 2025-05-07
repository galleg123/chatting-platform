import React, { useState } from 'react';
import { Chatroom } from '../types/index';
import styles from '../styles/Sidebar.module.css';

interface SidebarProps {
    chatrooms: Chatroom[];
    onSelectChatroom: (chatroom: Chatroom) => void;
    onCreateChatroom: (name: string) => void; // Callback to create a new chatroom
    onJoinChatroom: (chatroomName: string) => void; // Callback to join a chatroom
}

const Sidebar: React.FC<SidebarProps> = ({ chatrooms, onSelectChatroom, onCreateChatroom, onJoinChatroom }) => {
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [isJoinModalOpen, setIsJoinModalOpen] = useState(false);
    const [newChatroomName, setNewChatroomName] = useState('');
    const [joinChatroomName, setJoinChatroomName] = useState('');

    const handleCreateChatroom = () => {
        if (newChatroomName.trim()) {
            onCreateChatroom(newChatroomName);
            setNewChatroomName('');
            setIsCreateModalOpen(false);
        }
    };

    const handleJoinChatroom = () => {
        if (joinChatroomName.trim()) {
            onJoinChatroom(joinChatroomName);
            setJoinChatroomName('');
            setIsJoinModalOpen(false);
        }
    };

    return (
        <div className={styles.sidebar}>
            <h2>Chatrooms</h2>
            <ul>
                {chatrooms.map((chatroom) => (
                    <li
                        key={chatroom.id}
                        onClick={() => onSelectChatroom(chatroom)}
                        className={styles.chatroomItem}
                    >
                        {chatroom.name}
                    </li>
                ))}
            </ul>
            <button className={styles.createButton} onClick={() => setIsCreateModalOpen(true)}>
                Create Chatroom
            </button>
            <button className={styles.joinButton} onClick={() => setIsJoinModalOpen(true)}>
                Join Chatroom
            </button>

            {/* Create Chatroom Modal */}
            {isCreateModalOpen && (
                <div className={styles.modal}>
                    <div className={styles.modalContent}>
                        <h3>Create Chatroom</h3>
                        <input
                            type="text"
                            value={newChatroomName}
                            onChange={(e) => setNewChatroomName(e.target.value)}
                            placeholder="Enter chatroom name"
                        />
                        <div className={styles.modalActions}>
                            <button onClick={handleCreateChatroom}>Create</button>
                            <button onClick={() => setIsCreateModalOpen(false)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}

            {/* Join Chatroom Modal */}
            {isJoinModalOpen && (
                <div className={styles.modal}>
                    <div className={styles.modalContent}>
                        <h3>Join Chatroom</h3>
                        <input
                            type="text"
                            value={joinChatroomName}
                            onChange={(e) => setJoinChatroomName(e.target.value)}
                            placeholder="Enter chatroom name"
                        />
                        <div className={styles.modalActions}>
                            <button onClick={handleJoinChatroom}>Join</button>
                            <button onClick={() => setIsJoinModalOpen(false)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Sidebar;