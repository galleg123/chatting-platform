import { useState } from 'react';
import { Chatroom, User } from './types/index';
import Login from './components/Login';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import Chat from './components/Chat';
import { useChatrooms } from './hooks/useChatrooms';
import { useSignalR } from './hooks/useSignalR';
import './styles/App.css';

const App = () => {
    const [user, setUser] = useState<User | null>(null);
    const [selectedChatroom, setSelectedChatroom] = useState<Chatroom | null>(null);
    
    const {chatrooms, createChatroom, joinChatroom} = useChatrooms(user);
    const hubConnection = useSignalR(user);

    const handleSelectChatroom = (chatroom: Chatroom) => {
        setSelectedChatroom(chatroom);
    };

    const onLogin = (user: User) => {
        setUser(user);
    };


    if (!user) {
        return <Login onLogin={onLogin} />;
    }

    return (
        <div className="app-container">
            <header className="app-header">
                <Header username={user.username} platformName="Chatting Platform" />
            </header>
            <div className="app-body">
                <div className="sidebar">
                    <Sidebar
                        chatrooms={chatrooms}
                        onSelectChatroom={handleSelectChatroom}
                        onCreateChatroom={createChatroom}
                        onJoinChatroom={joinChatroom}
                    />
                </div>
                <div className="chat">
                    <Chat selectedChatroom={selectedChatroom} hubConnection={hubConnection} user={user} />
                </div>
            </div>
        </div>
    );
};

export default App;