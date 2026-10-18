
using System;
using System.Windows;
using System.Windows.Controls;
using Vehicles.Core;
using AppResources = Vehicles.WpfApp.Properties.Resources;

namespace Vehicles.WpfApp
{
    public partial class MainWindow : Window
    {
        private Garage carGarage = new();
        private Garage boatGarage = new();

        private Vehicle? selectedVehicle;

        public MainWindow()
        {
            InitializeComponent();

            // Tekstid Resources.resx failist
            Title = AppResources.WindowTitle;
            CarGarageTitleText.Text = AppResources.CarGarageTitle;
            BoatGarageTitleText.Text = AppResources.BoatGarageTitle;
            SelectedVehicleTitleText.Text = AppResources.SelectedVehicle;
            DistanceLabelText.Text = AppResources.DistanceLabel;
            AddCarButton.Content = AppResources.AddCarButton;
            AddBoatButton.Content = AppResources.AddBoatButton;
            AddAmphibiousButton.Content = AppResources.AddAmphibiousButton;
            RemoveCarButton.Content = AppResources.RemoveButton;
            RemoveBoatButton.Content = AppResources.RemoveButton;
            MoveButton.Content = AppResources.MoveButton;
            DriveButton.Content = AppResources.DriveButton;
            SwimButton.Content = AppResources.SwimButton;
            LogTitleText.Text = AppResources.LogTitle;

            CarList.ItemsSource = carGarage.Vehicles;
            BoatList.ItemsSource = boatGarage.Vehicles;

            // Mõlema garaaži teated lähevad logisse
            carGarage.Message += AddLog;
            boatGarage.Message += AddLog;

            carGarage.Add(new Car("Volvo", "V60"));
            carGarage.Add(new AmphibiousCar("Amphi", "X"));
            boatGarage.Add(new Boat("Bella", "600"));

            CarList.SelectedIndex = 0;
        }

        private void AddLog(string message)
        {
            LogList.Items.Add(message);
            LogList.ScrollIntoView(message);
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            carGarage.Add(new Car("Volvo", "V60"));
        }

        private void AddBoat_Click(object sender, RoutedEventArgs e)
        {
            boatGarage.Add(new Boat("Bella", "600"));
        }

        private void AddAmphibious_Click(object sender, RoutedEventArgs e)
        {
            carGarage.Add(new AmphibiousCar("Amphi", "X"));
        }

        private void RemoveCar_Click(object sender, RoutedEventArgs e)
        {
            if (CarList.SelectedItem is Vehicle vehicle)
            {
                carGarage.Remove(vehicle);
            }
            else
            {
                MessageBox.Show(AppResources.SelectVehicle);
            }
        }

        private void RemoveBoat_Click(object sender, RoutedEventArgs e)
        {
            if (BoatList.SelectedItem is Vehicle vehicle)
            {
                boatGarage.Remove(vehicle);
            }
            else
            {
                MessageBox.Show(AppResources.SelectVehicle);
            }
        }

        private void CarList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (CarList.SelectedItem is Vehicle vehicle)
            {
                BoatList.SelectedItem = null;
                SelectVehicle(vehicle);
            }
            else if (BoatList.SelectedItem == null)
            {
                ClearSelection();
            }
        }

        private void BoatList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (BoatList.SelectedItem is Vehicle vehicle)
            {
                CarList.SelectedItem = null;
                SelectVehicle(vehicle);
            }
            else if (CarList.SelectedItem == null)
            {
                ClearSelection();
            }
        }

        private void SelectVehicle(Vehicle vehicle)
        {
            selectedVehicle = vehicle;
            SelectedVehicleText.Text = vehicle.ToString();

            UpdateOdometer(vehicle);

            MoveButton.IsEnabled = true;
            DriveButton.IsEnabled = vehicle is IDriveable;
            SwimButton.IsEnabled = vehicle is ISwimmable;
        }

        private void ClearSelection()
        {
            selectedVehicle = null;
            SelectedVehicleText.Text = "";
            OdometerText.Text = "";

            MoveButton.IsEnabled = false;
            DriveButton.IsEnabled = false;
            SwimButton.IsEnabled = false;
        }

        private bool TryGetDistance(out double km)
        {
            if (!double.TryParse(DistanceTextBox.Text, out km) || km <= 0)
            {
                MessageBox.Show(AppResources.InvalidDistance);
                return false;
            }

            return true;
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVehicle == null)
            {
                MessageBox.Show(AppResources.SelectVehicle);
                return;
            }

            if (!TryGetDistance(out double km))
                return;

            try
            {
                AddLog(selectedVehicle.Move(km));
                UpdateOdometer(selectedVehicle);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(AppResources.InvalidDistance);
            }
        }

        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVehicle is not IDriveable vehicle)
                return;

            if (!TryGetDistance(out double km))
                return;

            try
            {
                AddLog(vehicle.Drive(km));
                UpdateOdometer(selectedVehicle!);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(AppResources.InvalidDistance);
            }
        }

        private void Swim_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVehicle is not ISwimmable vehicle)
                return;

            if (!TryGetDistance(out double km))
                return;

            try
            {
                AddLog(vehicle.Swim(km));
                UpdateOdometer(selectedVehicle!);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(AppResources.InvalidDistance);
            }
        }

        private void UpdateOdometer(Vehicle vehicle)
        {
            OdometerText.Text =
                $"{AppResources.OdometerLabel}: {vehicle.Odometer} km";
        }
    }
}
