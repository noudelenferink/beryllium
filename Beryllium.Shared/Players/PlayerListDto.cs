namespace Beryllium.Shared.Players
{
    public class PlayerListDto
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastNamePrefix { get; set; }

        public string FullName => $"{FirstName} {(!string.IsNullOrEmpty(LastNamePrefix) ? $"{LastNamePrefix} {LastName}" : LastName)}";
    }
}
