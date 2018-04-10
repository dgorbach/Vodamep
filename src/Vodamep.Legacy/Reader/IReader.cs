using System;

namespace Vodamep.Legacy.Reader
{

    public interface IReader
    {
        ReadResult Read(int year, int month);
    }
}