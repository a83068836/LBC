using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LBC
{
    public static class AttachedProperties
    {
        // 注册一个附加属性
        public static readonly DependencyProperty MySecondParameterProperty =
            DependencyProperty.RegisterAttached(
                "MySecondParameter",
                typeof(object),
                typeof(AttachedProperties),
                new PropertyMetadata(null));

        // 提供设置附加属性的方法
        public static void SetMySecondParameter(DependencyObject element, object value)
        {
            element.SetValue(MySecondParameterProperty, value);
        }

        // 提供获取附加属性的方法
        public static object GetMySecondParameter(DependencyObject element)
        {
            return element.GetValue(MySecondParameterProperty);
        }
    }
}
