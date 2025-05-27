using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

class CyberSecurityChatBot
{
    private Dictionary<string, KeywordData> keywordResponses;
    private List<string> generalResponses;

    public CyberSecurityChatBot(string jsonFilePath)
    {
        LoadKeywords(jsonFilePath);
        LoadGeneralResponses();
    }

    static void Main()
    {
        var chatbot = new CyberSecurityChatBot("keywords.json"); // Load from the same directory

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
        ShowAsciiArt("CyberSec Helper", ConsoleColor.Cyan);
        Console.Write("\nEnter your name: ");
        string userName = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(userName)) userName = "Guest";

        ShowWelcomeMessage(userName);
        Thread.Sleep(1000);

        while (true)
        {
            PrintHeader("Chat Session", ConsoleColor.DarkYellow);
            Console.Write("\nAsk a question ('exit' to end): ");
            string input = Console.ReadLine().Trim().ToLower();

            if (input == "exit") break;

            // Add the loading animation here
            ShowLoadingAnimation();

            bool responseFound = false;
            foreach (var key in chatbot.keywordResponses.Keys)
            {
                if (input.Contains(key))
                {
                    PrintResponse(key, input, chatbot.keywordResponses[key]);
                    responseFound = true;
                    break;
                }
            }

            if (!responseFound)
            {
                // Respond with a random general response
                PrintGeneralResponse();
            }
        }
    }

    private void LoadKeywords(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            keywordResponses = JsonConvert.DeserializeObject<Dictionary<string, KeywordData>>(jsonData);
        }
        else
        {
            keywordResponses = new Dictionary<string, KeywordData>();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️  Keywords file not found.");
            Console.ResetColor();
        }
    }

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

    static void PrintHeader(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n{new string('═', 50)}");
        Console.WriteLine($"⚡ {text.ToUpper()} ⚡");
        Console.WriteLine($"{new string('═', 50)}");
        Console.ResetColor();
    }

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

    static void PrintResponse(string key, string question, KeywordData keywordData)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[{DateTime.Now:t}] You asked: {question}");
        Console.ResetColor();

        var response = keywordData.Responses[new Random().Next(keywordData.Responses.Count)];
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n🔒 {key.ToUpper()}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n{response}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));
    }

    private void PrintGeneralResponse()
    {
        var randomResponse = generalResponses[new Random().Next(generalResponses.Count)];
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n🔒 {randomResponse}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));
    }
}

public class KeywordData
{
    public List<string> Responses { get; set; }
    public List<string> FollowUps { get; set; }
}
