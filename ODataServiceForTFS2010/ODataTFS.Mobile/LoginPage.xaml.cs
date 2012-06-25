// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.DPE.ODataTFS.Mobile
{
    using System;
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Samples.DPE.ODataTFS.Mobile.ViewModels;

    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoginViewModel viewModel = null;
            if (this.State.ContainsKey("CurrentLoginViewModel"))
            {
                viewModel = this.State["CurrentLoginViewModel"] as LoginViewModel;

                if (viewModel != null)
                {
                    App.LoginViewModel = viewModel;
                }
            }

            this.DataContext = App.LoginViewModel;
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as LoginViewModel;
            if (viewModel != null)
            {
                if (this.State.ContainsKey("CurrentLoginViewModel"))
                {
                    this.State.Remove("CurrentLoginViewModel");
                }

                this.State.Add("CurrentLoginViewModel", viewModel);
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Login(
                    r =>
                    {
                        if (!r.IsAuthenticated)
                        {
                            MessageBox.Show(r.Description, "Authentication Error", MessageBoxButton.OK);
                        }
                        else
                        {
                            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        }
                    });
            }
        }
    }
}