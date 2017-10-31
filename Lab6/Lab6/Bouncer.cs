using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer
    {
        MainWindow mw;
        public Bouncer(MainWindow mw)
        {
            this.mw = mw;

            Task t1 = Task.Run(() =>
            {
                while (true)
                {
                    if (mw.Open)
                    {
                        Thread.Sleep(new Random().Next(mw.BouncerTimeMin, mw.BouncerTimeMax + 1) * 1000);
                        if (mw.Open)
                        {
                            mw.BarQueue.Enqueue(new Patron(mw, mw.GetName()));
                            mw.Dispatcher.Invoke(() => mw.Title += '!');
                        }
                    }
                    else
                        Thread.Sleep(1);
                }
            });
        }
    }
}
