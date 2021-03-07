using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Diplom.DataBase;
using Diplom.DataBase.Infrastructure;
using Diplom.Helper;
using Diplom.Infrastructure.Common;
using Diplom.Infrastructure.Norm;
using Diplom.Infrastructure.Table.Norms;
using FluentNest;
using Nest;

namespace Diplom.Command.Import.OldImport
{
    public class ImportNormCommand : ValidatableBindableBase
    {
        private const int SettingsRas4et = 12; //todo:ÛÍ‡Á˚‚‡ÂÚ ÔÓÎ¸ÁÓ‚‡ÚÂÎ¸?

        public List<Triple<string, string, double>> AverageTrainTimeStruct;
        public List<Triple<string, string, double>> OffsetTrainTimeStruct;

        private new List<Triple<string, string, double>> AverageNumberFailuresStruct;
        private new List<Triple<string, string, double>> OffsetNumberFailuresStruct;

        //public List<Triple<string, string, Popravo4Nble—oefficient>> Popravo4Nble—oefficientStruct;

        private List<DataYear> _dataYears;
        private List<OriginDataNorm> _originDataNorms;
        private List<OriginDataPerevodEdinic> _originDataPerevodEdinic;
        private List<NormsP4Objects> _normsP4Objectses;
        private List<NormTimeYstrOtk> _normTimeYstrOtks;
        private List<Popravo4Nble—oefficient> _correctionFactors;
        private List<Popravo4Nble—oefficientObjects> _popravo4Nble—oefficientsObject;
        private List<FactIntensity> _factIntensities;
        private List<IntensityOtkIter> _intensityOtkIters;
        private List<double> _valueIntensityOtk;
        private List<IntensityIncident> _intensityIncidents;
        private List<KoefGot> _coefGot;
        private List<DataSvodTableVRVos> _svodTableVrVoses;
        private List<NormSRVRVosst> _normSrvrVossts;
        private List<OriginFrequencyObject> _originFrequencyObjects;
        private List<NormsFrequencyObject> _normsFrequencyObjects;
        private List<ResultNorm> _resultNorms;


        public ImportNormCommand()
        {

            AverageTrainTimeStruct = new List<Triple<string, string, double>>();
            OffsetTrainTimeStruct = new List<Triple<string, string, double>>();

            AverageNumberFailuresStruct = new List<Triple<string, string, double>>();
            OffsetNumberFailuresStruct = new List<Triple<string, string, double>>();
            //  Popravo4Nble—oefficientStruct = new List<Triple<string, string, Popravo4Nble—oefficient>>();

            #region Table

            _dataYears = new List<DataYear>();
            _originDataNorms = new List<OriginDataNorm>();
            _originDataPerevodEdinic = new List<OriginDataPerevodEdinic>();
            _normsP4Objectses = new List<NormsP4Objects>();
            _normTimeYstrOtks = new List<NormTimeYstrOtk>();
            _correctionFactors = new List<Popravo4Nble—oefficient>();
            _popravo4Nble—oefficientsObject = new List<Popravo4Nble—oefficientObjects>();
            _factIntensities = new List<FactIntensity>();
            _valueIntensityOtk = new List<double>();
            _intensityIncidents = new List<IntensityIncident>();
            _coefGot = new List<KoefGot>();
            _svodTableVrVoses = new List<DataSvodTableVRVos>();
            _normSrvrVossts = new List<NormSRVRVosst>();
            _originFrequencyObjects = new List<OriginFrequencyObject>();
            _normsFrequencyObjects = new List<NormsFrequencyObject>();
            _resultNorms = new List<ResultNorm>();
            _intensityOtkIters = new List<IntensityOtkIter>();

            #endregion


            PopravData();
        }

        private void PopravData()
        {

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 1,
                B = 1,
                C = 1,
                D = 0.96,
                E = 0.94
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 2,
                B = 2,
                C = 2,
                D = 0.92,
                E = 0.84
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 3,
                B = 11,
                C = 3,
                D = 0.87,
                E = 0.8
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 4,
                B = 4,
                C = 10,
                D = 0.85,
                E = 0.79
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 5,
                B = 1,
                C = 9,
                D = 0.97,
                E = 0.93
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 6,
                B = 10,
                C = 21,
                D = 0.93,
                E = 0.89
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 7,
                B = 22,
                C = 29,
                D = 0.87,
                E = 0.85
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 8,
                B = 30,
                C = 39,
                D = 0.85,
                E = 0.82
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 9,
                B = 40,
                C = 49,
                D = 0.83,
                E = 0.81
            });

