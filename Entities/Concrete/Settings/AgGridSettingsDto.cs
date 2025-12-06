using Core.Entities.Abstract;

namespace Entities.Concrete.Settings
{
    public class AgGridSettingsDto : IDto
    {
        public int Id { get; set; } // Added Id (useful for updates)

        public int UserId { get; set; } // ✅ Changed string -> int

        public string Key { get; set; }
        public string Value { get; set; }
    }
}