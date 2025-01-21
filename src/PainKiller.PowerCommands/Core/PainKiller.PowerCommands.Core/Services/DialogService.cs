using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Services
{
    public static class DialogService
    {
        public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
        {
            WriteHeader($"\n{question}"); ;
            Console.WriteLine($"({yesValue}/{noValue}):");

            var response = Console.ReadLine();
            return $"{response}".Trim().ToLower() == yesValue.ToLower();
        }
        public static string QuestionAnswerDialog(string question, string prompt = "")
        {
            WriteHeader($"{question}\n");
            Console.Write(string.IsNullOrEmpty(prompt) ? ConfigurationGlobals.GetPrompt() : prompt);
            var response = Console.ReadLine();
            return $"{response}".Trim();
        }
        public static string SecretPromptDialog(string question, int maxRetries = 3)
        {
            var retryCount = 0;
            var secret = "";
            while (retryCount < maxRetries)
            {
                WriteHeader($"\n{question} ");
                secret = PasswordPromptService.Service.ReadPassword();
                Console.WriteLine();
                Console.Write("Confirm: ".PadLeft(question.Length + 1));
                var confirm = PasswordPromptService.Service.ReadPassword();
                if (secret != confirm)
                {
                    ConsoleService.Service.WriteCritical(nameof(DialogService), "\nConfirmation failure, please try again.\n");
                    retryCount++;
                }
                else break;
            }

            return $"{secret}".Trim();
        }
        private static void WriteHeader(string text)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
    }
}