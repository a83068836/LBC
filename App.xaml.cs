using AutoUpdaterDotNET;
using LBC.Class;
using LBC.ViewModels;
using LBC.Views;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using ThemedDemo;

namespace LBC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Global global;
        public static int expirationTime;
        private TokenInfo tokenInfo;
        //public static TaskbarIcon TaskbarIcon;
        private MainView _mainWindow = null;
        public static LBC.Class.WebSocketTest WebSocketTest;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 注册编码提供程序以支持 GB2312 编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        static App()
        {
            ServiceInjector.InjectServices();
            global = new Global();
           
            // 设置更新源的URL
            AutoUpdater.AppCastURL = "https://lcb-1322323324.cos.ap-nanjing.myqcloud.com/AutoUpdaterStarter.xml";

            // 设置其他选项，如是否显示更新日志等
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            // 检查更新
            AutoUpdater.Start();
        }
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            Application.Current.MainWindow = _mainWindow = new MainView();
            MainWindow.DataContext = Workspace.This;
            //Sql sql = new Sql();
            //sql.Show();
            //return;
            if (MainWindow != null && Workspace.This != null)
            {
                Dispatcher.InvokeAsync(() => MainWindow.Show(), DispatcherPriority.ApplicationIdle);
                WebSocketTest = new WebSocketTest();
            }
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //MessageBox.Show("发生错误！请联系支持部门。" + Environment.NewLine + e.Exception.Message);
            MessageBox.Show("Error encountered! Details: " + Environment.NewLine +
                   "Exception: " + e.Exception.GetType().Name + Environment.NewLine +
                   "Message: " + e.Exception.Message + Environment.NewLine +
                   "Stack Trace: " + e.Exception.StackTrace);
            Shutdown(1);
            e.Handled = true;
        }
    }

}
