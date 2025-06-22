using BusinessLogicLayer.Services;
using BusinessObjects;
using System;
using System.Windows;

namespace Pham_Hong_Quan_SE18D05_Assignment_01.Views
{
    public partial class RoomEditWindow : Window
    {
        private readonly IRoomService _roomService;
        private readonly RoomInformation? _room;

        public RoomEditWindow(RoomInformation? room)
        {
            InitializeComponent();
            _roomService = new RoomService();
            _room = room;

            RoomTypeComboBox.ItemsSource = _roomService.GetAllRoomTypes();

            if (_room != null)
            {
                RoomNumberTextBox.Text = _room.RoomNumber;
                DescriptionTextBox.Text = _room.RoomDescription;
                CapacityTextBox.Text = _room.RoomMaxCapacity.ToString();
                PriceTextBox.Text = _room.RoomPricePerDate.ToString();
                RoomTypeComboBox.SelectedItem = _roomService.GetAllRoomTypes().Find(rt => rt.RoomTypeID == _room.RoomTypeID);
                Title = "Edit Room";
            }
            else
            {
                Title = "Add Room";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text) ||
                    string.IsNullOrWhiteSpace(CapacityTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    RoomTypeComboBox.SelectedItem == null)
                {
                    ErrorTextBlock.Text = "All fields are required.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                if (!int.TryParse(CapacityTextBox.Text, out int capacity) || capacity <= 0)
                {
                    ErrorTextBlock.Text = "Invalid capacity.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
                {
                    ErrorTextBlock.Text = "Invalid price.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                var room = new RoomInformation
                {
                    RoomID = _room?.RoomID ?? 0,
                    RoomNumber = RoomNumberTextBox.Text,
                    RoomDescription = DescriptionTextBox.Text,
                    RoomMaxCapacity = capacity,
                    RoomPricePerDate = price,
                    RoomTypeID = ((RoomType)RoomTypeComboBox.SelectedItem).RoomTypeID,
                    RoomStatus = 1
                };

                if (_room == null)
                {
                    _roomService.AddRoom(room);
                }
                else
                {
                    _roomService.UpdateRoom(room);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = ex.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}