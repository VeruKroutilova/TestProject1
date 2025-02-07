namespace TestProject1
{
    public static class Logger
    {
        private static string logFilePath = "test_log.txt";  // Cesta k logovacimu souboru

        // Metoda pro zapis zpravy do logu
        public static void Log(string message)
        {
            try
            {
                // Ziskat aktualni cas
                string actualTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Format logu: [cas] zprava
                string logMessage = $"[{actualTime}] {message}"; // dolar neboli string.Format = funkce pro formátování stringu

                // Zapis do souboru (pokud soubor neexistuje, bude vytvoren)
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // V pripade chyby pri zapisovani do logu, logovat chybu
                Console.WriteLine("Chyba při zapisování do logu: " + ex.Message);
            }
        }

        // Metoda pro zapis logu o chybe
        public static void LogError(string message)
        {
            Log($"ERROR: {message}");
        }

        // Metoda pro zapis logu o uspechu
        public static void LogSuccess(string message)
        {
            Log($"SUCCESS: {message}");
        }

        // Metoda pro zápis varování
        public static void LogWarning(string message)
        {
            Log($"WARNING: {message}");
        }
    }
}
