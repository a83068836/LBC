using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace LBC.Class
{
	public class WinApi
	{

		/// <summary>
		/// 填充COMBOBOX
		/// </summary>
		/// <param name="comboBoxHandle"></param>
		/// <param name="selectedIndex"></param>
		public static void SelectItemByHandle(IntPtr comboBoxHandle, int selectedIndex)
		{
			// 选择指定索引的项
			User32.SendMessage(comboBoxHandle, User32.CB_SETCURSEL, new IntPtr(selectedIndex), IntPtr.Zero);
		}
		/// <summary>
		/// 关闭窗口
		/// </summary>
		/// <param name="windowHandle">句柄</param>
		public static void CloseWindowByHandle(IntPtr windowHandle)
		{
			User32.SendMessage(windowHandle, User32.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
		}
		/// <summary>
		/// 给窗口发送内容
		/// </summary>
		/// <param name="hWnd">句柄</param>
		/// <param name="lParam">要发送的内容</param>
		public static void SetText(IntPtr hWnd, string lParam)
		{
			User32.SendMessage(hWnd, (uint)User32.WM_SETTEXT, (int)IntPtr.Zero, lParam);
		}
		/// <summary>
		/// 获取内容
		/// </summary>
		/// <param name="hWnd">句柄</param>
		/// <returns></returns>
		public static string GetText(IntPtr hWnd)
		{
			const int buffer_size = 256;
			StringBuilder buffer = new StringBuilder(buffer_size);

			User32.SendMessage(hWnd, User32.WM_GETTEXT, buffer_size, buffer);
			return buffer.ToString();
		}
		/// <summary>
		/// 句柄点击
		/// </summary>
		/// <param name="windowHandle">句柄</param>
		/// <param name="a">X坐标</param>
		/// <param name="b">Y坐标</param>
		public static void ClickAtLocation(IntPtr windowHandle, int x, int y)
		{
			User32.PostMessage(windowHandle, User32.WM_LBUTTONDOWN, 0, x + (y << 16));
			User32.PostMessage(windowHandle, User32.WM_LBUTTONUP, 0, x + (y << 16));
		}

		/// <summary>
		/// 枚举获取句柄
		/// </summary>
		/// <param name="parentHandle">句柄名称</param>
		/// <returns></returns>
		public static List<WindowInfo> GetChildWindows(IntPtr parentHandle)
		{
			List<WindowInfo> childWindows = new List<WindowInfo>();

			User32.EnumChildWindows(parentHandle, (hWnd, lParam) =>
			{
				StringBuilder classNameBuilder = new StringBuilder(256);
				User32.GetClassName(hWnd, classNameBuilder, classNameBuilder.Capacity);

				StringBuilder textBuilder = new StringBuilder(256);
				User32.GetWindowText(hWnd, textBuilder, textBuilder.Capacity);

				WindowInfo windowInfo = new WindowInfo
				{
					Handle = hWnd,
					ClassName = classNameBuilder.ToString(),
					Text = textBuilder.ToString()
				};

				childWindows.Add(windowInfo);

				return true;
			}, IntPtr.Zero);

			return childWindows;
		}

		/// <summary>
		/// 根据句柄获枚举出来的句柄获取句柄
		/// </summary>
		/// <param name="windowHandle">句柄</param>
		/// <param name="ClassName">类名</param>
		/// <param name="id">就是第几个，因为有些类名可能相同</param>
		/// <returns></returns>
		public static IntPtr GetChildWindowsTrue(IntPtr windowHandle, string ClassName, int id)
		{
			List<WindowInfo> childWindowHandles = WinApi.GetChildWindows(windowHandle);
			int ids = 0;
			foreach (var childHandle in childWindowHandles)
			{
				if (childHandle.ClassName == ClassName)
				{
					ids++;
					if (ids == id)
					{
						return childHandle.Handle;
					}
				}
			}
			return IntPtr.Zero;
		}
		public struct WindowInfo
		{
			public IntPtr Handle;
			public string ClassName;
			public string Text;
		}
	}
}
