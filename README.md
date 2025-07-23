# Shutdown Timer

A modern, user-friendly Windows application that allows you to schedule system shutdown, restart, or sleep operations with a customizable countdown timer.

## Features

- **Multiple System Actions**: Support for shutdown, restart, and sleep mode
- **Flexible Timer**: Set countdown time using hours, minutes, and seconds
- **Visual Countdown**: Real-time countdown display with progress bar
- **System Tray Integration**: Minimize to system tray and continue running in background
- **Warning System**: Visual and audio warnings in the final seconds
- **Modern UI**: Clean, modern interface using ModernWPF styling
- **Theme Support**: Automatically adapts to system theme (Light/Dark)

## Screenshots

### Main Interface
The application provides an intuitive interface for setting up your shutdown timer:

- Select your desired action (Shutdown, Restart, or Sleep)
- Set the countdown duration
- Start the timer and monitor progress

### System Tray
When minimized, the application continues running in the system tray with:
- Quick access menu
- Timer status display
- Cancel option

## Requirements

- Windows 10/11
- .NET 6.0 or later

## Installation

1. Download the latest release from the [Releases](../../releases) page
2. Extract the files to your desired location
3. Run `ShutdownTimer.exe`

## Usage

1. **Select Action**: Choose between Shutdown, Restart, or Sleep mode
2. **Set Duration**: Use the number boxes to set hours, minutes, and seconds
3. **Start Timer**: Click the "Start" button to begin countdown
4. **Monitor Progress**: Watch the real-time countdown and progress bar
5. **Cancel Anytime**: Use the "Cancel" button or system tray menu to stop the timer

### Warning System

- **30 seconds**: Orange countdown color
- **10 seconds**: Red countdown with flashing effect
- **Sound alerts**: Audio warnings every 2 seconds in final 10 seconds
- **Notifications**: System tray balloon tips at key moments

## Building from Source

### Prerequisites
- Visual Studio 2022 or later
- .NET 6.0 SDK or later

### Build Steps
1. Clone this repository
2. Open `src/ShutdownTimer.sln` in Visual Studio
3. Restore NuGet packages
4. Build the solution (Ctrl+Shift+B)

### Dependencies
- **ModernWpf**: Modern UI styling
- **Hardcodet.NotifyIcon.Wpf**: System tray functionality

## Technical Details

- **Framework**: .NET 6.0 WPF
- **UI Framework**: ModernWPF for modern Windows 11 styling
- **Architecture**: MVVM-friendly single window application
- **Threading**: Async/await pattern for non-blocking operations

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

If you encounter any issues or have feature requests, please create an issue on the [Issues](../../issues) page.