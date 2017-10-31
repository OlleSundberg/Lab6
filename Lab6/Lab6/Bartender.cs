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
        MainWindow mw;
        public Bartender(MainWindow mw)
        {
            this.mw = mw;            
        }

        public void Work()
        {
            Task t1 = Task.Run(() =>
            {
                while (true /*Baren är öppen och det finns gäster.*/)
                {
                    Log("Waiting for a customer.");
                    while (mw.BarQueue.Count < 1) { Thread.Sleep(1); }
                }
            });
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
