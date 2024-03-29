﻿using ENL_Distribution.Core;
using ENL_Distribution.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ENL_Distribution.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Reg_MouseDown(object sender, MouseButtonEventArgs e)

        {
            RegisterView registerView = new RegisterView();
            registerView.Show();
            this.Hide(); // Assuming 'this' refers to the LoginView

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Retrieve the ViewModel from DataContext
                LoginViewModel viewModel = DataContext as LoginViewModel;

                // Check if ViewModel is not null and if it has the LoginCommand
                if (viewModel != null && viewModel.LoginCommand != null && viewModel.LoginCommand.CanExecute(null))
                {
                    // Execute the LoginCommand
                    viewModel.LoginCommand.Execute(null);
                }
            }
        }

    }
}
