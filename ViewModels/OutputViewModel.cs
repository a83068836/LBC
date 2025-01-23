using ICSharpCode.AvalonEdit;
using LBC.Class;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using TextEditLib.Class;

namespace LBC.ViewModels
{
	internal class OutputViewModel : ToolViewModel
	{
        #region fields
        public const string ToolContentId = "Output";
        public ObservableCollection<UIElement> _controls;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public OutputViewModel()
            : base("属性")
        {
            ContentId = ToolContentId;
            Controls = new ObservableCollection<UIElement>();
        }
        #endregion constructors

        #region Properties
        public ObservableCollection<UIElement> Controls
        {
            get => _controls;
            set
            {
                _controls = value;
                RaisePropertyChanged(nameof(Controls));
            }
        }

        #endregion Properties

        #region methods

        public void OnLoad(string lineText, int Offset)
        {
            Controls.Clear();
            List<Shuxing> shuxings = GetType(lineText);
            if (shuxings != null)
            {
                if (shuxings.Count > 0)
                {
                    id = 1;
                    Label label = new Label();
                    label.Content = labeltext;
                    Style labelStyle = (Style)Application.Current.FindResource("LabelDanger.Small");
                    label.Style = labelStyle;
                    label.HorizontalAlignment = HorizontalAlignment.Stretch;
                    label.Margin = new Thickness(5, 0, 5, 0);
                    Controls.Add(label);
                    foreach (var shuxing in shuxings)
                    {
                        AddTextBox(shuxing.Title, shuxing.Shuoming, new Offsetleng() { Offset = Offset + matchindex, leng = matchleng, text = matchtext }, shuxing.Name);
                    }
                }

            }
        }
        int matchindex = 0;
        int matchleng = 0;
        string matchtext = string.Empty;
        string labeltext = string.Empty;
        public List<Shuxing> GetType(string lineText)
        {
            List<Shuxing> result = new List<Shuxing>();
            string input = lineText.TrimStart('\t');
            foreach (var item in App.global.RegexLists)
            {
                if (input.ToLower().Contains(item.key.ToLower()))
                {
                    //string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int count = Global.CountSpaces(input);
                    string reg = string.Empty;
                    reg += @"(^|\s+)";
                    reg += item.key;
                    for (int i = 0; i < count; i++)
                    {
                        reg += @"\s+([\d]+|\S+)";
                    }
                    reg += @"(\s+|\t|\n|$)";
                    string pattern = reg;
                    Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                    matchindex = match.Index;
                    //matchleng = GetLengthWithChineseCharacters(match.Value);
                    matchleng = match.Value.Length;
                    oldleng = matchleng;
                    if (match.Success)
                    {
                        if (id == 1)
                        {
                            NewMethod(result, input, pattern, match, item.key);
                            return result;
                        }
                    }
                }
            }
            return new List<Shuxing>();
        }

        private void NewMethod(List<Shuxing> result, string input, string pattern, Match match, string mat)
        {
            var list = App.global.Globals_L_items.FirstOrDefault(x => x.name.ToLower() == mat.ToLower());
            if (list != null)
            {
                labeltext = list.displayName;
                var list1 = App.global.Globals_L_shuxing.Where(x => x.tid == list.Id).ToList();
                int counter = 1;
                int length1 = 0;
                int i = 0;
                matchtext = input;

                foreach (Group group in match.Groups)
                {
                    if (group.Value != match.Value && group.Value != "")
                    {
                        try
                        {
                            string groupValue = group.Value;
                            string shuomingValue;
                            shuomingValue = list1[i].shuoming.ToString();

                            result.Add(new Shuxing(groupValue, shuomingValue, list1[i].title));
                            i++;

                            int startIndex = group.Index + length1;
                            int length = group.Length;
                            string replacedValue = "参数" + counter;

                            length1 += replacedValue.Length - group.Length;
                            matchtext = ReplaceSubstringAtPosition(matchtext, startIndex, length, replacedValue);
                            counter++;
                        }
                        catch { }

                    }

                }
                matchtext = Regex.Match(matchtext, pattern, RegexOptions.IgnoreCase).Value;
            }
        }

