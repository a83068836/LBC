using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;
using ICSharpCode.AvalonEdit;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TextEditLib.Class;
using TextEditLib.Enums;
using UnitComboLib.Models.Unit.Screen;
using UnitComboLib.Models.Unit;
using UnitComboLib.ViewModels;
using UnitComboLib.Command;
using LBC.LanguageHighlighting;
using HL.Interfaces;
using LBC.Class;

namespace LBC.ViewModels
{
    class FileViewModel : PaneViewModel
    {
        #region fields
        //private static ImageSourceConverter ISC = new ImageSourceConverter();
        private string _FilePath;

        private bool _IsDirty;
        private bool _IsReadOnly;
        private string _IsReadOnlyReason = string.Empty;

        private ICommand _HighlightingChangeCommand;
        private IHighlightingDefinition _HighlightingDefinition;

        private int _SynchronizedColumn, _SynchronizedLine;

        private Encoding _FileEncoding;
        private bool _WordWrap, _ShowLineNumbers;
        private ICommand _toggleEditorOptionCommand;
        private bool _IsContentLoaded;
        private ICommand _DisableHighlightingCommand;

        private readonly TextEditorOptions _TextOptions;
        private readonly TextDocument _Document;
        private RelayCommand _saveCommand = null;
        private RelayCommand _saveAsCommand = null;
        private RelayCommand _closeCommand = null;
        private RelayCommand _saveAllCommand = null;
        private RelayCommand _dragDropCommand;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor from file path.
        /// </summary>
        /// <param name="filePath"></param>
        public FileViewModel(string filePath)
        {
            inputTimer = new DispatcherTimer();
            inputTimer.Interval = TimeSpan.FromMilliseconds(500);
            inputTimer.Tick += InputTimer_Tick;
            inputTimer1 = new DispatcherTimer();
            inputTimer1.Interval = TimeSpan.FromMilliseconds(500);
            inputTimer1.Tick += InputTimer_Tick1;
            _ShowLineNumbers = true;
            Completions = CompletionDataT.GetCodeCompletions();
            _FileEncoding = Encoding.Default;
            FilePath = filePath;
            Title = FileName;
            //_Document = new TextDocument(string.Empty);

            var items = new ObservableCollection<UnitComboLib.Models.ListItem>(GenerateScreenUnitList());
            SizeUnitLabel = UnitComboLib.UnitViewModeService.CreateInstance(items, new ScreenConverter(), 0, 120);


            _TextOptions = new TextEditorOptions();
            _TextOptions.AllowToggleOverstrikeMode = true;

            //Set the icon only for open documents (just a test)
            //IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/document.png") as ImageSource;
        }

