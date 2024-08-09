import { useState } from 'react';
import './App.css';
import { MainContainer, MessageContainer, MessageHeader, MessageInput, MessageList, MinChatUiProvider } from '@minchat/react-chat-ui';

type CustomMessage = {
  id: string;
  text: string;
  senderName: string;
  timestamp: string;
}

function App() {
  const [messages, setMessages] = useState<CustomMessage[]>([]);

  const addMessage = (msg: string) => {
    // Add user message to state
    var currentMessages1 = [...messages];
    const message: CustomMessage = {
        id: (messages.length + 1).toString(),
        text: msg,
        senderName: 'User',
        timestamp: new Date().toISOString()
    };
    currentMessages1.push(message);
    setMessages(currentMessages1);

    // Fetch response from model and update state
    fetch(`https://localhost:7292/chat?msg=${msg}`)
      .then(response => response.text())
      .then(data => {
        const assistantMessage: CustomMessage = {
          id: (messages.length + 1).toString(),
          text: data,
          senderName: 'Assistant',
          timestamp: new Date().toISOString()
        };
        var currentMessages2 = [...currentMessages1];
        currentMessages2.push(assistantMessage);
        setMessages(currentMessages2);       
      });
  }

  return (
    <div className="App">
      <header className="App-header">
        <MinChatUiProvider theme="#6ea9d7">
          <MainContainer style={{ height: '60vh', width: '120vh' }}>
              <MessageContainer>
              <MessageHeader />
              <MessageList
                  currentUserId='User'
                  messages={messages.map(m => (
                      {
                          id: m.id,
                          text: m.text,
                          senderName: m.senderName,
                          timestamp: m.timestamp,
                          user: { id: m.senderName, name: m.senderName },
                          createdAt: new Date(m.timestamp)
                      }
                  ))}
              />
              <MessageInput onSendMessage={addMessage} showAttachButton={false} placeholder="Type message here" showSendButton />
              </MessageContainer>
          </MainContainer>
        </MinChatUiProvider>
      </header>
    </div>
  );
}

export default App;
