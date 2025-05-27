using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

class CyberSecurityChatBot
{
    private Dictionary<string, KeywordData> keywordResponses; // Stores keyword responses
    private List<string> generalResponses; // Stores general responses
    private UserProfile userProfile; // Stores user profile information
    private Dictionary<string, List<string>> sentimentResponses; // Stores responses based on sentiment
    private List<string> defaultResponses; // List for default responses

    // Constructor to initialize the chatbot with necessary data
    public CyberSecurityChatBot(string jsonFilePath, string userProfilePath)
    {
        LoadKeywords(jsonFilePath); // Load keyword responses from JSON file
        LoadGeneralResponses(); // Load general responses
        LoadUserProfile(userProfilePath); // Load user profile from JSON file
        LoadSentimentResponses(); // Load sentiment responses
        LoadDefaultResponses(); // Load default responses
    }

    static void Main()
    {
        // Create an instance of the chatbot
        var chatbot = new CyberSecurityChatBot("keywords.json", "userProfile.json"); // Load from the same directory

        // Play welcome sound
        try
        {
            using (var player = new SoundPlayer("welcome.wav"))
            {
                player.Play();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️  Could not play welcome sound: " + ex.Message);
            Console.ResetColor();
        }

        Console.Clear();
        ShowAsciiArt("CyberSec Helper", ConsoleColor.Cyan); // Display ASCII art

        // Ask for user name if not already stored
        if (string.IsNullOrEmpty(chatbot.userProfile.Name))
        {
            Console.Write("\nEnter your name: ");
            chatbot.userProfile.Name = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(chatbot.userProfile.Name)) chatbot.userProfile.Name = "Guest"; // Default to "Guest"
        }

        // Ask for favorite security topic if not already stored
        if (string.IsNullOrEmpty(chatbot.userProfile.FavoriteTopic))
        {
            Console.Write("\nWhat is your favorite security topic? ");
            chatbot.userProfile.FavoriteTopic = Console.ReadLine().Trim();
        }

        chatbot.SaveUserProfile("userProfile.json"); // Save user profile

        ShowWelcomeMessage(chatbot.userProfile.Name); // Display welcome message
        Thread.Sleep(1000); // Pause for a moment

        // Main chat loop
        while (true)
        {
            PrintHeader("Chat Session", ConsoleColor.DarkYellow); // Print session header
            Console.Write("\nAsk a question ('exit' to end): ");
            string input = Console.ReadLine().Trim().ToLower(); // Get user input

            if (input == "exit") break; // Exit the loop if user types 'exit'

            // Show loading animation while processing
            ShowLoadingAnimation();

            // Check for sentiment keywords
            if (chatbot.DetectSentiment(input, chatbot.sentimentResponses, out string sentimentResponse))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n🤗 {sentimentResponse}\n"); // Respond to sentiment
                Console.ResetColor();
            }

            // Check for keywords after sentiment detection
            bool responseFound = false; // Flag to check if a response was found
            foreach (var key in chatbot.keywordResponses.Keys)
            {
                // Check if input contains any keyword or its variants
                if (key.Split(',').Any(variant => input.Contains(variant.Trim())))
                {
                    PrintResponse(key, input, chatbot.keywordResponses[key]); // Print the response
                    responseFound = true; // Set flag to true
                    break; // Exit the loop
                }
            }

            if (!responseFound)
            {
                // If no keyword response was found, provide a default response
                chatbot.PrintDefaultResponse();
            }
        }
    }

    // Load keyword responses from a JSON file
    private void LoadKeywords(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            keywordResponses = JsonConvert.DeserializeObject<Dictionary<string, KeywordData>>(jsonData);
        }
        else
        {
            keywordResponses = new Dictionary<string, KeywordData>(); // Initialize empty dictionary if file not found
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️  Keywords file not found.");
            Console.ResetColor();
        }
    }

    // Load general responses
    private void LoadGeneralResponses()
    {
        generalResponses = new List<string>
        {
            "Did you know that strong passwords can significantly reduce the risk of hacking?",
            "Always be cautious when clicking on links in emails.",
            "Cybersecurity is everyone's responsibility!",
            "Regularly updating your software can help protect against vulnerabilities.",
            "Two-factor authentication adds an extra layer of security."
        };
    }

    // Load user profile from a JSON file
    private void LoadUserProfile(string userProfilePath)
    {
        if (File.Exists(userProfilePath))
        {
            var jsonData = File.ReadAllText(userProfilePath);
            userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonData);
        }
        else
        {
            userProfile = new UserProfile(); // Create a new profile if none exists
        }
    }

    // Save user profile to a JSON file
    private void SaveUserProfile(string userProfilePath)
    {
        var jsonData = JsonConvert.SerializeObject(userProfile, Formatting.Indented);
        File.WriteAllText(userProfilePath, jsonData);
    }

    // Load sentiment responses based on keywords
    private void LoadSentimentResponses()
    {
        sentimentResponses = new Dictionary<string, List<string>>
        {
            { "worried", new List<string> { "I'm here to help you with your concerns. What specifically worries you?", "It's okay to feel worried. Let's talk about it." } },
            { "curious", new List<string> { "Curiosity is great! What would you like to know more about?", "I'm glad you're curious! Ask away!" } },
            { "happy", new List<string> { "I'm glad to hear that! What made you happy?", "Happiness is important! Let's keep the good vibes going!" } },
            { "sad", new List<string> { "I'm sorry to hear that. Do you want to talk about what's bothering you?", "It's okay to feel sad sometimes. I'm here to listen." } }
        };
    }

    // Load default responses for unexpected inputs
    private void LoadDefaultResponses()
    {
        defaultResponses = new List<string>
        {
            "I'm not sure I understand. Could you please rephrase that?",
            "That's an interesting point! Can you tell me more?",
            "I’m here to help, but I didn’t quite catch that. What do you mean?",
            "Let’s try a different question. What else would you like to know?",
            "I’m not familiar with that topic. Can you ask something else?"
        };
    }

    // Detect sentiment in user input
    private bool DetectSentiment(string input, Dictionary<string, List<string>> sentimentResponses, out string response)
    {
        response = null; // Initialize response
        foreach (var sentiment in sentimentResponses.Keys)
        {
            if (input.Contains(sentiment)) // Check if input contains any sentiment keyword
            {
                response = sentimentResponses[sentiment][new Random().Next(sentimentResponses[sentiment].Count)]; // Get a random response for the detected sentiment
                return true; // Sentiment detected
            }
        }
        return false; // No sentiment detected
    }

    // Display ASCII art for the chatbot
    static void ShowAsciiArt(string title, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(@"
  ____     ____              _   _      _                 
 / ___|   / ___|  ___  ___  | | | | ___| |_ __   ___ _ __ 
| |   ____\___ \ / _ \/ __| | |_| |/ _ \ | '_ \ / _ \ '__|
| |__|_____|__) |  __/ (__  |  _  |  __/ | |_) |  __/ |   
 \____|   |____/ \___|\___| |_| |_|\___|_| .__/ \___|_|   
                                         |_|              
");
        Console.ResetColor();
    }

    // Display a welcome message to the user
    static void ShowWelcomeMessage(string name)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"
        ╔════════════════════════════╗
        ║                            ║
        ║  Welcome, {0,-10}       ║
        ║                            ║
        ╚════════════════════════════╝
