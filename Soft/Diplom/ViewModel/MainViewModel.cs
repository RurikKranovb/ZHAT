using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Diplom.Command.Delete;
using Diplom.Command.Get;
using Diplom.Command.Import;
using Diplom.Command.Import.OldImport;
using Diplom.Command.Set;
using Diplom.DataBase.Infrastructure;
using Diplom.Helper;
using Diplom.Infrastructure;
using Diplom.Infrastructure.Common;
using Diplom.Infrastructure.Fact;
using Diplom.Infrastructure.Norm;
using Diplom.View;
using ExcelDataReader;
using ExtendedGrid.ExtendedGridControl;
using Microsoft.Win32;
using Prism.Commands;
using NormOriginData = Diplom.Infrastructure.Common.NormOriginData;

namespace Diplom.ViewModel
{
    public class MainViewModel : ValidatableBindableBase
    {
        private DataSet _dataSet;
        private string _currentListExcel = "";
        private ExtendedDataGrid _extend;
        private bool _isImport;
        private Dispatcher _uiDiapatcher;
        private int _itemArrayLength;
        private ObservableCollection<GetFactDataItems> _factResultItemses;
        private ObservableCollection<GetNormDataItems> _normResultItemses;
        private ResultFact _selectListResultFact;
        private ResultNorm _selectListResultNorm;
        private GetNormDataItems _getNormDataItems;
        private bool _isDiagramm;
        private bool _isText;
        private GetFactDataItems _selectFactListItem;
        private GetNormDataItems _selectNormDataItem;


        public MainViewModel()
        {

            ListExcelTems = new ObservableCollection<ListExcelItem>();
            ListsExcelFile = new ObservableCollection<string>();
            //ImportNormCommand = new ImportNormCommand();
            _uiDiapatcher = Dispatcher.CurrentDispatcher;
            ImportNormDataItem = new ImportNormDataItems();
            _extend = new ExtendedDataGrid();
            //GetSaveFile();
            SetFactCommand = new SaveFactCommand();
            SetNormCommand = new SaveNormCommand();
            //:todo!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ImportFactCommand = new ImportFactCommand();
            ImportFactDataItemses = new ObservableCollection<ImportFactDataItems>();
            //FactOriginalData = new FactOriginalData();
            FactOriginalDataItems = new ImportFactDataItems();
            FactResultItemses = new ObservableCollection<GetFactDataItems>();
            GetFactCommand = new GetFactCommand();
            NormResultItemses = new ObservableCollection<GetNormDataItems>();
            ImportNormCommand = new ImportNormCommand();
            //ImportNormsCommand = new ImportNormsCommand();
            GetNormCommand = new GetNormCommand();

            SelectListResultFact = new ResultFact();
            SelectListResultNorm = new ResultNorm();
            SelectFactListItem = new GetFactDataItems();
            SelectNormDataItem = new GetNormDataItems();

            GetNormDataItems = new GetNormDataItems();

            //IsDiagramm = false;
            IsText = false;

            RemoveFactItemCommand = new RemoveFactItemCommand();
            RemoveNormItemCommand = new RemoveNormItemCommand();
                RaisePropertyChanged(() => IsDiagramm);

        }



        #region Property

        public GetFactDataItems SelectFactListItem
        {
            get => _selectFactListItem;
            set
            {
                _selectFactListItem = value;
                RaisePropertyChanged(() => SelectFactListItem);
                RaisePropertyChanged(() => IsDiagramm);
            }
        }

        public GetNormDataItems SelectNormDataItem
        {
            get => _selectNormDataItem;
            set
            {
                _selectNormDataItem = value;
                RaisePropertyChanged(() => SelectNormDataItem);
            }
        }

        public string CurrentListExcel
        {
            get => _currentListExcel;
            set
            {
                _currentListExcel = value;
                RaisePropertyChanged(() => CurrentListExcel);
                IsImport = !string.IsNullOrEmpty(CurrentListExcel);
            }
        }

        public bool IsImport
        {
            get => _isImport;
            set
            {
                _isImport = value;
                RaisePropertyChanged(() => IsImport);
            }
        }

        public bool IsText
        {
            get => _isText;
            set
            {
                _isText = value;
                RaisePropertyChanged(() => IsText);
            }
        }

        public bool IsDiagramm
        {
            get => _selectFactListItem.NameList != null && _getNormDataItems.NameList != null;
            //set
            //{
            //    _isDiagramm = value;
            //    RaisePropertyChanged(() => IsDiagramm);
            //}
        }

        public ImportNormCommand ImportNormCommand { get; set; }
        //public ImportNormsCommand ImportNormsCommand { get; set; }

        public ObservableCollection<GetNormDataItems> NormResultItemses
        {
            get { return _normResultItemses; }
            set
            {
                _normResultItemses = value;
                RaisePropertyChanged(() => NormResultItemses);
            }
        }

