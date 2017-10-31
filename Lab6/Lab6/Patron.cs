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
        MainWindow mw;
        static MainWindow smw;
        public static bool Paused = false;

        public bool HasBeer = false;
        public bool HasChair = false;
        int SeatID;

        public string Name;

        public Patron(MainWindow mw)
        {
            smw = mw;
            this.mw = mw;

            Log(Name + " entered the bar.");
            while (!HasBeer || Paused) { Thread.Sleep(1); }
            Log(Name + "is looking for a chair.");
            mw.ChairQueue.Enqueue(this);
            while (!HasChair || Paused) { Thread.Sleep(1); }
            Log(Name + " took a seat.");
            Thread.Sleep(new Random().Next(10, 21) * 1000);
            while (Paused) { Thread.Sleep(1); }
            mw.Sitting[SeatID] = null;
            Log(Name + " left the bar.");
        }


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
                                CurrentPatron.SeatID = n;
                                Done = true;
                                break;
                            }
                        }
                }
            });
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxPatrons.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
