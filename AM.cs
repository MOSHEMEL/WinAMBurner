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
        private const int RD_BUFF_SIZE = 1024 * 16;
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
        private uint prm_general = ERROR;

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

        public uint PrmGeneral { get => prm_general; set => prm_general = value; }

        public delegate void dProgress(Object progress, bool reset);
        public dProgress dprogress;
        public Object progress;

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
            serialPort.ReadBufferSize = RD_BUFF_SIZE;
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

        public async Task<ErrCode> AMCmd(Cmd cmd, string blockNum = "", List<string> request = null, string reply = "")
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
                case Cmd.READ_00:
                    errcode = await amCmdBlock(Cmd.READ_00);
                    break;
                case Cmd.WRITE_00:
                        errcode = await amCmdBlock(Cmd.WRITE_00, blockNum);
                    break;
                case Cmd.READ_01:
                        errcode = await amCmdBlock(Cmd.READ_01);
                    break;
                case Cmd.WRITE_01:
                    errcode = await amCmdBlock(Cmd.WRITE_01);
                    break;
                case Cmd.READ_03_FF:
                        errcode = await amCmdBlock(Cmd.READ_03_FF, blockNum);
                    break;
                case Cmd.WRITE_03_FF:
                    errcode = await amCmdBlock(Cmd.WRITE_03_FF, blockNum, maxi);
                    break;
                case Cmd.GENERAL:
                    errcode = await amCmdBlock(Cmd.GENERAL, request: request, reply: reply);
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
        private async Task<ErrCode> amCmdBlock(Cmd cmd, string blockNum = "", uint max = 0, List<string> request = null, string reply = "")
        {
            ErrCode errcode = ErrCode.OK;
            List<string> cmds = new List<string>();
            switch (cmd)
            {
                case Cmd.ID:
                    get_id(cmds);
                    break;
                case Cmd.READ_01:
                    read_01(cmds);
                    break;
                case Cmd.WRITE_01:
                    write_01(cmds);
                    break;
                case Cmd.READ_03_FF:
                    read_03_FF(blockNum, cmds);
                    break;
                case Cmd.WRITE_03_FF:
                    write_03_FF(blockNum, cmds, max);
                    break;
                case Cmd.READ_00:
                    read_00(cmds);
                    break;
                case Cmd.WRITE_00:
                    write_00(cmds);
                    break;
                case Cmd.GENERAL:
                    general(cmds, request);
                    break;
            }
            cmds.Add("NOP#");
            cmds.Add("NOP#");
            LogFile.logWrite(cmds);

            string dataRdStr = await serialReadWrite(cmds);
            //write to log
            LogFile.logWrite(dataSplit(dataRdStr));
            switch (cmd)
            {
                case Cmd.WRITE_03_FF:
                    {
                        uint lmaxi = ERROR;
                        uint ldate = ERROR;
                        uint lsnum = ERROR;
                        uint lfactor = ERROR;
                        uint lremaining = ERROR;
                        errcode = dataLineParseCheck_03_FF(dataSplit(dataRdStr), blockNum, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining, false);
                        break;
                    }
                case Cmd.WRITE_00:
                    {
                        uint lfactor = ERROR;
                        errcode = dataLineParseCheck_General(dataSplit(dataRdStr), "address 0x0", ref lfactor);
                        break;
                    }
                case Cmd.WRITE_01:
                    uint[] laptxid = { ERROR, ERROR, ERROR };
                    errcode = dataLineParseCheck_01(dataSplit(dataRdStr), laptxid);
                    break;
                case Cmd.GENERAL:
                    errcode = dataLineParseCheck_General(dataSplit(dataRdStr), reply, ref prm_general);
                    break;
                default:
                    errcode = parseBlock(dataRdStr, cmd, blockNum);
                    break;
            }
            return errcode;
        }

        private static void get_id(List<string> cmds)
        {
            if (cmds != null)
            {
                cmds.Add("getid,3#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void read_01(List<string> cmds)
        {
            if (cmds != null)
            {
                cmds.Add("rd,3,0x0001008#");
                cmds.Add("rd,3,0x0001004#");
                cmds.Add("rd,3,0x0001000#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void write_01(List<string> cmds)
        {
            if (cmds != null)
            {
                if (!checkError(aptxId))
                {
                    cmds.Add(string.Format("wrt,3,0x0001008,00{0:x}#", aptxId[0]));
                    cmds.Add(string.Format("wrt,3,0x0001004,00{0:x}#", aptxId[1]));
                    cmds.Add(string.Format("wrt,3,0x0001000,00{0:x}#", aptxId[2]));
                }
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void read_03_FF(string blockNum, List<string> cmds)
        {
            if (cmds != null)
            {
                cmds.Add("rd,3,0x000" + blockNum + "F40#");
                cmds.Add("Read REMAINING 3#");
                cmds.Add("rd,3,0x000" + blockNum + "FF6#");
                cmds.Add("Read SNUM 3#");
                cmds.Add("rd,3,0x000" + blockNum + "FEE#");
                cmds.Add("Read DATE 3#");
                cmds.Add("rd,3,0x000" + blockNum + "FE6#");
                cmds.Add("Read MAX 3#");
                cmds.Add("status2,3#");
                cmds.Add("status1,3#");
                if (blockNum == "3")
                    cmds.Add("rd,3,0x000" + blockNum + "F50#");
                else
                    cmds.Add("find,3,1#");
                //cmd.Add("NOP#");
                //cmd.Add("NOP#");
            }
        }

        private void write_03_FF(string blockNum, List<string> cmds, uint max)
        {
            if (cmds != null)
            {
                cmds.Add("!!!WRITE COMPLETE!!!#");
                //cmd.Add(string.Format("snum,3,{0}#", snum));
                //cmd.Add(string.Format("maxi,3,{0}#", maxi + maxiSet));
                //cmd.Add(string.Format("date,3,{0}#", (int)date));
                //cmd.Add(string.Format("wrt,3,0x00FFF50,00{0:x}#", factor));
                if ((factor >= max) && (factor <= (max + TOLERANCE)))
                    cmds.Add(string.Format("wrt,3,0x00" + blockNum + "F40,00{0:x}#", MAGIC_NUM));
                cmds.Add(string.Format("wrt,3,0x00" + blockNum + "FF6,00{0:x}#", snum));
                //cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FEE,00{0:x}#", (int)date));
                cmds.Add(string.Format("wrt,3,0x00" + blockNum + "FEE,00{0:x}#", (int)DateTime.Now.Subtract(epoch).TotalSeconds));
                //cmd.Add(string.Format("wrt,3,0x00" + blokNum + "FE6,00{0:x}#", maxi + maxiSet));
                cmds.Add(string.Format("wrt,3,0x00" + blockNum + "FE6,00{0:x}#", max));
                cmds.Add(string.Format("wrt,3,0x00" + blockNum + "F50,00{0:x}#", factor));

                //erase
                cmds.Add("scan,3,0#");
                cmds.Add("scan,3,0#");
                cmds.Add("scan,3,0#");
                for (int i = 0; i < 10; i++)
                    cmds.Add("NOP#");
                //cmd.Add("nuke,3#");
                cmds.Add("erase,3,0x" + blockNum + "000#");
                //erase
                cmds.Add("set registers readonly#");
                cmds.Add("status2,3#");
                cmds.Add("status1,3#");
                cmds.Add("statusw,3,8001#");
                cmds.Add("clear registers#");
                cmds.Add("status2,3#");
                cmds.Add("status1,3#");
                cmds.Add("statusw,3,0000#");
                cmds.Add("status2,3#");
                cmds.Add("status1,3#");
            }
        }

        private void read_00(List<string> cmds)
        {
            if (cmds != null)
            {
                cmds.Add("rd,3,0x0000000#");
            }
        }

        private void write_00(List<string> cmds)
        {
            if (cmds != null)
            {
                if ((factor > 0) && (factor != 0xffffffff))
                    cmds.Add(string.Format("wrt,3,0x000000,00{0:x}#", factor));
            }
        }

        private void general(List<string> cmds, List<string> request)
        {
            if ((cmds != null) && (request != null))
            {
                cmds.AddRange(request);
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
                    switch (cmd)
                    {
                        //id
                        case Cmd.ID:
                            errcode = dataLineParse_id(dataRd);
                            break;
                        case Cmd.READ_00:
                            errcode = dataLineParse_00(dataRd);
                            break;
                        case Cmd.READ_01:
                            errcode = dataLineParse_01(dataRd);
                            break;
                        case Cmd.READ_03_FF:
                            errcode = dataLineParse_03_FF(dataRd, blokNum);
                            break;
                        default:
                            LogFile.logWrite(string.Format("{0} cmd {1}", ErrCode.EUNKNOWN, cmd));
                            errcode = ErrCode.EUNKNOWN;
                            break;
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
                if (dataLineParseCheck_General(dataRd, "address 0x0", ref lfactor) == ErrCode.OK)
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

        //private ErrCode dataLineParseCheck_00(List<string> dataRd, ref uint lfactor)
        //{
        //    ErrCode errcode = ErrCode.OK;
        //    if (dataRd != null)
        //        //factor
        //        errcode = dataLineParse(dataRd, "address 0x0", ref lfactor);
        //    else
        //        errcode = ErrCode.EPARSE;
        //    return errcode;
        //}

        private ErrCode dataLineParseCheck_General(List<string> dataRd, string reply, ref uint lprm_general)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
                //prm_general
                errcode = dataLineParse(dataRd, reply, ref lprm_general);
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

        private bool checkError(uint[] prms)
        {
            bool check = true;
            foreach (uint prm in prms)
            {
                if (prm == ERROR)
                    return true;
                if (prm != 0)
                    check = false;
            }
            return check;
        }

        private List<string> dataSplit(string dataRdStr)
        {
            string[] dataSplit = null;
            if (dataRdStr != null)
                dataSplit = dataRdStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return dataSplit.ToList();
        }

        private ErrCode dataFindPattern(List<string> dataRd, string pattern, ref string dataFind)
        {
            ErrCode errcode = ErrCode.OK;
            if ((dataRd != null) && (pattern != null))
            {
                dataFind = dataRd.Find(data => data.Contains(pattern));
                if (dataFind == null)
                    errcode = ErrCode.EPARSE;
            }
            return errcode;
        }

        private ErrCode dataLineParse(List<string> dataRd, string pattern, ref uint number)
        {
            ErrCode errcode = ErrCode.OK;
            if (dataRd != null)
            {
                //string? dataFind = dataRd.Find data => data.Contains(pattern));
                //if (dataFind != null)
                string dataFind = null;
                if (dataFindPattern(dataRd, pattern, ref dataFind) == ErrCode.OK)
                {
                    string snumber = new string(dataFind.SkipWhile(c => c != 'x').Skip(1).TakeWhile(c => c != ':').ToArray());
                    if (snumber != null)
                    {
                        if (!uint.TryParse(snumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
                        {

                            string logsnum = snumber;
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
                    errcode = ErrCode.EFIND;
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
                //string? dataFind = dataRd.Find(data => data.Contains(pattern));
                //if (dataFind != null)
                string dataFind = null;
                if (dataFindPattern(dataRd, pattern, ref dataFind) == ErrCode.OK)
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
            if (cmd.Count > 0)
            {
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
            }
            return dataRdStr;
        }
    }
}