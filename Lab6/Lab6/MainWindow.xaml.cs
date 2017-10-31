using System;
using System.Diagnostics;
using System.Timers;
using System.Collections.Concurrent;
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
using System.Threading;

namespace Lab6
{
    public class Glass
    {
        public const int Total = 5000;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //settings:
        public int Chairs = 4;
        public int BouncerTimeMin = 1;
        public int BouncerTimeMax = 2;

        public bool Open = false;

        public int MessageID = 1;

        public ConcurrentQueue<Patron> BarQueue = new ConcurrentQueue<Patron>();
        public ConcurrentQueue<Patron> ChairQueue = new ConcurrentQueue<Patron>();
        public List<Patron> Sitting = new List<Patron>();
        public ConcurrentStack<Glass> Shelf = new ConcurrentStack<Glass>();

        Bartender bartender;
        Bouncer bouncer;

        public MainWindow()
        {
            InitializeComponent();
            
            Height += 10;
            Width += 10;

            for (int n = 0; n < Glass.Total; n++)
                Shelf.Push(new Glass());

            bartender = new Bartender(this);
            bartender.Work();

            bouncer = new Bouncer(this);

            Patron.smw = this;
            Patron.ChairHandler();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return NameList.Names[new Random().Next(NameList.Names.Count)];
        }

        private void btnOpenBar_Click(object sender, RoutedEventArgs e)
        {
            Open = !Open;
            if (Open)
                Title = "Bar (Open)";
            else
                Title = "Bar (Closed)";
        }

        public void UpdateGlassLbl() => Dispatcher.Invoke(() => lblNrOfGlasses.Content = $"There are {Shelf.Count} glasses. ({Glass.Total} total)");
        public void UpdatePatronLbl() => Dispatcher.Invoke(() => lblNrOfPatrons.Content = $"There are {Patron.Amount} guests.");

        private void btnPauseBartender_Click(object sender, RoutedEventArgs e)
        {
            bartender.Paused = !bartender.Paused;
        }
    }

}
