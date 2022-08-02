using PetaPoco;

namespace NetflixServer.Resources.Entities
{
    [TableName("USERS")]
    public class UserEntity
    {
        [Column(Name = "ID")]
        public long UserId { get; set; }

        [Column(Name = "EMAIL")]
        public string Email { get; set; }

        [Column(Name = "USER_NAME")]
        public string UserName { get; set; }

        [Column(Name = "ACTIVE")]
        public bool Active { get; set; }
    }
}
