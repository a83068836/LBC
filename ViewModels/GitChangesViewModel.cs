using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using HL.Interfaces;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LBC.ViewModels
{
    internal class GitChangesViewModel : ToolViewModel
    {
        #region fields
        public const string ToolContentId = "GitChanges";
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public GitChangesViewModel()
            : base("»’÷æ")
        {
            ContentId = ToolContentId;

            Document = new TextDocument();
            Document.FileName = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            var hlManager = Base.ModelBase.GetService<IThemedHighlightingManager>();
            HighlightDef = hlManager.GetDefinitionByExtension(".hpp");



        }

        public void Insert(string text)
        {
            Workspace.This.Git.Document.Insert(Document.TextLength, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + text + "\r\n");
        }
        #endregion constructors

        #region Properties
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
                }
            }
        }

        #region HighlightingDefinition

        private IHighlightingDefinition _highlightdef = null;
        public IHighlightingDefinition HighlightDef
        {
            get { return this._highlightdef; }
            set
            {
                if (this._highlightdef != value)
                {
                    this._highlightdef = value;
                    RaisePropertyChanged("HighlightDef");
                }
            }
        }
        #endregion
        #endregion Properties

        #region methods
        public void SetDockpanel(string text)
        {
            string pattern = @"\[@\w+\]\s+([\s\S]*?)(?=\s*\[@\w+\]|\s*$)";
            MatchCollection matches = Regex.Matches(text, pattern);

            foreach (Match match in matches)
            {
                string ss = match.Value + Environment.NewLine;
                if (ss.ToLower().IndexOf("#say") > -1)
                {
                    string[] strings = ss.Split("\r\n");
                    foreach (string s in strings)
                    {
                        if (s.ToLower().IndexOf("openmerchantbigdlg") > -1)
                        {
                            string[] splitStrings = Regex.Split(s, @"\s+");

                            foreach (string str in splitStrings)
                            {
                                //listBox.Items.Add(str);
                            }
                        }
                    }
                }
            }
        }

        #endregion methods

    }
}
