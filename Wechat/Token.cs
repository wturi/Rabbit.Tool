using Rabbit.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wechat
{
   public static   class Token
    {
        private static Model.RabbitMPEntities ModelContext = new Model.RabbitMPEntities();

       public static  string GetToken()
        {
            return ModelContext.Tokens.FirstOrDefault().TokenStr;
        }

    }
}
