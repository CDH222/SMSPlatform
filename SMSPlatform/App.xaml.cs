using SMSPlatform.common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SMSPlatform
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        Mutex mutex;
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }
        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new Mutex(true, "ElectronicNeedleTherapySystem", out ret);
            if (!ret)
            {
                MessageBox.Show("程序已在运行！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
                mutex.Close();
            }
            else
            {
                Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.Interval = 60000; //执行间隔时间,单位为毫秒; 这里实际间隔为1分钟
                timer.Start();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
            }
        }
        static void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int hour = int.Parse(ConfigurationManager.AppSettings["hour"]);
            int minute = int.Parse(ConfigurationManager.AppSettings["minute"]);
            // 得到 hour minute second  如果等于某个值就开始执行某个程序。
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            //每天执行时间
            if (intHour == hour && intMinute == minute)
            {
                SendBirthdaySMS sbs = new SendBirthdaySMS();
                sbs.Send();
            }
        }
        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        static void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("当前应用程序遇到一些问题，该操作已经终止，请重试。", "意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);//这里通常需要给用户一些较为友好的提示，并且后续可能的操作
            e.Handled = true;//使用这一行代码告诉运行时，该异常被处理了，不再作为UnhandledException抛出了。
        }
        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("当前应用程序遇到一些问题，该操作已经终止，请重试。" , "意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
