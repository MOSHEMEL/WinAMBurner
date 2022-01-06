using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Ports;

namespace WinAMBurner
{
    public class Am
    {
        private SerialPort serialPort;
        private const int serialPortBaudRate = 115200;

        private const int RD_TIMEOUT = 1000;
        private const int WR_TIMEOUT = 1000;
        private const int MAX_TIMEOUT = 3000;
        public const uint ERROR = 0xffffffff;
        private const int ID_LENGTH = 3;
        private const uint TOLERANCE = 10;
        private const uint MAGIC_NUM = 0x444f4e45;

        private readonly DateTime epoch = new DateTime(1970, 1, 1, 2, 0, 0, DateTimeKind.Utc);

        private uint snum = ERROR;
        private uint maxi = ERROR;
        private uint maxiprev = ERROR;
        private uint maxiSet = ERROR;
        private double date = ERROR;
        private uint factor = ERROR;
        private uint remaining = ERROR;
        private uint bckup_snum = ERROR;
        private uint bckup_maxi = ERROR;
        private uint bckup_maxiprev = ERROR;
        private double bckup_date = ERROR;
        private uint bckup_factor = ERROR;
        private uint backup_remaining = ERROR;
        private uint[] id = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
        private uint[] aptxId = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };

        double get(double param)
        {
            return param == ERROR ? 0 : param;
        }
        
        double set(double param)
        {
            return param == 0 ? ERROR : param;
        }

        uint get(uint param)
        {
            return (uint)get((double)param);
        }
        
        uint set(uint param)
        {
            return (uint)set((double)param);
        }

