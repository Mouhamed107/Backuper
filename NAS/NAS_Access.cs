using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
namespace Backuper.NAS
{
    public class NAS_Access
    {
        IPAddress _address;
        int _port;
        public NAS_Access()
        {

        }
        public NAS_Access(IPAddress iPAddress, int port)
        {
            this._address = iPAddress;
            this._port = port;
        }
        public static string getSharedFolder()
        {
            return "\\\\172.16.234.101\\NAS\\site1\\";
        }
        public static string getUsername()
        {
            return "mouhamedftp";
            //return "marckshop";
        }
        public static string getPasseword()
        {
            return "1234";
           // return "morata123456%";
        }
        public static int getPort()
        {
            return 5000; 
        }
        public static string getDomaine()
        {
            //return "ftp://192.168.1.20/";
            return "ftp://files.000webhost.com/";
        }
        public static string getBackupFolder()
        {
           // return "ftp://files.000webhost.com/Backup/";
            return "ftp://172.16.234.112/Backup/";
        }
    }
}
