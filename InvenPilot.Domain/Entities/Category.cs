    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace InvenPilot.Domain.Entities
    {
        public class Category
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public List<Product> products { get; set; } = new List<Product>();
        }
    }
