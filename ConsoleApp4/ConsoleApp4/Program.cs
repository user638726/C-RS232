using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using Mono.Terminal;

namespace ConsoleApp3
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            LineEditor le = new LineEditor("foo")
            {
                HeuristicsMode = "csharp"
            };


            SerialPort mySerialPort = new SerialPort("COM5");

            mySerialPort.BaudRate = 115200;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DtrEnable = true;
            mySerialPort.ReadTimeout = 2000;
            mySerialPort.WriteTimeout = 1500;
            mySerialPort.Open();
           
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            le.AutoCompleteEvent += delegate (string text, int pos) {
                string prefix = "";
                var completions = new string[] {
                "sys" };
                return new Mono.Terminal.LineEditor.Completion(prefix, completions);
            };
            string s;

            // Prompts the user for input
            while ((s = le.Edit("bp1> ", "")) != null)
            {
                mySerialPort.Write("s");
                mySerialPort.Write("y");
                mySerialPort.Write("s");
                mySerialPort.Write("\r");
            }
           
        }
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.Write(indata);
        }

    }

}
