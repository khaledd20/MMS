namespace MMS.API.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public ICollection<Role> Roles { get; set; }

    }
}
