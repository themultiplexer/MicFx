using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MicFx
{

    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEINCAPS

        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string szPname;
            public uint dwFormats;
            public ushort wChannels;
            public ushort wReserved1;

        }

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveInGetDevCaps(uint uDeviceID, ref WAVEINCAPS pwic, uint cbwic);

        [DllImport("winmm.dll", EntryPoint = "waveInGetNumDevs", SetLastError = true)]
        public static extern uint waveInGetNumDevs();

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEOUTCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwFormats;
            public short wChannels;
            public short wReserved;
            public uint dwSupport;
        }

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetDevCaps(uint hwo, ref WAVEOUTCAPS pwoc, uint cbwoc);

        [DllImport("winmm.dll", SetLastError = true)]
        static extern uint waveOutGetNumDevs();

        public Form1()
        {
            InitializeComponent();

            foreach (string s in GetRecordingDeviceList()) { 
                richTextBox1.Text += s + "\n";
            }

            foreach (string s in GetPlaybackDeviceList())
            {
                richTextBox2.Text += s + "\n";
            }
        }

        public string[] GetRecordingDeviceList()
        {
            uint total = waveInGetNumDevs();
            string[] list = new string[total];
            WAVEINCAPS pwoc = new WAVEINCAPS();
            for (uint i = 0; i < total; i++)
            {
                waveInGetDevCaps(i, ref pwoc, (uint)Marshal.SizeOf(typeof(WAVEINCAPS)));
                list[i] = pwoc.szPname;
            }
            return list;
        }

        public string[] GetPlaybackDeviceList()
        {
            uint total = waveOutGetNumDevs();
            string[] list = new string[total];
            WAVEOUTCAPS pwoc = new WAVEOUTCAPS();
            for (uint i = 0; i < total; i++)
            {
                waveOutGetDevCaps(i, ref pwoc, (uint)Marshal.SizeOf(typeof(WAVEOUTCAPS)));
                list[i] = pwoc.szPname;
            }
            return list;
        }
    }
}
