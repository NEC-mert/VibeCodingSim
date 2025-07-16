using UnityEngine;

namespace NEC.GameModule.Player.Assistant
{
    public static class AssistantLines
    {
        public static string[] Greetings =
        {
            "Hello, how can I assist you today?",
            "Greetings, what do you need help with?",
            "Hi there! Ready to help you out.",
            "Hey! What can I do for you?",
            "Welcome back! How can I assist you?",
            "What do you need help with?",
            "How can I assist you today?",
            "Is there something specific you need help with?",
            "Let me know how I can help you.",
            "What can I do for you right now?"
        };

        public static string RandomGreeting()
        {
            return Greetings[Random.Range(0, Greetings.Length)];
        }
        
        public static string[] Farewells =
        {
            "Goodbye! Have a great day!",
            "See you later! Take care!",
            "Farewell! Until next time.",
            "Bye! Don't hesitate to return if you need help.",
            "Catch you later! Stay safe!"
        };
        
        public static string RandomFarewell()
        {
            return Farewells[Random.Range(0, Farewells.Length)];
        }
    }
}