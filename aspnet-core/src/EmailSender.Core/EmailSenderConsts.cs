using EmailSender.Debugging;

namespace EmailSender
{
    public class EmailSenderConsts
    {
        public const string LocalizationSourceName = "EmailSender";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "cbed9942e98d4ce0b4c93f61a4a285b3";
    }
}
