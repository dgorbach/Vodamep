using Vodamep.Legacy.Model;

namespace Vodamep.Legacy.Reader
{
    public class ReadResult
    {
        public AdresseDTO[] A { get; set; }
        public PflegerDTO[] P { get; set; }
        public LeistungDTO[] L { get; set; }
        public VereinDTO V { get; set; }
    }
}