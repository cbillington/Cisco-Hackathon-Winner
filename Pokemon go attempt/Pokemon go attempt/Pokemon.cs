using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_go_attempt
{
    public class Pokemon
    {
        public List<Result> results { get; set; }//gives back an array but we want to use a list instead

        public string next {get;set;}

    }
        public class Result
        {
            public string Url { get; set; }

            public string Name { get; set; }

        }


    }

