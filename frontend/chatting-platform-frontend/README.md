# Chatting Platform

This project is a chatting platform built with React and TypeScript. It provides a user-friendly interface for users to communicate in various chatrooms.

## Features

- **Header**: Displays the username of the user and the name of the chatting platform.
- **Sidebar**: Provides an overview of the chatrooms the user is a member of, allowing easy navigation between them.
- **Main Area**: Displays the messages and name of the currently selected chatroom.

## Project Structure

```
chatting-platform-frontend
├── public
│   ├── index.html          # Main HTML file
│   └── favicon.ico         # Favicon for the application
├── src
│   ├── components          # React components
│   │   ├── Chat.tsx       # Chat component for displaying messages
│   │   ├── Header.tsx     # Header component for user and platform name
│   │   ├── Sidebar.tsx    # Sidebar component for chatroom overview
│   │   └── MainArea.tsx   # Main area for displaying selected chatroom
│   ├── styles              # CSS styles for components
│   │   ├── Chat.module.css
│   │   ├── Header.module.css
│   │   ├── Sidebar.module.css
│   │   └── MainArea.module.css
│   ├── App.tsx            # Main application component
│   ├── index.tsx          # Entry point for the React application
│   └── types              # TypeScript types and interfaces
│       └── index.ts
├── package.json            # npm configuration file
├── tsconfig.json           # TypeScript configuration file
└── README.md               # Project documentation
```

## Installation

1. Clone the repository:
   ```
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```
   cd chatting-platform-frontend
   ```
3. Install the dependencies:
   ```
   npm install
   ```

## Usage

To start the development server, run:
```
npm start
```

Open your browser and go to `http://localhost:3000` to view the application.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or features you'd like to add.

## License

This project is licensed under the MIT License.