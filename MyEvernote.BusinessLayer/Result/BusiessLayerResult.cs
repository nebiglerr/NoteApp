using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer.Result
{
    public class BusiessLayerResult<T> where T : class
    {

        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusiessLayerResult()
        {
            // EĞER EROR NESNESINE BOŞ GELİRSE HATA FIRLATMAMASI İÇİN  CTOR AYAĞA KALDIRIRLIR
            Errors = new List<ErrorMessageObj>();
        }
        public void AddError(ErrorMessageCode code , string message)
        {
            Errors.Add(new ErrorMessageObj() { Code=code, Message= message });
        }
    }
}
