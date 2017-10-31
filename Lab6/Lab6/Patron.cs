using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron
    {
        MainWindow mw;
        public Patron(MainWindow mw)
        {
            this.mw = mw;
        }


        public void OrderDrink()
        {

        }

        void Log(string Message) => mw.Dispatcher.Invoke(() => mw.lbxPatrons.Items.Add($"{mw.MessageID++}: {Message}"));
    }
}
