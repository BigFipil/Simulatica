using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public interface ILogger
    {
        public void Add(string text);
        public void Add(string text, object[] obj);
        //public void CreateLogg(string path);
    }
}
