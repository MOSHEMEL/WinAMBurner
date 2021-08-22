using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Ports;

namespace WinAMBurner
{
    //enum ErrCode
    //{
    //    OK = 0,
    //    ERROR = -1,
    //    EPARAM = -2,
    //    MAX = -3,
    //    ERASE = -4
    //}

    class SerialPortEventArgs : EventArgs
    {
        public int maximum;
        public int progress;
        public SerialPortEventArgs(int maximum, int progress)
        {
            this.maximum = maximum;
            this.progress = progress;
        }
    }

    class AM
    {
        private SerialPort serialPort;
        private const int serialPortBaudRate = 115200;

        private const int RD_TIMEOUT = 1000;
        private const int WR_TIMEOUT = 1000;
        private const int MAX_TIMEOUT = 3000;
        private const uint ERROR = 0xFFFFFFFF;
        private const int ID_LENGTH = 3;

        private readonly DateTime epoch = new DateTime(1970, 1, 1, 2, 0, 0, DateTimeKind.Utc);

        private uint snum = ERROR;
        private uint maxi = ERROR;
        private uint maxiSet = ERROR;
        private double date = 0;
        private uint[] id = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
        private uint[] aptxId = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
        private uint factor = ERROR;

        public uint SNum { get => snum; set => snum = value; }
        public uint Maxi { get => maxi; set => maxi = value; }
        public uint MaxiSet { get => maxiSet; set => maxiSet = value; }
        public uint Factor { get => factor; set => factor = value; }
        public uint[] AptxId
        {
            get
            {
                uint[] value = new uint[ID_LENGTH];
                for (int i = 0; i < aptxId.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(aptxId[i]);
                    Array.Reverse(bytes, 0, bytes.Length);
                    value[i] = BitConverter.ToUInt32(bytes, 0);
                }
                return value;
            }
            set
            {
                for (int i = 0; i < aptxId.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(value[i]);
                    Array.Reverse(bytes, 0, bytes.Length);
                    aptxId[i] = BitConverter.ToUInt32(bytes, 0);
                }
            }
        }

        public DateTime Date
        {
            get => epoch.AddSeconds(date);
            set => date = value.Subtract(epoch).TotalSeconds;
        }

        public event EventHandler serialPortProgressEvent;

        public AM()
        {
            serialPort = new SerialPort();
            serialPort.BaudRate = serialPortBaudRate;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = RD_TIMEOUT;
            serialPort.WriteTimeout = WR_TIMEOUT;
        }

        public async Task<ErrCode> AMDataCheckConnect()
        {
            ErrCode errcode = ErrCode.ERROR;
            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string port in serialPorts)
            {
                serialPort.PortName = port;
                errcode = await serialPortCheckConnect();
                if (errcode == ErrCode.OK)
                {
                    LogFile.logWrite(serialPort.PortName);
                    break;
                }
            }
            return errcode;
        }

