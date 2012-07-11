using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Ordina.TFSMonitor.WP7
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            ThemeManager.ToLightTheme();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger                
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                OnAnyError(e.ExceptionObject);
            }
            e.Handled = true;  //optional
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger                
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                OnAnyError(e.Exception);
            }
            e.Handled = true;
            //optional
        }

        private void OnAnyError(Exception e)
        {
            if (MessageBox.Show("Do we have your permission to send this error to our support team?  That way they can fix it so it doesn't happen again.  We'll show you the entire message before it's sent.", "Application error", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.To = "tfssupport@ordina.be";
                emailComposeTask.Subject = "An error in TFS Monitor.";
                emailComposeTask.Body = e.ToString();
                emailComposeTask.Show();
            }
        }
    }
}