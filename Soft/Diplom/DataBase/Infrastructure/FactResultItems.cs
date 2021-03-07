using System.Collections.Generic;
using Nest;

namespace Diplom.DataBase.Infrastructure
{
    public class FactResultItems
    {
        public string NameList { get; set; }

        [Nested(IncludeInParent = true)]
        public ICollection<ResultFact> ResultData { get; set; } = new HashSet<ResultFact>();
    }
}
