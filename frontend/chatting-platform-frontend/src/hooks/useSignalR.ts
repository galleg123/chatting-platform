import { useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import { User } from '../types/index';

export const useSignalR = (user: User | null) => {
    const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        if (user) {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl('http://localhost:5062/chathub')
                .withAutomaticReconnect()
                .build();

            connection.start()
                .then(() => console.log('SignalR connected'))
                .catch((err) => console.error('Error connecting to SignalR:', err));

            setHubConnection(connection);

            return () => {
                connection.stop();
            };
        }
    }, [user]);

    return hubConnection;
};