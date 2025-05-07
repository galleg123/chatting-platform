import React, { useState } from 'react';
import styles from '../styles/Header.module.css';
import { FaCog } from 'react-icons/fa'; // Import cogwheel icon from React Icons

interface HeaderProps {
  username: string;
  platformName: string;
  onColorChange: (color: string) => void; // Callback to handle color change
}

const Header: React.FC<HeaderProps> = ({ username, platformName, onColorChange }) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const handleColorChange = (color: string) => {
    onColorChange(color); // Notify parent component of the color change
    setIsMenuOpen(false); // Close the menu
  };

  
  return (
    <header className={styles.header}>
      <h1>{platformName}</h1>
      <div className={styles.userInfo}>
        <p>
          Logged in as: {username}
          <FaCog
            className={styles.cogIcon}
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          />
        </p>
        {isMenuOpen && (
          <div className={styles.colorMenu}>
            <p>Select a color:</p>
            <div className={styles.colorOptions}>
              <button
                className={styles.colorButton}
                style={{ backgroundColor: '#007bff' }}
                onClick={() => handleColorChange('#007bff')}
              />
              <button
                className={styles.colorButton}
                style={{ backgroundColor: '#ff5733' }}
                onClick={() => handleColorChange('#ff5733')}
              />
              <button
                className={styles.colorButton}
                style={{ backgroundColor: '#28a745' }}
                onClick={() => handleColorChange('#28a745')}
              />
              <button
                className={styles.colorButton}
                style={{ backgroundColor: '#ffc107' }}
                onClick={() => handleColorChange('#ffc107')}
              />
            </div>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;