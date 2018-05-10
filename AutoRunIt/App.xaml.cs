using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AutoRunIt
{
    public partial class App
    {
        #region Fields and Properties

        private static Mutex _mutex = new Mutex(true, "{4A24562A-A5B3-423F-A5AB-DE50677468D4}");

        #endregion

        #region  Constructors

        public App()
        {
            DispatcherUnhandledException += (send, e) =>
            {
                LogHelper.LogError("Unhandled exception happen:", e.Exception);
                e.Handled = true;
            };

            CheckSinglonInstance(Environment.GetCommandLineArgs());

            Exit += (sender, e) =>
            {
                if (_mutex == null) return;
                _mutex.Close();
                _mutex = null;
            };
        }

        #endregion

        #region  Methods

        private void CheckSinglonInstance(IEnumerable<string> args)
        {
            //if (args.Contains("restart")) return;


            void Shutduow()
            {
                MessageBox.Show("已经再干活了！!", "禁止多开！", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }

            try
            {
                if (_mutex.WaitOne(TimeSpan.Zero, true)) _mutex.ReleaseMutex();
                else Shutduow();
            }
            catch (Exception)
            {
                Shutduow();
            }
        }

        #endregion
    }
}