using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron
    {
        public static int Amount = 0;

        MainWindow mw;
        public static MainWindow smw;
        public static bool Paused = false;

        public bool HasBeer = false;
        public bool HasChair = false;
        int SeatID;

        public string Name;

        public Patron(MainWindow mw, string Name)
        {
            this.mw = mw;
            this.Name = Name;
            Amount++;
            mw.UpdatePatronLbl();

            Task t1 = Task.Run(() =>
            {
                Log(Name + " entered the bar.");
                while (!HasBeer || Paused) { Thread.Sleep(1); }
                Log(Name + " is looking for a chair.");
                mw.ChairQueue.Enqueue(this);
                while (!HasChair || Paused) { Thread.Sleep(1); }
                Log(Name + " took a seat.");
                Thread.Sleep(new Random().Next(10, 21) * 1000);
                while (Paused) { Thread.Sleep(1); }
                mw.Sitting[SeatID] = null;
                Log(Name + " left the bar.");
                Amount--;
                mw.UpdatePatronLbl();
            });
        }


        public static void ChairHandler()
        {
            Patron CurrentPatron;
            for (int n = 0; n < smw.Chairs; n++)
                smw.Sitting.Add(null);

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