        /// <summary>
        /// Default class constructor
        /// </summary>
        public FileViewModel()
        {
            inputTimer = new DispatcherTimer();
            inputTimer.Interval = TimeSpan.FromMilliseconds(500);
            inputTimer.Tick += InputTimer_Tick;
            inputTimer1 = new DispatcherTimer();
            inputTimer1.Interval = TimeSpan.FromMilliseconds(500);
            inputTimer1.Tick += InputTimer_Tick1;
            _ShowLineNumbers = true;
            Completions = CompletionDataT.GetCodeCompletions();
            _FileEncoding = Encoding.Default;
            Document = new TextDocument(string.Empty);
            FilePath = "无标题.txt";

            HighlightingDefinition = null;
            var hlManager = LBC.ViewModels.Base.ModelBase.GetService<IThemedHighlightingManager>();
            string extension = System.IO.Path.GetExtension(".txt");
            HighlightingDefinition = hlManager.GetDefinitionByExtension(extension);
            IsContentLoaded = true;
            Title = FileName;


            var items = new ObservableCollection<UnitComboLib.Models.ListItem>(GenerateScreenUnitList());
            SizeUnitLabel = UnitComboLib.UnitViewModeService.CreateInstance(items, new ScreenConverter(), 0, 120);


            _TextOptions = new TextEditorOptions();
            _TextOptions.AllowToggleOverstrikeMode = true;

        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets/sets the AvalonEdit document object that contains the text edit
        /// information being displayed in the editor control (text backend storage).
        /// </summary>		
        private TextDocument _document = null;
        public TextDocument Document
        {
            get { return this._document; }
            set
            {
                if (this._document != value)
                {
                    this._document = value;
                    RaisePropertyChanged("Document");
                    IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets/sets whether the file has been changed (edited) and should
        /// therefore, be saved on exit, or not.
        /// </summary>		
        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    RaisePropertyChanged(nameof(IsDirty));
                    RaisePropertyChanged(nameof(FileName));
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// Gets whether the document content (text) is pesent or not.
        /// </summary>
        public bool IsContentLoaded
        {
            get { return _IsContentLoaded; }

            protected set
            {
                if (_IsContentLoaded != value)
                {
                    _IsContentLoaded = value;
                    RaisePropertyChanged(nameof(IsContentLoaded));
                }
            }
        }

        /// <summary>
        /// Gets/sets the path of the current file.
        /// </summary>		
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (_FilePath != value)
                {
                    _FilePath = value;
                    RaisePropertyChanged(nameof(FilePath));
                    RaisePropertyChanged(nameof(FileName));
                    RaisePropertyChanged(nameof(Title));
                    LoadDocument(FilePath);
                }
            }
        }

        #region Title
        /// <summary>
        /// Title is the string that is usually displayed - with or without dirty mark '*' - in the docking environment
        /// </summary>
        public string Title
        {
            get
            {
                return System.IO.Path.GetFileName(this.FilePath) + (this.IsDirty == true ? "*" : string.Empty);
            }

            set
            {
                base.Title = value;
            }
        }
        #endregion

        public string FileName
        {
            get
            {
                if (FilePath == null)
                    return "Noname" + (IsDirty ? "*" : "");

                return System.IO.Path.GetFileName(FilePath) + (IsDirty ? "*" : "");
            }
        }

        /// <summary>
        /// Gets/sets whether a file is readonly or not (can be edit and saved to).
        /// </summary>		
        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }

            protected set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    RaisePropertyChanged(nameof(IsReadOnly));
                }
            }
        }
        private IEnumerable<ICompletionData> _completions;
        public IEnumerable<ICompletionData> Completions
        {
            get { return _completions; }

            protected set
            {
                if (_completions != value)
                {
                    _completions = value;
                    RaisePropertyChanged(nameof(Completions));
                }
            }
        }

        #region Output属性参数
        private string _outputLineText;
        public string OutputLineText
        {
            get { return _outputLineText; }

            set
            {
                if (_outputLineText != value)
                {
                    _outputLineText = value;
                    RaisePropertyChanged(nameof(OutputLineText));
                }
            }
        }

        private int _outputOffset;
        public int OutputOffset
        {
            get { return _outputOffset; }

            set
            {
                if (_outputOffset != value)
                {
                    _outputOffset = value;
                    RaisePropertyChanged(nameof(OutputOffset));
                }
            }
        }

        private bool _outputbool1;
        public bool Outputbool1
        {
            get { return _outputbool1; }

            set
            {
                if (_outputbool1 != value)
                {
                    _outputbool1 = value;
                    RaisePropertyChanged(nameof(Outputbool1));
                }
            }
        }

        private bool _outputbool2;
        public bool Outputbool2
        {
            get { return _outputbool2; }

            set
            {
                if (_outputbool2 != value)
                {
                    _outputbool2 = value;
                    RaisePropertyChanged(nameof(Outputbool2));
                }
                CanSetPositionChanged();
            }
        }
        #endregion

        #region SetTextChanged
        RelayCommand<object> _setTextChanged = null;
        public ICommand SetTextChanged
        {
            get
            {
                _setTextChanged = new RelayCommand<object>((p) => OnSetTextChanged(), (p) => CanSetTextChanged());
                return _setTextChanged;
            }
        }
        RelayCommand<object> _setMouseHover = null;
        public ICommand SetMouseHover
        {
            get
            {
                _setMouseHover = new RelayCommand<object>((p) => OnSetMouseHover());
                return _setMouseHover;
            }
        }
        RelayCommand<object> _setMouseHoverStopped = null;
        public ICommand SetMouseHoverStopped
        {
            get
            {
                _setMouseHoverStopped = new RelayCommand<object>((p) => OnSetMouseHoverStopped());
                return _setMouseHoverStopped;
            }
        }
        RelayCommand<KeyEventArgs> _setPreviewKeyDown = null;
        public ICommand SetPreviewKeyDown
        {
            get
            {
                if (_setPreviewKeyDown == null)
                {
                    _setPreviewKeyDown = new RelayCommand<KeyEventArgs>(OnSetPreviewKeyDown);
                }

                return _setPreviewKeyDown;
            }
        }
        RelayCommand<object> _setPositionChanged = null;
        public ICommand SetPositionChanged
        {
            get
            {
                _setPositionChanged = new RelayCommand<object>((p) => OnSetPositionChanged(), (p) => CanSetPositionChanged());
                return _setPositionChanged;
            }
        }

        public bool CanSetPositionChanged()
        {
            lastInputTime = DateTime.Now;
            inputTimer1.Stop();
            inputTimer1.Start();
            return true; // 返回异步方法的结果
        }
        private void OnSetPositionChanged()
        {
            //ApplicationViewModel.This.Close(this);
        }

        public bool CanSetTextChanged()
        {
           
            return true; // 返回异步方法的结果
        }

        private void OnSetTextChanged()
        {
            // 记录当前时间
            lastInputTime = DateTime.Now;
            //if (Outputbool1)
            //{
            //    Workspace.This.Output.OnLoad(OutputLineText, OutputOffset);
            //}
            inputTimer.Stop();
            inputTimer.Start();
            Workspace.This.Navigation.AddHistoryItem(Backforward);
            //Workspace.This.Git.SetDockpanel(this.Document.Text);
        }

        private void OnSetMouseHover()
        {

        }
        private void OnSetMouseHoverStopped()
        {

        }
        private void OnSetPreviewKeyDown(KeyEventArgs e)
        {
            var toolTipKey = e.Key;
            if (Document.FileName != null)
            {
                if (toolTipLang.id == 1 && toolTipKey == Key.F12)
                {
                    var fileViewModel = Workspace.This.Open(toolTipLang.text);
                    Workspace.This.ActiveDocument = fileViewModel;
                }
                if (toolTipLang.id == 2 && toolTipKey == Key.F12)
                {
                    if (File.Exists(toolTipLang.text))
                    {
                        var fileViewModel = Workspace.This.Open(toolTipLang.text);
                        Workspace.This.ActiveDocument = fileViewModel;
                    }
                    else
                    {
                        if (!Directory.Exists(System.IO.Path.GetDirectoryName(toolTipLang.text)))
                        {
                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(toolTipLang.text));
                        }
                        FileHelpers.WriteTextToFile(toolTipLang.text, ";本程序由代码生成器自动生成\r\n[@main]\r\n#if\r\n#act\r\n#say", Encoding.GetEncoding("GB2312"));
                        var fileViewModel = Workspace.This.Open(toolTipLang.text);
                        Workspace.This.ActiveDocument = fileViewModel;
                    }

                }

            }
        }
        private void SetTooltip(string TT, MouseEventArgs e)
        {

        }

        private DispatcherTimer inputTimer;
        private DispatcherTimer inputTimer1;
        private DateTime lastInputTime;
        private bool defaultTimer = true;
        private void InputTimer_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop();
            Workspace.This.Props.FoldingManagerUpdateAsync(this._document.Text);

            // 在停止输入时执行的操作
        }
        private void InputTimer_Tick1(object sender, EventArgs e)
        {
            if (defaultTimer)
            {
                if (Outputbool1)
                {
                    Workspace.This.Output.OnLoad(OutputLineText, OutputOffset);
                }
                if (Outputbool2 == false)
                {
                    Workspace.This.Output.OnLoad(OutputLineText, OutputOffset);
                }
                defaultTimer=false;
            }
            TimeSpan interval = DateTime.Now - lastInputTime;
            if (interval.TotalMilliseconds >= 500)
            {
                if (Outputbool1)
                {
                    Workspace.This.Output.OnLoad(OutputLineText, OutputOffset);
                }
                if (Outputbool2 == false)
                {
                    Workspace.This.Output.OnLoad(OutputLineText, OutputOffset);
                }
            }
              
            inputTimer1.Stop();
            // 在停止输入时执行的操作
        }
        #endregion

        #region ScrollLine

        private FoldInfo _scrollLine = new FoldInfo();
        public FoldInfo ScrollLine
        {
            get { return this._scrollLine; }
            set
            {
                if (this._scrollLine != value)
                {
                    this._scrollLine = value;
                    RaisePropertyChanged("ScrollLine");
                }
            }
        }

        private int _caretOffset;
        public int CaretOffset
        {
            get { return _caretOffset; }
            set
            {
                if (_caretOffset != value)
                {
                    _caretOffset = value;
                    RaisePropertyChanged("CaretOffset");
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets/sets a humanreadle string that describe why a file may not be edible
        /// if it appears to be available in readonly mode, only.
        /// </summary>		
        public string IsReadOnlyReason
        {
            get
            {
                return _IsReadOnlyReason;
            }

            protected set
            {
                if (_IsReadOnlyReason != value)
                {
                    _IsReadOnlyReason = value;
                    RaisePropertyChanged(nameof(IsReadOnlyReason));
                }
            }
        }

        #region Highlighting Definition
        /// <summary>
        /// Gets a copy of all highlightings.
        /// </summary>
        public ReadOnlyCollection<IHighlightingDefinition> HighlightingDefinitions
        {
            get
            {
                var hlManager = Base.ModelBase.GetService<IThemedHighlightingManager>();

                if (hlManager != null)
                    return hlManager.HighlightingDefinitions;

                return null;
            }
        }

        /// <summary>
        /// AvalonEdit exposes a Highlighting property that controls whether keywords,
        /// comments and other interesting text parts are colored or highlighted in any
        /// other visual way. This property exposes the highlighting information for the
        /// text file managed in this viewmodel class.
        /// </summary>
        public IHighlightingDefinition HighlightingDefinition
        {
            get
            {
                return _HighlightingDefinition;
            }

            set
            {
                if (_HighlightingDefinition != value)
                {
                    _HighlightingDefinition = value;
                    RaisePropertyChanged(nameof(HighlightingDefinition));
                }
            }
        }

        /// <summary>
        /// Gets a command that changes the currently selected syntax highlighting in the editor.
        /// </summary>
        public ICommand HighlightingChangeCommand
        {
            get
            {
                if (_HighlightingChangeCommand == null)
                {
                    _HighlightingChangeCommand = new RelayCommand<object>((p) =>
                    {
                        var parames = p as object[];

                        if (parames == null)
                            return;

                        if (parames.Length != 1)
                            return;

                        var param = parames[0] as IHighlightingDefinition;
                        if (param == null)
                            return;

                        HighlightingDefinition = param;
                    });
                }

                return _HighlightingChangeCommand;
            }
        }

        /// <summary>
        /// Gets a command that turns off editors syntax highlighting.
        /// </summary>
        public ICommand DisableHighlightingCommand
        {
            get
            {
                if (_DisableHighlightingCommand == null)
                {
                    _DisableHighlightingCommand = new RelayCommand<object>(
                        (p) => { HighlightingDefinition = null; },
                        (p) =>
                        {
                            if (HighlightingDefinition != null)
                                return true;

                            return false;
                        }
                        );
                }

                return _DisableHighlightingCommand;
            }
        }
        #endregion Highlighting Definition


        /// <summary>
        /// Gets a scale viewmodel of the text in percentages of the font size
        /// See https://github.com/Dirkster99/UnitComboLib for more details.
        /// </summary>
        public IUnitViewModel SizeUnitLabel { get; }

        #region Synchronized Caret Position
        /// <summary>
        /// Gets/sets the caret positions column from the last time when the
        /// caret position in the left view has been synchronzied with the right view (or vice versa).
        /// </summary>
        public int SynchronizedColumn
        {
            get
            {
                return _SynchronizedColumn;
            }

            set
            {
                if (_SynchronizedColumn != value)
                {
                    _SynchronizedColumn = value;
                    RaisePropertyChanged(nameof(SynchronizedColumn));
                }
            }
        }

        /// <summary>
        /// Gets/sets the caret positions line from the last time when the
        /// caret position in the left view has been synchronzied with the right view (or vice versa).
        /// </summary>
        public int SynchronizedLine
        {
            get
            {
                return _SynchronizedLine;
            }

            set
            {
                if (_SynchronizedLine != value)
                {
                    _SynchronizedLine = value;
                    RaisePropertyChanged(nameof(SynchronizedLine));
                }
            }
        }
        #endregion Synchronized Caret Position

        /// <summary>
        /// Get/set file encoding of current text file.
        /// </summary>
        public Encoding FileEncoding
        {
            get { return _FileEncoding; }

            protected set
            {
                if (!Equals(_FileEncoding, value))
                {
                    _FileEncoding = value;
                    RaisePropertyChanged(nameof(FileEncoding));
                    RaisePropertyChanged(nameof(FileEncodingDescription));
                }
            }
        }

        /// <summary>
        /// Gets descriptive and human readable string of the file encoding used to load the data.
        /// </summary>
        public string FileEncodingDescription
        {
            get
            {
                return
                    string.Format("{0}, Header: {1} Body: {2}",
                    _FileEncoding.EncodingName, _FileEncoding.HeaderName, _FileEncoding.BodyName);
            }
        }

        /// <summary>
        /// Gets a command to toggle common editor options (such as WordWrap, Show Line Numbers).
        /// </summary>
        public ICommand ToggleEditorOptionCommand
        {
            get
            {
                return _toggleEditorOptionCommand ??
                    (_toggleEditorOptionCommand = new RelayCommand<ToggleEditorOption>
                       ((p) => OnToggleEditorOption(p),
                        (p) => { return OnToggleEditorOptionCanExecute(p); }
                       )
                    );
            }
        }

        /// <summary>
        /// Get/set whether word wrap is currently activated or not.
        /// </summary>
        public bool WordWrap
        {
            get { return _WordWrap; }

            set
            {
                if (_WordWrap != value)
                {
                    _WordWrap = value;
                    RaisePropertyChanged(nameof(WordWrap));
                }
            }
        }

        /// <summary>
        /// Get/set whether line numbers are currently shown or not.
        /// </summary>
        public bool ShowLineNumbers
        {
            get { return _ShowLineNumbers; }

            set
            {
                if (_ShowLineNumbers != value)
                {
                    _ShowLineNumbers = value;
                    RaisePropertyChanged(nameof(ShowLineNumbers));
                }
            }
        }

        /// <summary>
        /// Get/set whether the end of each line is currently shown or not.
        /// </summary>
        public bool ShowEndOfLine               // Toggle state command
        {
            get { return TextOptions.ShowEndOfLine; }

            set
            {
                if (TextOptions.ShowEndOfLine != value)
                {
                    TextOptions.ShowEndOfLine = value;
                    RaisePropertyChanged(nameof(ShowEndOfLine));
                }
            }
        }

        /// <summary>
        /// Get/set whether the spaces are highlighted or not.
        /// </summary>
        public bool ShowSpaces               // Toggle state command
        {
            get { return TextOptions.ShowSpaces; }

            set
            {
                if (TextOptions.ShowSpaces != value)
                {
                    TextOptions.ShowSpaces = value;
                    RaisePropertyChanged(nameof(ShowSpaces));
                }
            }
        }

        private LangClass _toolTipLang;
        public LangClass toolTipLang               // Toggle state command
        {
            get { return _toolTipLang; }

            set
            {
                if (_toolTipLang != value)
                {
                    _toolTipLang = value;
                    RaisePropertyChanged(nameof(toolTipLang));
                }
            }
        }
        private HistoryItem _Backforward;
        public HistoryItem Backforward               // Toggle state command
        {
            get { return _Backforward; }

            set
            {
                if (_Backforward != value)
                {
                    _Backforward = value;
                    RaisePropertyChanged(nameof(Backforward));
                }
            }
        }
        
        /// <summary>
        /// Get/set whether the tabulator characters are highlighted or not.
        /// </summary>
        public bool ShowTabs               // Toggle state command
        {
            get { return TextOptions.ShowTabs; }

            set
            {
                if (TextOptions.ShowTabs != value)
                {
                    TextOptions.ShowTabs = value;
                    RaisePropertyChanged(nameof(ShowTabs));
                }
            }
        }

        /// <summary>
        /// Get/Set texteditor options from <see cref="AvalonEdit"/> editor as <see cref="TextEditorOptions"/> instance.
        /// </summary>
        public TextEditorOptions TextOptions
        {
            get { return _TextOptions; }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Loads a text document from the persistance of the file system
        /// and updates all corresponding states in this viewmodel.
        /// </summary>
        /// <param name="paramFilePath"></param>
        /// <returns></returns>
        internal bool LoadDocument(string paramFilePath)
        {
            if (File.Exists(paramFilePath))
            {
                this._document = new TextDocument();
                this.IsDirty = false;
                IsReadOnly = false;

                // Check file attributes and set to read-only if file attributes indicate that
                if ((System.IO.File.GetAttributes(paramFilePath) & FileAttributes.ReadOnly) != 0)
                {
                    IsReadOnly = true;
                    IsReadOnlyReason = "This file cannot be edit because another process is currently writting to it.\n" +
                                       "Change the file access permissions or save the file in a different location if you want to edit it.";
                }

                try
                {
                    var fileEncoding = FileEncodings.GetType(paramFilePath);

                    using (FileStream fs = new FileStream(paramFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader reader = FileReader.OpenStream(fs, fileEncoding))
                        {
                            this._document = new TextDocument(reader.ReadToEnd());
                            FileEncoding = reader.CurrentEncoding; // assign encoding after ReadToEnd() so that the StreamReader can autodetect the encoding
                        }
                    }

                    //FilePath = paramFilePath;
                    IsContentLoaded = true;

                    // Setting this to null and then to some useful value ensures that the Foldings work
                    // Installing Folding Manager is invoked via HighlightingChange
                    // (so this works even when changing from test.XML to test1.XML)
                    HighlightingDefinition = null;
                    var hlManager = Base.ModelBase.GetService<IThemedHighlightingManager>();

                    //hlManager.SetCurrentTheme("Dark");

                    string extension = System.IO.Path.GetExtension(paramFilePath);
                    HighlightingDefinition = hlManager.GetDefinitionByExtension(extension);
                    ContentId = _FilePath;
                    this._document.FileName = paramFilePath;
                }
                catch (System.Exception exc)
                {
                    IsReadOnly = true;
                    IsReadOnlyReason = exc.Message;
                    Document.Text = string.Empty;

                    FilePath = string.Empty;
                    IsContentLoaded = false;
                    HighlightingDefinition = null;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// 
        /// source: https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filename)
        {

            // Read the BOM
            var bom = new byte[4];

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
                return Encoding.UTF7;

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                return Encoding.UTF8;

            if (bom[0] == 0xff && bom[1] == 0xfe)
                return Encoding.Unicode; //UTF-16LE

            if (bom[0] == 0xfe && bom[1] == 0xff)
                return Encoding.BigEndianUnicode; //UTF-16BE

            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
                return Encoding.UTF32;

            return Encoding.Default;
        }

        /// <summary>
        /// Initialize Scale View with useful units in percent and font point size
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UnitComboLib.Models.ListItem> GenerateScreenUnitList()
        {
            List<UnitComboLib.Models.ListItem> unitList = new List<UnitComboLib.Models.ListItem>();

            var percentDefaults = new ObservableCollection<string>() { "25", "50", "75", "100", "125", "150", "175", "200", "300", "400", "500" };
            var pointsDefaults = new ObservableCollection<string>() { "3", "6", "8", "9", "10", "12", "14", "16", "18", "20", "24", "26", "32", "48", "60" };

            unitList.Add(new UnitComboLib.Models.ListItem(Itemkey.ScreenPercent, "Percent", "%", percentDefaults));
            unitList.Add(new UnitComboLib.Models.ListItem(Itemkey.ScreenFontPoints, "Point", "pt", pointsDefaults));

            return unitList;
        }

        #region ToggleEditorOption
        private void OnToggleEditorOption(object parameter)
        {
            if (parameter == null)
                return;

            if ((parameter is ToggleEditorOption) == false)
                return;

            ToggleEditorOption t = (ToggleEditorOption)parameter;

            switch (t)
            {
                case ToggleEditorOption.WordWrap:
                    this.WordWrap = !this.WordWrap;
                    break;

                case ToggleEditorOption.ShowLineNumber:
                    this.ShowLineNumbers = !this.ShowLineNumbers;
                    break;

                case ToggleEditorOption.ShowSpaces:
                    this.TextOptions.ShowSpaces = !this.TextOptions.ShowSpaces;
                    break;

                case ToggleEditorOption.ShowTabs:
                    this.TextOptions.ShowTabs = !this.TextOptions.ShowTabs;
                    break;

                case ToggleEditorOption.ShowEndOfLine:
                    this.TextOptions.ShowEndOfLine = !this.TextOptions.ShowEndOfLine;
                    break;

                default:
                    break;
            }
        }

        private bool OnToggleEditorOptionCanExecute(object parameter)
        {
            if (parameter == null)
                return false;

            if (parameter is ToggleEditorOption)
                return true;

            return false;
        }
        #endregion ToggleEditorOption

        /// <summary>
        /// Invoke this method to apply a change of theme to the content of the document
        /// (eg: Adjust the highlighting colors when changing from "Dark" to "Light"
        ///      WITH current text document loaded.)
        /// </summary>
        internal void OnAppThemeChanged(IThemedHighlightingManager hlManager)
        {

            if (hlManager == null)
                return;

            // Does this highlighting definition have an associated highlighting theme?
            if (hlManager.CurrentTheme.HlTheme != null)
            {
                // A highlighting theme with GlobalStyles?
                // Apply these styles to the resource keys of the editor
                //foreach (var item in hlManager.CurrentTheme.HlTheme.GlobalStyles)
                //{
                //	switch (item.TypeName)
                //	{
                //		case "DefaultStyle":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorBackground, item.backgroundcolor);
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorForeground, item.foregroundcolor);
                //			break;

                //		case "CurrentLineBackground":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorCurrentLineBackgroundBrushKey, item.backgroundcolor);
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorCurrentLineBorderBrushKey, item.bordercolor);
                //			break;

                //		case "LineNumbersForeground":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorLineNumbersForeground, item.foregroundcolor);
                //			break;

                //		case "Selection":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorSelectionBrush, item.backgroundcolor);
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorSelectionBorder, item.bordercolor);
                //			break;

                //		case "Hyperlink":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorLinkTextBackgroundBrush, item.backgroundcolor);
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorLinkTextForegroundBrush, item.foregroundcolor);
                //			break;

                //		case "NonPrintableCharacter":
                //			ApplyToDynamicResource(TextEditLib.Themes.ResourceKeys.EditorNonPrintableCharacterBrush, item.foregroundcolor);
                //			break;

                //		default:
                //			throw new System.ArgumentOutOfRangeException("GlobalStyle named '{0}' is not supported.", item.TypeName);
                //	}
                //}
            }

            // 1st try: Find highlighting based on currently selected highlighting
            // The highlighting name may be the same as before, but the highlighting theme has just changed
            if (HighlightingDefinition != null)
            {
                // Reset property for currently select highlighting definition
                HighlightingDefinition = hlManager.GetDefinition(HighlightingDefinition.Name);

                if (HighlightingDefinition != null)
                    return;
            }

            // 2nd try: Find highlighting based on extension of file currenlty being viewed
            if (string.IsNullOrEmpty(FilePath))
                return;

            string extension = System.IO.Path.GetExtension(FilePath);

            if (string.IsNullOrEmpty(extension))
                return;

            // Reset property for currently select highlighting definition
            HighlightingDefinition = hlManager.GetDefinitionByExtension(extension);
        }

        /// <summary>
        /// Re-define an existing <seealso cref="SolidColorBrush"/> and backup the originial color
        /// as it was before the application of the custom coloring.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newColor"></param>
        private void ApplyToDynamicResource(ComponentResourceKey key, Color? newColor)
        {
            if (Application.Current.Resources[key] == null || newColor == null)
                return;

            // Re-coloring works with SolidColorBrushs linked as DynamicResource
            if (Application.Current.Resources[key] is SolidColorBrush)
            {
                //backupDynResources.Add(resourceName);

                var newColorBrush = new SolidColorBrush((Color)newColor);
                newColorBrush.Freeze();

                Application.Current.Resources[key] = newColorBrush;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand((p) => OnSave(p), (p) => CanSave(p));
                }

                return _saveCommand;
            }
        }

        public ICommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                {
                    _saveAsCommand = new RelayCommand((p) => OnSaveAs(p), (p) => CanSaveAs(p));
                }

                return _saveAsCommand;
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand((p) => OnClose(), (p) => CanClose());
                }

                return _closeCommand;
            }
        }

        public ICommand SaveAllCommand
        {
            get
            {
                if (_saveAllCommand == null)
                {
                    _saveAllCommand = new RelayCommand((p) => OnSaveAll(), (p) => CanSaveAll());
                }

                return _saveAllCommand;
            }
        }
        private bool CanSaveAll()
        {
            return true;
        }

        private void OnSaveAll()
        {
            Workspace.This.SaveAll();
        }

        private bool CanClose()
        {
            return true;
        }

        private void OnClose()
        {
            Workspace.This.Close(this);
        }

        private bool CanSave(object parameter)
        {
            return IsDirty;
        }

        private void OnSave(object parameter)
        {
            Workspace.This.Save(this, false);
        }

        private bool CanSaveAs(object parameter)
        {
            return IsDirty;
        }

        private void OnSaveAs(object parameter)
        {
            Workspace.This.Save(this, true);
        }
        #endregion methods

        #region 注释，取消注释
        private RelayCommand _commentCommand = null;
        private RelayCommand _unCommentCommand = null;
        public ICommand CommentCommand
        {
            get
            {
                if (_commentCommand == null)
                {
                    _commentCommand = new RelayCommand((p) => OnComment(p));
                }

                return _commentCommand;
            }
        }
        public ICommand UncommentCommand
        {
            get
            {
                if (_unCommentCommand == null)
                {
                    _unCommentCommand = new RelayCommand((p) => OnUncomment(p));
                }

                return _unCommentCommand;
            }
        }

        private bool _isTextCommented;

        public bool IsTextCommented
        {
            get { return _isTextCommented; }
            set
            {
                if (_isTextCommented != value)
                {
                    _isTextCommented = value;
                    RaisePropertyChanged(nameof(IsTextCommented));
                }
            }
        }
        private bool _isUnTextCommented;

        public bool IsUnTextCommented
        {
            get { return _isUnTextCommented; }
            set
            {
                if (_isUnTextCommented != value)
                {
                    _isUnTextCommented = value;
                    RaisePropertyChanged(nameof(IsUnTextCommented));
                }
            }
        }

        private void OnComment(object parameter)
        {
            // 执行注释逻辑
            if (IsTextCommented)
                IsTextCommented = false;
            else
                IsTextCommented = true;
        }

        private void OnUncomment(object parameter)
        {
            // 执行取消注释逻辑
            if (IsUnTextCommented)
                IsUnTextCommented = false;
            else
                IsUnTextCommented = true;
        }
        #endregion

        #region 按钮保存
        RelayCommand<KeyEventArgs> _setKeyDownChanged = null;
        public ICommand SetKeyDownChanged
        {
            get
            {
                if (_setKeyDownChanged == null)
                {
                    _setKeyDownChanged = new RelayCommand<KeyEventArgs>((p) => OnSetKeyDownChanged(p), (p) => CanSetKeyDownChanged(p));
                }

                return _setKeyDownChanged;
            }
        }

        private bool CanSetKeyDownChanged(KeyEventArgs e)
        {
            return true;
        }

        private void OnSetKeyDownChanged(KeyEventArgs e)
        {
            if ((e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)) && e.KeyboardDevice.IsKeyDown(Key.S))
            {
                Workspace.This.Save(this, false);
            }
        }
        #endregion

        #region 拖拽
        public RelayCommand DragDropCommand
        {
            get
            {
                if (_dragDropCommand == null)
                {
                    _dragDropCommand = new RelayCommand(OnDragDrop, CanDragDrop);
                }

                return _dragDropCommand;
            }
        }

        private void OnDragDrop(object parameter)
        {
            DragEventArgs e = parameter as DragEventArgs;
            if (e != null)
            {
                // 在这里定义 DragDropCommand 被执行时的逻辑，可以使用 e 参数
            }
        }

        private bool CanDragDrop(object parameter)
        {
            DragEventArgs e = parameter as DragEventArgs;
            //if (e != null)
            //{
            //	// 在这里定义 DragDropCommand 是否可执行的判断逻辑，可以使用 e 参数
            //	return true; // 或者根据相关条件返回 true 或 false
            //}
            //return false; // 如果参数不是 DragEventArgs，则不可执行

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string[] allFilePaths = FileHelpers.GetAllFilePaths(files);
                foreach (string filePath in allFilePaths)
                {
                    var fileViewModel = Workspace.This.Open(filePath);
                    Workspace.This.ActiveDocument = fileViewModel;
                }
            }
            return false;
        }
        #endregion
    }
    #region 获取文件编码
    public class FileEncodings
    {
        /// <summary> 
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
        /// </summary> 
        /// <param name=“FILE_NAME“>文件路径</param> 
        /// <returns>文件的编码类型</returns> 
        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        public static System.Text.Encoding GetType(FileStream fs)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding reVal = Encoding.GetEncoding("gb2312");
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;

        }


    }

    #endregion
}
