using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron
    {
        public static int Amount = 0;
        static int SatisfiedCustomers = 0;

        MainWindow mw;
        public static MainWindow smw;
        public static bool Paused = false;

        public bool HasBeer = false;
        public bool HasChair = false;
        int SeatID;
        bool Served = false;
        static int ratings = 0;
        static int score = 0;

        public string Name;

        /// <summary>
        /// Create an instance of Patron
        /// </summary>
        /// <param name="mainWindow">The mainwindow class. Ex: Create Bartender in MainWindow.xaml.cs and use "this" as the parameter.</param>
        public Patron(MainWindow mainWindow, string Name)
        {
            mw = mainWindow;

            Task t1 = Task.Run(() =>
            {
                this.Name = Name;
                Amount++;
                mainWindow.UpdatePatronLbl();
                Log(Name + " entered the bar.");
                Thread LifeThread = new Thread(Exist);
                LifeThread.Start();
            });
        }

        void Exist()
        {
            Task.Run(() => Satisfaction());
            while (!HasBeer || Paused) { Thread.Sleep(1); }
            Log(Name + " is looking for a chair.");
            for (int n = 0; n < 1 * 10 / (mw.TimeScale * mw.PatronTS); n++)
                Thread.Sleep(100);
            mw.ChairQueue.Enqueue(this);
            while (!HasChair || Paused) { Thread.Sleep(1); }
            Log(Name + " found a seat.");
            for (int n = 0; n < 2 * 10 / (mw.TimeScale * mw.PatronTS); n++)
                Thread.Sleep(100);
            Log(Name + " took a seat.");
            Served = true;
            int delay = new Random().Next(10, 21);
            for (int n = 0; n < delay * 10 / (mw.TimeScale * mw.PatronTS); n++)
                Thread.Sleep(100);
            while (Paused) { Thread.Sleep(1); }
            mw.Sitting[SeatID] = null;
            mw.UpdateChairLbl();
            Log(Name + " left the bar.");
            Amount--;
            mw.TableGlasses.Enqueue(new Glass());
            mw.UpdatePatronLbl();
            mw.Dispatcher.Invoke(() => mw.lblSatisfied.Content = "Satisfied customers: " + ++SatisfiedCustomers);
        }

        void Satisfaction()
        {
            int TimeArrived = mw.ElapsedTime;
            Log(Name + " started his/her stopwatch.");
            while (!Served) { Thread.Sleep(1); }
            int TimeDone = mw.ElapsedTime;
            int TimeSpent = TimeDone - TimeArrived;
            Log(Name + " had to wait for a total of " + TimeSpent + " seconds");
            int myscore = 110 - TimeSpent;
            score += myscore > 0 ? myscore : 0;
            ratings++;
            mw.Dispatcher.Invoke(() =>
            {
                mw.lblLastRating.Content = "Last rating: " + myscore;
                mw.lblAverageRating.Content = "Average rating: " + score / ratings + $"({ratings})";
            });
        }

        static double GetScore()
        {
            return score / ratings;
        }

        /// <summary>
        /// Needs to be run once for patrons to be able to find chairs.
        /// </summary>
        public static void ChairHandler()
        {
            Patron CurrentPatron;

            Task t1 = Task.Run(() =>
            {
                while (true)
                {
                    bool Done = false;
                    while (!smw.ChairQueue.TryDequeue(out CurrentPatron)) { Thread.Sleep(1); }
                    while (!Done)
                        for (int n = 0; n < smw.Chairs; n++)
                        {
                            if (smw.Sitting[n] == null)
                            {
                                CurrentPatron.HasChair = true;
                                smw.Sitting[n] = CurrentPatron;
                                smw.UpdateChairLbl();
                                CurrentPatron.SeatID = n;
                                Done = true;
                                break;
                            }
                        }
                }
            });
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxPatrons.Items.Insert(0, $"{mw.MessageID++}: {Message}"));
    }
}
