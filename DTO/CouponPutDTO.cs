using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.DTO
{
    public class CouponPutDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