        public GetNormDataItems GetNormDataItems
        {
            get { return _getNormDataItems; }
            set
            {
                _getNormDataItems = value;
               
                RaisePropertyChanged(() => IsDiagramm);
                RaisePropertyChanged(() => GetNormDataItems);
            }
        }



        public ObservableCollection<ListExcelItem> ListExcelTems { get; }
        public ObservableCollection<string> ListsExcelFile { get; }

        public ObservableCollection<ImportFactDataItems> ImportFactDataItemses { get; }

        //public FactOriginalData FactOriginalData { get; set; }
        public ImportFactDataItems FactOriginalDataItems { get; set; }

        public ObservableCollection<GetFactDataItems> FactResultItemses
        {
            get { return _factResultItemses; }
            set
            {
                _factResultItemses = value;
                RaisePropertyChanged(() => FactResultItemses);
            }
        }

        public ResultFact SelectListResultFact
        {
            get { return _selectListResultFact; }
            set
            {
                _selectListResultFact = value;
                RaisePropertyChanged(() => SelectListResultFact);
            }
        }

        public ResultNorm SelectListResultNorm
        {
            get { return _selectListResultNorm; }
            set
            {
                _selectListResultNorm = value;
                RaisePropertyChanged(() => SelectListResultNorm);
            }
        }

        #endregion

        #region Main


        public RemoveFactItemCommand RemoveFactItemCommand { get; set; }
        public RemoveNormItemCommand RemoveNormItemCommand { get; set; }
        public GetNormCommand GetNormCommand { get; set; }
        public GetFactCommand GetFactCommand { get; set; }
        public SaveFactCommand SetFactCommand { get; set; }
        public SaveNormCommand SetNormCommand { get; set; }


        public ICommand LoadCommand => new DelegateCommand(LoadCommandAction);

        private void LoadCommandAction()
        {
            FactResultItemses.Clear();

            FactResultItemses = GetFactCommand.GetFile();

            NormResultItemses.Clear();

            NormResultItemses = GetNormCommand.GetFile();
        }

        public ICommand SaveCommand => new DelegateCommand(SaveCommandAction);

        private void SaveCommandAction()
        {
            foreach (var getFactDataItemse in FactResultItemses)
            {
                SetFactCommand.SetFile(getFactDataItemse);
            }

            foreach (var getNormDataItemse in NormResultItemses)
            {
                SetNormCommand.SetFile(getNormDataItemse);
            }
        }

        #endregion

        #region import


        public ImportFactCommand ImportFactCommand { get; set; }

        //public ImportNormCommand ImportNormCommand { get; set; }
        public ImportNormDataItems ImportNormDataItem { get; set; }

        #region SelectExcelFileCommand      

        public ICommand SelectExcelFileCommand => new DelegateCommand(SelectExcelFileCommandAction);

        private void SelectExcelFileCommandAction()
        {
            ListsExcelFile.Clear();
            var ofd = new OpenFileDialog
            {
                Filter = " xlsx файл(*.xlsx) | *.xlsx|xls файл(*.xls) | *.xls|csv файл(*.csv) | *.csv",
                Multiselect = false
            };
            var result = ofd.ShowDialog();
            if (result == false) return;

            GetItemExcel(ofd.FileName);
        }

        private void GetItemExcel(string ofdFileName)
        {
            var extension = Path.GetExtension(ofdFileName)?.ToLower();
            using (var stream = new FileStream(ofdFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var sw = new Stopwatch();
                sw.Start();
                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream);
                }

                if (reader == null)
                    return;

                using (reader)
                {
                    _dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = false,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                }
                GetTableNames(_dataSet.Tables);
            }
        }

        private void GetTableNames(DataTableCollection tables)
        {
            foreach (var table in tables)
            {
                ListsExcelFile.Add(table.ToString());
                ListExcelTems.Clear();
                ListExcelTems.Add(new ListExcelItem()
                {
                    CurrentListExcel = table.ToString()
                });
            }
            IsText = true;
        }

        #endregion

        #region ImportingListCommand

        public ICommand ImportingListCommand => new DelegateCommand(ImportingListCommandAction);

        private void ImportingListCommandAction()
        {
            GetValues(_dataSet, _currentListExcel);
        }

