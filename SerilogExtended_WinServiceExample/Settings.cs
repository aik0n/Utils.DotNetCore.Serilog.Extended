namespace SerilogExtended_WinServiceExample
{
    public class Settings
    {
        public string SerilogDebugPath { get; set; }

        public string ApplicationInstanceGuid { get; set; }

        public int HostPort { get; set; }

        public int LogMessagesIntervalSeconds { get; set; }
    }
}