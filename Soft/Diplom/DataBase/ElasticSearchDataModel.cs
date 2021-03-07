using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Diplom.DataBase.Infrastructure;
using Diplom.ViewModel;
using Elasticsearch.Net;
using Nest;

namespace Diplom.DataBase
{
   public class ElasticSearchDataModel
    {
        ConnectionSettings _settings;
        private readonly object _indexReadyLock = new object();

        private readonly ConnectionStringSettingsCollection _connections = ConfigurationManager.ConnectionStrings;

        static ElasticSearchDataModel()
        {
        }

        public ElasticSearchDataModel()
        {
            if (Settings == null)
            {
                Settings = CreateClient("original", a => a
                    .Mappings(m => m
                            .Map<FactResultItems>(descriptor => descriptor.AutoMap()
                                .Properties(p => p
                                    .String(s => s
                                        .Name(n => n.NameList)
                                        .Fields(f => f
                                            .String(fs => fs
                                                .Name("raw").NotAnalyzed())
                                            .String(fs => fs
                                                .Name("index").IncludeInAll())
                                        ))))

                            .Map<NormResultItems>(descriptor => descriptor.AutoMap()
                                .Properties(p => p
                                    .String(s => s
                                        .Name(n => n.NameList)
                                        .Fields(f => f
                                            .String(fs => fs
                                                .Name("raw").NotAnalyzed())
                                            .String(fs => fs
                                                .Name("index").IncludeInAll())
                                        ))))));
            }




            //if (IsInitFirst())
            //{

            //    //MainViewModel = new MainViewModel();

            //    //var search = Settings
            //    //  .Search<SelectionList>(s => s
            //    //      .Skip(0).Take(Int32.MaxValue));

            //    //var items = search.Aggs.Aggregations.Count == 0
            //    //    ? new List<SelectionList>()
            //    //    : search.Aggs
            //    //        .GetGroupBy<SelectionList>(arg => arg)
            //    //        .SelectMany(
            //    //            bucket => bucket.GetSortedTopHits<SelectionList>(x => x, SortType.Ascending))
            //    //        .ToList();
            //}
        }

        public MainViewModel MainViewModel { get; set; }

        private bool IsInitFirst()
        {
            try
            {
                var initFirst = Settings
                    .Search<InitFirst>(s => s)
                    .Documents
                    .FirstOrDefault();
                if (initFirst != null) return false;
            }
            catch (Exception)
            {
                // ignored
            }

            Settings.Index(new InitFirst()
            {
                InitTime = DateTime.Now
            });
            return true;
        }

        public string PrfixIndex
        {
            get
            {
                var indexName = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DataIndex"])
                    ? "data"
                    : ConfigurationManager.AppSettings["DataIndex"]).ToLower();

                return indexName;

            }
        }

        public static ElasticSearchDataModel Instance { get; } = new ElasticSearchDataModel();


        private ElasticClient CreateClient(string index, Action<CreateIndexDescriptor> action)
        {
            try
            {
                lock (_indexReadyLock)
                {
                    var nodes = _connections.OfType<ConnectionStringSettings>()
                        .Where(stringSettings => stringSettings.Name.ToLower().Contains("node"))
                        .Select(list => new Uri(list.ConnectionString));

                    var indexName = PrfixIndex + "." + index;

                    var connectionPool = new SniffingConnectionPool(nodes);

                    _settings = new ConnectionSettings(connectionPool);
                    _settings.DisableDirectStreaming();
                    _settings.DefaultIndex(indexName);

                    var dataClient = new ElasticClient(_settings);
                    var indexDescriptor = new CreateIndexDescriptor(indexName);
                    action(indexDescriptor);

                    var response = dataClient.CreateIndex(indexDescriptor);
                    if (response.IsValid || response.ServerError?.Error?.Type == "index_already_exists_exception")
                        return dataClient;

                    System.Diagnostics.Trace.TraceError(response.DebugInformation);
                }
                return null;
            }
            catch (Exception e)
            {

                MessageBox.Show(Application.Current.MainWindow ?? throw new InvalidOperationException(),
                    $"Отсутствует соединение с ElasticSearch",
                    "Внимание!!!", MessageBoxButton.OK);

                Process.GetCurrentProcess().Kill();
            }

            return null;
        }

        public ElasticClient Settings { get; private set; }
    }
}

