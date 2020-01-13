using System;
using System.Collections.Generic;
using System.Text;

namespace IrDAServiceClient
{

    enum IrProtocol
    {
        None,
        TinyTP,
        IrCOMM,
        IrLMP,
    }

    class WellKnownIrdaSvc
    {
        public static readonly WellKnownIrdaSvc[] s_wellknownServices = { 
            new WellKnownIrdaSvc("OBEX, general", "OBEX", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("OBEX, file transfer only", "OBEX:IrXfer", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("IrLPT", "IrLPT", IrProtocol.IrLMP),
            new WellKnownIrdaSvc("IrCOMM Cooked/9-Wire", "IrDA:IrCOMM", IrProtocol.IrCOMM),
            new WellKnownIrdaSvc("IrCOMM Raw", "IrDA:IrCOMM", IrProtocol.IrLMP),
            new WellKnownIrdaSvc("IrMC (Mobile Communications)", "IrDA:TELECOM", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("IrTran-P (Image Transfer) over IrCOMM", "IrDA:IrCOMM", IrProtocol.IrCOMM),
            new WellKnownIrdaSvc("IrTran-P (Image Transfer) over TinyTP", "IrTranPv1", IrProtocol.IrCOMM),
            new WellKnownIrdaSvc("IrLAN", "IrLAN", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("JetSend", "JetSend", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("----", "", IrProtocol.None),
            new WellKnownIrdaSvc("IrNET", "IrNetv1", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("IrModem; rare, usually IrCOMM 9-Wire", "IrModem", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("----", "", IrProtocol.None),
            new WellKnownIrdaSvc("Polar HRM", "HRM", IrProtocol.TinyTP),
            //
            new WellKnownIrdaSvc("Nokia PhoNet", "Nokia:PhoNet", IrProtocol.TinyTP),
            new WellKnownIrdaSvc("Nokia PhoNet over IrLMP", "Nokia:PhoNet", IrProtocol.IrLMP),
            new WellKnownIrdaSvc("Nokia PhoNet Games", "Nokia:PhoNet_Games", IrProtocol.TinyTP),
            //new WellKnownIrdaSvc("Nokia PhoNet Games over IrLMP", "Nokia:PhoNet_Games", IrProtocol.IrLMP),
            //new WellKnownIrdaSvc("Nokia NBSrouter", "Nokia:NBSrouter", IrProtocol.TinyTP),
            //new WellKnownIrdaSvc("Nokia NBSrouter over IrLMP", "Nokia:NBSrouter", IrProtocol.IrLMP),
            //
            //new WellKnownIrdaSvc("----", "", IrProtocol.None),
            //new WellKnownIrdaSvc("dummy/fake", "fake", IrProtocol.None),
            };//


        //--------------------------------------------------------------
        readonly public String name;
        readonly public String serviceName;
        readonly public IrProtocol protocolType;

        public WellKnownIrdaSvc (String name, String serviceName, IrProtocol protocolType)
        {
            this.name = name;
            this.serviceName = serviceName;
            this.protocolType = protocolType;
        }//

        public override string ToString ()
        {
            return this.name;
        }//fn

    }//class

}
