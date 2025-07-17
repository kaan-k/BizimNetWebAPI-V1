using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BusinessUserDetailsDto : IDto
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string CompanyAddress { get; set; }
    }
}
