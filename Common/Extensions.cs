using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Text;

namespace WSCIEMP.Common
{
    public static class Extensions
    {
        public static string ToQueryString(this NameValueCollection c)
        {
            var s = new StringBuilder("?");
            c.AllKeys.ToList().ForEach(k => s.AppendFormat("{0}={1}&", k, c[k]));
            return s.ToString().TrimEnd("&".ToCharArray());
        }
    }
}