namespace FribergCarRentals.Core.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public required string UserId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Admin admin &&
                   UserId == admin.UserId;
        }
    }
}
