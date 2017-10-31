using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            while (true /*Baren är öppen och det finns gäster.*/)
            {

            }
        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxBartender.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
