using Nest;

namespace Diplom.Infrastructure.Common
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class NormOriginData
    {
        public long Id { get; set; }
        public string A { get; set; }

        // ReSharper disable once InconsistentNaming
        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        //количество пар поездов
        public string E { get; set; }

        //количество отказов
        public string F { get; set; }

        //Продолжительность отказов 
        public string G { get; set; }

        ////кол-во отказов с задержкой поездов
        public string H { get; set; }

        ////кол-во задержанных поездов
        public string I { get; set; }

        //продолжительность задержки
        public string J { get; set; }

        //регламентное время восстановления
        public string K { get; set; }

        //кол-во предотказов
        public string L { get; set; }

        //кол-во отклонений от норм содержания
        public string M { get; set; }

        //кол-во отказов 3 кат и без категории
        public string N { get; set; }

        //продолжиетльность отказов 3 и без категории
        public string O { get; set; }

        public string P { get; set; }

        public string Q { get; set; }

        public string R { get; set; }
        public string S { get; set; }
        public string T { get; set; }
        public string V { get; set; }
    }
}
