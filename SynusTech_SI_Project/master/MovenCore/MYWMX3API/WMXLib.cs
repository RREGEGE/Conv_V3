using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;

namespace MYWMX3API
{
    partial class WMXLib
    {
        static public WMX3Api WMX3Api = new WMX3Api();
        static public Ecat WMX3Ecat = new Ecat(WMX3Api);

        public WMXLib()
        {
            WMX3Api = new WMX3Api();
            WMX3Ecat = new Ecat(WMX3Api);

            Datas.Reset();
            Controls.Engine.TryCreateDevice();
        }
        public void Dispose()
        {
            if (WMX3Ecat != null)
            {
                WMX3Ecat.Dispose();
                WMX3Ecat = null;
            }

            if (WMX3Api != null)
            {
                if (WMX3Api.IsDeviceValid())
                    WMX3Api.CloseDevice();

                WMX3Api.Dispose();
                WMX3Api = null;
            }
        }
        static public string ErrorCodeToString(int err)
        {
            if (err >= 0x100 && err < 0x1000)
                return WMX3Api.ErrorToString(err);
            else if (err >= 0x2000 && err < 0x3000)
                return Ecat.ErrorToString(err);
            else
                return string.Format("It's not defined error code(0x{0:X})", err);
        }
    }
}
