using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atmakur.Testing.Core.Utilities
{
    public class TextFixtureArgs : IEnumerable
    {
        private static List<string[]> FixtureArgs = null;

        public TextFixtureArgs()
        {
            FixtureArgs = new List<string[]>(AppConfig.BrowsersEnabled.Select(x => new string[] { x.Browser, x.Version, x.Platform }));
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var args in FixtureArgs)
            {
                yield return args;
            }
        }
    }
}
