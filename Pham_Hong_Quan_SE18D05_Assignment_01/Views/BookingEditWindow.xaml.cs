using Pham_Hong_Quan_SE18D05_Assignment_01.ViewModels;
using System.Windows;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class BookingEditWindow : Window
    {
        public BookingEditWindow(ViewModels.BookingEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (s, e) =>
            {
                DialogResult = viewModel.IsSaved;
                Close();
            };
        }
    }
}