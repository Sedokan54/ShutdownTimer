using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using ModernWpf.Controls;

namespace ShutdownTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer countdownTimer;
        private DateTime targetTime;
        private bool isTimerRunning = false;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize timer
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            // Calculate total time
            double hours = nbHours.Value;
            double minutes = nbMinutes.Value;
            double seconds = nbSeconds.Value;

            int totalSeconds = (int)(hours * 3600 + minutes * 60 + seconds);

            if (totalSeconds == 0)
            {
                MessageBox.Show("Please enter a valid time!", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Set target time
            targetTime = DateTime.Now.AddSeconds(totalSeconds);

            // Update UI
            btnStart.IsEnabled = false;
            btnCancel.IsEnabled = true;
            miCancel.IsEnabled = true;
            CountdownPanel.Visibility = Visibility.Visible;

            // Reset countdown display color
            txtCountdown.Foreground = new SolidColorBrush(Colors.Black);

            // Start timer
            isTimerRunning = true;
            countdownTimer.Start();

            // Show notification
            TrayIcon.ShowBalloonTip("Shutdown Timer",
                $"Timer started. {GetActionText()} will be executed in {totalSeconds} seconds.",
                Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

            // Force UI update
            this.UpdateLayout();
        }

        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            if (!isTimerRunning) return;

            TimeSpan remaining = targetTime - DateTime.Now;

            if (remaining.TotalSeconds <= 0)
            {
                // Timer completed, execute action
                countdownTimer.Stop();
                ExecuteAction();
                ResetUI();
            }
            else
            {
                // Update countdown display
                string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)remaining.TotalHours, 
                    remaining.Minutes, 
                    remaining.Seconds);
                
                txtCountdown.Text = timeText;

                // Warning in last 10 seconds
                if (remaining.TotalSeconds <= 10)
                {
                    // Change color to red
                    txtCountdown.Foreground = new SolidColorBrush(Colors.Red);
                    
                    // Flash effect for last 10 seconds
                    if ((int)remaining.TotalSeconds % 2 == 0)
                    {
                        txtCountdown.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        txtCountdown.FontWeight = FontWeights.Normal;
                    }

                    // Play warning sound every 2 seconds in last 10 seconds
                    if ((int)remaining.TotalSeconds % 2 == 0)
                    {
                        try
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        catch
                        {
                            // Ignore sound errors
                        }
                    }

                    // Show balloon tip at 10 seconds
                    if ((int)remaining.TotalSeconds == 10)
                    {
                        TrayIcon.ShowBalloonTip("Shutdown Timer",
                            $"Warning! {GetActionText()} will execute in 10 seconds!",
                            Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning);
                    }
                }
                else
                {
                    // Normal display
                    txtCountdown.Foreground = new SolidColorBrush(Colors.Black);
                    txtCountdown.FontWeight = FontWeights.Bold;
                }

                // Update window title with remaining time
                this.Title = $"Shutdown Timer - {timeText}";
            }
        }

        private void ExecuteAction()
        {
            string command = "";
            string args = "";

            if (rbShutdown.IsChecked == true)
            {
                command = "shutdown";
                args = "/s /t 0 /f";
            }
            else if (rbRestart.IsChecked == true)
            {
                command = "shutdown";
                args = "/r /t 0 /f";
            }
            else if (rbSleep.IsChecked == true)
            {
                command = "rundll32.exe";
                args = "powrprof.dll,SetSuspendState 0,1,0";
            }

            try
            {
                // Show final warning
                TrayIcon.ShowBalloonTip("Shutdown Timer",
                    $"Executing {GetActionText()} now!",
                    Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning);

                // Wait a moment for the notification
                System.Threading.Thread.Sleep(1000);

                Process.Start(new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Operation could not be performed: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ResetUI();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelTimer();
        }

        private void CancelTimer()
        {
            if (isTimerRunning)
            {
                countdownTimer.Stop();
                isTimerRunning = false;
                ResetUI();

                TrayIcon.ShowBalloonTip("Shutdown Timer",
                    "Timer cancelled.",
                    Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
            }
        }

        private void ResetUI()
        {
            btnStart.IsEnabled = true;
            btnCancel.IsEnabled = false;
            miCancel.IsEnabled = false;
            CountdownPanel.Visibility = Visibility.Collapsed;
            txtCountdown.Text = "00:00:00";
            txtCountdown.Foreground = new SolidColorBrush(Colors.Black);
            txtCountdown.FontWeight = FontWeights.Bold;
            isTimerRunning = false;
            
            // Reset window title
            this.Title = "Shutdown Timer";
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();

                if (isTimerRunning)
                {
                    TrayIcon.ShowBalloonTip("Shutdown Timer",
                        "Application continues running in system tray.",
                        Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                }
            }
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowWindow_Click(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
            Focus();
        }

        private void CancelTimer_Click(object sender, RoutedEventArgs e)
        {
            CancelTimer();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (isTimerRunning)
            {
                var result = MessageBox.Show(
                    "There is an active timer. Are you sure you want to exit?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;
            }

            // Stop timer if running
            if (countdownTimer != null)
            {
                countdownTimer.Stop();
            }

            // Clean up system tray icon
            TrayIcon.Dispose();

            // Close application
            Application.Current.Shutdown();
        }

        private string GetActionText()
        {
            if (rbShutdown.IsChecked == true)
                return "Computer shutdown";
            else if (rbRestart.IsChecked == true)
                return "Computer restart";
            else if (rbSleep.IsChecked == true)
                return "Sleep mode";

            return "Unknown action";
        }
    }
}