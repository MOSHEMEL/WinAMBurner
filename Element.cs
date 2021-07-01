using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
    class Element
    {
        Control control;
        string text;
        string deflt;
        List<Object> items;
        EventHandler eventHandler;
        Gui.Place hPlace;
        Gui.Place vPlace;
    }
}
