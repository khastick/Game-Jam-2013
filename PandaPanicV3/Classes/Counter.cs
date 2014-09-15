using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandaPanicV3
{
    public class Counter
    {
        int     current, limit;
        string  name;

        public string Name
        {
            get { return name; }
        }

        public int Current
        {
            get { return current; }
        }

        public int Limit
        {
            get { return limit; }
        }

        public Counter(int limit, string name = "")
        {
            this.limit = limit;
            this.name = name;
            current = 0;
        }

        public void reset()
        {
            current = 0;
        }

        public bool isReady()
        {
            current++;
            if (current % limit == 0)
            {
                current = 0;
                return true;
            }
            else return false;
        }
    }
}
