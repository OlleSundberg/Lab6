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
        public bool Active = false;
        public bool Paused = false;
        MainWindow mw;
        public Bouncer(MainWindow mw)
        {
            this.mw = mw;

        }
        public void Work()
        {
            if (!Active)
            {
                Task t1 = Task.Run(() =>
                {
                    Log("Bouncer starts working.");
                    Active = true;
                    while (mw.Open)
                    {
                        while (Paused) { Thread.Sleep(1); }
                        int delay = new Random().Next(mw.BouncerTimeMin, mw.BouncerTimeMax + 1);
                        for (int n = 0; n < delay * 10 / (mw.TimeScale * mw.BouncerTS); n++)
                        {
                            while(Paused) { Thread.Sleep(1); }
                            Thread.Sleep(100);
                        }
                        if (mw.Open)
                        {
                            for (int n = 0; n < mw.Groups; n++)
                                mw.BarQueue.Enqueue(new Patron(mw, mw.GetName()));
                        }
                    }
                    Log("Bouncer is going home.");
                    Active = false;
                });
            }
        }
        void Log(string Message) => mw.Dispatcher.Invoke(()=>mw.lbxPatrons.Items.Insert(0,$"{mw.MessageID++}: {Message}"));


        public void PartyBus(int Amount)
        {
            for (int n = 0; n < Amount; n++)
                mw.BarQueue.Enqueue(new Patron(mw, mw.GetName()));
            Thread.Sleep(1000);
            Log($"A bus full of {Amount} people arrived");
        }

        Random rnd = new Random();
        public void PartyBus()
        {
            int Amount = rnd.Next(5, 31);
            for (int n = 0; n < Amount; n++)
                mw.BarQueue.Enqueue(new Patron(mw, mw.GetName()));
            Task.Run(() =>
            {
                Thread.Sleep(1500);
                Log($"A bus full of {Amount} people arrived");
            });
        }
    }
}
