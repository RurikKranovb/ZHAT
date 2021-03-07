using System.Collections.Generic;
using Nest;

namespace Diplom.DataBase.Infrastructure
{
    [ElasticsearchType(IdProperty = "id")]
    public class NormResultItems
    {
        public string NameList { get; set; }

        [Nested(IncludeInParent = true)]
        public ICollection<ResultNorm> ResultData { get; set; } = new HashSet<ResultNorm>();

    }
}
