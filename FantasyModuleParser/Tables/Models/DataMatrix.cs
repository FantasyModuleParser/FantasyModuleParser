
using System.Collections;
using System.Collections.Generic;

namespace FantasyModuleParser.Tables.Models
{
    public class DataMatrix : IEnumerable
    {
        public List<string> Columns { get; set; }
        public List<object[]> Rows { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GenericEnumerator(Rows.ToArray());
        }
    }
}
