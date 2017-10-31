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
        bool Paused = false;

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
                    while(Paused) { Thread.Sleep(1); }
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

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
