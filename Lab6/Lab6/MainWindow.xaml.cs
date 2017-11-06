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
        public static int Total = 9;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Settings:
        Settings settings;
        #region Changeable in main window:
        public double TimeScale = 1;
        public double BartenderTS = 1;
        public double WaiterTS = 1;
        public double BouncerTS = 1;
        public double PatronTS = 1;
        #endregion

        #region Changeable in settings:
        public int Groups = 1; //Mängd människor som kommer in i taget.
        public int Chairs = 8;
        public int AutoClose = 0; //0 = disabled
        public bool PartyBusEnabled = false;
        public int MaxTimeScale = 5;
        public int MinPeopleInBus = 5;
        public int MaxPeopleInBus = 12;
        public double ChanceOfBus = 1.66;
        #endregion
        #endregion

        public int BouncerTimeMin = 3;
        public int BouncerTimeMax = 10;

        public bool Open = false;

        public int MessageID = 1;

        public ConcurrentQueue<Patron> BarQueue = new ConcurrentQueue<Patron>();
        public ConcurrentQueue<Patron> ChairQueue = new ConcurrentQueue<Patron>();
        public ConcurrentQueue<Glass> TableGlasses = new ConcurrentQueue<Glass>();
        public List<Patron> Sitting = new List<Patron>();
        public ConcurrentStack<Glass> Shelf = new ConcurrentStack<Glass>();

        Random rnd = new Random();

        Bartender bartender;
        Bouncer bouncer;
        Waiter waiter;

        public MainWindow()
        {
            InitializeComponent();

            string Path = AppDomain.CurrentDomain.BaseDirectory + "BS.png";

            try
            {
                Icon = new BitmapImage(new Uri(Path));
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Unable to load the icon.\r\nCouldn't find " + Path);
            }

            for (int n = 0; n < Chairs; n++)
                Sitting.Add(null);

            Height += 10;
            Width += 10;

            for (int n = 0; n < Glass.Total; n++)
                Shelf.Push(new Glass());

            UpdatePatronLbl();
            UpdateGlassLbl();
            UpdateChairLbl();

            bartender = new Bartender(this);
            bouncer = new Bouncer(this);
            waiter = new Waiter(this);

            Patron.smw = this;
            Patron.ChairHandler();

            Task t1 = Task.Run(() => Timer());
            }

        public string GetName()
            {
                return NameList.Names[rnd.Next(NameList.Names.Count)];
            }

            private void btnOpenBar_Click(object sender, RoutedEventArgs e)
            {
                Open = !Open;
                if (Open)
                {
                    bartender.Work();
                    waiter.Work();
                    bouncer.Work();
                }
            }

            public void UpdateGlassLbl() => Dispatcher.Invoke(() => lblNrOfGlasses.Content = "There " + (Shelf.Count == 1 ? "is" : "are") + $" {Shelf.Count} " + (Shelf.Count == 1 ? "glass." : "glasses.") + $" ({Glass.Total} total)");
            public void UpdatePatronLbl() => Dispatcher.Invoke(() => lblNrOfPatrons.Content = "There " + (Patron.Amount == 1 ? "is" : "are") + $" {Patron.Amount} " + (Patron.Amount == 1 ? "guest." : "guests."));
            public void UpdateChairLbl()
            {
                int FreeChairs = 0;
                for (int n = 0; n < Chairs; n++)
                    if (Sitting[n] == null)
                        FreeChairs++;

                Dispatcher.Invoke(() =>
                {
                    lblNrOfChairs.Content = $"There " + (FreeChairs == 1 ? "is" : "are") + $" {FreeChairs} empty " + (FreeChairs == 1 ? "chair." : "chairs.") + $"\n({Chairs} total)";
                });
            }

        public int ElapsedTime = 0;
        void Timer()
        {
            Stopwatch sw = new Stopwatch();

            while (true)
            {
                if (Open)
                {
                    sw.Restart();
                    while (sw.ElapsedMilliseconds < 995 / TimeScale) { Thread.Sleep(1); }
                    ElapsedTime++;

                    Dispatcher.Invoke(() => Title = (Open ? "Bar [Open] " : "Bar [Closed] ") + $"({ElapsedTime}s" + (AutoClose == 0 ? ")" : $" / {AutoClose}s)"));

                    if (PartyBusEnabled && ChanceOfBus > 0)
                        if (new Random().Next(1, Convert.ToInt32(100 / ChanceOfBus)) == 1)
                            bouncer.PartyBus();

                    if (AutoClose > 0 && ElapsedTime >= AutoClose)
                    {
                        Open = false;
                        Dispatcher.Invoke(() => Title = $"Bar [Closed] ({AutoClose}s / {AutoClose}s) [+0]");
                    }
                }
                else if (AutoClose == 0 && waiter.Active)
                {
                    sw.Restart();
                    while (sw.ElapsedMilliseconds < 995 / TimeScale) { Thread.Sleep(1); }
                    ElapsedTime++;

                    Dispatcher.Invoke(() => Title = (Open ? "Bar [Open] " : "Bar [Closed] ") + $"({ElapsedTime}s)");
                }

                else
                {
                    if (ElapsedTime >= AutoClose && waiter.Active)
                    {

                        sw.Restart();
                        while (sw.ElapsedMilliseconds < 995 / TimeScale) { Thread.Sleep(1); }

                        ElapsedTime++;

                        Dispatcher.Invoke(() => Title = $"Bar [Closed] ({AutoClose}s / {AutoClose}s) [+{ElapsedTime - AutoClose}]");
                    }
                    else { Thread.Sleep(1); }
                }
            }
        }

        private void btnPauseBartender_Click(object sender, RoutedEventArgs e)
        {
            bartender.Paused = !bartender.Paused;
            Dispatcher.Invoke(() => lblBartender.Content = "Bartender " + (bartender.Paused ? "(on break)" : ""));
        }

        private void btnWaiterPause_Click(object sender, RoutedEventArgs e)
        {
            waiter.Paused = !waiter.Paused;
            Dispatcher.Invoke(() => lblWaiter.Content = "Waiter " + (waiter.Paused ? "(on break)" : ""));
        }

        private void btnPausePatrons_Click(object sender, RoutedEventArgs e)
        {
            bouncer.Paused = !bouncer.Paused;
            Dispatcher.Invoke(() => lblPatrons.Content = "Patrons " + (bouncer.Paused ? "(door closed)" : ""));
        }

        private void btnPanic_Click(object sender, RoutedEventArgs e)
        {
            bartender.Paused = true;
            waiter.Paused = true;
            bouncer.Paused = true;
            Dispatcher.Invoke(() =>
            {
                lblBartender.Content = "Bartender (on break)";
                lblWaiter.Content = "Waiter (on break)";
                lblPatrons.Content = "Patrons (door closed)";
            });
        }

        private void sldTimescale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeScale = sldTimescale.Value;
            if (lblTimescaleValue != null)
                lblTimescaleValue.Content = TimeScale.ToString();
        }

        private void sldBartenderTS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BartenderTS = sldBartenderTS.Value;
            if (lblBartenderTSValue != null)
                lblBartenderTSValue.Content = BartenderTS.ToString();
        }

        private void sldWaiterTS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WaiterTS = sldWaiterTS.Value;
            if (lblWaiterTSValue != null)
                lblWaiterTSValue.Content = WaiterTS.ToString();
        }

        private void sldPatronsTS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BouncerTS = sldPatronsTS.Value;
            if (lblPatronsTSValue != null)
                lblPatronsTSValue.Content = BouncerTS.ToString();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            settings = new Settings(this);
            settings.ShowDialog();
        }
    }
}
