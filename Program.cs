using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;

class CyberSecurityChatBot
{
    private static Dictionary<string, (string Title, string Response)> responses = new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
    {
        {"hello", ("Greeting", "Welcome to Cybersecurity Awareness! How can I help you today?")},
        {"how are you", ("Status", "I'm a chatbot, so I don't have feelings, but I'm fully operational and ready to assist with cybersecurity matters!")},
        {"purpose", ("Mission", "My purpose is to educate users about:\n- Password security\n- Phishing detection\n- Safe browsing practices\n- Device protection\n- Network security")},
        {"password", ("Password Security", "🔑 Create strong passwords:\n- Minimum 12 characters\n- Mix letters, numbers & symbols\n- Avoid personal information\n- Use a password manager")},
        {"phishing", ("Phishing Alert", "🚨 Watch for:\n- Urgent or threatening language\n- Misspellings in email addresses\n- Suspicious attachments\n- Requests for sensitive info\nWhen in doubt, verify directly!")},
        {"browsing", ("Safe Browsing", "🌐 Safety tips:\n- Look for HTTPS in URLs\n- Use ad-blockers\n- Keep browsers updated\n- Avoid public WiFi for sensitive tasks\n- Use VPN for extra protection")},
        {"malware", ("Malware Defense", "🛡️ Protection strategies:\n- Install reputable antivirus\n- Regular system updates\n- Don't open unknown attachments\n- Backup data regularly\n- Enable firewall protection")},
        {"bye", ("Exit", "Stay secure! Always verify links and think before you click. Goodbye!")}
    };

    static void Main()
    {
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
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                {
                    PrintResponse(key, input);
                    responseFound = true;
                    break;
                }
            }

            if (!responseFound)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n⚠️  I'm not sure about that. Try asking about:");
                Console.WriteLine("• Passwords • Phishing • Safe browsing • Malware");
                Console.ResetColor();
            }
        }
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

    static void PrintResponse(string key, string question)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[{DateTime.Now:t}] You asked: {question}");
        Console.ResetColor();

        var response = responses[key];
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n🔒 {response.Title.ToUpper()}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n{response.Response}\n");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));
    }
}