        public void GetValues(DataSet dataset, string listName)
        {
            try
            {
                FactOriginalDataItems = new ImportFactDataItems();
                ImportNormDataItem = new ImportNormDataItems();

                foreach (System.Data.DataRow row in dataset.Tables[listName].Rows)
                {
                    _itemArrayLength = row.ItemArray.Length;

                    if (_itemArrayLength == 6)
                    {
                        FactOriginalDataItems.NameList = CurrentListExcel.ToLower();

                        FactOriginalDataItems.ImportFactListItems.Add(new FactOriginalData()
                        {
                            A = row.ItemArray[0].ToString(),
                            //B = row.ItemArray[1].ToString(),
                            C = row.ItemArray[1].ToString(),
                            D = row.ItemArray[2].ToString(),
                            //E = row.ItemArray[4].ToString(),
                            F = row.ItemArray[3].ToString(),
                            G = row.ItemArray[4].ToString(),
                            //H = row.ItemArray[7].ToString(),
                            //I = row.ItemArray[8].ToString(),
                            J = row.ItemArray[5].ToString(),
                            //K = row.ItemArray[10].ToString(),
                            //L = row.ItemArray[11].ToString(),
                            //M = row.ItemArray[12].ToString(),
                            //N = row.ItemArray[13].ToString(),
                            //O = row.ItemArray[14].ToString(),
                        });
                    }
                    else if (_itemArrayLength == 8)
                    {
                        ImportNormDataItem.NameList = CurrentListExcel.ToLower();

                        ImportNormDataItem.NormOriginImportItems.Add(new NormOriginData()
                        {
                            A = row.ItemArray[0].ToString(),
                            //B = row.ItemArray[1].ToString(),
                            C = row.ItemArray[1].ToString(),
                            D = row.ItemArray[2].ToString(),
                            E = row.ItemArray[3].ToString(),
                            F = row.ItemArray[4].ToString(),
                            G = row.ItemArray[5].ToString(),
                            H = row.ItemArray[6].ToString(),
                            I = row.ItemArray[7].ToString(),
                            //J = row.ItemArray[9].ToString(),
                            //K = row.ItemArray[10].ToString(),
                            //L = row.ItemArray[11].ToString(),
                            //M = row.ItemArray[12].ToString(),
                            //N = row.ItemArray[13].ToString(),
                            //O = row.ItemArray[14].ToString(),
                            //P = row.ItemArray[15].ToString(),
                            //Q = row.ItemArray[16].ToString(),
                            //R = row.ItemArray[17].ToString(),
                        });
                    }
                    else
                    {
                        if (MessageBox.Show(Application.Current.MainWindow ?? throw new InvalidOperationException(),
                                $"Выбраны не подходящие данные",
                                "Внимание!!! ", MessageBoxButton.OK) == MessageBoxResult.OK)
                        {
                            return;
                        }
                    }
                }

                if (_itemArrayLength == 6)
                    Task.Factory.StartNew(() =>
                        {
                            IsImport = false;

                            ImportFactCommand.SetFactDataBase(FactOriginalDataItems);
                        })
                        .ContinueWith(task =>
                        {
                            _uiDiapatcher.Invoke(() =>
                            {
                                //FactResultItemses.Clear();
                              
                                FactResultItemses = GetFactCommand.GetFile(); 

                                IsImport = true;
                                ListsExcelFile.Clear();
                                IsText = false;
                                RaisePropertyChanged(() => FactResultItemses);
                            });
                        });
                else if (_itemArrayLength == 8)
                {
                    Task.Factory.StartNew(() =>
                        {
                            IsImport = false;

                            ImportNormCommand
                                .SetNormDataBase(
                                    ImportNormDataItem);

                        })
                        .ContinueWith(task =>
                        {
                            _uiDiapatcher.Invoke(() =>
                            {
                                //NormResultItemses.Clear();
                                
                                NormResultItemses = GetNormCommand.GetFile(); 
                                IsImport = true;
                                ListsExcelFile.Clear();
                                IsText = false;
                                RaisePropertyChanged(() => NormResultItemses);
                            });
                        });
                }
            }
            catch (Exception e)
            {
                //ignore
            }

        }

        #endregion

        public ICommand PlotCommand => new DelegateCommand(PlotCommandAction);
    


        private void PlotCommandAction()
        {
            var chartView = new ColumnChartView();

            chartView.GetPieView(GetNormDataItems);
            chartView.GetChartView(GetNormDataItems);

            chartView.DiagramKg(GetNormDataItems, SelectFactListItem);

            chartView.Show();

        }

        #endregion

        public ICommand RemoveFactListItem => new DelegateCommand(RemoveFactListItemAction);

        private void RemoveFactListItemAction()
        {
            if (!RemoveFactItemCommand.Remove(_selectFactListItem.NameList)) return;
            FactResultItemses.Clear();

            FactResultItemses = GetFactCommand.GetFile();
        }

        public ICommand RemoveNormDataItem => new DelegateCommand(RemoveNormDataItemAction);

        private void RemoveNormDataItemAction()
        {
            if (!RemoveNormItemCommand.Remove(_getNormDataItems.NameList)) return;
            NormResultItemses.Clear();

            NormResultItemses = GetNormCommand.GetFile();
        }

    }
}
