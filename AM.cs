using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Ports;

namespace WinAMBurner
{
    class Am
    {
        private SerialPort serialPort;
        private const int serialPortBaudRate = 115200;

        private const int RD_TIMEOUT = 1000;
        private const int WR_TIMEOUT = 1000;
        private const int MAX_TIMEOUT = 3000;
        private const uint ERROR = 0xffffffff;
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

        public uint SNum { get => get(snum); set => snum = set(value); }
        public uint Maxi { get => get(maxi); set => maxi = set(value); }
        public uint MaxiPrev { get => get(maxiprev); set => maxiprev = set(value); }
        public uint MaxiSet { get => get(maxiSet); set => maxiSet = set(value); }
        public uint Factor { get => get(factor); set => factor = set(value); }
        public uint Current { get => get(maxi - factor); }
        public uint CurrentPrev { get => get(maxiprev - factor); }
        public uint[] AptxId { get => aptxId.Reverse().Select(a => { return get(a); }).ToArray(); set => value.Reverse().Select(a => { return set(a); }); }
        //{
        //    get
        //    {
        //        uint[] value = new uint[ID_LENGTH];
        //        for (int i = 0; i < aptxId.Length; i++)
        //        {
        //            byte[] bytes = BitConverter.GetBytes(get(aptxId[i]));
        //            Array.Reverse(bytes, 0, bytes.Length);
        //            value[i] = BitConverter.ToUInt32(bytes, 0);
        //        }
        //        return value;
        //        
        //    }
        //    set
        //    {
        //        for (int i = 0; i < aptxId.Length; i++)
        //        {
        //            byte[] bytes = BitConverter.GetBytes(set(value[i]));
        //            Array.Reverse(bytes, 0, bytes.Length);
        //            aptxId[i] = BitConverter.ToUInt32(bytes, 0);
        //        }
        //        
        //    }
        //}

        public DateTime Date
        {
            get => epoch.AddSeconds(get(date));
            set => date = set(value.Subtract(epoch).TotalSeconds);
        }

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
            serialPort.ReadBufferSize = 8192;
            this.dprogress = dprogress;
        }

