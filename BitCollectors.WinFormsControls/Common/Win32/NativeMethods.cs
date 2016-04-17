using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

// ReSharper disable InconsistentNaming

namespace BitCollectors.WinFormsControls.Common.Win32
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public  static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref Rectangle lParam);

        #region Constants (non-public)
        public const int
            EM_GETRECT = 0xB2,
            EM_SETRECT = 0xB3,
            WM_NCACTIVATE = 0x0086,
            EC_LEFTMARGIN = 0x1,
            EM_SETMARGINS = 0xD3,
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_GETTEXTLENGTH = 0xe,
            CBN_CLOSEUP = 8,
            CBN_DROPDOWN = 7,
            WM_COMMAND = 0x0111,
            WM_MOUSEMOVE = 0x200,
            WM_LBUTTONDOWN = 0x0201,
            WM_RBUTTONDOWN = 0x204,
            WM_MBUTTONDOWN = 0x207,
            WM_LBUTTONUP = 0x202,
            WM_RBUTTONUP = 0x205,
            WM_MBUTTONUP = 0x208,
            WM_LBUTTONDBLCLK = 0x203,
            WM_RBUTTONDBLCLK = 0x206,
            WM_MBUTTONDBLCLK = 0x209,
            WM_REFLECT = WM_USER + 0x1C00,
            ACM_OPENA = (0x0400 + 100),
            ACM_OPENW = (0x0400 + 103),
            BFFM_SETSELECTIONA = 0x400 + 102,
            BFFM_SETSELECTIONW = 0x400 + 103,
            CBEM_GETITEMA = (0x0400 + 4),
            CBEM_GETITEMW = (0x0400 + 13),
            CBEM_INSERTITEMA = (0x0400 + 1),
            CBEM_INSERTITEMW = (0x0400 + 11),
            CBEM_SETITEMA = (0x0400 + 5),
            CBEM_SETITEMW = (0x0400 + 12),
            CBEN_ENDEDITA = ((0 - 800) - 5),
            CBEN_ENDEDITW = ((0 - 800) - 6),
            DTM_SETFORMATA = (0x1000 + 5),
            DTM_SETFORMATW = (0x1000 + 50),
            DTN_FORMATA = ((0 - 760) + 4),
            DTN_FORMATQUERYA = ((0 - 760) + 5),
            DTN_FORMATQUERYW = ((0 - 760) + 18),
            DTN_FORMATW = ((0 - 760) + 17),
            DTN_USERSTRINGA = ((0 - 760) + 2),
            DTN_USERSTRINGW = ((0 - 760) + 15),
            DTN_WMKEYDOWNA = ((0 - 760) + 3),
            DTN_WMKEYDOWNW = ((0 - 760) + 16),
            EM_CANUNDO = 0x00C6,
            EM_LIMITTEXT = 0x00C5,
            EM_SCROLLCARET = 0x00B7,
            EM_UNDO = 0x00C7,
            EMR_POLYTEXTOUTA = 96,
            EMR_POLYTEXTOUTW = 97,
            ES_CENTER = 0x0001,
            ES_LEFT = 0x0000,
            ES_RIGHT = 0x0002,
            HDM_GETITEMA = (0x1200 + 3),
            HDM_GETITEMW = (0x1200 + 11),
            HDM_INSERTITEMA = (0x1200 + 1),
            HDM_INSERTITEMW = (0x1200 + 10),
            HDM_SETITEMA = (0x1200 + 4),
            HDM_SETITEMW = (0x1200 + 12),
            HDN_BEGINTRACKA = ((0 - 300) - 6),
            HDN_BEGINTRACKW = ((0 - 300) - 26),
            HDN_DIVIDERDBLCLICKA = ((0 - 300) - 5),
            HDN_DIVIDERDBLCLICKW = ((0 - 300) - 25),
            HDN_ENDTRACKA = ((0 - 300) - 7),
            HDN_ENDTRACKW = ((0 - 300) - 27),
            HDN_GETDISPINFOA = ((0 - 300) - 9),
            HDN_GETDISPINFOW = ((0 - 300) - 29),
            HDN_ITEMCHANGEDA = ((0 - 300) - 1),
            HDN_ITEMCHANGEDW = ((0 - 300) - 21),
            HDN_ITEMCHANGINGA = ((0 - 300) - 0),
            HDN_ITEMCHANGINGW = ((0 - 300) - 20),
            HDN_ITEMCLICKA = ((0 - 300) - 2),
            HDN_ITEMCLICKW = ((0 - 300) - 22),
            HDN_ITEMDBLCLICKA = ((0 - 300) - 3),
            HDN_ITEMDBLCLICKW = ((0 - 300) - 23),
            HDN_TRACKA = ((0 - 300) - 8),
            HDN_TRACKW = ((0 - 300) - 28),
            LANG_NEUTRAL = 0x00,
            LVM_EDITLABELA = (0x1000 + 23),
            LVM_EDITLABELW = (0x1000 + 118),
            LVM_FINDITEMA = (0x1000 + 13),
            LVM_FINDITEMW = (0x1000 + 83),
            LVM_GETCOLUMNA = (0x1000 + 25),
            LVM_GETCOLUMNW = (0x1000 + 95),
            LVM_GETISEARCHSTRINGA = (0x1000 + 52),
            LVM_GETISEARCHSTRINGW = (0x1000 + 117),
            LVM_GETITEMA = (0x1000 + 5),
            LVM_GETITEMTEXTA = (0x1000 + 45),
            LVM_GETITEMTEXTW = (0x1000 + 115),
            LVM_GETITEMW = (0x1000 + 75),
            LVM_GETSTRINGWIDTHA = (0x1000 + 17),
            LVM_GETSTRINGWIDTHW = (0x1000 + 87),
            LVM_INSERTCOLUMNA = (0x1000 + 27),
            LVM_INSERTCOLUMNW = (0x1000 + 97),
            LVM_INSERTITEMA = (0x1000 + 7),
            LVM_INSERTITEMW = (0x1000 + 77),
            LVM_SETBKIMAGEA = (0x1000 + 68),
            LVM_SETBKIMAGEW = (0x1000 + 138),
            LVM_SETCOLUMNA = (0x1000 + 26),
            LVM_SETCOLUMNW = (0x1000 + 96),
            LVM_SETITEMA = (0x1000 + 6),
            LVM_SETITEMTEXTA = (0x1000 + 46),
            LVM_SETITEMTEXTW = (0x1000 + 116),
            LVM_SETITEMW = (0x1000 + 76),
            LVN_BEGINLABELEDITA = ((0 - 100) - 5),
            LVN_BEGINLABELEDITW = ((0 - 100) - 75),
            LVN_ENDLABELEDITA = ((0 - 100) - 6),
            LVN_ENDLABELEDITW = ((0 - 100) - 76),
            LVN_GETDISPINFOA = ((0 - 100) - 50),
            LVN_GETDISPINFOW = ((0 - 100) - 77),
            LVN_GETINFOTIPA = ((0 - 100) - 57),
            LVN_GETINFOTIPW = ((0 - 100) - 58),
            LVN_ODFINDITEMA = ((0 - 100) - 52),
            LVN_ODFINDITEMW = ((0 - 100) - 79),
            LVN_SETDISPINFOA = ((0 - 100) - 51),
            LVN_SETDISPINFOW = ((0 - 100) - 78),
            PSM_SETFINISHTEXTA = (0x0400 + 115),
            PSM_SETFINISHTEXTW = (0x0400 + 121),
            PSM_SETTITLEA = (0x0400 + 111),
            PSM_SETTITLEW = (0x0400 + 120),
            RB_INSERTBANDA = (0x0400 + 1),
            RB_INSERTBANDW = (0x0400 + 10),
            RDW_ERASE = 0x0004,
            RDW_ERASENOW = 0x0200,
            RDW_FRAME = 0x0400,
            RDW_INVALIDATE = 0x0001,
            RDW_UPDATENOW = 0x0100,
            RGN_XOR = 3,
            SB_ENDSCROLL = 8,
            SB_GETTEXTA = (0x0400 + 2),
            SB_GETTEXTLENGTHA = (0x0400 + 3),
            SB_GETTEXTLENGTHW = (0x0400 + 12),
            SB_GETTEXTW = (0x0400 + 13),
            SB_GETTIPTEXTA = (0x0400 + 18),
            SB_GETTIPTEXTW = (0x0400 + 19),
            SB_SETTEXTA = (0x0400 + 1),
            SB_SETTEXTW = (0x0400 + 11),
            SB_SETTIPTEXTA = (0x0400 + 16),
            SB_SETTIPTEXTW = (0x0400 + 17),
            SB_THUMBTRACK = 5,
            SUBLANG_DEFAULT = 0x01,
            TB_ADDBUTTONSA = (0x0400 + 20),
            TB_ADDBUTTONSW = (0x0400 + 68),
            TB_ADDSTRINGA = (0x0400 + 28),
            TB_ADDSTRINGW = (0x0400 + 77),
            TB_GETBUTTONINFOA = (0x0400 + 65),
            TB_GETBUTTONINFOW = (0x0400 + 63),
            TB_GETBUTTONTEXTA = (0x0400 + 45),
            TB_GETBUTTONTEXTW = (0x0400 + 75),
            TB_INSERTBUTTONA = (0x0400 + 21),
            TB_INSERTBUTTONW = (0x0400 + 67),
            TB_MAPACCELERATORA = (0x0400 + 78),
            TB_MAPACCELERATORW = (0x0400 + 90),
            TB_SAVERESTOREA = (0x0400 + 26),
            TB_SAVERESTOREW = (0x0400 + 76),
            TB_SETBUTTONINFOA = (0x0400 + 66),
            TB_SETBUTTONINFOW = (0x0400 + 64),
            TBN_GETBUTTONINFOA = ((0 - 700) - 0),
            TBN_GETBUTTONINFOW = ((0 - 700) - 20),
            TBN_GETDISPINFOA = ((0 - 700) - 16),
            TBN_GETDISPINFOW = ((0 - 700) - 17),
            TBN_GETINFOTIPA = ((0 - 700) - 18),
            TBN_GETINFOTIPW = ((0 - 700) - 19),
            TCM_GETITEMA = (0x1300 + 5),
            TCM_GETITEMW = (0x1300 + 60),
            TCM_INSERTITEMA = (0x1300 + 7),
            TCM_INSERTITEMW = (0x1300 + 62),
            TCM_SETITEMA = (0x1300 + 6),
            TCM_SETITEMW = (0x1300 + 61),
            TCS_RIGHT = 0x0002,
            TRANSPARENT = 1,
            TTM_ADDTOOLA = (0x0400 + 4),
            TTM_ADDTOOLW = (0x0400 + 50),
            TTM_DELTOOLA = (0x0400 + 5),
            TTM_DELTOOLW = (0x0400 + 51),
            TTM_ENUMTOOLSA = (0x0400 + 14),
            TTM_ENUMTOOLSW = (0x0400 + 58),
            TTM_GETCURRENTTOOLA = (0x0400 + 15),
            TTM_GETCURRENTTOOLW = (0x0400 + 59),
            TTM_GETTEXTA = (0x0400 + 11),
            TTM_GETTEXTW = (0x0400 + 56),
            TTM_GETTOOLINFOA = (0x0400 + 8),
            TTM_GETTOOLINFOW = (0x0400 + 53),
            TTM_HITTESTA = (0x0400 + 10),
            TTM_HITTESTW = (0x0400 + 55),
            TTM_NEWTOOLRECTA = (0x0400 + 6),
            TTM_NEWTOOLRECTW = (0x0400 + 52),
            TTM_SETTITLEA = (WM_USER + 32),
            TTM_SETTITLEW = (WM_USER + 33),
            TTM_SETTOOLINFOA = (0x0400 + 9),
            TTM_SETTOOLINFOW = (0x0400 + 54),
            TTM_UPDATE = (0x0400 + 29),
            TTM_UPDATETIPTEXTA = (0x0400 + 12),
            TTM_UPDATETIPTEXTW = (0x0400 + 57),
            TTN_GETDISPINFOA = ((0 - 520) - 0),
            TTN_GETDISPINFOW = ((0 - 520) - 10),
            TTN_NEEDTEXTA = ((0 - 520) - 0),
            TTN_NEEDTEXTW = ((0 - 520) - 10),
            TV_FIRST = 0x1100,
            TVGN_NEXT = 0x0001,
            TVGN_PREVIOUS = 0x0002,
            TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON),
            TVHT_ONITEMICON = 0x0002,
            TVHT_ONITEMLABEL = 0x0004,
            TVHT_ONITEMSTATEICON = 0x0040,
            TVIS_EXPANDED = 0x0020,
            TVM_EDITLABELA = (0x1100 + 14),
            TVM_EDITLABELW = (0x1100 + 65),
            TVM_GETISEARCHSTRINGA = (0x1100 + 23),
            TVM_GETISEARCHSTRINGW = (0x1100 + 64),
            TVM_GETITEMA = (0x1100 + 12),
            TVM_GETITEMW = (0x1100 + 62),
            TVM_INSERTITEMA = (0x1100 + 0),
            TVM_INSERTITEMW = (0x1100 + 50),
            TVM_SETEXTENDEDSTYLE = (TV_FIRST + 44),
            TVM_SETITEMA = (0x1100 + 13),
            TVM_SETITEMW = (0x1100 + 63),
            TVN_BEGINDRAGA = ((0 - 400) - 7),
            TVN_BEGINDRAGW = ((0 - 400) - 56),
            TVN_BEGINLABELEDITA = ((0 - 400) - 10),
            TVN_BEGINLABELEDITW = ((0 - 400) - 59),
            TVN_BEGINRDRAGA = ((0 - 400) - 8),
            TVN_BEGINRDRAGW = ((0 - 400) - 57),
            TVN_ENDLABELEDITA = ((0 - 400) - 11),
            TVN_ENDLABELEDITW = ((0 - 400) - 60),
            TVN_GETDISPINFOA = ((0 - 400) - 3),
            TVN_GETDISPINFOW = ((0 - 400) - 52),
            TVN_ITEMEXPANDEDA = ((0 - 400) - 6),
            TVN_ITEMEXPANDEDW = ((0 - 400) - 55),
            TVN_ITEMEXPANDINGA = ((0 - 400) - 5),
            TVN_ITEMEXPANDINGW = ((0 - 400) - 54),
            TVN_SELCHANGEDA = ((0 - 400) - 2),
            TVN_SELCHANGEDW = ((0 - 400) - 51),
            TVN_SELCHANGINGA = ((0 - 400) - 1),
            TVN_SELCHANGINGW = ((0 - 400) - 50),
            TVN_SETDISPINFOA = ((0 - 400) - 4),
            TVN_SETDISPINFOW = ((0 - 400) - 53),
            TVS_EX_DOUBLEBUFFER = 0x0004,
            WM_ACTIVATE = 0x0006,
            WM_CHAR = 0x0102,
            WM_CLEAR = 0x0303,
            WM_CONTEXTMENU = 0x007B,
            WM_COPY = 0x0301,
            WM_CTLCOLOR = 0x0019,
            WM_CUT = 0x0300,
            WM_DESTROY = 0x0002,
            WM_GETTEXT = 0x000D,
            WM_HSCROLL = 0x0114,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_INITMENU = 0x0116,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_KILLFOCUS = 0x0008,
            WM_NCDESTROY = 0x0082,
            WM_NCPAINT = 0x0085,
            WM_NOTIFY = 0x004E,
            WM_PAINT = 0x000F,
            WM_PASTE = 0x0302,
            WM_POWER = 0x0048,
            WM_PRINT = 0x0317,
            WM_SETFOCUS = 0x0007,
            WM_SIZE = 0x0005,
            WM_UNDO = 0x0304,
            WM_USER = 0x0400,
            WM_VSCROLL = 0x0115,
            WS_CLIPCHILDREN = 0x02000000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RIGHT = 0x00001000,
            WS_MAXIMIZE = 0x01000000,
            WS_MINIMIZE = 0x20000000,
            HH_FTS_DEFAULT_PROXIMITY = -1;

        internal const string
            MSH_SCROLL_LINES = "MSH_SCROLL_LINES_MSG";
        #endregion

        #region Constructor(s)
        static NativeMethods()
        {
            if (Marshal.SystemDefaultCharSize == 1)
            {
                BFFM_SETSELECTION = BFFM_SETSELECTIONA;
                CBEM_GETITEM = CBEM_GETITEMA;
                CBEM_SETITEM = CBEM_SETITEMA;
                CBEN_ENDEDIT = CBEN_ENDEDITA;
                CBEM_INSERTITEM = CBEM_INSERTITEMA;
                LVM_GETITEMTEXT = LVM_GETITEMTEXTA;
                LVM_SETITEMTEXT = LVM_SETITEMTEXTA;
                ACM_OPEN = ACM_OPENA;
                DTM_SETFORMAT = DTM_SETFORMATA;
                DTN_USERSTRING = DTN_USERSTRINGA;
                DTN_WMKEYDOWN = DTN_WMKEYDOWNA;
                DTN_FORMAT = DTN_FORMATA;
                DTN_FORMATQUERY = DTN_FORMATQUERYA;
                EMR_POLYTEXTOUT = EMR_POLYTEXTOUTA;
                HDM_INSERTITEM = HDM_INSERTITEMA;
                HDM_GETITEM = HDM_GETITEMA;
                HDM_SETITEM = HDM_SETITEMA;
                HDN_ITEMCHANGING = HDN_ITEMCHANGINGA;
                HDN_ITEMCHANGED = HDN_ITEMCHANGEDA;
                HDN_ITEMCLICK = HDN_ITEMCLICKA;
                HDN_ITEMDBLCLICK = HDN_ITEMDBLCLICKA;
                HDN_DIVIDERDBLCLICK = HDN_DIVIDERDBLCLICKA;
                HDN_BEGINTRACK = HDN_BEGINTRACKA;
                HDN_ENDTRACK = HDN_ENDTRACKA;
                HDN_TRACK = HDN_TRACKA;
                HDN_GETDISPINFO = HDN_GETDISPINFOA;
                LVM_SETBKIMAGE = LVM_SETBKIMAGEA;
                LVM_GETITEM = LVM_GETITEMA;
                LVM_SETITEM = LVM_SETITEMA;
                LVM_INSERTITEM = LVM_INSERTITEMA;
                LVM_FINDITEM = LVM_FINDITEMA;
                LVM_GETSTRINGWIDTH = LVM_GETSTRINGWIDTHA;
                LVM_EDITLABEL = LVM_EDITLABELA;
                LVM_GETCOLUMN = LVM_GETCOLUMNA;
                LVM_SETCOLUMN = LVM_SETCOLUMNA;
                LVM_GETISEARCHSTRING = LVM_GETISEARCHSTRINGA;
                LVM_INSERTCOLUMN = LVM_INSERTCOLUMNA;
                LVN_BEGINLABELEDIT = LVN_BEGINLABELEDITA;
                LVN_ENDLABELEDIT = LVN_ENDLABELEDITA;
                LVN_ODFINDITEM = LVN_ODFINDITEMA;
                LVN_GETDISPINFO = LVN_GETDISPINFOA;
                LVN_GETINFOTIP = LVN_GETINFOTIPA;
                LVN_SETDISPINFO = LVN_SETDISPINFOA;
                PSM_SETTITLE = PSM_SETTITLEA;
                PSM_SETFINISHTEXT = PSM_SETFINISHTEXTA;
                RB_INSERTBAND = RB_INSERTBANDA;
                SB_SETTEXT = SB_SETTEXTA;
                SB_GETTEXT = SB_GETTEXTA;
                SB_GETTEXTLENGTH = SB_GETTEXTLENGTHA;
                SB_SETTIPTEXT = SB_SETTIPTEXTA;
                SB_GETTIPTEXT = SB_GETTIPTEXTA;
                TB_SAVERESTORE = TB_SAVERESTOREA;
                TB_ADDSTRING = TB_ADDSTRINGA;
                TB_GETBUTTONTEXT = TB_GETBUTTONTEXTA;
                TB_MAPACCELERATOR = TB_MAPACCELERATORA;
                TB_GETBUTTONINFO = TB_GETBUTTONINFOA;
                TB_SETBUTTONINFO = TB_SETBUTTONINFOA;
                TB_INSERTBUTTON = TB_INSERTBUTTONA;
                TB_ADDBUTTONS = TB_ADDBUTTONSA;
                TBN_GETBUTTONINFO = TBN_GETBUTTONINFOA;
                TBN_GETINFOTIP = TBN_GETINFOTIPA;
                TBN_GETDISPINFO = TBN_GETDISPINFOA;
                TTM_ADDTOOL = TTM_ADDTOOLA;
                TTM_SETTITLE = TTM_SETTITLEA;
                TTM_DELTOOL = TTM_DELTOOLA;
                TTM_NEWTOOLRECT = TTM_NEWTOOLRECTA;
                TTM_GETTOOLINFO = TTM_GETTOOLINFOA;
                TTM_SETTOOLINFO = TTM_SETTOOLINFOA;
                TTM_HITTEST = TTM_HITTESTA;
                TTM_GETTEXT = TTM_GETTEXTA;
                TTM_UPDATETIPTEXT = TTM_UPDATETIPTEXTA;
                TTM_ENUMTOOLS = TTM_ENUMTOOLSA;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLA;
                TTN_GETDISPINFO = TTN_GETDISPINFOA;
                TTN_NEEDTEXT = TTN_NEEDTEXTA;
                TVM_INSERTITEM = TVM_INSERTITEMA;
                TVM_GETITEM = TVM_GETITEMA;
                TVM_SETITEM = TVM_SETITEMA;
                TVM_EDITLABEL = TVM_EDITLABELA;
                TVM_GETISEARCHSTRING = TVM_GETISEARCHSTRINGA;
                TVN_SELCHANGING = TVN_SELCHANGINGA;
                TVN_SELCHANGED = TVN_SELCHANGEDA;
                TVN_GETDISPINFO = TVN_GETDISPINFOA;
                TVN_SETDISPINFO = TVN_SETDISPINFOA;
                TVN_ITEMEXPANDING = TVN_ITEMEXPANDINGA;
                TVN_ITEMEXPANDED = TVN_ITEMEXPANDEDA;
                TVN_BEGINDRAG = TVN_BEGINDRAGA;
                TVN_BEGINRDRAG = TVN_BEGINRDRAGA;
                TVN_BEGINLABELEDIT = TVN_BEGINLABELEDITA;
                TVN_ENDLABELEDIT = TVN_ENDLABELEDITA;
                TCM_GETITEM = TCM_GETITEMA;
                TCM_SETITEM = TCM_SETITEMA;
                TCM_INSERTITEM = TCM_INSERTITEMA;
            }
            else
            {
                BFFM_SETSELECTION = BFFM_SETSELECTIONW;
                CBEM_GETITEM = CBEM_GETITEMW;
                CBEM_SETITEM = CBEM_SETITEMW;
                CBEN_ENDEDIT = CBEN_ENDEDITW;
                CBEM_INSERTITEM = CBEM_INSERTITEMW;
                LVM_GETITEMTEXT = LVM_GETITEMTEXTW;
                LVM_SETITEMTEXT = LVM_SETITEMTEXTW;
                ACM_OPEN = ACM_OPENW;
                DTM_SETFORMAT = DTM_SETFORMATW;
                DTN_USERSTRING = DTN_USERSTRINGW;
                DTN_WMKEYDOWN = DTN_WMKEYDOWNW;
                DTN_FORMAT = DTN_FORMATW;
                DTN_FORMATQUERY = DTN_FORMATQUERYW;
                EMR_POLYTEXTOUT = EMR_POLYTEXTOUTW;
                HDM_INSERTITEM = HDM_INSERTITEMW;
                HDM_GETITEM = HDM_GETITEMW;
                HDM_SETITEM = HDM_SETITEMW;
                HDN_ITEMCHANGING = HDN_ITEMCHANGINGW;
                HDN_ITEMCHANGED = HDN_ITEMCHANGEDW;
                HDN_ITEMCLICK = HDN_ITEMCLICKW;
                HDN_ITEMDBLCLICK = HDN_ITEMDBLCLICKW;
                HDN_DIVIDERDBLCLICK = HDN_DIVIDERDBLCLICKW;
                HDN_BEGINTRACK = HDN_BEGINTRACKW;
                HDN_ENDTRACK = HDN_ENDTRACKW;
                HDN_TRACK = HDN_TRACKW;
                HDN_GETDISPINFO = HDN_GETDISPINFOW;
                LVM_SETBKIMAGE = LVM_SETBKIMAGEW;
                LVM_GETITEM = LVM_GETITEMW;
                LVM_SETITEM = LVM_SETITEMW;
                LVM_INSERTITEM = LVM_INSERTITEMW;
                LVM_FINDITEM = LVM_FINDITEMW;
                LVM_GETSTRINGWIDTH = LVM_GETSTRINGWIDTHW;
                LVM_EDITLABEL = LVM_EDITLABELW;
                LVM_GETCOLUMN = LVM_GETCOLUMNW;
                LVM_SETCOLUMN = LVM_SETCOLUMNW;
                LVM_GETISEARCHSTRING = LVM_GETISEARCHSTRINGW;
                LVM_INSERTCOLUMN = LVM_INSERTCOLUMNW;
                LVN_BEGINLABELEDIT = LVN_BEGINLABELEDITW;
                LVN_ENDLABELEDIT = LVN_ENDLABELEDITW;
                LVN_ODFINDITEM = LVN_ODFINDITEMW;
                LVN_GETDISPINFO = LVN_GETDISPINFOW;
                LVN_GETINFOTIP = LVN_GETINFOTIPW;
                LVN_SETDISPINFO = LVN_SETDISPINFOW;
                PSM_SETTITLE = PSM_SETTITLEW;
                PSM_SETFINISHTEXT = PSM_SETFINISHTEXTW;
                RB_INSERTBAND = RB_INSERTBANDW;
                SB_SETTEXT = SB_SETTEXTW;
                SB_GETTEXT = SB_GETTEXTW;
                SB_GETTEXTLENGTH = SB_GETTEXTLENGTHW;
                SB_SETTIPTEXT = SB_SETTIPTEXTW;
                SB_GETTIPTEXT = SB_GETTIPTEXTW;
                TB_SAVERESTORE = TB_SAVERESTOREW;
                TB_ADDSTRING = TB_ADDSTRINGW;
                TB_GETBUTTONTEXT = TB_GETBUTTONTEXTW;
                TB_MAPACCELERATOR = TB_MAPACCELERATORW;
                TB_GETBUTTONINFO = TB_GETBUTTONINFOW;
                TB_SETBUTTONINFO = TB_SETBUTTONINFOW;
                TB_INSERTBUTTON = TB_INSERTBUTTONW;
                TB_ADDBUTTONS = TB_ADDBUTTONSW;
                TBN_GETBUTTONINFO = TBN_GETBUTTONINFOW;
                TBN_GETINFOTIP = TBN_GETINFOTIPW;
                TBN_GETDISPINFO = TBN_GETDISPINFOW;
                TTM_ADDTOOL = TTM_ADDTOOLW;
                TTM_SETTITLE = TTM_SETTITLEW;
                TTM_DELTOOL = TTM_DELTOOLW;
                TTM_NEWTOOLRECT = TTM_NEWTOOLRECTW;
                TTM_GETTOOLINFO = TTM_GETTOOLINFOW;
                TTM_SETTOOLINFO = TTM_SETTOOLINFOW;
                TTM_HITTEST = TTM_HITTESTW;
                TTM_GETTEXT = TTM_GETTEXTW;
                TTM_UPDATETIPTEXT = TTM_UPDATETIPTEXTW;
                TTM_ENUMTOOLS = TTM_ENUMTOOLSW;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLW;
                TTN_GETDISPINFO = TTN_GETDISPINFOW;
                TTN_NEEDTEXT = TTN_NEEDTEXTW;
                TVM_INSERTITEM = TVM_INSERTITEMW;
                TVM_GETITEM = TVM_GETITEMW;
                TVM_SETITEM = TVM_SETITEMW;
                TVM_EDITLABEL = TVM_EDITLABELW;
                TVM_GETISEARCHSTRING = TVM_GETISEARCHSTRINGW;
                TVN_SELCHANGING = TVN_SELCHANGINGW;
                TVN_SELCHANGED = TVN_SELCHANGEDW;
                TVN_GETDISPINFO = TVN_GETDISPINFOW;
                TVN_SETDISPINFO = TVN_SETDISPINFOW;
                TVN_ITEMEXPANDING = TVN_ITEMEXPANDINGW;
                TVN_ITEMEXPANDED = TVN_ITEMEXPANDEDW;
                TVN_BEGINDRAG = TVN_BEGINDRAGW;
                TVN_BEGINRDRAG = TVN_BEGINRDRAGW;
                TVN_BEGINLABELEDIT = TVN_BEGINLABELEDITW;
                TVN_ENDLABELEDIT = TVN_ENDLABELEDITW;
                TCM_GETITEM = TCM_GETITEMW;
                TCM_SETITEM = TCM_SETITEMW;
                TCM_INSERTITEM = TCM_INSERTITEMW;
            }
        }
        #endregion

        #region Other methods
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int CombineRgn(HandleRef hRgn, HandleRef hRgn1, HandleRef hRgn2, int nCombineMode);

        [ResourceExposure(ResourceScope.Process)]
        [ResourceConsumption(ResourceScope.Process)]
        internal static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
        {
            return HandleCollector.Add(IntCreateRectRgn(x1, y1, x2, y2), CommonHandles.GDI);
        }

        internal static bool DeleteObject(HandleRef hObject)
        {
            HandleCollector.Remove((IntPtr)hObject, CommonHandles.GDI);
            return IntDeleteObject(hObject);
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport("user32.dll")]
        internal static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

        [ResourceExposure(ResourceScope.Process)]
        [ResourceConsumption(ResourceScope.Process)]
        internal static IntPtr GetWindowDC(HandleRef hWnd)
        {
            return HandleCollector.Add(IntGetWindowDC(hWnd), CommonHandles.HDC);
        }

        public static uint HIWORD(int n)
        {
            return (uint)(n >> 16) & 0xffff;
        }

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "CreateRectRgn",
            CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "DeleteObject",
            CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool IntDeleteObject(HandleRef hObject);

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "GetWindowDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern IntPtr IntGetWindowDC(HandleRef hWnd);

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        internal static int MAKELANGID(int primary, int sub)
        {
            return ((((ushort)(sub)) << 10) | (ushort)(primary));
        }

        [DllImport(ExternDll.User32, CharSet = CharSet.Unicode, EntryPoint = "MessageBoxW", ExactSpelling = true)]
        internal static extern int MessageBoxSystem(IntPtr hWnd, string text, string caption, int type);

        [DllImport("User32", SetLastError = true)]
        internal static extern Int32 MsgWaitForMultipleObjects(Int32 nCount, IntPtr pHandles, Int16 bWaitAll,
            Int32 dwMilliseconds, Int32 dwWakeMask);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool RedrawWindow(HandleRef hwnd, ref RECT rcUpdate, HandleRef hrgnUpdate, int flags);

        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            HandleCollector.Remove((IntPtr)hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, ref RECT lParam);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        internal static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        #endregion

        #region Nested Classes
        internal sealed class CommonHandles
        {
            internal static readonly int Accelerator = HandleCollector.RegisterType("Accelerator", 80, 50);
            internal static readonly int CompatibleHDC = HandleCollector.RegisterType("ComptibleHDC", 50, 50);
            internal static readonly int Cursor = HandleCollector.RegisterType("Cursor", 20, 500);
            internal static readonly int EMF = HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);
            internal static readonly int Find = HandleCollector.RegisterType("Find", 0, 1000);
            internal static readonly int GDI = HandleCollector.RegisterType("GDI", 50, 500);
            internal static readonly int HDC = HandleCollector.RegisterType("HDC", 100, 2);
            internal static readonly int Icon = HandleCollector.RegisterType("Icon", 20, 500);
            internal static readonly int Kernel = HandleCollector.RegisterType("Kernel", 0, 1000);
            internal static readonly int Menu = HandleCollector.RegisterType("Menu", 30, 1000);
            internal static readonly int Window = HandleCollector.RegisterType("Window", 5, 1000);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ComboBoxInfo
        {
            public int cbSize;
            public readonly RECT rcItem;
            public readonly RECT rcButton;
            public readonly IntPtr stateButton;
            public readonly IntPtr hwndCombo;
            public readonly IntPtr hwndEdit;
            public readonly IntPtr hwndList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle r)
            {
                left = r.Left;
                top = r.Top;
                right = r.Right;
                bottom = r.Bottom;
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            internal Size Size
            {
                get { return new Size(right - left, bottom - top); }
            }
        }
        #endregion

        internal static readonly int
            LANG_USER_DEFAULT = MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
            BFFM_SETSELECTION,
            CBEM_GETITEM,
            CBEM_SETITEM,
            CBEN_ENDEDIT,
            CBEM_INSERTITEM,
            LVM_GETITEMTEXT,
            LVM_SETITEMTEXT,
            ACM_OPEN,
            DTM_SETFORMAT,
            DTN_USERSTRING,
            DTN_WMKEYDOWN,
            DTN_FORMAT,
            DTN_FORMATQUERY,
            EMR_POLYTEXTOUT,
            HDM_INSERTITEM,
            HDM_GETITEM,
            HDM_SETITEM,
            HDN_ITEMCHANGING,
            HDN_ITEMCHANGED,
            HDN_ITEMCLICK,
            HDN_ITEMDBLCLICK,
            HDN_DIVIDERDBLCLICK,
            HDN_BEGINTRACK,
            HDN_ENDTRACK,
            HDN_TRACK,
            HDN_GETDISPINFO,
            LVM_GETITEM,
            LVM_SETBKIMAGE,
            LVM_SETITEM,
            LVM_INSERTITEM,
            LVM_FINDITEM,
            LVM_GETSTRINGWIDTH,
            LVM_EDITLABEL,
            LVM_GETCOLUMN,
            LVM_SETCOLUMN,
            LVM_GETISEARCHSTRING,
            LVM_INSERTCOLUMN,
            LVN_BEGINLABELEDIT,
            LVN_ENDLABELEDIT,
            LVN_ODFINDITEM,
            LVN_GETDISPINFO,
            LVN_GETINFOTIP,
            LVN_SETDISPINFO,
            PSM_SETTITLE,
            PSM_SETFINISHTEXT,
            RB_INSERTBAND,
            SB_SETTEXT,
            SB_GETTEXT,
            SB_GETTEXTLENGTH,
            SB_SETTIPTEXT,
            SB_GETTIPTEXT,
            TB_SAVERESTORE,
            TB_ADDSTRING,
            TB_GETBUTTONTEXT,
            TB_MAPACCELERATOR,
            TB_GETBUTTONINFO,
            TB_SETBUTTONINFO,
            TB_INSERTBUTTON,
            TB_ADDBUTTONS,
            TBN_GETBUTTONINFO,
            TBN_GETINFOTIP,
            TBN_GETDISPINFO,
            TTM_ADDTOOL,
            TTM_SETTITLE,
            TTM_DELTOOL,
            TTM_NEWTOOLRECT,
            TTM_GETTOOLINFO,
            TTM_SETTOOLINFO,
            TTM_HITTEST,
            TTM_GETTEXT,
            TTM_UPDATETIPTEXT,
            TTM_ENUMTOOLS,
            TTM_GETCURRENTTOOL,
            TTN_GETDISPINFO,
            TTN_NEEDTEXT,
            TVM_INSERTITEM,
            TVM_GETITEM,
            TVM_SETITEM,
            TVM_EDITLABEL,
            TVM_GETISEARCHSTRING,
            TVN_SELCHANGING,
            TVN_SELCHANGED,
            TVN_GETDISPINFO,
            TVN_SETDISPINFO,
            TVN_ITEMEXPANDING,
            TVN_ITEMEXPANDED,
            TVN_BEGINDRAG,
            TVN_BEGINRDRAG,
            TVN_BEGINLABELEDIT,
            TVN_ENDLABELEDIT,
            TCM_GETITEM,
            TCM_SETITEM,
            TCM_INSERTITEM;

        internal static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
    }

    internal static class ExternDll
    {
        public const string Gdi32 = "gdi32.dll";
        public const string Imm32 = "imm32.dll";
        public const string User32 = "user32.dll";
    }

    internal static class Win32NativeSafeNativeMethods
    {
        public const int
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
            FORMAT_MESSAGE_FROM_STRING = 0x00000400,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;

        public const int
            FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF,
            FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;

        public const int
            MB_RIGHT = 0x00080000,
            MB_RTLREADING = 0x00100000;


        [SecurityCritical]
        public static int MessageBox(IntPtr hWnd, string text, string caption, int type)
        {
            try
            {
                return NativeMethods.MessageBoxSystem(hWnd, text, caption, type);
            }
            catch (DllNotFoundException)
            {
                return 0;
            }
            catch (EntryPointNotFoundException)
            {
                return 0;
            }
        }
    }

    internal class UnSafeNativeMethods
    {
        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr ImmAssociateContext(HandleRef hWnd, HandleRef hIMC);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr ImmCreateContext();

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmDestroyContext(HandleRef hIMC);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr ImmGetContext(HandleRef hWnd);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmGetConversionStatus(HandleRef hIMC, ref int conversion, ref int sentence);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmGetOpenStatus(HandleRef hIMC);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmNotifyIME(HandleRef hIMC, int dwAction, int dwIndex, int dwValue);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmReleaseContext(HandleRef hWnd, HandleRef hIMC);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmSetConversionStatus(HandleRef hIMC, int conversion, int sentence);

        [DllImport(ExternDll.Imm32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool ImmSetOpenStatus(HandleRef hIMC, bool open);
    }

}