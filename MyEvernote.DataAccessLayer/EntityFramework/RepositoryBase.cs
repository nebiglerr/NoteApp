using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
   public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockObj = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }
        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_lockObj) // lock multithread uygulamalar için  DatabaseContext oluşmasını bu değişkene aktarılmasını sağlamış oluyoruz . lock bir bağlantı işini bitirrmeden diğerine geçiş sağlamaz
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                    
                }
               
            }     
        }
    }
}