        static string ReplaceSubstringAtPosition(string input, int startIndex, int length, string replacement)
        {
            if (startIndex >= 0 && startIndex < input.Length && length > 0 && startIndex + length <= input.Length)
            {
                string before = input.Substring(0, startIndex);
                string after = input.Substring(startIndex + length);
                return before + replacement + after;
            }
            return input;
        }
        int id = 1;
        public void AddTextBox(string text, string hint, Offsetleng offsetleng, string name)
        {

            HandyControl.Controls.TextBox textBox = new HandyControl.Controls.TextBox();
            textBox.Text = text;
            textBox.Tag = offsetleng;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.Margin = new Thickness(5);
            textBox.Name = "CCC" + offsetleng.Offset;
            textBox.TextChanged += TextBox_TextChanged;
            //HintAssist.SetHint(textBox, hint);
            Expander expander = new Expander();
            expander.Header = name;
            expander.Margin = new Thickness(5, 0, 5, 0);
            expander.IsExpanded = true;
            Style myStyle = new Style(typeof(Expander));
            myStyle.BasedOn = (Style)Application.Current.FindResource("Expander.Small");
            expander.Name = "AAA" + offsetleng.Offset;
            expander.Style = myStyle;
            expander.Content = textBox;
            ToolTip toolTip = new ToolTip();
            toolTip.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dadfe7"));
            toolTip.BorderThickness = new Thickness(3);
            toolTip.Placement = PlacementMode.Right;
            toolTip.PlacementTarget = expander;
            toolTip.Content = aaa(hint, hint);
            expander.ToolTip = toolTip;
            Controls.Add(expander);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox != null)
            {
                Expander expander = textBox.Parent as Expander;
                ToolTip toolTip = expander.ToolTip as ToolTip;
                if (toolTip != null)
                {
                    toolTip.IsOpen = false;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox != null)
            {
                Expander expander = textBox.Parent as Expander;
                ToolTip toolTip = expander.ToolTip as ToolTip;
                if (toolTip != null)
                {
                    toolTip.IsOpen = true;
                }
                textBox.SelectAll();
            }
        }

        public void setA()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dadfe7"));
            toolTip.BorderThickness = new Thickness(3);
            toolTip.Placement = PlacementMode.Right;

        }
        object aaa(string text, string obj)
        {
            string[] strings = Regex.Split(obj, @"\\\\n", RegexOptions.IgnoreCase);
            if (strings.Length == 0)
                return null;
            StackPanel stackPanel = new StackPanel();
            int i = 1;
            foreach (string s in strings)
            {
                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Orientation = Orientation.Horizontal;
                TextBlock block = new TextBlock();
                block.Text = s.ToUpper();
                block.FontWeight = FontWeights.Bold;
                //block.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fc8531"));
                block.HorizontalAlignment = HorizontalAlignment.Left;
                block.SetValue(Grid.RowProperty, 0);
                block.SetValue(Grid.ColumnProperty, 1);
                stackPanel1.Children.Add(block);
                stackPanel.Children.Add(stackPanel1);
                i++;
            }
            return stackPanel;
        }
        int oldleng = 0;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            string text = matchtext;
            int a = 1;
            foreach (Expander item in this.Controls.OfType<Expander>())
            {
                TextBox item1 = item.Content as TextBox;
                text = text.Replace("参数" + a, item1.Text);
                a++;
            }
            Workspace.This.ActiveDocument.Document.Replace(((Offsetleng)textBox.Tag).Offset, oldleng, text);
            oldleng = text.Length;
        }
        #endregion methods
        public static int GetLengthWithChineseCharacters(string text)
        {
            int length = 0;

            foreach (char c in text)
            {
                if (IsChineseCharacter(c))
                {
                    length += 2;
                }
                else
                {
                    length++;
                }
            }

            return length;
        }

        public static bool IsChineseCharacter(char c)
        {
            // 按照常见的汉字编码范围进行判断
            return c >= 0x4E00 && c <= 0x9FFF;
        }

    }
}
