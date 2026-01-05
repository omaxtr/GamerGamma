using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GamerGamma
{
    public class PointDef { public int X { get; set; } public int Y { get; set; } }

    public enum ChannelMode { Linked, Red, Green, Blue }
    public enum GammaApi { GDI, VESA, NvAPI }
    public enum TransferMode { PowerLaw, BT709, BT2020 }
    public enum MonoMode { None, Red, Green, Blue, Amber, Cyan, Magenta, Yellow }

    public class ChannelData
    {
        public double Gamma { get; set; } = 1.0;
        public double Brightness { get; set; } = 1.0; 
        public double Contrast { get; set; } = 1.0;   
        public double BlackLevel { get; set; } = 0.0;
        public double BlackFloor { get; set; } = 0.0;
        public double WhiteCeiling { get; set; } = 0.0; 
        public double BlackStab { get; set; } = 0.0;
        public double WhiteStab { get; set; } = 0.0;
        public double MidGamma { get; set; } = 0.0;
        public double Luminance { get; set; } = 0.0;
        public double DeHaze { get; set; } = 0.0;
        public double ToneSculpt { get; set; } = 0.0;
        public double SmartContrast { get; set; } = 0.0;
        public double Dithering { get; set; } = 0.0;
        public double WhiteLevel { get; set; } = 1.0;
        public double Solarization { get; set; } = 0.0;
        public double Inversion { get; set; } = 0.0;
        public double Clipping { get; set; } = 0.0;
        public MonoMode MonoMode { get; set; } = MonoMode.None;
        public double MonoStrength { get; set; } = 0.0;

        public ChannelData Clone()
        {
            return (ChannelData)MemberwiseClone();
        }
    }

    public class ExtendedColorSettings
    {
        public ChannelData Red { get; set; } = new ChannelData();
        public ChannelData Green { get; set; } = new ChannelData();
        public ChannelData Blue { get; set; } = new ChannelData();
        public double Saturation { get; set; } = 1.0;
        public double Hue { get; set; }
        public double Luminance { get; set; }
        public double SmartContrast { get; set; }
        public double DeHaze { get; set; }
        public double Temperature { get; set; }
        public double Tint { get; set; }
        public double ToneSculpt { get; set; }
        public double WhiteLevel { get; set; } = 1.0;
        public string SelectedLut { get; set; }
        public double LutStrength { get; set; } = 1.0;
        public MonoMode MonoMode { get; set; }
        public double MonoStrength { get; set; }
        public int ShadowTint { get; set; }
        public int HighlightTint { get; set; }
        public List<PointDef> CurvesR { get; set; }
        public List<PointDef> CurvesG { get; set; }
        public List<PointDef> CurvesB { get; set; }
        public List<PointDef> CurvesMaster { get; set; }
        
        public bool Smooth { get; set; }

        // public List<PointDef> PointCurvePoints { get; set; } // Removed

        public double Dithering { get; set; }
        public double Sharpness { get; set; }
        public TransferMode TransferMode { get; set; }
        public GammaApi Api { get; set; }

        public ExtendedColorSettings()
        {
            // Initialize with Identity Curves (0,0) -> (255,255) to match Reset() behavior logic
            var defCurve = new List<PointDef> { new PointDef { X = 0, Y = 0 }, new PointDef { X = 255, Y = 255 } };
            CurvesR = new List<PointDef>(defCurve);
            CurvesG = new List<PointDef>(defCurve);
            CurvesB = new List<PointDef>(defCurve);
            CurvesMaster = new List<PointDef>(defCurve);
            
            // Default Tints (White = Normal)
            // Color.White.ToArgb() is -1 (0xFFFFFFFF)
            HighlightTint = -1;
            // Color.Black.ToArgb() is -16777216 (0xFF000000)
            ShadowTint = -16777216;
        }
    }

    public class ColorProfile
    {
        public string Name { get; set; }
        public ExtendedColorSettings Settings { get; set; }
        public int Hotkey { get; set; } 
        public int HotkeyModifiers { get; set; }
    }

    public class AppSettings
    {
        public List<ColorProfile> Profiles { get; set; } = new List<ColorProfile>();
        public int SelectedProfileIndex { get; set; } = -1;
        
        public bool MinimizeToTray { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
        public bool ShowTooltips { get; set; } = true;
        
        public string SelectedMonitorDeviceName { get; set; }
        public ExtendedColorSettings CurrentSettings { get; set; } = new ExtendedColorSettings();
        public Dictionary<string, ExtendedColorSettings> MonitorSettings { get; set; } = new Dictionary<string, ExtendedColorSettings>();
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RAMP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Red;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Green;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Blue;
    }
}