        public async Task<ErrCode> AMCheckConnect()
        {
            ErrCode errcode = ErrCode.ERROR;
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
            switch (cmd)
            {
                case Cmd.ID:
                    if ((errcode = await amReadBlock("ID")) == ErrCode.OK)
                        errcode = ErrCode.OK;
                    break;
                case Cmd.READ:
                    if ((errcode = await amReadBlock("ID")) == ErrCode.OK)
                        if ((errcode = await amReadBlock("1")) == ErrCode.OK)
                            if ((errcode = await amReadBlock("FF")) == ErrCode.OK)
                                errcode = ErrCode.OK;
                    break;
                case Cmd.WRITE:
                    if (maxi == ERROR)
                        maxi = 0;
                    if (maxiprev == ERROR)
                        maxiprev = 0;
                    if ((errcode = await amReadBlock("ID")) == ErrCode.OK)
                        if ((errcode = await amWriteBlock("3", maxi + maxiSet)) == ErrCode.OK)
                            if ((errcode = await amWriteBlock("FF", maxi + maxiSet)) == ErrCode.OK)
                                errcode = ErrCode.OK;
                    break;
                case Cmd.RESTORE:
                    if ((errcode = await amReadBlock("ID")) == ErrCode.OK)
                        if ((errcode = await amReadBlock("3")) == ErrCode.OK)
                        {
                            snum = bckup_snum;
                            maxi = bckup_maxi;
                            date = bckup_date;
                            factor = bckup_factor;

                            if ((errcode = await amWriteBlock("FF", maxi)) == ErrCode.OK)
                                if ((errcode = await amReadBlock("FF")) == ErrCode.OK)
                                    errcode = ErrCode.OK;
                        }
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

        private async Task<ErrCode> amReadBlock(string blockNum)//2 4 11
        {
            ErrCode errcode = ErrCode.ERROR;
            List<string> cmd = new List<string>();
            if (blockNum == "ID")
            {
                cmd.Add("getid,3#");
                cmd.Add("NOP#");
                cmd.Add("NOP#");
            }
            else if (blockNum == "1")
            {
                cmd.Add("rd,3,0x0001008#");
                cmd.Add("rd,3,0x0001004#");
                cmd.Add("rd,3,0x0001000#");
                cmd.Add("NOP#");
                cmd.Add("NOP#");
            }
            else if ((blockNum == "3") || (blockNum == "FF"))
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
                cmd.Add("NOP#");
                cmd.Add("NOP#");
            }
            LogFile.logWrite(cmd);

            string dataRdStr = await serialReadWrite(cmd);
            //write to log
            LogFile.logWrite(dataSplit(dataRdStr));

            errcode = parseBlock(dataRdStr, blockNum);
            return errcode;
        }

        private async Task<ErrCode> amWriteBlock(string blockNum, uint max)//31
        {
            ErrCode errcode = ErrCode.ERROR;
            Date = DateTime.Now;
            List<string> cmd = new List<string>();
            cmd.Add("!!!WRITE COMPLETE!!!#");
            //cmd.Add(string.Format("snum,3,{0}#", snum));
            //cmd.Add(string.Format("maxi,3,{0}#", maxi + maxiSet));
            //cmd.Add(string.Format("date,3,{0}#", (int)date));
            //cmd.Add(string.Format("wrt,3,0x00FFF50,00{0:x}#", factor));
            cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FF6,00{0:x}#", snum));
            cmd.Add(string.Format("wrt,3,0x00" + blockNum + "FEE,00{0:x}#", (int)date));
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
            cmd.Add("NOP#");
            cmd.Add("NOP#");
            LogFile.logWrite(cmd);

            string dataRdStr = await serialReadWrite(cmd);
            //write to log
            LogFile.logWrite(dataSplit(dataRdStr));
            uint lmaxi = ERROR;
            uint ldate = ERROR;
            uint lsnum = ERROR;
            uint lfactor = ERROR;
            uint lremaining = ERROR;
            errcode = dataLineParseCheck_03_FF(dataSplit(dataRdStr), blockNum, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining, false);

            return errcode;
        }

        private ErrCode parseBlock(string dataRdStr, string blokNum)
        {
            ErrCode errcode = ErrCode.ERROR;
            if (dataRdStr != null)
            {
                //parse to lines
                List<string> dataRd = dataSplit(dataRdStr);

                if (dataRd != null)
                {
                    //maxi
                    if (blokNum == "ID")
                    {
                        uint[] lid = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
                        if (dataLineParseCheck_id(dataRd, lid) == ErrCode.OK)
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
                                        errcode = ErrCode.EPARSE;
                                        LogFile.logWrite(string.Format("{0} lid[{1}] {2} id[{3}] {4}", errcode, i, lid[i], i, id[i]));
                                        return errcode;
                                    }
                                }
                            }
                            LogFile.logWrite(string.Format("set id: {0}", id.Aggregate("", (r, m) => r += "0x" + m.ToString("x") + " ")));
                            errcode = ErrCode.OK;
                        }
                    }
                    else if ((blokNum == "FF") || (blokNum == "3"))
                    {
                        uint lmaxi = ERROR;
                        uint ldate = ERROR;
                        uint lsnum = ERROR;
                        uint lfactor = ERROR;
                        uint lremaining = ERROR;
                        //maxi
                        if (dataLineParseCheck_03_FF(dataRd, blokNum, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining, true) == ErrCode.OK)
                        {
                            //maxiprev = maxi;
                            //maxi = lmaxi;
                            //date = ldate;
                            //if (snum == ERROR)
                            //    snum = lsnum;
                            //else
                            //{
                            //    if (lsnum != snum)
                            //    {
                            //        errcode = ErrCode.EPARSE;
                            //        LogFile.logWrite(string.Format("{0} lsnum {1} snum {2}", errcode, lsnum, snum));
                            //        return errcode;
                            //    }
                            //}
                            //
                            //if (factor == ERROR)
                            //    factor = lfactor;
                            //else
                            //{
                            //    if (lfactor != factor)
                            //    {
                            //        errcode = ErrCode.EPARSE;
                            //        LogFile.logWrite(string.Format("{0} lfactor {1} factor {2}", errcode, lfactor, factor));
                            //        return errcode;
                            //    }
                            //}
                            //errcode = ErrCode.OK;
                            if (blokNum == "3")
                                errcode = dataSet(ref bckup_maxiprev, ref bckup_maxi, ref bckup_date, ref bckup_snum, ref bckup_factor, ref backup_remaining, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining);
                            else
                                errcode = dataSet(ref maxiprev, ref maxi, ref date, ref snum, ref factor, ref remaining, ref lmaxi, ref ldate, ref lsnum, ref lfactor, ref lremaining);
                        }
                        else
                        {
                            errcode = ErrCode.EPARSE;
                            return errcode;
                        }
                    }
                    else if (blokNum == "1")
                    {
                        uint[] laptxId = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
                        if (dataLineParseCheck_01(dataRd, laptxId) == ErrCode.OK)
                        {
                            if (checkError(aptxId))
                            {
                                for (int i = 0; i < laptxId.Length; i++)
                                {
                                 if(laptxId[i] == ERROR)
                                    aptxId[i] = laptxId[i];
                                }
                            }
                            else
                            {
                                for (int i = 0; i < laptxId.Length; i++)
                                {
                                    if (laptxId[i] != aptxId[i])
                                    {
                                        errcode = ErrCode.EPARSE;
                                        LogFile.logWrite(string.Format("{0} laptxId[{1}] {2} aptxId[{3}] {4}", errcode, i, laptxId[i], i, aptxId[i]));
                                        return errcode;
                                    }
                                }
                            }
                            LogFile.logWrite(string.Format("set aptxId: {0}", aptxId.Aggregate("", (r, m) => r += "0x" + m.ToString("x") + " ")));
                            errcode = ErrCode.OK;
                        }
                        else
                        {
                            errcode = ErrCode.EPARSE;
                            return errcode;
                        }
                    }
                }
                else
                {
                    errcode = ErrCode.EPARSE;
                    LogFile.logWrite(string.Format("{0} dataRd {1}", errcode, dataRd));
                    return errcode;
                }
            }
            else
            {
                errcode = ErrCode.EPARSE;
                LogFile.logWrite(string.Format("{0} dataRdStr {1}", errcode, dataRdStr));
                return errcode;
            }
            return errcode;
        }

        private ErrCode dataSet(ref uint maxiprev, ref uint maxi, ref double date, ref uint snum, ref uint factor, ref uint remaining, ref uint lmaxi, ref uint ldate, ref uint lsnum, ref uint lfactor, ref uint lremaining)
        {
            ErrCode errcode = ErrCode.ERROR;
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
                        return errcode;
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
                        return errcode;
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
                errcode = ErrCode.OK;
            }
            else
            {
                errcode = ErrCode.EEMPTY;
                LogFile.logWrite(string.Format("{0} lsnum {1} lmaxi {2} lfactor {3}", errcode, lsnum, lmaxi, lfactor));
                return errcode;
            }
            return errcode;
        }

        private ErrCode dataLineParseCheck_01(List<string> dataRd, uint[] laptxId)
        {
            ErrCode errcode = ErrCode.ERROR;
            if (dataRd != null)
            {
                if ((dataLineParse(dataRd, "0x1000", ref laptxId[0]) == ErrCode.OK) &&
                    (dataLineParse(dataRd, "0x1004", ref laptxId[1]) == ErrCode.OK) &&
                    (dataLineParse(dataRd, "0x1008", ref laptxId[2]) == ErrCode.OK))
                    errcode = ErrCode.OK;
            }
            return errcode;
        }

        private ErrCode dataLineParseCheck_03_FF(List<string> dataRd, string blokNum, ref uint lmaxi, ref uint ldate, ref uint lsnum, ref uint lfactor, ref uint lremaining, bool read)
        {
            ErrCode errcode = ErrCode.ERROR;
            if (dataRd != null)
            {
                //maxi
                if (dataLineParse(dataRd, "0x" + blokNum + "FE6", ref lmaxi) == ErrCode.OK)
                    //date
                    if (dataLineParse(dataRd, "0x" + blokNum + "FEE", ref ldate) == ErrCode.OK)
                        //snum
                        if (dataLineParse(dataRd, "0x" + blokNum + "FF6", ref lsnum) == ErrCode.OK)
                            //factor
                            if (((read) && (dataLineParse(dataRd, "pulses written", ref lfactor) == ErrCode.OK)) ||
                                ((!read) && (dataLineParse(dataRd, "0x" + blokNum + "F50", ref lfactor) == ErrCode.OK)))
                                //remaining
                                if (((read) && (dataLineParse(dataRd, "0x" + blokNum + "F40", ref lremaining) == ErrCode.OK)) ||
                                    (!read))
                                    errcode = ErrCode.OK;
            }
            return errcode;
        }

        //private ErrCode dataParseId(string dataRdStr)
        //{
        //    ErrCode errcode = ErrCode.ERROR;
        //    if (dataRdStr != null)
        //    {
        //        uint[] lid = new uint[ID_LENGTH] { ERROR, ERROR, ERROR };
        //        //parse to lines
        //        string[] dataRd = dataSplit(dataRdStr);
        //        if (dataRd != null)
        //        {
        //            //maxi
        //            if (dataLineParseCheck_id(dataRd, lid) == ErrCode.OK)
        //            {
        //                if (checkError(id))
        //                {
        //                    for (int i = 0; i < lid.Length; i++)
        //                        id[i] = lid[i];
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < lid.Length; i++)
        //                    {
        //                        if (lid[i] != id[i])
        //                        {
        //                            errcode = ErrCode.EPARSE;
        //                            LogFile.logWrite(string.Format("{0} lid[{1}] {2} id[{3}] {4}", errcode, i, lid[i], i, id[i]));
        //                            return errcode;
        //                        }
        //                    }
        //                }
        //                errcode = ErrCode.OK;
        //            }
        //        }
        //        else
        //        {
        //            errcode = ErrCode.EPARSE;
        //            LogFile.logWrite(string.Format("{0} dataRd {1}", errcode, dataRd));
        //            return errcode;
        //        }
        //    }
        //    else
        //    {
        //        errcode = ErrCode.EPARSE;
        //        LogFile.logWrite(string.Format("{0} dataRdStr {1}", errcode, dataRdStr));
        //        return errcode;
        //    }
        //    return errcode;
        //}

        private ErrCode dataLineParseCheck_id(List<string> dataRd, uint[] lid)
        {
            return dataLineParse(dataRd, "0x1f-0x85-0x01", ref lid);
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
            ErrCode errcode = ErrCode.ERROR;
            if (dataRd != null)
            {
                string? dataFind = dataRd.Find(data => data.Contains(pattern));
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
                            string? logsnum = snumber;
                            snumber = new string(dataFind.SkipWhile(c => (c < '0') || (c > '9')).ToArray());
                            if (snumber != null)
                            {
                                if (uint.TryParse(snumber, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                                {
                                    errcode = ErrCode.OK;
                                }
                                else
                                {
                                    errcode = ErrCode.EPARSE;
                                    LogFile.logWrite(string.Format("{0} pattern \"{1}\" logsnum {2} snumber {3} number {4}", errcode, pattern, logsnum, snumber, number));
                                    return errcode;
                                }
                            }
                            else
                            {
                                errcode = ErrCode.EPARSE;
                                LogFile.logWrite(string.Format("{0} pattern \"{1}\" logsnum {2} snumber {3}", errcode, pattern, logsnum, snumber));
                                return errcode;
                            }
                        }
                    }
                    else
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} pattern \"{1}\" snumber {2}", errcode, pattern, snumber));
                        return errcode;
                    }
                }
                else
                {
                    errcode = ErrCode.EPARSE;
                    LogFile.logWrite(string.Format("{0} dataFind {1} pattern \"{2}\"", errcode, dataFind, pattern));
                    return errcode;
                }
            }
            else
            {
                errcode = ErrCode.EPARSE;
                LogFile.logWrite(string.Format("{0} dataRd {1} pattern \"{2}\"", errcode, dataRd, pattern));
                return errcode;
            }
            return errcode;
        }

        private ErrCode dataLineParse(List<string> dataRd, string pattern, ref uint[] number)
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
                        for (int i = 0; i < number.Count(); i++)
                        {
                            if (i < dataSplit.Count())
                            {
                                if (!uint.TryParse(new string(dataSplit[i].SkipWhile(c => c != 'x').Skip(1).ToArray())
                                    , NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number[i]))
                                {
                                    errcode = ErrCode.EPARSE;
                                    LogFile.logWrite(string.Format("{0} number[{1}] {2} pattern \"{3}\"", errcode, i, number, pattern));
                                    return errcode;
                                }
                            }
                            else
                            {
                                errcode = ErrCode.EPARSE;
                                LogFile.logWrite(string.Format("{0} dataSplit.Count() {1} pattern \"{2}\"", errcode, dataSplit.Count(), pattern));
                                return errcode;
                            }
                        }
                    }
                    else
                    {
                        errcode = ErrCode.EPARSE;
                        LogFile.logWrite(string.Format("{0} dataSplit {1} pattern \"{2}\"", errcode, dataSplit, pattern));
                        return errcode;
                    }
                }
                else
                {
                    errcode = ErrCode.EPARSE;
                    LogFile.logWrite(string.Format("{0} dataFind {1} pattern \"{2}\"", errcode, dataFind, pattern));
                    return errcode;
                }
            }
            else
            {
                errcode = ErrCode.EPARSE;
                LogFile.logWrite(string.Format("{0} dataRd {1} pattern \"{2}\"", errcode, dataRd, pattern));
                return errcode;
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
                dprogress(progress, false);
            }
            return dataRdStr;
        }
    }
}