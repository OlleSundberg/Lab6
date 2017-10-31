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
                    while (!mw.BarQueue.TryDequeue(out CurrentCustomer)) { Thread.Sleep(1); }
                }
            });
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
