using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vodamep.Hkpv.Model;
using Xunit;

namespace Vodamep.Tests
{
    public class TypeNameHelper
    {

        //[Fact]
        public void GetRD_xml()
        {
            var type = typeof(Tuple<IList<Person>, IEnumerable<string>>);

            var names = GetNamesToAddInRD_xml(type);
            var xml = GetXmlToAddInRD_xml(type);

            System.Diagnostics.Debug.Print(xml);

            Assert.Equal(type, Type.GetType(names.type));
        }

        public static (string assembly, string type) GetNamesToAddInRD_xml(Type type)
        {
            var pattern = new Regex(@", Version=.*?PublicKeyToken=.*?(?=(]|$))");

            var typeName = pattern.Replace(type.FullName, "");
            var assemblyName = pattern.Replace(type.Assembly.FullName, "");

            return (assemblyName, typeName);
        }


        public static string GetXmlToAddInRD_xml(Type type)
        {
            var names = GetNamesToAddInRD_xml(type);
            // eventuell auch mit Dynamic="Required All"
            var x = $@"<Assembly Name=""{names.assembly}"">
<Type Name=""{names.type}"" />
  </Assembly>";

            return x;
        }
    }
}
