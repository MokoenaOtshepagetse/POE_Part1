# C-Sec Helper: Cybersecurity Awareness Chatbot

![ASCII Logo Preview](https://i.imgur.com/ABCD123.png) *Example of the ASCII header*

A command-line chatbot designed to educate users about cybersecurity best practices through interactive conversations.

## Features

- 🎵 **Sound Welcome**: Plays a WAV file on startup
- 🎨 **ASCII Art Interface**: Cyber-themed text graphics
- 💬 **Interactive Q&A**: Answers common cybersecurity questions
- ⏳ **Loading Animation**: Simulated "thinking" response
- 🛡️ **Security Topics Covered**:
  - Password security
  - Phishing detection
  - Safe browsing practices
  - Malware protection
- 🎨 **Color-coded UI**: Enhanced command-line experience
- ❌ **Error Handling**: Graceful handling of unknown queries

## Installation

### Requirements
- .NET 6.0 SDK or later
- Windows OS (for sound functionality)
- Text-to-speech compatible audio device

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/MokoenaOtshepagetse/POE_Part1

Usage
Basic Commands:

Enter your name when prompted

Ask questions about:

Passwords

Phishing

Safe browsing

Malware

General cybersecurity

Type exit to end session

Example Questions:

"How do I create strong passwords?"

"What should I look for in phishing emails?"

"How can I browse safely online?"

"What's your purpose?"

"How do I protect against malware?"

Configuration
Sound Settings:

To disable sound: Remove welcome.wav file

Custom sound: Replace with any 16-bit PCM WAV file

Sound path: bin/Debug/net6.0/welcome.wav

Visual Customization:

Modify ShowAsciiArt() method for different logos

Adjust colors in Console.ForegroundColor calls

Edit animation timing in ShowLoadingAnimation()
