import { User } from "../types";

export const useUser = (User: User | null) => {

    const colorChange = async (color: string) => {
        if (!User) return; // Ensure User is not null
        try {
            const response = await fetch(`http://localhost:5062/api/User/color/${User.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ color }),
            });
            if (!response.ok) {
                throw new Error('Failed to update user color');
            }
        } catch (error) {
            console.error('Error updating user color:', error);
        }
    }

    return { colorChange };

}