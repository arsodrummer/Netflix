using PetaPoco;
using System;

namespace NetflixServer.Resources.Entities
{
    [TableName("PLANS")]
    public class PlanEntity
    {
        [Column(Name = "ID")]
        public long PlanId { get; set; }

        [Column(Name = "PRICE")]
        public decimal Price { get; set; }

        [Column(Name = "NAME")]
        public string Name { get; set; }

        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }

        [Column(Name = "EXPIRATION_DATE")]
        public DateTime? ExpirationDate { get; set; }
    }
}