            _correctionFactors.Add(new Popravo4Nble—oefficient()
            {
                A = 10,
                B = 50,
                C = 100,
                D = 0.78,
                E = 0.8
            });

            _valueIntensityOtk.Add(0.00001);
            _valueIntensityOtk.Add(0.00005);
            _valueIntensityOtk.Add(0.0001);
            _valueIntensityOtk.Add(0.0005);
            _valueIntensityOtk.Add(0.001);
            _valueIntensityOtk.Add(0.005);
            _valueIntensityOtk.Add(0.01);
            _valueIntensityOtk.Add(0.05);

        }


        //public Dictionary<string, string>

        public void SetNormDataBase(ImportNormDataItems normOriginImport)
        {
            #region Table

            _dataYears = new List<DataYear>();
            _originDataNorms = new List<OriginDataNorm>();
            _originDataPerevodEdinic = new List<OriginDataPerevodEdinic>();
            _normsP4Objectses = new List<NormsP4Objects>();
            _normTimeYstrOtks = new List<NormTimeYstrOtk>();
            _popravo4Nble—oefficientsObject = new List<Popravo4Nble—oefficientObjects>();
            _factIntensities = new List<FactIntensity>();
            _intensityOtkIters = new List<IntensityOtkIter>();
            _coefGot = new List<KoefGot>();

            #endregion

     
            // Popravo4Nble—oefficientStruct = new List<Triple<string, string, Popravo4Nble—oefficient>>();

            AverageTrainTimeStruct = new List<Triple<string, string, double>>();
            OffsetTrainTimeStruct = new List<Triple<string, string, double>>();

            AverageNumberFailuresStruct = new List<Triple<string, string, double>>();
            OffsetNumberFailuresStruct = new List<Triple<string, string, double>>();

            try
            {
                var client = ElasticSearchDataModel.Instance.Settings;
                if (client == null) return;

                var search = client
                    .Search<NormResultItems>(s => s
                        .Skip(0).Take(0)
                        .FilterOn(host => host.NameList == normOriginImport.NameList.ToLower())
                        .Aggregations(a => a
                            .SortedTopHits(1, x => x.NameList, SortType.Ascending)
                            .GroupBy(selectionList => selectionList.NameList)));

                var lists = search.Aggs.Aggregations.Count == 0
                    ? null
                    : search.Aggs
                        .GetGroupBy<NormResultItems>(selectionList => selectionList.NameList)
                        .SelectMany(bucket =>
                            bucket.GetSortedTopHits<NormResultItems>(selectionList => selectionList.NameList,
                                SortType.Ascending))
                        .FirstOrDefault();

                if (lists == null)
                {

                    var hits = client.Search<NormResultItems>(s => s)
                        .Hits;

                    foreach (var hit in hits)
                    {
                        if (String.Equals(normOriginImport.NameList, hit.Source.NameList,
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            var delete = client.Delete(new DocumentPath<NormResultItems>(hit.Id));
                        }
                    }

                    AddNormItems(client, normOriginImport);
                }
                else
                {
                    AddNormItems(client, normOriginImport);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

        private void AddNormItems(ElasticClient client, ImportNormDataItems normOriginImport)
        {
            foreach (var item in normOriginImport.NormOriginImportItems)
            {
                _originDataNorms.Add(new OriginDataNorm()
                {
                    A = string.IsNullOrEmpty(item.A) ? "0" : item.A,
                    //B = string.IsNullOrEmpty(item.B) ? "0" : item.B,
                    C = string.IsNullOrEmpty(item.C) ? "0" : item.C,
                    D = string.IsNullOrEmpty(item.D) ? "0" : item.D,
                    E = string.IsNullOrEmpty(item.E) ? "0" : item.E,
                    //F = string.IsNullOrEmpty(item.F) ? "0" : item.F,
                    //G = string.IsNullOrEmpty(item.G) ? "0" : item.G,
                    H = string.IsNullOrEmpty(item.F) ? "0" : item.F, 
                    //I = string.IsNullOrEmpty(item.I) ? "0" : item.I,
                    J = string.IsNullOrEmpty(item.H) ? "0" : item.H,
                    K = string.IsNullOrEmpty(item.I) ? "0" : item.I,
                    Q = string.IsNullOrEmpty(item.G) ? "0" : item.G,
                    //L = string.IsNullOrEmpty(item.J) ? "0" : item.J,
                    //M = string.IsNullOrEmpty(item.K) ? "0" : item.K,
                    N = 3,
                    O = 5,
                    P = 5
                    //R = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //S = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //T = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //U = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                });
            }

            foreach (var originDataNorm in _originDataNorms)
            {
                IcxDanniePerevodEdinic(originDataNorm.A,  originDataNorm.C, originDataNorm.D,
                    originDataNorm.E, originDataNorm.H, originDataNorm.J,
                    originDataNorm.K, originDataNorm.N, originDataNorm.O,
                    originDataNorm.P, originDataNorm.Q);

                PopravCoeficientObject(originDataNorm);
            }

            var classLine = _originDataPerevodEdinic.Select(dannie => dannie.C).Distinct().OrderBy(s => s).ToList();
            var specialization =
                _originDataPerevodEdinic.Select(dannie => dannie.D).Distinct().OrderBy(s => s).ToList();

            foreach (var tem in classLine)
            {
                var special =
                    _originDataPerevodEdinic.Where(dannie => dannie.C == tem).ToList() /*.Where(dannie => dannie.D)*/;
                foreach (var sp in specialization)
                {
                    var originDataPerevodEdinics = special.Where(dannie => dannie.D == sp).ToList();
                    if (originDataPerevodEdinics.Count == 0) continue;
                    {
                        var sr = originDataPerevodEdinics.Select(dannie => dannie.W).Average();

                        double sumOfSquaresOfDifferences =
                            originDataPerevodEdinics.Select(val => (val.W - sr) * (val.W - sr)).Sum();
                        double sd = Math.Sqrt(sumOfSquaresOfDifferences / (originDataPerevodEdinics.Count - 1));


                        AverageTrainTimeStruct.Add(new Triple<string, string, double>(tem, sp, sr));

                        OffsetTrainTimeStruct.Add(new Triple<string, string, double>(tem, sp, sd));
                    }
                }
            }

            foreach (var originDataPerevodEdinic in _originDataPerevodEdinic)
            {
                var e = AverageTrainTimeStruct.Where(triple => triple.First == originDataPerevodEdinic.C)
                    .FirstOrDefault(triple => triple.Second == originDataPerevodEdinic.D).Third;
                e = e > 0 ? e : 0.05;
                var g = OffsetTrainTimeStruct.Where(triple => triple.First == originDataPerevodEdinic.C)
                    .FirstOrDefault(triple => triple.Second == originDataPerevodEdinic.D).Third;
                g = g > 0 ? g : 0;
                NormPchObjects(originDataPerevodEdinic, e, g);
            }

            foreach (var originDataPerevodEdinic in _originDataPerevodEdinic)
            {
                NormaTimeUstrOtkazov(originDataPerevodEdinic);
            }

            foreach (var originDataPerevodEdinic in _originDataPerevodEdinic)
            {

                var b = originDataPerevodEdinic.Q;
                var c = originDataPerevodEdinic.S;

                var d = b / (365 * 24 - c);

                _factIntensities.Add(new FactIntensity()
                {
                    A = originDataPerevodEdinic.A,
                    B = originDataPerevodEdinic.Q,
                    C = originDataPerevodEdinic.S,
                    D = d,
                });
            }

            List<IntensityOtkIter> listIntensity = new List<IntensityOtkIter>();

            foreach (var item in _originDataNorms)
            {
                //                KeyValuePair<double, double> List;

                IntensityOtkazov(item, out var e, out var j, out var d, out var h, out var f, out var g, out var i,
                    out KeyValuePair<double, double> list, out var gq);

                listIntensity.Add(new IntensityOtkIter()
                {
                    A = item.A,
                    B = item.O,
                    C = SettingsRas4et,
                    D = d,
                    E = e,
                    F = f,
                    G = g,
                    H = h,
                    I = i,
                    J = j,
                    GN = list.Key,
                    GO = list.Value,
                    GQ = gq
                });
            }

            var sum = listIntensity.Sum(iter => iter.GN);
            var sums = listIntensity.Sum(iter => iter.GQ);

            var gr = sums / sum;

            foreach (var intensityOtkIter in listIntensity)
            {

                var gs = intensityOtkIter.GN * gr;
                var gt = (double)gs * 365 * 24;
                gt = Math.Ceiling(gt);
                var gu = gt / (365 * 24) * 1.01;

                _intensityOtkIters.Add(new IntensityOtkIter()
                {
                    A = intensityOtkIter.A,
                    B = intensityOtkIter.B,
                    C = intensityOtkIter.C,
                    D = intensityOtkIter.D,
                    E = intensityOtkIter.E,
                    F = intensityOtkIter.F,
                    G = intensityOtkIter.F,
                    H = intensityOtkIter.F,
                    I = intensityOtkIter.F,
                    J = intensityOtkIter.F,
                    GN = intensityOtkIter.GN,
                    GO = intensityOtkIter.GO,
                    GQ = intensityOtkIter.GQ,
                    GR = gr,
                    GS = gs,
                    GT = gt,
                    GU = gu
                });
            }
            listIntensity.Clear();

            KoefGotMethod();


            foreach (var originDataNorm in _originDataNorms)
            {
                var normTimeYstrOtk = _normTimeYstrOtks.FirstOrDefault(otk => otk.A == originDataNorm.A);

                _svodTableVrVoses.Add(new DataSvodTableVRVos()
                {
                    A = originDataNorm.A,
                    B = normTimeYstrOtk.B,
                    C = normTimeYstrOtk.D,
                    D = normTimeYstrOtk.H
                });
            }


            var reglamentTimeList = _svodTableVrVoses.Select(dannie => dannie.D).Distinct().OrderBy(s => s).ToList();

            Dictionary<int, double> averageTimeRecovery = new Dictionary<int, double>();
            Dictionary<int, double> offsetTimeRecovery = new Dictionary<int, double>();

            foreach (var i in reglamentTimeList)
            {
                var bools = _svodTableVrVoses.Where(vos => vos.D == i).ToList();
                if (bools.Count == 0) continue;
                var sr = bools.Select(dannie => dannie.C).Average();

                double sumOfSquaresOfDifferences =
                    bools.Select(val => (val.C - sr) * (val.C - sr)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / (bools.Count - 1));

                averageTimeRecovery.Add(i, sr);
                offsetTimeRecovery.Add(i, sd);
            }

            foreach (var originItem in _originDataNorms)
            {
                var dataSvodTableVrVos = _svodTableVrVoses.FirstOrDefault(vos => vos.A == originItem.A);

                var a = originItem.A;
                var b = dataSvodTableVrVos.D;
                var c = averageTimeRecovery.FirstOrDefault(pair => pair.Key == b).Value;
                var d = c < 0.1 ? 0.1 : c;
                var e = offsetTimeRecovery.FirstOrDefault(pair => pair.Key == b).Value;
                var f = e;
                var g = dataSvodTableVrVos.B;
                var h = Math.Round(d + 0.25 * f, 1);
                var i = h < g ? g : h;

                _normSrvrVossts.Add(new NormSRVRVosst()
                {
                    A = a,
                    B = b,
                    C = c,
                    D = d,
                    E = e,
                    F = f,
                    G = g,
                    H = h,
                    I = i,
                });
            }

            foreach (var originItem in _originDataNorms)
            {
                var tem = _originDataPerevodEdinic.FirstOrDefault(edinic => edinic.A == originItem.A).Q;

                _originFrequencyObjects.Add(new OriginFrequencyObject()
                {
                    A = originItem.A,
                    B = tem,
                    C = originItem.C,
                    D = originItem.D,
                });
            }

            var classRailways = _originFrequencyObjects.Select(o => o.C).Distinct().OrderBy(s => s).ToList();

            var specializationRailways = _originFrequencyObjects.Select(o => o.D).Distinct().OrderBy(s => s).ToList();


            foreach (var tem in classRailways)
            {
                var special =
                    _originFrequencyObjects.Where(dannie => dannie.C == tem).ToList() /*.Where(dannie => dannie.D)*/;
                foreach (var sp in specializationRailways)
                {
                    var originFrequencyObjects = special.Where(dannie => dannie.D == sp).ToList();
                    if (originFrequencyObjects.Count == 0) continue;
                    {
                        var sr = originFrequencyObjects.Select(dannie => dannie.B).Average();

                        double sumOfSquaresOfDifferences =
                            originFrequencyObjects.Select(val => (val.B - sr) * (val.B - sr)).Sum();
                        double sd = Math.Sqrt(sumOfSquaresOfDifferences / (originFrequencyObjects.Count - 1));

                        AverageNumberFailuresStruct.Add(new Triple<string, string, double>(tem, sp, sr));

                        OffsetNumberFailuresStruct.Add(new Triple<string, string, double>(tem, sp, sd));
                    }
                }
            }


            foreach (var originFrequencyObject in _originFrequencyObjects)
            {
                var a = originFrequencyObject.A;
                var b = originFrequencyObject.C;
                var c = originFrequencyObject.D;
                var d = AverageNumberFailuresStruct.Where(triple => triple.First == b)
                    .FirstOrDefault(triple => triple.Second == c).Third;
                var e = OffsetNumberFailuresStruct.Where(triple => triple.First == b)
                    .FirstOrDefault(triple => triple.Second == c).Third;
                var f = (d == 0 ? 0.1 : d);
                var g = e;
                var h = f + 0.25 * g;
                var i = Math.Ceiling(h);

                _normsFrequencyObjects.Add(new NormsFrequencyObject()
                {
                    A = a,
                    B = b,
                    C = c,
                    D = d,
                    E = e,
                    F = f,
                    G = g,
                    H = h,
                    I = i

                });
            }

            Result(client, normOriginImport.NameList);



        }

        private void Result(ElasticClient client, string nameList)
        {
            foreach (var originItem in _originDataNorms)
            {
                var a = originItem.A;
                //var b = originItem.B;
                var c = originItem.C;
                var d = originItem.D;

                var firstOrDefault = _normsP4Objectses.FirstOrDefault(objects => objects.A == a);
                var normTimeYstrOtk = _normTimeYstrOtks.FirstOrDefault(objects => objects.A == a);
                var orDefault = _coefGot.FirstOrDefault(objects => objects.A == a);
                //var intensityIncident = _intensityIncidents.FirstOrDefault(objects => objects.A == a);
                var normSrvrVosst = _normSrvrVossts.FirstOrDefault(objects => objects.A == a);
                var intensityOtkIter = _intensityOtkIters.FirstOrDefault(objects => objects.A == a);
                var normsFrequencyObject = _normsFrequencyObjects.FirstOrDefault(objects => objects.A == a);

                var e = firstOrDefault.K;
                var f = normTimeYstrOtk.G;
                var g = orDefault.D;
                //var h = intensityIncident.P;
                var i = normSrvrVosst.I;
                var j = intensityOtkIter.GS;
                var k = intensityOtkIter.GU;
                var l = normsFrequencyObject.H;
                var m = normsFrequencyObject.I;

                _resultNorms.Add(new ResultNorm()
                {
                    A = a.ToString(),
                    //B = b.ToString(),
                    C = c.ToString(),
                    D = d.ToString(),
                    E = Math.Round(e, 2).ToString(CultureInfo.InvariantCulture), //–‡Ò˜ÂÚÌÓÂ ÁÌ‡˜ÂÌËÂ ÔÓÚÂ¸ ÔÓÂÁ‰Ó-˜‡ÒÓ‚ Ì‡ ÓÚ˜ÂÚÌ˚È ÔÂËÓ‰
                    F = Math.Round(f, 2).ToString(CultureInfo.InvariantCulture), //ƒÓÔÛÒÚËÏÓÂ ÁÌ‡˜ÂÌËÂ ÒÂ‰ÌÂ„Ó ‚ÂÏÂÌË ÛÒÚ‡ÌÂÌËˇ ÓÚÍ‡ÁÓ‚ 1 Ë 2 Í‡ÚÂ„ÓËË
                    G = Math.Round(g, 6).ToString(CultureInfo.InvariantCulture), //ƒÓÔÛÒÚËÏÓÂ ÁÌ‡˜ÂÌËÂ ÍÓ˝ÙÙËˆËÂÌÚ‡ „ÓÚÓ‚ÌÓÒÚË ÔÓ ÓÚÍ‡Á‡Ï 1 Ë 2 Í‡ÚÂ„ÓËË
                    //H = Math.Round(h, 7).ToString(CultureInfo.InvariantCulture),
                    I = i.ToString(CultureInfo.InvariantCulture),
                    J = j.ToString(CultureInfo.InvariantCulture),
                    K = Math.Round(k, 6).ToString(CultureInfo.InvariantCulture), //ƒÓÔÛÒÚËÏÓÂ ÁÌ‡˜ÂÌËÂ ËÌÚÂÌÒË‚ÌÓÒÚË ÓÚÍ‡ÁÓ‚ 1 Ë 2 Í‡ÚÂ„ÓËË
                    L = l.ToString(CultureInfo.InvariantCulture),
                    M = m.ToString(CultureInfo.InvariantCulture)
                });

            }

            var isOk = client.Index(new NormResultItems()
            {
                NameList = nameList.ToLower(),

                ResultData = _resultNorms
            });
        }

        private void KoefGotMethod()
        {
            try
            {
                foreach (var originItem in _originDataNorms)
                {
                    var a = originItem.A;

                    var b = _intensityOtkIters.FirstOrDefault(iter => iter.A == a).GU;

                    var c = _normTimeYstrOtks.FirstOrDefault(otk => otk.A == a).G;

                    var d = 1 / (1 + b * c);

                    _coefGot.Add(new KoefGot()
                    {
                        A = a,
                        B = b,
                        C = c,
                        D = d,
                    });
                }
            }
            catch (Exception e)
            {
                //
            }          
        }

        private void IntensityOtkazov(OriginDataNorm item, out double e, out double j, out double d, out double h,
            out double f, out int g, out double i, out KeyValuePair<double, double> list, out double gq)
        {
            var normTime = _normTimeYstrOtks.FirstOrDefault(otk => otk.A == item.A);
            var popravCoef = _popravo4Nble—oefficientsObject.FirstOrDefault(objects => objects.A == item.A);
            var normp4 = _normsP4Objectses.FirstOrDefault(objects => objects.A == item.A);
            var valueIntensyty = _factIntensities.FirstOrDefault(objects => objects.A == item.A);

            e = double.Parse(item.E) / 24;
            j = normp4.I;
            d = normTime?.G ?? 0;
            h = popravCoef.G;
            f = (double)1 / d;
            g = 60 / item.O;
            i = popravCoef.H;

            double j1 = j;
            gq = valueIntensyty.D;

            var k = (double)1 / g;

            var l = e / (2 * Math.Pow(g, 2));

            var m = e / g;

            var n = (k * (1 - m) + l) / (1 - m);
            var o = SettingsRas4et * 10;
            Dictionary<double, double> results = new Dictionary<double, double>();

            foreach (var value in _valueIntensityOtk)
            {
                var coefB = value / (2 * Math.Pow(f, 2));
                var coefD = value / f;
                var s = (k * (1 - coefD - m) + coefB + l) / ((1 - coefD - m) * (1 - coefD));

                var t = (s - n) * h * Math.Pow(10, 2);

                var u = e * t * o * i;

                var result = u * t;
                results.Add(value, result);
            }

            var orderedEnumerable = results.Where(pair => pair.Value > j1).OrderBy(pair => pair.Value).FirstOrDefault();

            var bv = orderedEnumerable.Key;
            var bu = orderedEnumerable.Value;

            double cf = 0;
            var cg = cf / (2 * Math.Pow(g, 2));
            var ch = cf / f;
            var ci = (k * (1 - ch - m) + cg + l) / ((1 - ch - m) * (1 - ch));
            var cj = (ci - n) * h * (2 * Math.Pow(f, 2));
            var ck = e * cj * o * i;
            var cl = ck * cj;

            results.Clear();

            for (int p = 0; p < 30; p++)
            {
                cf = ((j - cl) * (bv - cf)) / (bu - cl) + cf;

                cg = cf / (2 * Math.Pow(f, 2));
                ch = cf / f;
                ci = (k * (1 - ch - m) + cg + l) / ((1 - ch - m) * (1 - ch));
                cj = (ci - n) * h * Math.Pow(10, 2);
                ck = e * cj * o * i;
                cl = ck * cj;

                var firstOrDefault = results.FirstOrDefault(pair => pair.Key == cf);
                if (firstOrDefault.Key != cf)
                {
                    results.Add(cf, cl);

                }
            }

            list = results.OrderByDescending(pair => pair.Key).FirstOrDefault();

        }

        private void NormaTimeUstrOtkazov(OriginDataPerevodEdinic originDataPerevodEdinic)
        {
            double c = originDataPerevodEdinic.K;

            var e = double.Parse(originDataPerevodEdinic.C) / originDataPerevodEdinic.Q;

            e = double.IsInfinity(e) ? originDataPerevodEdinic.K : e;

            var h = 0;
            if (originDataPerevodEdinic.K <= 0.5)
            {
                h = 1;
            }
            else if (originDataPerevodEdinic.K <= 1)
            {
                h = 2;
            }
            else if (originDataPerevodEdinic.K <= 1.5)
            {
                h = 3;
            }
            else if (originDataPerevodEdinic.K <= 2)
            {
                h = 4;
            }
            else if (originDataPerevodEdinic.K <= 2.5)
            {
                h = 5;
            }
            else if (originDataPerevodEdinic.K <= 3)
            {
                h = 6;
            }
            else
            {
                h = 7;
            }

            _normTimeYstrOtks.Add(new NormTimeYstrOtk()
            {
                A = originDataPerevodEdinic.A,
                B = originDataPerevodEdinic.K,
                C = c,
                D = c,
                E = e,
                F = e,
                G = originDataPerevodEdinic.K,
                H = h
            });
        }

        private void NormPchObjects(OriginDataPerevodEdinic originDataPerevodEdinic, double e, double g)
        {
            var f = e;
            var i = f + 0.25 * g;
            var j = i * SettingsRas4et / 12;
            var l = j < 0.3 ? 0.3 : j;
            var k = l > 3.7 ? 3.7 : l;
            var m = i < 0.1 ? 0.1 : i;
            //var h = 0;

            _normsP4Objectses.Add(new NormsP4Objects()
            {
                A = originDataPerevodEdinic.A,                
                E = e,
                F = f,
                G = g,
                H = g,
                J = j,
                I = i,
                D = originDataPerevodEdinic.D,
                K = k,
                C = originDataPerevodEdinic.C,
                M = m,
                L = l
            });
        }

        private void PopravCoeficientObject(OriginDataNorm originDataNorm)
        {
            var kg = _correctionFactors.FirstOrDefault(Òoefficient => Òoefficient.A == originDataNorm.P);

            _popravo4Nble—oefficientsObject.Add(new Popravo4Nble—oefficientObjects()
            {
                A = originDataNorm.A,
                C = originDataNorm.C,
                D = originDataNorm.D,
                F = originDataNorm.P,
                G = kg?.D ?? 0,
                H = kg?.E ?? 0,
            });
        }

        private void IcxDanniePerevodEdinic(string a, string c, string d, string e, string h, string j, string k, int n, int o, int p, string q)
        {
            var a1 = a;
            var c1 = c;
            var d1 = d;
            var e1 = e;
            var h1 = h;
            var j1 = j;
            double k1 = double.Parse(k) / 60;

            var o1 = n;
            double q1 = double.Parse(h1) / o1;

            var s1 = double.Parse(q) / o1 / 60;

            var w1 = double.Parse(j1) / (60 * o1);

            var x1 = w1 == 0 ? 0 : 1;


            _originDataPerevodEdinic.Add(new OriginDataPerevodEdinic()
            {
                A = a1,
                C = c1,
                D = d1,
                E = e1,
                H = h1,
                J = j1,
                K = k1,
                O = o1,
                Q = q1,
                S = s1,
                W = w1,
                X = x1
            });
        }
    }
}
