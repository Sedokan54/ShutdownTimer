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
        private DateTime startTime;
        private bool isTimerRunning = false;
        private int totalSeconds = 0;

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

            totalSeconds = (int)(hours * 3600 + minutes * 60 + seconds);

            if (totalSeconds == 0)
            {
                MessageBox.Show("Please enter a valid time!", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Set target time
            targetTime = DateTime.Now.AddSeconds(totalSeconds);
            startTime = DateTime.Now;

            // Switch panels - Hide duration and control buttons, show countdown
            DurationPanel.Visibility = Visibility.Collapsed;
            ControlButtonsPanel.Visibility = Visibility.Collapsed;
            CountdownPanel.Visibility = Visibility.Visible;

            // Update UI - Keep cancel button visible and enabled
            btnStart.IsEnabled = false;
            btnCancel.IsEnabled = true;
            btnCancel.Visibility = Visibility.Visible;
            miCancel.IsEnabled = true;

            // Disable action selection during countdown
            rbShutdown.IsEnabled = false;
            rbRestart.IsEnabled = false;
            rbSleep.IsEnabled = false;

            // Reset countdown display
            txtCountdown.Foreground = this.Foreground;
            progressTimer.Value = 100;
            txtTimeInfo.Text = "Timer running";

            // Start timer
            isTimerRunning = true;
            countdownTimer.Start();

            // Show notification
            TrayIcon.ShowBalloonTip("Shutdown Timer",
                $"Timer started. {GetActionText()} will be executed in {FormatTime(totalSeconds)}.",
                Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

            // Force UI update
            this.UpdateLayout();
        }

        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            if (!isTimerRunning) return;

            TimeSpan remaining = targetTime - DateTime.Now;
            TimeSpan elapsed = DateTime.Now - startTime;

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

                // Update progress bar
                double progressPercentage = (remaining.TotalSeconds / totalSeconds) * 100;
                progressTimer.Value = Math.Max(0, progressPercentage);

                // Warning in last 10 seconds
                if (remaining.TotalSeconds <= 10)
                {
                    // Change color to red
                    txtCountdown.Foreground = Brushes.Red;
                    
                    // Flash effect for last 10 seconds
                    if ((int)remaining.TotalSeconds % 2 == 0)
                    {
                        txtCountdown.FontWeight = FontWeights.ExtraBold;
                        txtTimeInfo.Text = "⚠️ WARNING! Action will execute soon!";
                    }
                    else
                    {
                        txtCountdown.FontWeight = FontWeights.Bold;
                        txtTimeInfo.Text = $"⚠️ {GetActionText()} in {(int)remaining.TotalSeconds} seconds!";
                    }

                    // Play warning sound every 2 seconds in last 10 seconds
                    if ((int)remaining.TotalSeconds % 2 == 0)
                    {
                        try
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        catch (Exception ex)
                        {
                            // Log sound error but continue execution
                            System.Diagnostics.Debug.WriteLine($"Warning sound could not be played: {ex.Message}");
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
                else if (remaining.TotalSeconds <= 30)
                {
                    // Yellow warning for last 30 seconds
                    txtCountdown.Foreground = Brushes.Orange;
                    txtCountdown.FontWeight = FontWeights.Bold;
                    txtTimeInfo.Text = $"⏰ {GetActionText()} in {(int)remaining.TotalSeconds} seconds";
                }
                else
                {
                    // Normal display - use system colors for theme consistency
                    txtCountdown.Foreground = this.Foreground;
                    txtCountdown.FontWeight = FontWeights.Bold;
                    txtTimeInfo.Text = $"{GetActionText()} in {timeText}";
                }

                // Update window title with remaining time
                this.Title = $"Shutdown Timer - {timeText}";
            }
        }

        private async void ExecuteAction()
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

                // Wait a moment for the notification without blocking UI
                await System.Threading.Tasks.Task.Delay(1000);

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
            // Switch panels back - Show duration and control buttons, hide countdown
            DurationPanel.Visibility = Visibility.Visible;
            ControlButtonsPanel.Visibility = Visibility.Visible;
            CountdownPanel.Visibility = Visibility.Collapsed;

            // Reset buttons - Cancel button stays visible
            btnStart.IsEnabled = true;
            btnCancel.IsEnabled = false;
            btnCancel.Visibility = Visibility.Visible;
            miCancel.IsEnabled = false;

            // Re-enable action selection
            rbShutdown.IsEnabled = true;
            rbRestart.IsEnabled = true;
            rbSleep.IsEnabled = true;

            // Reset countdown display
            txtCountdown.Text = "00:00:00";
            txtCountdown.Foreground = this.Foreground;
            txtCountdown.FontWeight = FontWeights.Bold;
            progressTimer.Value = 100;
            txtTimeInfo.Text = "Timer ready";
            
            isTimerRunning = false;
            totalSeconds = 0;
            
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

        private string FormatTime(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;
            
            if (hours > 0)
                return $"{hours}h {minutes}m {secs}s";
            else if (minutes > 0)
                return $"{minutes}m {secs}s";
            else
                return $"{secs}s";
        }
    }
}