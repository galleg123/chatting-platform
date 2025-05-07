import React from 'react';
import styles from '../styles/Header.module.css';

interface HeaderProps {
  username: string;
  platformName: string;
}

const Header: React.FC<HeaderProps> = ({ username, platformName }) => {
  return (
    <header className={styles.header}>
      <h1>{platformName}</h1>
      <p>Logged in as: {username}</p>
    </header>
  );
};

export default Header;