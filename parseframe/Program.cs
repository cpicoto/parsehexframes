using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parseframe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("parseframe <File with Frames>.hex");
                Environment.Exit(0);
            }
            var fs = new FileStream(args[0], FileMode.Open);
            byte[] header = new byte[37];
            byte[] packet;
            Boolean continueFlag = true;
            int size;
            String _TxCall="";
            String _RxCall = "";
            while (fs.Position < fs.Length)
            {
                _TxCall = "";
                _RxCall = "";
                fs.Read(header, 0, 36);
                size = (int)header[28]+(int)header[29]*256;
                packet = new byte[size];
                switch ((char)header[4])
                {                   
                    case 'G':
                        Console.WriteLine("G Packet with {0} {0:X2}", size);
                        fs.Read(packet,0, size);
                        Console.WriteLine("Packet:[{0}]", System.Text.Encoding.Default.GetString(packet)); 
                        break;
                    case 'K':
                        //Console.WriteLine("K Packet with {0}", size);
                        
                        fs.Read(packet,0, size);
                        //Console.WriteLine("Packet:[{0}]", System.Text.Encoding.Default.GetString(packet));
                        if (size > 13)
                        {
                            for (int k = 1; k < 6; k++)
                            {
                                if (((packet[k] / 2) > 31) && ((packet[k] / 2 < 256)))
                                    _TxCall += (Char)(packet[k] / 2);
                            }


                            for (int k = 8; k < 13; k++)
                            {
                                if (((packet[k] / 2) > 31) && ((packet[k] / 2 < 256)))
                                    _RxCall += (Char)(packet[k] / 2);
                            }
                            Console.WriteLine("K packet From: {0} -> {1} Size:{2} @{3} @{3:X4}", _RxCall, _TxCall, size, fs.Position - size);
                        } else
                        {
                            Console.WriteLine("K packet too small: {0} @{1} @{1:X4}", size, fs.Position - size);
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown packet type:{0} size:{1} @{2:X4}", (char)header[4], (int)header[28], fs.Position - size);
                        break;

                }
            }
        }
    }
}
