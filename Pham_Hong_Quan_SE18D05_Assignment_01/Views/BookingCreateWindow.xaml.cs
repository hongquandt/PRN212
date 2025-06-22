using Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class BookingCreateWindow : Window
    {
        public BookingCreateWindow(ViewModels.BookingCreateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (s, e) =>
            {
                DialogResult = viewModel.IsSaved;
                Close();
            };
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker && DataContext is BookingCreateViewModel viewModel)
            {
                if (datePicker.Name == "StartDatePicker")
                {
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] Start Date changed to: {viewModel.StartDate?.ToString("dd/MM/yyyy")}");
                }
                else if (datePicker.Name == "EndDatePicker")
                {
                    System.Diagnostics.Debug.WriteLine($"[05:06 PM +07, 22/06/2025] End Date changed to: {viewModel.EndDate?.ToString("dd/MM/yyyy")}");
                }
            }
        }
    }
}