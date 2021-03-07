using System;
using System.Collections.Generic;
using System.Linq;
using Diplom.DataBase;
using Diplom.Helper;
using Diplom.Infrastructure.Common;
using Diplom.Infrastructure.Norm;
using Diplom.Infrastructure.Table.Norms;
using FluentNest;
using Nest;

namespace Diplom.Command.Import
{/// <summary>
/// Todo: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Гонки!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
    public class ImportNormsCommand 
    {
        static object locker = new object();

        #region Collection
        private List<DataYear> _dataYears;
        private List<OriginDataNorm> _originDataNorms;        
        private List<OriginDataPerevodEdinic> _originDataPerevodEdinic;
        private List<NormsP4Objects> _normsP4Objectses;
        private List<NormTimeYstrOtk> _normTimeYstrOtks;
        private List<Popravo4NbleСoefficient> _correctionFactors;
        private List<Popravo4NbleСoefficientObjects> _popravo4NbleСoefficientsObject;
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
        #endregion
        private List<Triple<string, string, double>> _averageTrainTimeStruct;
        private List<Triple<string, string, double>> _offsetTrainTimeStruct;

        private const int SettingsСalculation = 12; //Todo:указывает пользователь?

        private List<Triple<string, string, double>> _averageNumberFailuresStruct;
        private  List<Triple<string, string, double>> _offsetNumberFailuresStruct;

        public ImportNormsCommand()
        {           
            Initialize();
        }

        private void Initialize()
        {
            _dataYears = new List<DataYear>();
            _originDataNorms = new List<OriginDataNorm>();
            _originDataPerevodEdinic = new List<OriginDataPerevodEdinic>();
            _normsP4Objectses = new List<NormsP4Objects>();
            _normTimeYstrOtks = new List<NormTimeYstrOtk>();
            _correctionFactors = new List<Popravo4NbleСoefficient>();
            _popravo4NbleСoefficientsObject = new List<Popravo4NbleСoefficientObjects>();
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

            _averageTrainTimeStruct = new List<Triple<string, string, double>>();
            _offsetTrainTimeStruct = new List<Triple<string, string, double>>();

            _averageNumberFailuresStruct = new List<Triple<string, string, double>>();
            _offsetNumberFailuresStruct = new List<Triple<string, string, double>>();

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 1,
                B = 1,
                C = 1,
                D = 0.96,
                E = 0.94
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 2,
                B = 2,
                C = 2,
                D = 0.92,
                E = 0.84
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 3,
                B = 11,
                C = 3,
                D = 0.87,
                E = 0.8
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 4,
                B = 4,
                C = 10,
                D = 0.85,
                E = 0.79
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 5,
                B = 1,
                C = 9,
                D = 0.97,
                E = 0.93
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 6,
                B = 10,
                C = 21,
                D = 0.93,
                E = 0.89
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 7,
                B = 22,
                C = 29,
                D = 0.87,
                E = 0.85
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 8,
                B = 30,
                C = 39,
                D = 0.85,
                E = 0.82
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
            {
                A = 9,
                B = 40,
                C = 49,
                D = 0.83,
                E = 0.81
            });

            _correctionFactors.Add(new Popravo4NbleСoefficient()
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

        public void SetNormDataBase(ImportNormDataItems normOriginImport)
        {
            Initialize();

            ClientElasticsearch(normOriginImport);          
        }

        private void ClientElasticsearch(ImportNormDataItems normOriginImport)
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

            var lists = search.Aggs.Aggregations.Count == 0//Todo:Переделать
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

                AddNormItems(normOriginImport, client);
            }
            else
            {
                AddNormItems(normOriginImport, client);
            }
        }


