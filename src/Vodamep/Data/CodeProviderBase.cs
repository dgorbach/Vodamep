using System.Collections.Generic;
using System.IO;

namespace Vodamep.Data
{

    public abstract class CodeProviderBase
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();
        protected CodeProviderBase()
        {
            this.Init();
        }

        public bool IsValid(string code) => _dict.ContainsKey(code ?? string.Empty);

        private void Init()
        {
            var assembly = this.GetType().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{this.ResourceName}");

            using (var reader = new StreamReader(resourceStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var xx = line.Split(';');
                    _dict.Add(xx[0], xx[1]);
                }
            }
        }

        protected abstract string ResourceName { get; }

        public static CodeProviderBase GetInstance<T>()
            where T : CodeProviderBase
        {
            return (CodeProviderBase)typeof(T).GetProperty("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
        }
    }
}
