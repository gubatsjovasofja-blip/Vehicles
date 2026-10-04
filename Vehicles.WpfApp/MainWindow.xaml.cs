using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Vehicles.Core;
using AppResources = Vehicles.WpfApp.Properties.Resources;

namespace Vehicles.WpfApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Vehicle> vehicles = new();

        public MainWindow()
        {
            InitializeComponent();

            // Tekstid Resources.resx failist
            Title = AppResources.WindowTitle;
            VehiclesTitleText.Text = AppResources.VehiclesTitle;
            SelectedVehicleTitleText.Text = AppResources.SelectedVehicle;
            DistanceLabelText.Text = AppResources.DistanceLabel;
            AddCarButton.Content = AppResources.AddCarButton;
            AddBoatButton.Content = AppResources.AddBoatButton;
            AddAmphibiousButton.Content = AppResources.AddAmphibiousButton;
            MoveButton.Content = AppResources.MoveButton;
            DriveButton.Content = AppResources.DriveButton;
            SwimButton.Content = AppResources.SwimButton;
            RemoveButton.Content = AppResources.RemoveButton;
            LogTitleText.Text = AppResources.LogTitle;

            VehicleList.ItemsSource = vehicles;

            vehicles.Add(new Car("Volvo", "V60"));
            vehicles.Add(new Boat("Bella", "600"));
            vehicles.Add(new AmphibiousCar("Amphi", "X"));
            VehicleList.SelectedIndex = 0;
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            vehicles.Add(new Car("Volvo", "V60"));
        }

        private void AddBoat_Click(object sender, RoutedEventArgs e)
        {
            vehicles.Add(new Boat("Bella", "600"));
        }

        private void AddAmphibious_Click(object sender, RoutedEventArgs e)
        {
            vehicles.Add(new AmphibiousCar("Amphi", "X"));
        }

        private void VehicleList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (VehicleList.SelectedItem is Vehicle vehicle)
            {
                SelectedVehicleText.Text = vehicle.ToString();
                UpdateOdometer(vehicle);

                MoveButton.IsEnabled = true;
                DriveButton.IsEnabled = vehicle is IDriveable;
                SwimButton.IsEnabled = vehicle is ISwimmable;
            }
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            if (VehicleList.SelectedItem is not Vehicle vehicle)
            {
                MessageBox.Show(AppResources.SelectVehicle);
                return;
            }

            if (!double.TryParse(DistanceTextBox.Text, out double km))
            {
                MessageBox.Show(AppResources.InvalidDistance);
                return;
            }

            try
            {
                LogList.Items.Add(vehicle.Move(km));
                UpdateOdometer(vehicle);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(AppResources.InvalidDistance);
            }
        }

        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            if (VehicleList.SelectedItem is IDriveable vehicle &&
                double.TryParse(DistanceTextBox.Text, out double km))
            {
                try
                {
                    LogList.Items.Add(vehicle.Drive(km));
                    UpdateSelectedVehicle();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(AppResources.InvalidDistance);
                }
            }
        }

        private void Swim_Click(object sender, RoutedEventArgs e)
        {
            if (VehicleList.SelectedItem is ISwimmable vehicle &&
                double.TryParse(DistanceTextBox.Text, out double km))
            {
                try
                {
                    LogList.Items.Add(vehicle.Swim(km));
                    UpdateSelectedVehicle();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(AppResources.InvalidDistance);
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (VehicleList.SelectedItem is Vehicle vehicle)
            {
                vehicles.Remove(vehicle);
                LogList.Items.Add(AppResources.VehicleRemoved);
            }
            else
            {
                MessageBox.Show(AppResources.SelectVehicle);
            }
        }

        private void UpdateSelectedVehicle()
        {
            if (VehicleList.SelectedItem is Vehicle vehicle)
            {
                UpdateOdometer(vehicle);
            }
        }

        private void UpdateOdometer(Vehicle vehicle)
        {
            OdometerText.Text =
                $"{AppResources.OdometerLabel}: {vehicle.Odometer} km";
        }
    }
}