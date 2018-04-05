using PowerArgs;

namespace Vodamep.Client
{

    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<VodamepProgram>(args);
            
            //var loc = new DisplayNameResolver();

            //ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            //var r = DataGenerator.Instance.CreateHkpvReport(100, 5, true);

            //using (var s = File.OpenWrite("test"))
            //{
            //    r.WriteTo(s);
            //}

            //var v = new HkpvReportValidator();
            //var rr = v.Validate(r);

            //var xxx = new HkpvReportValidationResultFormatter();
            //Console.WriteLine(xxx.Format(r, rr));
            //Console.Write(rr.IsValid.ToString());
        }
    }

}
