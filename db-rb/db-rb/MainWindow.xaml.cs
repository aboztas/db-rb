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
using System.Windows.Navigation;
using System.Windows.Shapes;
using db_rb.Database;

namespace db_rb
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseEntities db;
        public MainWindow()
        {
            InitializeComponent();
            db = new DatabaseEntities();
            DataGrid_Passenger.ItemsSource = db.Passenger.ToList();
            
            List<ReservationListItem> reservationList = new List<ReservationListItem>();
            foreach(Reservation r in db.Reservation.ToList()){
                ReservationListItem reservationListItem = new ReservationListItem();
                reservationListItem.Flugdatum = r.Flight.Date.ToShortDateString();
                reservationListItem.Flugroute = r.Flight.Departure.Code + "->" + r.Flight.Arrival.Code;
                reservationListItem.Buchungsdatum = r.Date.ToShortDateString();
                String passengerString = "";
                for (int i = 0; i < r.Passenger.Count; i++)
                {
                    Database.Passenger p=r.Passenger.ElementAt(i);
                    passengerString = passengerString + p.Sex + p.Surname + p.Name;
                    if(i<(r.Passenger.Count-1)){
                        passengerString = passengerString + "/";
                    }
                }

                reservationListItem.Passagiere = passengerString;
                reservationList.Add(reservationListItem);
            }

            DataGrid_Reservations.ItemsSource = reservationList;
        
        }

        private void DataGrid_Passenger_Sorting(object sender, DataGridSortingEventArgs e)
        {

        }

        private void SearchBox_Passenger_SelectionChanged(object sender, RoutedEventArgs e)
        {
            String seachString = SearchBox_Passenger.Text;
            DataGrid_Passenger.ItemsSource = db.Passenger.Where(x => x.Name.Contains(seachString) || x.Name.Equals(seachString) || x.Surname.Contains(seachString) || x.Surname.Equals(seachString) || (x.Surname + " " + x.Name).Contains(seachString) || (x.Name + " " + x.Surname).Contains(seachString)).ToList();
        }

        private void DataGrid_Passenger_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Database.Passenger p = (Database.Passenger)DataGrid_Passenger.SelectedItem;
            if (p != null)
            {
                Passenger.PassengerDetails passengerDetails = new Passenger.PassengerDetails(p.Id);
                passengerDetails.Show();
            }
        }

        private void DataGrid_Reservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Config_Click(object sender, RoutedEventArgs e)
        {
            Config.Config config = new Config.Config();
            config.Show();
        }
    }
}
