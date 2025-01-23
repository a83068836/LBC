using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TextEditLib.Class;

namespace LBC.LanguageHighlighting
{
	//public class MyCompletionData : ICompletionData
	//{
	//	public string Text { get; }
	//	public object Content => Text;
	//	public object Description { get; }

	//	public ImageSource Image { get; }

	//	public double Priority { get; set; }

	//	public List<Shuxing> Shuxing { get; set; } = new List<Shuxing>();

	//	public MyCompletionData(string text, string description, ImageSource imageSource, double priority, List<Shuxing> shuxings)
	//	{
	//		Text = text;
	//		Description = description;
	//		Image = imageSource;
	//		Priority = priority;
	//		Shuxing = shuxings;
	//	}

	//	public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
	//	{
	//		// 在代码编辑器中插入代码提示项的文本
	//		textArea.Document.Replace(completionSegment, Text);
	//	}
	//}
	public class CompletionDataT
	{
		public static IEnumerable<ICompletionData> GetCodeCompletions()
		{

			List<ICompletionData> completions = new List<ICompletionData>();
			foreach (var items in App.global.Globals_L_items)
			{
				var filteredList = App.global.Globals_L_shuxing.Where(shuxingItem => shuxingItem.tid == items.Id).ToList();
				List<Shuxing> shuxings = new List<Shuxing>();
				foreach (var shuxing1 in filteredList)
				{
					shuxings.Add(new Shuxing(shuxing1.title, shuxing1.shuoming,shuxing1.title));
				}
				var my = new MyCompletionData(items.name,items.geshi,new BitmapImage(new Uri("pack://application:,,,/TextEditLib;component/Themes/class_libraries.png", UriKind.RelativeOrAbsolute)),0, shuxings,items.displayName);
				
				completions.Add(my);
			}
			return completions;
		}

		//public class CompletionT
		//{
		//	public string name { get; set; }
		//	public double Priority { get; set; }
		//}
	}
	public class Offset
	{
		public int startOffset { get; set; }
		public int endOffset { get; set; }
		//public Offset(int start, int end)
		//{
		//    startOffset = start;
		//    endOffset = end;
		//}
	}
}