        private void AddNormItems(ImportNormDataItems normOriginImport, ElasticClient client)
        {

                var listName = normOriginImport.NameList;

                ParseData(normOriginImport);

                OriginData(_dataYears);

                ParseOriginData(_originDataNorms);

                AverageOffsetTrainTimeStruct();

                NormsTrainTimeObjects(_originDataPerevodEdinic);



                List<IntensityOtkIter> listIntensity = new List<IntensityOtkIter>();

                foreach (var originDataNorm in _originDataNorms)
                {
                    var item = IntensityOtkazov(originDataNorm);

                    listIntensity.Add(item);
                    ResultNormIntensityFunction(listIntensity);
                }




                IntensityIncidentMethod(_originDataNorms, _originDataPerevodEdinic);

                KoefGotMethod(_originDataNorms, _intensityOtkIters, _normTimeYstrOtks);

                OriginDataRecoveryTime(_originDataNorms, _normTimeYstrOtks);


                AverageOffsetRecoveryTimeDictionary(_svodTableVrVoses, _originDataNorms);

                OriginFrequency(_originDataNorms);

                AverageOffsetNumberFailuresStruct(_originFrequencyObjects);


                FrequencyObject(_originFrequencyObjects);


                Result(client, listName);
        }

        private void Result(ElasticClient client, string listName)
        {
            var c = client.Index(new NormResultItems()
            {
                NameList = listName.ToLower(),

                ResultData = _resultNorms
            });
        }

