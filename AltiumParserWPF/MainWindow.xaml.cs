using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AltiumParserWPF.AltiumParser.Records;
using OpenMcdf;

namespace AltiumParserWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var filename = @"F:\SN74LVCH245ADBR_ETT_v2\SN74LVCH245ADBR_ETT_v2.SchDoc";

            var small = "DIN41612R.РОЗ.УГЛ.48";
            var big = "DIN41612R.РОЗ.УГЛ.96";

            var parser = new AltiumParser.AltiumParser(filename);

            var Jacks = new List<Component>();
            var ChanelsPins = new List<Pin>();

            var ett = new List<EttChanel>();

            foreach (var component in parser.Components)
            {
                if (component.DesignItemId.Contains(small) || component.DesignItemId.Contains(big))
                {
                    Jacks.Add(component);
                }
            }

            foreach (var jack in Jacks)
            {
                foreach (var pin in jack.PinList)
                {
                    if (pin.Name.Contains("CH")) 
                    {
                        ChanelsPins.Add(pin);
                    }
                }
            }

            foreach (var pin in ChanelsPins)
            {
                foreach (var net in parser.Nets)
                {
                    if (net.Connection.IsMatch(pin.Connection)) 
                    {
                        ett.Add(new EttChanel(pin, net));
                    }
                }
            }

            var result = ett.OrderBy(a => a.ChanelNet.Text);
            foreach (var ettChanel in result)
            {
                Console.WriteLine(ettChanel.ChanelPin.Name + " Connected to Net " + ettChanel.ChanelNet.Text);
            }
        }
    }

    public class EttChanel
    {
        public Pin ChanelPin;
        public Net ChanelNet;

        public EttChanel(Pin pin, Net net)
        {
            ChanelPin = pin;
            ChanelNet = net;
        }
    }
}
