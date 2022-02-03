using MyEvernote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            DatabaseContext db = new DatabaseContext();
            db.Database.CreateIfNotExists();
        }
    }
}
