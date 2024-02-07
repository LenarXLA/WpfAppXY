using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace WpfAppXY.Model
{
    public struct Dot
    {
        [Index(0)]
        public int DotX
        {
            get;
            set;
        }
        
        [Index(1)]
        public int DotY
        {
            get;
            set;
        }
    }
}