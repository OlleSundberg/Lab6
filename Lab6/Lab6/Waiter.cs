﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{   
    class Waiter
    {
        public bool Paused = false;
        public bool Active = false;
        MainWindow mw;

        /// <summary>
        /// Create an instance of Waiter
        /// </summary>
        /// <param name="mainWindow">The mainwindow class. Ex: Create Bartender in MainWindow.xaml.cs and use "this" as the parameter.</param>
        public Waiter(MainWindow mainWindow)
        {
            mw = mainWindow;            
        }

        /// <summary>
        /// Activates the waiter, causing him to work until it's closed, all customers are left and all the dishes have been done.
        /// </summary>
        public void Work()
        {
            if (!Active)
            {
                mw.btnSettings.IsEnabled = false;
                Task t1 = Task.Run(() =>
                {
                    Active = true;
                    while (mw.Open || Patron.Amount > 0 || mw.TableGlasses.Count > 0)
                    {
                        while (Paused) { Thread.Sleep(1); }
                        Log("Clearing tables.");
                        for (int n = 0; n < 10 * 10 / (mw.TimeScale * mw.WaiterTS); n++)
                        {
                            while(Paused) { Thread.Sleep(1); }
                            Thread.Sleep(100);
                        }

                        int GlassesFound = mw.TableGlasses.Count;
                        mw.TableGlasses = new ConcurrentQueue<Glass>();
                        if (GlassesFound < 1)
                        {
                            Log("Found 0 glasses.");
                            for (int n = 0; n < 1 * 10 / (mw.TimeScale * mw.WaiterTS); n++)
                            {
                                while (Paused) { Thread.Sleep(1); }
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            Log($"Washing {GlassesFound} " + (GlassesFound == 1 ? "glass." : "glasses."));
                            for (int n = 0; n < 15 * 10 / (mw.TimeScale * mw.WaiterTS); n++)
                            {
                                while (Paused) { Thread.Sleep(1); }
                                Thread.Sleep(100);
                            }
                            Log("Finished washing glasses.");
                            for (int n = 0; n < GlassesFound; n++)
                                mw.Shelf.Push(new Glass());
                            mw.UpdateGlassLbl();
                        }
                    }
                    Active = false;
                    Log("Going home.");
                    mw.Dispatcher.Invoke(()=>mw.btnSettings.IsEnabled = true);
                });
            }
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxWaiter.Items.Insert(0, $"{mw.MessageID++}: {Message}"));
    }
}
