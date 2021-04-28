using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class Filter4Get
    {
        public DateTime time1
        {
            get;
            set;
        }
        public DateTime time2
        {
            get;
            set;
        }

        public int Skip
        {
            get;
            set;
        } = 0;
        public int Take
        {
            get;
            set;
        } = Int32.MaxValue;
    }
}
