using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CF2SDPCOM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InTheHand.Windows.Forms.SelectBluetoothDeviceDialog sbdd = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
            sbdd.Info = "Testing dialog";
            sbdd.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
InTheHand.Net.Sockets.BluetoothClient bc = new InTheHand.Net.Sockets.BluetoothClient();
            InTheHand.Net.Sockets.BluetoothDeviceInfo[] devices = bc.DiscoverDevices();
            if (devices.Length > 0)
            {
                /*
                object o = InTheHand.ActivatorHelper.CreateInstance(typeof(InTheHand.Net.Bluetooth.SdpStream));

                InTheHand.Net.Bluetooth.ISdpStream ss = (InTheHand.Net.Bluetooth.ISdpStream)o; // new InTheHand.Net.Bluetooth.SdpStream();

                foreach (InTheHand.Net.Sockets.BluetoothDeviceInfo d in devices)
                {
                    if (MessageBox.Show(d.DeviceName,"Device", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        byte[] record = devices[0].GetServiceRecordsUnparsed(InTheHand.Net.Bluetooth.BluetoothService.L2CapProtocol)[0];

                        //uint err;
                        //ss.Validate(pRecord, record.Length, out err);
                        int found;
                        ss.VerifySequenceOf(record, record.Length, InTheHand.Net.Bluetooth.SDP_TYPE.SEQUENCE, null, out found);

                        int numrecs = found;
                        IntPtr[] recs = new IntPtr[found];

                        ss.RetrieveRecords(record, record.Length, recs, ref numrecs);
                        InTheHand.Net.Bluetooth.ISdpRecord[] records = new InTheHand.Net.Bluetooth.ISdpRecord[numrecs];
                        for (int i = 0; i < numrecs; i++)
                        {
                            InTheHand.Net.Bluetooth.SdpWalker w = new InTheHand.Net.Bluetooth.SdpWalker();

                            records[i] = (InTheHand.Net.Bluetooth.ISdpRecord)InTheHand.Runtime.InteropServices.MarshalHelper.GetTypedObjectForIUnknown(recs[i], typeof(InTheHand.Net.Bluetooth.ISdpRecord));
                            records[i].Walk(w);
                            //InTheHand.Net.Bluetooth.NodeData n;
                            IntPtr n = System.Runtime.InteropServices.Marshal.AllocHGlobal(32);
                            Guid g;
                            records[i].GetServiceClass(out g);
                            if (g == InTheHand.Net.Bluetooth.BluetoothService.SerialPort)
                            {
                                records[i].GetAttribute((ushort)InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList, n);
                                Console.Write(System.Runtime.InteropServices.Marshal.ReadInt16(n).ToString());
                            }
                            else
                            {
                                Console.Write(g.ToString());
                            }
                            //records[i].GetAttribute((ushort)InTheHand.Net.Bluetooth.AttributeIds.ServiceDiscoveryServerAttributeId.ServiceDatabaseState, n);
                            //InTheHand.Net.Bluetooth.SDP_SPECIFICTYPE st = (InTheHand.Net.Bluetooth.SDP_SPECIFICTYPE)BitConverter.ToInt16(n, 2);

                            
                        }
                    }
                }*/
                
            }
        }
    }
}