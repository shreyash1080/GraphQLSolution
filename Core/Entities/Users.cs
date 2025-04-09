using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public  class Users
    {
        public int user_id { get; set; }

        public string email { get; set; } = null!;

        public string password_hash { get; set; } = null!;

        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

    }
}
