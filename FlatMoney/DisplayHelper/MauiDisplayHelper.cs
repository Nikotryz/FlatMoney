using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.DisplayHelper
{
    public class MauiDisplayHelper
    {
        public const double UnitPixelRatio = 2.63;

        public static readonly double DisplayHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
        public static readonly double DisplayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

        public static readonly double UnitHeight = DisplayHeight / UnitPixelRatio;
        public static readonly double UnitWidth = DisplayWidth / UnitPixelRatio;
    }
}
