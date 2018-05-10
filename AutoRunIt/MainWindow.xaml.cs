using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AutoRunIt
{
    public sealed partial class MainWindow : INotifyPropertyChanged
    {
        #region Fields and Properties

        private MenuItem _menuItemCloseMainWindow;
        private MenuItem _menuItemShowMainWindow;

        private NotifyIcon _notifyIcon;

        private static readonly SimpleClock SimpleClock = new SimpleClock(1);

        public string DateTimeNow =>
            SimpleClock.Clock.ToString(CultureInfo.InstalledUICulture.DateTimeFormat.FullDateTimePattern);

        #endregion

        #region  Constructors

        public MainWindow()
        {
            Hide();
            InitializeComponent();
            SetNotifyIcon();
            Closing += OnClosing;
            SimpleClock.ClockChanged += clock => OnPropertyChanged($"DateTimeNow");
        }

        #endregion

        #region  Methods

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Hide();
            cancelEventArgs.Cancel = true;
        }

        private void OpenFileDialogButtonOnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = Properties.Resources.MainWindow_ButtonBase_OnClick_exe_files____exe____exe_All_files__________,
                RestoreDirectory = true,
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ExePath = openFileDialog.FileName;
        }


        private void SetNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("app.ico"),
                Text = "",
                Visible = true,
                ContextMenu = new ContextMenu()
            };
            _notifyIcon.DoubleClick += (sender, args) => ShowAndActivateMainWindow();

            _menuItemShowMainWindow = new MenuItem
            {
                Text = Properties.Resources.MainWindow_SetNotifyIcon_打开程序,
                DefaultItem = true
            };
            _menuItemShowMainWindow.Click += (sender, args) => ShowAndActivateMainWindow();
            _menuItemCloseMainWindow = new MenuItem { Text = Properties.Resources.MainWindow_SetNotifyIcon_退出程序 };
            _menuItemCloseMainWindow.Click += (sender, args) => Process.GetCurrentProcess().Kill();

            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemShowMainWindow);
            _notifyIcon.ContextMenu.MenuItems.Add("-");
            _notifyIcon.ContextMenu.MenuItems.Add(_menuItemCloseMainWindow);
        }

        private void ShowAndActivateMainWindow()
        {
            Show();
            Activate();
        }

        private void StartButtonOnClick(object sender, RoutedEventArgs e)
        {
            IsModifyEnable = false;
            SimpleClock.ClockChanged += clock =>
            {
                Task.Factory.StartNew(() =>
                {
                    switch (clock.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            if ((WeekInt & 0b0000001) == 0b0000001 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Tuesday:
                            if ((WeekInt & 0b00000010) == 0b00000010 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Wednesday:
                            if ((WeekInt & 0b000000100) == 0b000000100 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Thursday:
                            if ((WeekInt & 0b0000001000) == 0b0000001000 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Friday:
                            if ((WeekInt & 0b00000010000) == 0b00000010000 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Saturday:
                            if ((WeekInt & 0b000000100000) == 0b000000100000 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                        case DayOfWeek.Sunday:
                            if ((WeekInt & 0b0000001000000) == 0b0000001000000 &&
                                clock.ToString("hh:mm:ss") == WhenToStart.ToString("hh:mm:ss"))
                                Process.Start(ExePath);
                            break;
                    }
                });
            };
        }

        #endregion

        #region string ExePath(INotifyPropertyChangedProperty)

        private string _ExePath;

        public string ExePath
        {
            get => _ExePath;
            set
            {
                if (_ExePath != null && _ExePath.Equals(value)) return;
                _ExePath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyProperties

        #region OnPropertyChanged(INotifyPropertyChanged)

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region int WeekInt(INotifyPropertyChangedProperty)

        private int _WeekInt = 0b1111_1111_1111_1111;

        public int WeekInt
        {
            get => _WeekInt;
            set
            {
                if (value == 0) return;

                var testPosition = 0b0000_0000_0000_0001 << (Math.Abs(value) - 1);
                if (value > 0)
                    _WeekInt = _WeekInt | testPosition;
                else if (value < 0)
                    _WeekInt = _WeekInt & ~testPosition;
                OnPropertyChanged();
            }
        }

        #endregion

        #region bool IsModifyEnable(INotifyPropertyChangedProperty)

        private bool _IsModifyEnable = true;

        public bool IsModifyEnable
        {
            get => _IsModifyEnable;
            set
            {
                if (_IsModifyEnable.Equals(value)) return;
                _IsModifyEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DateTime WhenToStart(INotifyPropertyChangedProperty)

        private DateTime _WhenToStart;

        public DateTime WhenToStart
        {
            get => _WhenToStart;
            set
            {
                if (_WhenToStart.Equals(value)) return;
                _WhenToStart = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion
    }
}