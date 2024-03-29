﻿using System;

namespace NetflixServer.Business.Domain
{
    public class Plan
    {
        public long PlanId { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