        //public uint SNum { get => get(snum); set => snum = set(value); }
        //public uint MaxiSet { get => get(maxiSet); set => maxiSet = set(value); }
        //public uint CurrentPrev { get => get(maxiprev - factor); }
        //public uint[] AptxId { get => aptxId.Reverse().Select(a => { return get(a); }).ToArray(); set => value.Reverse().Select(a => { return set(a); }); }
        public uint SNum { get => snum; set => snum = value; }
        //public uint Maxi { get => maxi; set => maxi = value; }
        public uint Maxi { get => get(maxi); set => maxi = set(value); }
        public uint MaxiPrev { get => maxiprev; set => maxiprev = value; }
        public uint MaxiSet { get => maxiSet; set => maxiSet = value; }
        //public uint Factor { get => factor; set => factor = value; }
        public uint Factor { get => get(factor); set => factor = set(value); }
        //public uint Current { get => maxi - factor; }
        public uint Current { get => get(maxi) - get(factor); }
        public uint CurrentPrev { get => maxiprev - factor; }
        //public uint[] AptxId { get => aptxId.Reverse().Select(a => { return a; }).ToArray(); set => aptxId = value.Reverse().Select(a => { return a; }); }
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
            get => epoch.AddSeconds(get(date));
            set => date = set(value.Subtract(epoch).TotalSeconds);
        }

        public delegate void dProgress(Object progress, bool reset);
        public dProgress dprogress;
        public Object progress;
        //public bool Nuke;

        public Am(dProgress dprogress)
        {
            serialPort = new SerialPort();
            serialPort.BaudRate = serialPortBaudRate;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            //serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = RD_TIMEOUT;
            serialPort.WriteTimeout = WR_TIMEOUT;
            serialPort.ReadBufferSize = 8192;
            this.dprogress = dprogress;
        }

        public async Task<ErrCode> AMCheckConnect()
        {
            ErrCode errcode = ErrCode.ECONNECT;
            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string port in serialPorts)
            {
                serialPort.PortName = port;
                errcode = await AMCmd(Cmd.ID);
                if (errcode == ErrCode.OK)
                {
                    LogFile.logWrite(serialPort.PortName);
                    break;
                }
            }
            return errcode;
        }

        public async Task<ErrCode> AMCmd(Cmd cmd)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
                return errcode;
            }
            switch (cmd)
            {
                case Cmd.ID:
                    if ((errcode = await amCmdBlock(Cmd.ID)) == ErrCode.OK)
                        errcode = ErrCode.OK;
                    break;
                case Cmd.READ:
                    if ((errcode = await amCmdBlock(Cmd.ID)) == ErrCode.OK)
                        if ((errcode = await amCmdBlock(Cmd.READ_01)) == ErrCode.OK)
                            if ((errcode = await amCmdBlock(Cmd.READ_03_FF, "FF")) == ErrCode.OK)
                                errcode = ErrCode.OK;
                    break;
                case Cmd.WRITE:
                    if (maxi == ERROR)
                        maxi = 0;
                    if (maxiprev == ERROR)
                        maxiprev = 0;
                    if ((errcode = await amCmdBlock(Cmd.ID)) == ErrCode.OK)
                        if ((errcode = await amCmdBlock(Cmd.WRITE_03_FF, "3", maxi + maxiSet)) == ErrCode.OK)
                            if ((errcode = await amCmdBlock(Cmd.WRITE_03_FF, "FF", maxi + maxiSet)) == ErrCode.OK)
                                errcode = ErrCode.OK;
                    break;
                case Cmd.RESTORE:
                    if ((errcode = await amCmdBlock(Cmd.ID)) == ErrCode.OK)
                        if ((errcode = await amCmdBlock(Cmd.READ_03_FF, "3")) == ErrCode.OK)
                        {
                            snum = bckup_snum;
                            maxi = bckup_maxi;
                            date = bckup_date;
                            factor = bckup_factor;

                            if ((errcode = await amCmdBlock(Cmd.WRITE_03_FF, "FF", maxi)) == ErrCode.OK)
                                if ((errcode = await amCmdBlock(Cmd.READ_03_FF, "FF")) == ErrCode.OK)
                                    errcode = ErrCode.OK;
                        }
                    break;
                case Cmd.INIT:
                    //if (Nuke)
                    //    if ((errcode = await amCmdBlock(Cmd.NUKE)) != ErrCode.OK)
                    //        break;
                    errcode = await amCmdBlock(Cmd.NUKE);
                    errcode = await amCmdBlock(Cmd.WRITE_00);
                    errcode = await amCmdBlock(Cmd.READ_00);
                    errcode = await amCmdBlock(Cmd.WRITE_01);
                    errcode = await amCmdBlock(Cmd.READ_01);
                    errcode = await amCmdBlock(Cmd.WRITE_03_FF, "FF", maxi);
                    errcode = await amCmdBlock(Cmd.READ_03_FF, "FF");
                    break;
                case Cmd.READALL:
                    errcode = await amCmdBlock(Cmd.ID);
                    errcode = await amCmdBlock(Cmd.READ_01);
                    errcode = await amCmdBlock(Cmd.READ_03_FF, "FF");
                    break;
            }
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

        //private async Task<ErrCode> amReadBlock(string blockNum)
        private async Task<ErrCode> amCmdBlock(Cmd cmd, string blockNum = "", uint max = 0)
        {
            List<string> cmds = new List<string>();
            if (cmd == Cmd.ID)
            {
                get_id(cmds);
            }
            else if (cmd == Cmd.READ_01)
            {
                read_01(cmds);
            }
            else if (cmd == Cmd.WRITE_01)
            {
                write_01(cmds);
            }
            else if (cmd == Cmd.READ_03_FF)
            {
                read_03_FF(blockNum, cmds);
            }
            else if (cmd == Cmd.WRITE_03_FF)
            {
                write_03_FF(blockNum, cmds, max);
            }
            else if (cmd == Cmd.READ_00)
            {
                read_00(cmds);
            }
            else if (cmd == Cmd.WRITE_00)
            {
                write_00(cmds);
            }
            else if (cmd == Cmd.NUKE)
            {
                nuke(cmds);
            }
            cmds.Add("NOP#");
            cmds.Add("NOP#");
            LogFile.logWrite(cmds);

            string dataRdStr = await serialReadWrite(cmds);
            //write to log
            LogFile.logWrite(dataSplit(dataRdStr));
            if (cmd == Cmd.WRITE_03_FF)
            {
                uint lmaxi = ERROR;
                uint ldate = ERROR;
                uint lsnum = ERROR;
                uint lfactor = ERROR;
                uint lremaining = ERROR;
                return dataLineParseCheck_03_FF(dataSplit(dataRdStr), blockNum, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining, false);
            }
            else if (cmd == Cmd.WRITE_00)
            {
                uint lfactor = ERROR;
                return dataLineParseCheck_00(dataSplit(dataRdStr), ref lfactor);
            }
            else if (cmd == Cmd.WRITE_01)
            {
                uint[] laptxid = { ERROR, ERROR, ERROR };
                return dataLineParseCheck_01(dataSplit(dataRdStr), laptxid);
            }
            else if (cmd == Cmd.NUKE)
            {
                uint lnuke = ERROR;
                return dataLineParseCheck_Nuke(dataSplit(dataRdStr), lnuke);
            }
            else
            {
                return parseBlock(dataRdStr, cmd, blockNum);
            }
        }

        private static void get_id(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add("getid,3#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void read_01(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add("rd,3,0x0001008#");
                cmd.Add("rd,3,0x0001004#");
                cmd.Add("rd,3,0x0001000#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void write_01(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add(string.Format("wrt,3,0x0001008,00{0:x}#", aptxId[0]));
                cmd.Add(string.Format("wrt,3,0x0001004,00{0:x}#", aptxId[1]));
                cmd.Add(string.Format("wrt,3,0x0001000,00{0:x}#", aptxId[2]));
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void read_03_FF(string blockNum, List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add("rd,3,0x000" + blockNum + "F40#");
                cmd.Add("Read REMAINING 3#");
                cmd.Add("rd,3,0x000" + blockNum + "FF6#");
                cmd.Add("Read SNUM 3#");
                cmd.Add("rd,3,0x000" + blockNum + "FEE#");
                cmd.Add("Read DATE 3#");
                cmd.Add("rd,3,0x000" + blockNum + "FE6#");
                cmd.Add("Read MAX 3#");
                cmd.Add("status2,3#");
                cmd.Add("status1,3#");
                if (blockNum == "3")
                    cmd.Add("rd,3,0x000" + blockNum + "F50#");
                else
                    cmd.Add("find,3,1#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void write_03_FF(string blockNum, List<string> cmd, uint max)
        {
            if (cmd != null)
            {
                cmd.Add("!!!WRITE COMPLETE!!!#");
                //cmd.Add(string.Format("snum,3,{0}#", snum));
                //cmd.Add(string.Format("maxi,3,{0}#", maxi + maxiSet));
                //cmd.Add(string.Format("date,3,{0}#", (int)date));
                //cmd.Add(string.Format("wrt,3,0x00FFF50,00{0:x}#", factor));
                if ((factor >= max) && (factor <= (max + TOLERANCE)))
                    cmd.Add(string.Format("wrt,3,0x00" + blockNum + "F40,00{0:x}#", MAGIC_NUM));
                cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FF6,00{0:x}#", snum));
                //cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FEE,00{0:x}#", (int)date));
                cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FEE,00{0:x}#", (int)DateTime.Now.Subtract(epoch).TotalSeconds));
                //cmd.Add(string.Format("wrt,3,0x00" + blokNum + "FE6,00{0:x}#", maxi + maxiSet));
                cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FE6,00{0:x}#", max));
                cmd.Add(string.Format("wrt,3,0x00" + blockNum + "F50,00{0:x}#", factor));

                //erase
                cmd.Add("scan,3,0#");
                cmd.Add("scan,3,0#");
                cmd.Add("scan,3,0#");
                for (int i = 0; i < 10; i++)
                    cmd.Add("NOP#");
                //cmd.Add("nuke,3#");
                cmd.Add("erase,3,0x" + blockNum + "000#");
                //erase
                cmd.Add("set registers readonly#");
                cmd.Add("status2,3#");
                cmd.Add("status1,3#");
                cmd.Add("statusw,3,8001#");
                cmd.Add("clear registers#");
                cmd.Add("status2,3#");
                cmd.Add("status1,3#");
                cmd.Add("statusw,3,0000#");
                cmd.Add("status2,3#");
                cmd.Add("status1,3#");
            }
        }

        private void read_00(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add("rd,3,0x0000000#");
            }
        }

        private void write_00(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add(string.Format("wrt,3,0x000000,00{0:x}#", factor));
            }
        }

        private void nuke(List<string> cmd)
        {
            if (cmd != null)
            {
                cmd.Add("scan,3,0#");
                cmd.Add("scan,3,0#");
                cmd.Add("scan,3,0#");
                for (int i = 0; i < 10; i++)
                    cmd.Add("NOP#");
                cmd.Add("nuke,3#");
            }
        }

        private ErrCode parseBlock(string dataRdStr, Cmd cmd, string blokNum)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRdStr != null)
            {
                //parse to lines
                List<string> dataRd = dataSplit(dataRdStr);

                if (dataRd != null)
                {
                    //id
                    if (cmd == Cmd.ID)
                    {
                        errcode = dataLineParse_id(dataRd);
                    }
                    else if (cmd == Cmd.READ_00)
                    {
                        errcode = dataLineParse_00(dataRd);
                    }
                    else if (cmd == Cmd.READ_01)
                    {
                        errcode = dataLineParse_01(dataRd);
                    }
                    else if (cmd == Cmd.READ_03_FF)
                    {
                        errcode = dataLineParse_03_FF(dataRd, blokNum);
                    }
                    else
                    {
                        LogFile.logWrite(string.Format("{0} cmd {1}", ErrCode.EUNKNOWN, cmd));
                        errcode = ErrCode.EUNKNOWN;
                    }
                }
                else
                {
                    LogFile.logWrite(string.Format("{0} dataRd {1}", ErrCode.EPARSE, dataRd));
                    errcode = ErrCode.EPARSE;
                }
            }
            else
            {
                LogFile.logWrite(string.Format("{0} dataRdStr {1}", ErrCode.EPARSE, dataRdStr));
                errcode = ErrCode.EPARSE;
            }
            return errcode;
        }

        private ErrCode dataLineSet_id(uint[] id, uint[] lid)
        {
            ErrCode errcode = ErrCode.OK;
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
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} lid[{1}] {2} id[{3}] {4}", errcode, i, lid[i], i, id[i]));
                    }
                }
            }
            LogFile.logWrite(string.Format("set id: {0}", id.Aggregate("", (r, m) => r += "0x" + m.ToString("x") + " ")));
            return errcode;
        }

        private ErrCode dataLineParse_id(List<string> dataRd)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                uint[] lid = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
                if (dataLineParseCheck_id(dataRd, lid) == ErrCode.OK)
                {
                    LogFile.logWrite(string.Format("set Id:"));
                    errcode = dataLineSet_id(id, lid);
                }
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParse_01(List<string> dataRd)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                uint[] laptxId = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
                if (dataLineParseCheck_01(dataRd, laptxId) == ErrCode.OK)
                {
                    if (!checkError(laptxId))
                    {
                        LogFile.logWrite(string.Format("set aptxId:"));
                        errcode = dataLineSet_id(aptxId, laptxId);
                    }
                    else
                    {
                        errcode = ErrCode.EREMOTE;
                        LogFile.logWrite(string.Format("{0} laptxId {1}", errcode, laptxId.Aggregate("", (r, m) => r += "0x" + m.ToString("x") + " ")));
                    }
                }
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParse_00(List<string> dataRd)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                uint lfactor = ERROR;
                //factor
                if (dataLineParseCheck_00(dataRd, ref lfactor) == ErrCode.OK)
                {
                    if (factor == ERROR)
                    {
                        factor = lfactor;
                    }
                    else
                    {
                        if (lfactor > (factor + TOLERANCE))
                        {
                            errcode = ErrCode.EPARSE;
                            LogFile.logWrite(string.Format("{0} lfactor {1} factor {2}", errcode, lfactor, factor));
                        }
                    }
                    LogFile.logWrite(string.Format("set factor: {0}", factor));
                }
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineSet_03_FF(ref uint maxiprev, ref uint maxi, ref double date, ref uint snum, ref uint factor, ref uint remaining, ref uint lmaxi, ref uint ldate, ref uint lsnum, ref uint lfactor, ref uint lremaining)
        {
            ErrCode errcode = ErrCode.OK;
            if ((lsnum != 0) && (lsnum != ERROR))
            {
                if (snum == ERROR)
                {
                    snum = lsnum;
                }
                else
                {
                    if (lsnum != snum)
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} lsnum {1} snum {2}", errcode, lsnum, snum));
                    }
                }
                LogFile.logWrite(string.Format("set snum: {0}", snum));
                if (factor == ERROR)
                {
                    factor = lfactor;
                    if (lremaining == MAGIC_NUM)
                        factor = lmaxi;
                }
                else
                {
                    if (lfactor > (factor + TOLERANCE))
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} lfactor {1} factor {2}", errcode, lfactor, factor));
                    }
                }
                LogFile.logWrite(string.Format("set factor: {0}", factor));
                maxiprev = maxi;
                LogFile.logWrite(string.Format("set maxiprev: {0}", maxiprev));
                maxi = lmaxi;
                LogFile.logWrite(string.Format("set maxi: {0}", maxi));
                date = ldate;
                LogFile.logWrite(string.Format("set date: {0}", date));
                remaining = lremaining;
                LogFile.logWrite(string.Format("set remaining: {0}", remaining));
            }
            else
            {
                errcode = ErrCode.EEMPTY;
                LogFile.logWrite(string.Format("{0} lsnum {1} lmaxi {2} lfactor {3}", errcode, lsnum, lmaxi, lfactor));
            }
            return errcode;
        }

        private ErrCode dataLineParse_03_FF(List<string> dataRd, string blokNum)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                uint lmaxi = ERROR;
                uint ldate = ERROR;
                uint lsnum = ERROR;
                uint lfactor = ERROR;
                uint lremaining = ERROR;
                //maxi
                if (dataLineParseCheck_03_FF(dataRd, blokNum, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining, true) == ErrCode.OK)
                {
                    if (blokNum == "3")
                        errcode = dataLineSet_03_FF(ref bckup_maxiprev, ref bckup_maxi, ref bckup_date, ref bckup_snum, ref bckup_factor, ref backup_remaining, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining);
                    else
                        errcode = dataLineSet_03_FF(ref maxiprev, ref maxi, ref date, ref snum, ref factor, ref remaining, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining);
                }
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParseCheck_01(List<string> dataRd, uint[] laptxId)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                if ((errcode = dataLineParse(dataRd, "0x1000", ref laptxId[0])) == ErrCode.OK)
                    if ((errcode = dataLineParse(dataRd, "0x1004", ref laptxId[1])) == ErrCode.OK)
                        if ((errcode = dataLineParse(dataRd, "0x1008", ref laptxId[2])) == ErrCode.OK)
                            errcode = ErrCode.OK;
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParseCheck_03_FF(List<string> dataRd, string blokNum, ref uint lmaxi, ref uint ldate, ref uint lsnum, ref uint lfactor, ref uint lremaining, bool read)
        {
            
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                //maxi
                if ((errcode = dataLineParse(dataRd, "0x" + blokNum + "FE6", ref lmaxi)) == ErrCode.OK)
                    //date
                    if ((errcode = dataLineParse(dataRd, "0x" + blokNum + "FEE", ref ldate)) == ErrCode.OK)
                        //snum
                        if ((errcode = dataLineParse(dataRd, "0x" + blokNum + "FF6", ref lsnum)) == ErrCode.OK)
                            //factor
                            if (((read) && ((errcode = dataLineParse(dataRd, "pulses written", ref lfactor)) == ErrCode.OK)) ||
                                ((!read) && ((errcode = dataLineParse(dataRd, "0x" + blokNum + "F50", ref lfactor)) == ErrCode.OK)))
                                //remaining
                                if (((read) && ((errcode = dataLineParse(dataRd, "0x" + blokNum + "F40", ref lremaining)) == ErrCode.OK)) ||
                                    (!read))
                                    errcode = ErrCode.OK;
            }
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParseCheck_00(List<string> dataRd, ref uint lfactor)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
                //factor
                errcode = dataLineParse(dataRd, "address 0x0", ref lfactor);
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParseCheck_Nuke(List<string> dataRd, uint lnuke)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
                //nuke
                errcode = dataLineParse(dataRd, "nuke,", ref lnuke);
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private ErrCode dataLineParseCheck_id(List<string> dataRd, uint[] lid)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
                //id
                errcode = dataLineParse(dataRd, "0x1f-0x85-0x01", ref lid);
            else
                errcode = ErrCode.EPARSE;
            return errcode;
        }

        private bool checkError(uint [] prms)
        {
            foreach (uint prm in prms)
                if (prm == ERROR)
                    return true;
            return false;
        }

        private List<string> dataSplit(string dataRdStr)
        {
            string[] dataSplit = null;
            if (dataRdStr != null)
                dataSplit = dataRdStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return dataSplit.ToList();
        }

        private ErrCode dataLineParse(List<string> dataRd, string pattern, ref uint number)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                string? dataFind = dataRd.Find(data => data.Contains(pattern));
                if (dataFind != null)
                {
                    string? snumber = new string(dataFind.SkipWhile(c => c != 'x').Skip(1).TakeWhile(c => c != ':').ToArray());
                    if (snumber != null)
                    {
                        if (!uint.TryParse(snumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
                        {

                            string? logsnum = snumber;
                            snumber = new string(dataFind.SkipWhile(c => (c < '0') || (c > '9')).ToArray());
                            if (snumber != null)
                            {
                                if (!uint.TryParse(snumber, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                                {
                                    errcode = ErrCode.EPARSE;
                                    LogFile.logWrite(string.Format("{0} pattern \"{1}\" logsnum {2} snumber {3} number {4}", errcode, pattern, logsnum, snumber, number));
                                }
                            }
                            else
                            {
                                errcode = ErrCode.EPARSE;
                                LogFile.logWrite(string.Format("{0} pattern \"{1}\" logsnum {2} snumber {3}", errcode, pattern, logsnum, snumber));
                            }
                        }
                    }
                    else
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} pattern \"{1}\" snumber {2}", errcode, pattern, snumber));
                    }
                }
                else
                {
                    errcode = ErrCode.EPARSE;
                    LogFile.logWrite(string.Format("{0} dataFind {1} pattern \"{2}\"", errcode, dataFind, pattern));
                }
            }
            else
            {
                errcode = ErrCode.EPARSE;
                LogFile.logWrite(string.Format("{0} dataRd {1} pattern \"{2}\"", errcode, dataRd, pattern));
            }
            return errcode;
        }

        private ErrCode dataLineParse(List<string> dataRd, string pattern, ref uint[] numbers)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                string? dataFind = dataRd.Find(data => data.Contains(pattern));
                if (dataFind != null)
                {
                    string[] dataSplit = dataFind.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (dataSplit != null)
                    {
                        for (int i = 0; i < numbers.Count(); i++)
                        {
                            if (i < dataSplit.Count())
                            {
                                if (!uint.TryParse(new string(dataSplit[i].SkipWhile(c => c != 'x').Skip(1).ToArray())
                                    , NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numbers[i]))
                                {
                                    errcode = ErrCode.EPARSE;
                                    LogFile.logWrite(string.Format("{0} number[{1}] {2} pattern \"{3}\"", errcode, i, numbers, pattern));
                                }
                            }
                            else
                            {
                                errcode = ErrCode.EPARSE;
                                LogFile.logWrite(string.Format("{0} dataSplit.Count() {1} pattern \"{2}\"", errcode, dataSplit.Count(), pattern));
                                break;
                            }
                        }
                    }
                    else
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} dataSplit {1} pattern \"{2}\"", errcode, dataSplit, pattern));
                    }
                }
                else
                {
                    errcode = ErrCode.EPARSE;
                    LogFile.logWrite(string.Format("{0} dataFind {1} pattern \"{2}\"", errcode, dataFind, pattern));
                }
            }
            else
            {
                errcode = ErrCode.EPARSE;
                LogFile.logWrite(string.Format("{0} dataRd {1} pattern \"{2}\"", errcode, dataRd, pattern));
            }
            return errcode;
        }

        private async Task<string> serialReadWrite(List<string> cmd)
        {
            int time = 0;
            string dataRdStr = string.Empty;
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
                if (dprogress != null)
                    dprogress(progress, false);
            }
            return dataRdStr;
        }
    }
}