        private async Task<ErrCode> serialPortCheckConnect()
        {
            ErrCode errcode = ErrCode.ERROR;
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            List<string> cmd = new List<string>();
            cmd.Add("getid,3#");
            //cmd.Add("debug#");

            string dataRdStr = await serialReadWrite(cmd);
            //write to log
            LogFile.logWrite(cmd, dataRdStr);

            if (dataRdStr.Contains("Burn the cow, Armenta Ltd 2020 Version"))
            {
                dataRdStr = await serialReadWrite(cmd);
                LogFile.logWrite(cmd, dataRdStr);
            }
            //parse data
            errcode = amDataParseId(dataRdStr);
            try
            {
                serialPort.Close();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            return errcode;
        }

        public async Task<ErrCode> AMDataRead()
        {
            ErrCode errcode = ErrCode.ERROR;
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            List<string> cmd = new List<string>();
            cmd.Add("rd,3,0x0001008#");
            cmd.Add("rd,3,0x0001004#");
            cmd.Add("rd,3,0x0001000#");
            cmd.Add("getid,3#");
            cmd.Add("rd,3,0x000FFFF6#");
            //cmd.Add("Read SNUM 3#");
            cmd.Add("rd,3,0x000FFFEE#");
            //cmd.Add("Read DATE 3#");
            cmd.Add("rd,3,0x000FFFE6#");
            cmd.Add("find,3,1#");
            //cmd.Add("Read MAX 3#");
            //cmd.Add("status2,3#");
            //cmd.Add("status1,3#");
            //cmd.Add("testread,3#");
            //cmd.Add("debug#");

            string dataRdStr = await serialReadWrite(cmd);
            //write to log
            LogFile.logWrite(cmd, dataRdStr);
            //parse data
            errcode = amDataParse(dataRdStr);
            try
            {
                serialPort.Close();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            return errcode;
        }

        public async Task<ErrCode> AMDataWrite()
        {
            ErrCode errcode = ErrCode.ERROR;
            //if ((snum == 0) || (maxiSet == 0))
            if (snum == 0)
            {
                errcode = ErrCode.EPARAM;
                return errcode;
            }
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            List<string> cmd = new List<string>();
            //cmd.Add("!!!WRITE COMPLETE!!!#");
            //cmd.Add(string.Format("wrt,3,0x0001008,00{0:x}#", aptxId[2]));
            //cmd.Add(string.Format("wrt,3,0x0001004,00{0:x}#", aptxId[1]));
            //cmd.Add(string.Format("wrt,3,0x0001000,00{0:x}#", aptxId[0]));
            cmd.Add(string.Format("wrt,3,0x00FFFDE,00{0:x}#", factor));
            cmd.Add(string.Format("snum,3,{0}#", snum));
            cmd.Add(string.Format("maxi,3,{0}#", maxi + maxiSet));
            Date = DateTime.Now;
            cmd.Add(string.Format("date,3,{0}#", (int)date));
            //erase
            //cmd.Add("scan,3,0#");
            //cmd.Add("scan,3,0#");
            //cmd.Add("scan,3,0#");
            for (int i = 0; i < 10; i++)
                cmd.Add("NOP#");
            //cmd.Add("nuke,3#");
            cmd.Add("erase,3,0xff000#");
            //erase
            //cmd.Add("set registers readonly#");
            //cmd.Add("status2,3#");
            //cmd.Add("status1,3#");
            //cmd.Add("statusw,3,8001#");
            //cmd.Add("clear registers#");
            //cmd.Add("status2,3#");
            //cmd.Add("status1,3#");
            //cmd.Add("statusw,3,0000#");
            //cmd.Add("status2,3#");
            //cmd.Add("status1,3#");
            //cmd.Add("testread,3#");
            //cmd.Add("debug#");

            string dataRdStr = await serialReadWrite(cmd);
            //write to log
            LogFile.logWrite(cmd, dataRdStr);
            try
            {
                serialPort.Close();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            errcode = ErrCode.OK;
            return errcode;
        }

        private ErrCode amDataParse(string dataRdStr)
        {
            ErrCode errcode = ErrCode.ERROR;
            //uint[] lid = new uint[3] { ERROR, ERROR, ERROR };
            uint lmaxi = ERROR;
            uint ldate = ERROR;
            uint lsnum = ERROR;
            uint[] laptxId = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
            uint lfactor = ERROR;
            //parse to lines
            string[] dataRd = amDataParseStr(dataRdStr);
            //maxi
            //if ((dataLineParse(dataRd, "0x1f-0x85-0x01", ref lid) >= 0) &&
            if ((amDataParseId(dataRdStr) >= 0) &&
                //maxi
                (dataLineParse(dataRd, "0xFFFE6", ref lmaxi) == ErrCode.OK) &&
                //date
                (dataLineParse(dataRd, "0xFFFEE", ref ldate) == ErrCode.OK) &&
                //snum
                (dataLineParse(dataRd, "0xFFFF6", ref lsnum) == ErrCode.OK) &&
                //aptx_id[0]
                (dataLineParse(dataRd, "0x1000", ref laptxId[0]) == ErrCode.OK) &&
                //aptx_id[1]
                (dataLineParse(dataRd, "0x1004", ref laptxId[1]) == ErrCode.OK) &&
                //aptx_id[2]
                (dataLineParse(dataRd, "0x1008", ref laptxId[2]) == ErrCode.OK) &&
                //factor
                (dataLineParse(dataRd, "pulses written", ref lfactor) == ErrCode.OK))
            {
                if (snum == ERROR)
                    snum = lsnum;
                else
                {
                    if (lsnum != snum)
                    {
                        errcode = ErrCode.ERROR;
                        return errcode;
                    }
                }

                if (checkError(aptxId))
                {
                    for (int i = 0; i < laptxId.Length; i++)
                        aptxId[i] = laptxId[i];
                }
                else
                {
                    for (int i = 0; i < laptxId.Length; i++)
                    {
                        if (laptxId[i] != aptxId[i])
                        {
                            errcode = ErrCode.ERROR;
                            return errcode;
                        }
                    }
                }
                maxi = lmaxi;
                date = ldate;
                if (factor == ERROR)
                    factor = lfactor;
                else
                {
                    if (lfactor != factor)
                    {
                        errcode = ErrCode.ERROR;
                        return errcode;
                    }
                }
                errcode = ErrCode.OK;
            }
            return errcode;
        }

        private ErrCode amDataParseId(string dataRdStr)
        {
            ErrCode errcode = ErrCode.ERROR;
            uint[] lid = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
            //parse to lines
            string[] dataRd = amDataParseStr(dataRdStr);
            //maxi
            if ((dataLineParse(dataRd, "0x1f-0x85-0x01", ref lid) >= 0))
            {
                if (checkError(id))
                {
                    for (int i = 0; i < lid.Length; i++)
                        id[i] = lid[i];
                }
                else
                {
                    for (int i = 0; i < lid.Length; i++)
                    {
                        if (lid[i] != id[i])
                        {
                            errcode = ErrCode.ERROR;
                            return errcode;
                        }
                    }
                }
                errcode = ErrCode.OK;
            }
            return errcode;
        }

        private bool checkError(uint [] prms)
        {
            foreach (uint prm in prms)
                if (prm == ERROR)
                    return true;
            return false;
        }

        private string[] amDataParseStr(string dataRdStr)
        {
            return dataRdStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private ErrCode dataLineParse(string[] dataRd, string pattern, ref uint number)
        {
            ErrCode errcode = ErrCode.ERROR;
            string? dataFind = dataRd.ToList().Find(data => data.Contains(pattern));
            if (dataFind != null)
            {
                string? snumber = new string(dataFind.SkipWhile(c => c != 'x').Skip(1).TakeWhile(c => c != ':').ToArray());
                if (snumber != null)
                {
                    if (uint.TryParse(snumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
                    {
                        errcode = ErrCode.OK;
                    }
                    else
                    {
                        snumber = new string(dataFind.SkipWhile(c => (c < '0') || (c > '9')).ToArray());
                        if (snumber != null)
                        {
                            if (uint.TryParse(snumber, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                                errcode = ErrCode.OK;
                        }
                    }
                }

                //string[] dataSplit = dataFind.Split(new char[] { ' ', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries);
                //if (uint.TryParse(dataSplit[3], NumberStyles.HexNumber,
                //    CultureInfo.InvariantCulture, out number))
                //    errcode = ErrCode.OK;
            }
            return errcode;
        }

        private ErrCode dataLineParse(string[] dataRd, string pattern, ref uint[] number)
        {
            //ErrCode errcode = ErrCode.ERROR;
            ErrCode errcode = ErrCode.OK;
            string? dataFind = dataRd.ToList().Find(data => data.Contains(pattern));
            if (dataFind != null)
            {
                string[] dataSplit = dataFind.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < number.Count(); i++)
                {
                    if (i < dataSplit.Count())
                    {
                        if (!uint.TryParse(new string(dataSplit[i].SkipWhile(c => c != 'x').Skip(1).ToArray())
                            , NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number[i]))
                            errcode = ErrCode.ERROR;
                    }
                    else
                        errcode = ErrCode.ERROR;
                }
                //string[] dataSplit = dataFind.Split(new char[] { 'x', '-' }, StringSplitOptions.RemoveEmptyEntries);
                //if (uint.TryParse(dataSplit[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number[0]) &&
                //    uint.TryParse(dataSplit[3], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number[1]) &&
                //    uint.TryParse(dataSplit[5], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number[2]))
                //    errcode = ErrCode.OK;
            }
            return errcode;
        }

        private async Task<string> serialReadWrite(List<string> cmd)
        {
            int time = 0;
            string dataRdStr = string.Empty;
            string dataDump = string.Empty;
            try
            {
                dataDump += serialPort.ReadExisting();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return dataDump;
            }
            for (int i = cmd.Count - 1; i >= 0; i--)
            {
                try
                {
                    serialPort.Write(cmd[i]);
                }
                catch (Exception e)
                {
                    LogFile.logWrite(e.ToString());
                    return dataRdStr;
                }
                time = 0;
                string dataExist = string.Empty;
                while (time < MAX_TIMEOUT)
                {
                    await Task.Delay(RD_TIMEOUT);
                    try
                    {
                        dataExist += serialPort.ReadExisting();
                    }
                    catch (Exception e)
                    {
                        LogFile.logWrite(e.ToString());
                        return dataRdStr;
                    }
                    if (dataExist == string.Empty)
                        time += RD_TIMEOUT;
                    else
                        break;
                }
                dataRdStr += dataExist;
                serialPortProgressEvent.Invoke(this, new SerialPortEventArgs(cmd.Count - 1, cmd.Count - 1 - i));
                //, new AsyncCallback(delegate (IAsyncResult target) { return; }), new object());
            }
            return dataRdStr;
        }
    }
}