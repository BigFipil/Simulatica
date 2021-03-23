using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public interface ILogger
    {
        public void Add(string text);
        public void Add(string text, string path);
        public void Add(string text, object[] obj);

        public string loggerPath { get; set; }
        public string DefaultPath { get; }
        //public void CreateLogg(string path);
    }
}
