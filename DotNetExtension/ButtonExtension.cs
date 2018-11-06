/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
#if !DISABLE_WINFORMS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDToolbox
{
    public static class ButtonExtension
    {
        public static void Check(this Button btn)
        {
            Check(btn, true);
        }

        public static void UnCheck(this Button btn)
        {
            Check(btn, false);
        }

        public static void Check(this Button btn, bool check)
        {
            btn.Tag = check.ToString();
            //btn.FlatAppearance.CheckedBackColor;
            btn.FlatStyle = check ? FlatStyle.System : FlatStyle.Flat;
        }

        public static bool IsChecked(this Button btn)
        {
            return bool.Parse((btn.Tag ?? (false.ToString())).ToString());
        }

        public static void Toggle(this Button btn)
        {
            Check(btn, !IsChecked(btn));
        }

    }
}
#endif