namespace MMS.API.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<Permission> Permissions { get; set; }

    }
}