        private void FrequencyObject(List<OriginFrequencyObject> originFrequencyObjects)
        {
            foreach (var originFrequencyObject in originFrequencyObjects)
            {
                var a = originFrequencyObject.A;
                var b = originFrequencyObject.C;
                var c = originFrequencyObject.D;
                var d = _averageNumberFailuresStruct.Where(triple => triple.First == b).FirstOrDefault(triple => triple.Second == c).Third;
                var e = _offsetNumberFailuresStruct.Where(triple => triple.First == b).FirstOrDefault(triple => triple.Second == c).Third;
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
        }

        private void AverageOffsetNumberFailuresStruct(List<OriginFrequencyObject> originFrequencyObjects)
        {
            var classRailways = originFrequencyObjects.Select(o => o.C).Distinct().OrderBy(s => s).ToList();

            var specializationRailways = originFrequencyObjects.Select(o => o.D).Distinct().OrderBy(s => s).ToList();


            foreach (var tem in classRailways)
            {
                var special =
                    _originFrequencyObjects.Where(dannie => dannie.C == tem).ToList() /*.Where(dannie => dannie.D)*/;
                foreach (var sp in specializationRailways)
                {
                    var list = special.Where(dannie => dannie.D == sp).ToList();
                    if (list.Count == 0) continue;
                    {
                        var sr = list.Select(dannie => dannie.B).Average();

                        double sumOfSquaresOfDifferences =
                            list.Select(val => (val.B - sr) * (val.B - sr)).Sum();
                        double sd = Math.Sqrt(sumOfSquaresOfDifferences / (list.Count - 1));

                        _averageNumberFailuresStruct.Add(new Triple<string, string, double>(tem, sp, sr));

                        _offsetNumberFailuresStruct.Add(new Triple<string, string, double>(tem, sp, sd));
                    }
                }
            }

        }

        private void OriginFrequency(List<OriginDataNorm> originDataNorms)
        {
            foreach (var originItem in originDataNorms)
            {
                OriginDataPerevodEdinic first = _originDataPerevodEdinic.FirstOrDefault(edinic => edinic.A == originItem.A);
                if (first == null) continue;
                var tem = first.Q;

                _originFrequencyObjects.Add(new OriginFrequencyObject()
                {
                    A = originItem.A,
                    B = tem,
                    C = originItem.C,
                    D = originItem.D,
                });
            }
        }

        private void AverageOffsetRecoveryTimeDictionary(List<DataSvodTableVRVos> svodTableVrVoses, List<OriginDataNorm> originDataNorms)
        {
            Dictionary<int, double> averageTimeRecovery = new Dictionary<int, double>();
            Dictionary<int, double> offsetTimeRecovery = new Dictionary<int, double>();

            var reglamentTimeList = svodTableVrVoses.Select(dannie => dannie.D).Distinct().OrderBy(s => s).ToList();

            foreach (var i in reglamentTimeList)
            {
                var bools = svodTableVrVoses.Where(vos => vos.D == i).ToList();
                if (bools.Count == 0) continue;
                var sr = bools.Select(dannie => dannie.C).Average();

                double sumOfSquaresOfDifferences =
                    bools.Select(val => (val.C - sr) * (val.C - sr)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / (bools.Count - 1));

                averageTimeRecovery.Add(i, sr);
                offsetTimeRecovery.Add(i, sd);
            }

            TableRecoveryTime(originDataNorms, svodTableVrVoses, averageTimeRecovery, offsetTimeRecovery);


        }

        private void TableRecoveryTime(List<OriginDataNorm> originDataNorms, List<DataSvodTableVRVos> svodTableVrVoses,
            Dictionary<int, double> averageTimeRecovery, Dictionary<int, double> offsetTimeRecovery)
        {
            foreach (var originDataNorm in originDataNorms)
            {
                var dataSvodTableVrVos = svodTableVrVoses.FirstOrDefault(vos => vos.A == originDataNorm.A);

                var a = originDataNorm.A;
                if (dataSvodTableVrVos == null) continue;
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
                    A = originDataNorm.A,
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
        }

        private void OriginDataRecoveryTime(List<OriginDataNorm> originDataNorms, List<NormTimeYstrOtk> normTimeYstrOtks)
        {
            foreach (var originDataNorm in originDataNorms)
            {
                var normTimeYstrOtk = normTimeYstrOtks.FirstOrDefault(otk => otk.A == originDataNorm.A);

                _svodTableVrVoses.Add(new DataSvodTableVRVos()
                {
                    A = originDataNorm.A,
                    B = normTimeYstrOtk.B,
                    C = normTimeYstrOtk.D,
                    D = normTimeYstrOtk.H
                });
            }
        }

        private void KoefGotMethod(List<OriginDataNorm> originDataNorms, List<IntensityOtkIter> intensityOtkIters,
            List<NormTimeYstrOtk> normTimeYstrOtks)
        {
            foreach (var originItem in originDataNorms)
            {
                var a = originItem.A;

                IntensityOtkIter first = intensityOtkIters?.FirstOrDefault(iter => iter.A == a);
                if (first == null) continue;
                var b = first.GU;

                NormTimeYstrOtk first1 = normTimeYstrOtks?.FirstOrDefault(otk => otk.A == a);
                if (first1 == null) continue;
                var c = first1.G;

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

        private void IntensityIncidentMethod(List<OriginDataNorm> originDataNorms, List<OriginDataPerevodEdinic> originDataPerevodEdinic)
        {
            var c = originDataNorms.Select(norm => double.Parse(norm.H)).Sum();
            var d = originDataPerevodEdinic.Select(edinic => edinic.U).Count();
            var e = originDataNorms.Sum(norm => double.Parse(norm.L));
            var f = originDataPerevodEdinic.Sum(edinic => edinic.N);
            var g = originDataNorms.Sum(norm => double.Parse(norm.M));
            var h = originDataNorms.Sum(norm => norm.F);

            foreach (var intensityItem in _intensityOtkIters)
            {

                var a = intensityItem.A;
                var b = intensityItem.GS;


                OriginDataNorm first1 = _originDataNorms.FirstOrDefault(norm => norm.A == a);
                if (first1 == null) continue;
                NormTimeYstrOtk first2 = _normTimeYstrOtks?.FirstOrDefault(otk => otk.A == a);
                if (first2 == null) continue;
                OriginDataPerevodEdinic first3 = originDataPerevodEdinic.FirstOrDefault(edinic => edinic.A == a);
                if (first3 == null) continue;


                var i = first2.D;
                var j = first1.F;

                var k = (e / f + (g + h) / d) / (c / d);

                var l = ((g + h) / d) / (c / d);
        
                var m = first3.N;

                var n = m == 1 ? (b * k) : b * l;

                var first = Math.Ceiling(n * (365 * 24 - j * i));
                var second = Math.Ceiling(n * (365 * 24 - j * i));

                var o = first > 300 ? 300 : second;

                var p = o / (365 * 24 - j * i);

                _intensityIncidents.Add(new IntensityIncident()
                {
                    A = a,
                    B = b,
                    C = c,
                    D = d,
                    E = e,
                    F = f,
                    G = g,
                    H = h,
                    J = j,
                    K = k,
                    L = l,
                    M = m,
                    N = n,
                    O = o,
                    P = p
                });
            }
        }

        private void ParseOriginData(List<OriginDataNorm> originDataNorms)
        {
            foreach (var originDataNorm in originDataNorms)
            {
                OriginDataUnitTransfer(originDataNorm);

                CorrectionFactor(originDataNorm);
            }
        }

        private void ResultNormIntensityFunction(List<IntensityOtkIter> listIntensity)
        {
            var sum = listIntensity.Sum(iter => iter.GN);
            var sums = listIntensity.Sum(iter => iter.GQ);

            var gr = sums / sum;

            foreach (var intensityOtkIter in listIntensity)
            {

                var gs = intensityOtkIter.GN * gr;
                var gt = (double) gs * 365 * 24;
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
        }

        private IntensityOtkIter IntensityOtkazov(OriginDataNorm item)
        {
            var normTime = _normTimeYstrOtks.FirstOrDefault(otk => otk.A == item.A);
            var popravCoef = _popravo4NbleСoefficientsObject.FirstOrDefault(objects => objects.A == item.A);
            var normp4 = _normsP4Objectses.FirstOrDefault(objects => objects.A == item.A);
            var valueIntensyty = _factIntensities.FirstOrDefault(objects => objects.A == item.A);

            var e = double.Parse(item.E) / 24;
            var j = normp4.I;
            var d = normTime?.G ?? 0;
            var h = popravCoef.G;
            var f = (double) 1 / d;
            var g = 60 / item.O;
            var i = popravCoef.H;

            var k = (double) 1 / g;
            var l = e / (2 * Math.Pow(g, 2));
            var m = e / g;
            var n = (k * (1 - m) + l) / (1 - m);
            var o = SettingsСalculation * 10;

            var results = new Dictionary<double, double>();
            
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

            var list =  ResultTrainClock(results, g, f, k, n, h, l, m, o, e, i,j);

            if (valueIntensyty != null)
                return new IntensityOtkIter()
                {
                    A = item.A,
                    B = item.O,
                    C = SettingsСalculation,
                    D = d,
                    E = e,
                    F = f,
                    G = g,
                    H = h,
                    I = i,
                    J = j,
                    GN = list.Key,
                    GO = list.Value,
                    GQ = valueIntensyty.D
                };
            return null;
        }

        private KeyValuePair<double, double> ResultTrainClock(Dictionary<double, double> results, int g, double f, double k, double n, double h, double l, double m, int o, double e, double i, double j)
        {
            var orderedEnumerable = results.Where(pair => pair.Value > j).OrderBy(pair => pair.Value).FirstOrDefault();

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

            var list = results.OrderByDescending(pair => pair.Key).FirstOrDefault();
            return list;
        }
 
        private void CorrectionFactor(OriginDataNorm originDataNorm)
        {
            var kg = _correctionFactors.FirstOrDefault(сoefficient => сoefficient.A == originDataNorm.P);

            _popravo4NbleСoefficientsObject.Add(new Popravo4NbleСoefficientObjects()
            {
                A = originDataNorm.A,
                B = originDataNorm.B,
                C = originDataNorm.C,
                D = originDataNorm.D,
                E = originDataNorm.R,
                F = originDataNorm.P,
                G = kg?.D ?? 0,
                H = kg?.E ?? 0,
            });

        }

        private void NormsTrainTimeObjects(List<OriginDataPerevodEdinic> originDataPerevodEdinic)
        {
            lock (originDataPerevodEdinic)
            {

                foreach (var dataPerevodEdinic in originDataPerevodEdinic)
                {
                    WorkWithObjectTrainTime(dataPerevodEdinic);

                    NormsTimeRescueSurrender(dataPerevodEdinic);

                    FactIntensityMethod(dataPerevodEdinic);
                }
            }

        }

        private void FactIntensityMethod(OriginDataPerevodEdinic dataPerevodEdinic)
        {
            var b = dataPerevodEdinic.Q;
            var c = dataPerevodEdinic.S;

            var d = b / (365 * 24 - c);

            _factIntensities.Add(new FactIntensity()
            {
                A = dataPerevodEdinic.A,
                B = dataPerevodEdinic.Q,
                C = dataPerevodEdinic.S,
                D = d,
            });
        }

        private void NormsTimeRescueSurrender(OriginDataPerevodEdinic dataPerevodEdinic)
        {
            double c = dataPerevodEdinic.R / dataPerevodEdinic.P;
            c = double.IsNaN(c) ? dataPerevodEdinic.K : c;

            var e = double.Parse(dataPerevodEdinic.C) / dataPerevodEdinic.Q;
            e = double.IsInfinity(e) ? dataPerevodEdinic.K : e;

            var h = 0;
            if (dataPerevodEdinic.K <= 0.5)
            {
                h = 1;
            }
            else if (dataPerevodEdinic.K <= 1)
            {
                h = 2;
            }
            else if (dataPerevodEdinic.K <= 1.5)
            {
                h = 3;
            }
            else if (dataPerevodEdinic.K <= 2)
            {
                h = 4;
            }
            else if (dataPerevodEdinic.K <= 2.5)
            {
                h = 5;
            }
            else if (dataPerevodEdinic.K <= 3)
            {
                h = 6;
            }
            else
            {
                h = 7;
            }

            _normTimeYstrOtks.Add(new NormTimeYstrOtk()
            {
                A = dataPerevodEdinic.A,
                B = dataPerevodEdinic.K,
                C = c,
                D = c,
                E = e,
                F = e,
                G = dataPerevodEdinic.K,
                H = h
            });

        }

        private void WorkWithObjectTrainTime(OriginDataPerevodEdinic dataPerevodEdinic)
        {

            var e = _averageTrainTimeStruct.Where(triple => triple.First == dataPerevodEdinic.C)
                .FirstOrDefault(triple => triple.Second == dataPerevodEdinic.D).Third;
            e = e > 0 ? e : 0.05;
            var g = _offsetTrainTimeStruct.Where(triple => triple.First == dataPerevodEdinic.C)
                .FirstOrDefault(triple => triple.Second == dataPerevodEdinic.D).Third;
            //g = g > 0 ? g : 0;

            var f = e;
            var i = f + 0.25 * g;
            var j = i * SettingsСalculation / 12;
            var l = j < 0.3 ? 0.3 : j;
            var k = l > 3.7 ? 3.7 : l;
            var m = i < 0.1 ? 0.1 : i;
            //var h = 0;

            _normsP4Objectses.Add(new NormsP4Objects()
            {
                A = dataPerevodEdinic.A,
                B = dataPerevodEdinic.B,
                E = e,
                F = f,
                G = g,
                H = g,
                J = j,
                I = i,
                D = dataPerevodEdinic.D,
                K = k,
                C = dataPerevodEdinic.C,
                M = m,
                L = l
            });

        }

        private void AverageOffsetTrainTimeStruct()
        {
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


                        _averageTrainTimeStruct.Add(new Triple<string, string, double>(tem, sp, sr));

                        _offsetTrainTimeStruct.Add(new Triple<string, string, double>(tem, sp, sd));
                    }
                }
            }
        }

        private void OriginDataUnitTransfer(OriginDataNorm originDataNorm)
        {

            var g = originDataNorm.G / 60;

                double k = double.Parse(originDataNorm.K) / 60;

                double l = double.Parse(originDataNorm.L);

                var n = (l > 0) ? 1 : 0;
                var o = originDataNorm.N;
                var p = originDataNorm.F / o;
                double q = double.Parse(originDataNorm.H) / o;
                var r = g / o;
                var s = double.Parse(originDataNorm.Q) / o / 60;

                var t = l / o;
                var u = double.Parse(originDataNorm.M) / o;


                var v = originDataNorm.I / o;
                var w = double.Parse(originDataNorm.J) / (60 * o);

                var x = w == 0 ? 0 : 1;

                _originDataPerevodEdinic.Add(new OriginDataPerevodEdinic()
                {
                    A = originDataNorm.A,
                    B = originDataNorm.B,
                    C = originDataNorm.C,
                    D = originDataNorm.D,
                    E = originDataNorm.E,
                    F = originDataNorm.F,
                    G = g,
                    H = originDataNorm.H,
                    I = originDataNorm.I,
                    J = originDataNorm.J,
                    K = k,
                    L = l,
                    M = originDataNorm.M,
                    N = n,
                    O = o,
                    P = p,
                    Q = q,
                    R = r,
                    S = s,
                    T = t,
                    U = u,
                    V = v,
                    W = w,
                    X = x
                });
        }

        private void OriginData(List<DataYear> dataYears)
        {
            foreach (var dataYear in dataYears)
            {
                double f = double.Parse(dataYear.N) + double.Parse(dataYear.F);

                double g = double.Parse(dataYear.G) + double.Parse(dataYear.O);

                var n = 3; //todo:Указывает пользователь
                var o = 5; //todo:Указывает пользователь
                var p = 5; //todo:Указывает пользователь
                var r = "тип"; //todo:Указывает пользователь, уточнить

                _originDataNorms.Add(new OriginDataNorm()
                {
                    A = dataYear.A,
                    B = dataYear.B,
                    C = dataYear.C,
                    D = dataYear.D,
                    E = dataYear.E,
                    F = f,
                    G = g,
                    H = dataYear.F,
                    I = dataYear.I,
                    J = dataYear.J,
                    K = dataYear.K,
                    L = dataYear.L,
                    M = dataYear.M,
                    N = n,
                    O = o,
                    P = p,
                    Q = dataYear.G,
                    R = r,
                });
            }
        }

        private void ParseData(ImportNormDataItems normOriginImport)
        {
            foreach (var item in normOriginImport.NormOriginImportItems)
            {
                _dataYears.Add(new DataYear()
                {
                    A = string.IsNullOrEmpty(item.A) ? "0" : item.A,
                    B = string.IsNullOrEmpty(item.B) ? "0" : item.B,
                    C = string.IsNullOrEmpty(item.C) ? "0" : item.C,
                    D = string.IsNullOrEmpty(item.D) ? "0" : item.D,
                    E = string.IsNullOrEmpty(item.E) ? "0" : item.E,
                    F = string.IsNullOrEmpty(item.F) ? "0" : item.F,
                    G = string.IsNullOrEmpty(item.G) ? "0" : item.G,
                    H = 0, //todo:спросить
                    I = 0, //todo:спросить
                    J = string.IsNullOrEmpty(item.H) ? "0" : item.H,
                    K = string.IsNullOrEmpty(item.I) ? "0" : item.I,
                    L = string.IsNullOrEmpty(item.J) ? "0" : item.J,
                    M = string.IsNullOrEmpty(item.K) ? "0" : item.K,
                    N = string.IsNullOrEmpty(item.L) ? "0" : item.L,
                    O = string.IsNullOrEmpty(item.M) ? "0" : item.M,
                    //P = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //Q = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //R = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //S = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //T = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                    //U = string.IsNullOrEmpty(item.M) ? "-" : item.M,
                });
            }
        }
    }
}
