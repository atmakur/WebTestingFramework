using System;
using System.Collections.Generic;
using System.Linq;

namespace Atmakur.Testing.Core.Utilities
{
    public class TestCaseResult
    {
        private List<Tuple<string, object>> TestResults = null;

        public TestCaseResult()
        {
            TestResults = new List<Tuple<string, object>>();
        }

        public IEnumerator<Tuple<string, object>> GetEnumerator()
        {
            return TestResults.GetEnumerator();
        }

        public void Add(string key, object value)
        {
            TestResults.Add(Tuple.Create(key, value));
        }

        public void Add(string key, string value)
        {
            TestResults.Add(Tuple.Create(key, Convert.ChangeType(value, TypeCode.Object)));
        }

        public object Get(string key)
        {
            return TestResults.Where(x => x.Item1.Equals(key)).FirstOrDefault();
        }
    }
}
