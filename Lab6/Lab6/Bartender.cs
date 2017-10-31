using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bartender
    {
        Patron CurrentCustomer;
        Glass glass;
        public bool Paused = false;
        public bool Active = false;

        MainWindow mw;
        public Bartender(MainWindow mw)
        {
            this.mw = mw;
        }

        public void Work()
        {
            if (!Active)
            {
                Task t1 = Task.Run(() =>
                {
                    Active = true;
                    while (mw.Open || mw.BarQueue.Count > 0)
                    {
                        Log("Waiting for a customer.");
                        while (!mw.BarQueue.TryDequeue(out CurrentCustomer) || Paused) { if (!mw.Open) goto EndOfShift; Thread.Sleep(1); }
                        for (int n = 0; n < 1 * 10 / (mw.TimeScale * mw.BartenderTS); n++)
                            Thread.Sleep(100);
                        if (mw.Shelf.Count < 1)
                            Log("Waiting for a glass.");
                        while (!mw.Shelf.TryPop(out glass) || Paused) { Thread.Sleep(1); }
                        mw.UpdateGlassLbl();
                        Log("Grabbing a glass from the shelf.");
                        for (int n = 0; n < 3 * 10 / (mw.TimeScale * mw.BartenderTS); n++)
                            Thread.Sleep(100);
                        while (Paused) { Thread.Sleep(1); }
                        Log("Pouring up a beer for " + CurrentCustomer.Name);
                        for (int n = 0; n < 3 * 10 / (mw.TimeScale * mw.BartenderTS); n++)
                            Thread.Sleep(100);
                        while (Paused) { Thread.Sleep(1); }
                        CurrentCustomer.HasBeer = true;
                    }
                    EndOfShift:
                    Active = false;
                    Log("Going home.");
                });
            }
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Insert(0, $"{mw.MessageID++}: {Message}"));
    }
}
