using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GAPinstance
    {
        public string name;
        public int numCli;
        public int numServ;
        public int[,] cost;
        public int[,] req;
        public int[] cap;

        public int[] sol;
        public int n; //n client
        public int m; //m sever
    }
}