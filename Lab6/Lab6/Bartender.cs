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

        MainWindow mw;
        public Bartender(MainWindow mw)
        {
            this.mw = mw;
        }

        public void Work()
        {
            Task t1 = Task.Run(() =>
            {
                while (true /*Baren är öppen ELLER det finns gäster.*/)
                {
                    Log("Waiting for a customer.");
                    while (!mw.BarQueue.TryDequeue(out CurrentCustomer) || Paused) { Thread.Sleep(1); }
                    Thread.Sleep(1000);
                    if (mw.Shelf.Count < 1)
                        Log("Waiting for a glass.");
                    while (!mw.Shelf.TryPop(out glass) || Paused) { Thread.Sleep(1); }
                    mw.UpdateGlassLbl();
                    Log("Grabbing a glass from the shelf.");
                    Thread.Sleep(3000);
                    while (Paused) { Thread.Sleep(1); }
                    Log("Pouring up a beer for " + CurrentCustomer.Name);
                    Thread.Sleep(3000);
                    while (Paused) { Thread.Sleep(1); }
                    CurrentCustomer.HasBeer = true;
                }
            });
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Insert(0, $"{mw.MessageID++}: {Message}"));        
    }
}
