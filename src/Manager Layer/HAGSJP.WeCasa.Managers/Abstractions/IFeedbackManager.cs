using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Managers.Abstractions
{
    internal interface IFeedbackManager
    {
        public Result storeFeedbackTicket(Feedback feedback);
    }
}