", name);
        Console.ResetColor();
    }

    // Print a header for the chat session
    static void PrintHeader(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n{new string('═', 50)}");
        Console.WriteLine($"⚡ {text.ToUpper()} ⚡");
        Console.WriteLine($"{new string('═', 50)}");
        Console.ResetColor();
    }

    // Show a loading animation while processing input
    private static void ShowLoadingAnimation()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("\nC-Sec Helper is thinking");

        for (int i = 0; i < 3; i++) // Three sets of dots
        {
            for (int j = 0; j < 3; j++) // Three dots per set
            {
                Thread.Sleep(300); // 300ms between dots
                Console.Write(".");
                Console.Out.Flush(); // Force immediate display
            }
            Thread.Sleep(300);
            Console.Write("\b\b\b   \b\b\b"); // Backspace to remove dots
        }

        Console.WriteLine("..."); // Final dots
        Console.ResetColor();
    }

    // Print the response for a recognized keyword
    static void PrintResponse(string key, string question, KeywordData keywordData)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[{DateTime.Now:t}] You asked: {question}");
        Console.ResetColor();

        var response = keywordData.Responses[new Random().Next(keywordData.Responses.Count)]; // Get a random response for the keyword
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n🔒 {key.ToUpper()}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n{response}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));

        // Provide a follow-up question if available
        if (keywordData.FollowUps != null && keywordData.FollowUps.Count > 0)
        {
            var followUp = keywordData.FollowUps[new Random().Next(keywordData.FollowUps.Count)];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"🤔 {followUp}");
            Console.ResetColor();
        }
    }

    // Print a random general response
    private void PrintGeneralResponse()
    {
        var randomResponse = generalResponses[new Random().Next(generalResponses.Count)];
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n🔒 {randomResponse}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));
    }

    // Print a random default response for unexpected inputs
    private void PrintDefaultResponse()
    {
        var randomDefaultResponse = defaultResponses[new Random().Next(defaultResponses.Count)];
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n⚠️ {randomDefaultResponse}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));
    }
}

// Class to hold keyword data and responses
public class KeywordData
{
    public List<string> Responses { get; set; } // List of responses for the keyword
    public List<string> FollowUps { get; set; } // List of follow-up questions for the keyword
}

// Class to hold user profile information
public class UserProfile
{
    public string Name { get; set; } // User's name
    public string FavoriteTopic { get; set; } // User's favorite security topic
}