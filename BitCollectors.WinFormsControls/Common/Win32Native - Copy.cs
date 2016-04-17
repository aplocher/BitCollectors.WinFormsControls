using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using HandleCollector = BitCollectors.WinFormsControls.Internal.HandleCollector;

namespace BitCollectors.WinFormsControls.Common
{
    internal static class Win32Native
    {
        internal const int
            TVHT_ONITEMICON = 0x0002,
            TVHT_ONITEMLABEL = 0x0004,
            TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON),

            TVHT_ONITEMSTATEICON = 0x0040,

            WM_DESTROY = 0x0002,

            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,

            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,

            WM_GETTEXT = 0x000D,

            WM_PAINT = 0x000F,

            WM_POWER = 0x0048,

            WM_NOTIFY = 0x004E,

            WM_INPUTLANGCHANGE = 0x0051,

            WM_CONTEXTMENU = 0x007B,

            WM_NCDESTROY = 0x0082,

            WM_NCPAINT = 0x0085,

            WM_CHAR = 0x0102,

            WM_CTLCOLOR = 0x0019,

            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_COMPOSITION = 0x010F,

            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,
            WM_INITMENU = 0x0116,

            WM_CUT = 0x0300,
            WM_COPY = 0x0301,
            WM_PASTE = 0x0302,
            WM_CLEAR = 0x0303,
            WM_UNDO = 0x0304,

            WM_PRINT = 0x0317,

            WM_USER = 0x0400,

            WS_MINIMIZE = 0x20000000,

            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,

            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,

            TVM_SETEXTENDEDSTYLE = (TV_FIRST + 44),
            TVS_EX_DOUBLEBUFFER = 0x0004,

            R2_NOT = 6,

            R2_MASKPEN = 9,

            R2_MERGEPEN = 15,

            SHGFI_ICON = 0x000000100,

            AUTOSUGGEST = 0x10000000,

            AUTOAPPEND = 0x40000000,

            ACM_OPENA = (0x0400 + 100),
            ACM_OPENW = (0x0400 + 103),

            BDR_RAISED = 0x0005,
            BDR_SUNKEN = 0x000a,

            BFFM_SETSELECTIONA = 0x400 + 102,
            BFFM_SETSELECTIONW = 0x400 + 103,

            BS_OWNERDRAW = 0x0000000B,

            BS_RIGHT = 0x00000200,

            BS_TOP = 0x00000400,
            BS_BOTTOM = 0x00000800,

            cmb4 = 0x0473,

            CF_TEXT = 1,

            CBS_DROPDOWN = 0x0002,

            CB_GETLBTEXT = 0x0148,

            CB_FINDSTRING = 0x014C,

            CDRF_NOTIFYITEMDRAW = 0x00000020,

            CDDS_ITEM = 0x00010000,

            CBEM_INSERTITEMA = (0x0400 + 1),
            CBEM_GETITEMA = (0x0400 + 4),
            CBEM_SETITEMA = (0x0400 + 5),
            CBEM_INSERTITEMW = (0x0400 + 11),
            CBEM_SETITEMW = (0x0400 + 12),
            CBEM_GETITEMW = (0x0400 + 13),
            CBEN_ENDEDITA = ((0 - 800) - 5),
            CBEN_ENDEDITW = ((0 - 800) - 6),

            CSIDL_DESKTOP = 0x0000,
            CSIDL_INTERNET = 0x0001,

            CSIDL_PROGRAM_FILES = 0x0026,

            DUPLICATE = 0x06,
            DISPID_UNKNOWN = (-1),

            DTM_SETFORMATA = (0x1000 + 5),
            DTM_SETFORMATW = (0x1000 + 50),

            DTN_USERSTRINGA = ((0 - 760) + 2),
            DTN_USERSTRINGW = ((0 - 760) + 15),
            DTN_WMKEYDOWNA = ((0 - 760) + 3),
            DTN_WMKEYDOWNW = ((0 - 760) + 16),
            DTN_FORMATA = ((0 - 760) + 4),
            DTN_FORMATW = ((0 - 760) + 17),
            DTN_FORMATQUERYA = ((0 - 760) + 5),
            DTN_FORMATQUERYW = ((0 - 760) + 18),

            DVASPECT_CONTENT = 1,

            EMR_POLYTEXTOUTA = 96,
            EMR_POLYTEXTOUTW = 97,

            ES_LEFT = 0x0000,
            ES_CENTER = 0x0001,
            ES_RIGHT = 0x0002,

            EM_SCROLL = 0x00B5,
            EM_SCROLLCARET = 0x00B7,

            EM_GETLINE = 0x00C4,
            EM_LIMITTEXT = 0x00C5,
            EM_CANUNDO = 0x00C6,
            EM_UNDO = 0x00C7,

            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,

            GMEM_FIXED = 0x0000,
            GMEM_MOVEABLE = 0x0002,

            GMEM_ZEROINIT = 0x0040,

            GMEM_NOT_BANKED = 0x1000,

            ATTR_INPUT = 0x00,

            HTBOTTOM = 15,

            HDM_INSERTITEMA = (0x1200 + 1),
            HDM_INSERTITEMW = (0x1200 + 10),
            HDM_GETITEMA = (0x1200 + 3),
            HDM_GETITEMW = (0x1200 + 11),

            HDM_SETITEMA = (0x1200 + 4),
            HDM_SETITEMW = (0x1200 + 12),
            HDN_ITEMCHANGINGA = ((0 - 300) - 0),
            HDN_ITEMCHANGINGW = ((0 - 300) - 20),
            HDN_ITEMCHANGEDA = ((0 - 300) - 1),
            HDN_ITEMCHANGEDW = ((0 - 300) - 21),
            HDN_ITEMCLICKA = ((0 - 300) - 2),
            HDN_ITEMCLICKW = ((0 - 300) - 22),
            HDN_ITEMDBLCLICKA = ((0 - 300) - 3),
            HDN_ITEMDBLCLICKW = ((0 - 300) - 23),
            HDN_DIVIDERDBLCLICKA = ((0 - 300) - 5),
            HDN_DIVIDERDBLCLICKW = ((0 - 300) - 25),

            HDN_BEGINTRACKA = ((0 - 300) - 6),
            HDN_BEGINTRACKW = ((0 - 300) - 26),

            HDN_ENDTRACKA = ((0 - 300) - 7),
            HDN_ENDTRACKW = ((0 - 300) - 27),
            HDN_TRACKA = ((0 - 300) - 8),
            HDN_TRACKW = ((0 - 300) - 28),
            HDN_GETDISPINFOA = ((0 - 300) - 9),
            HDN_GETDISPINFOW = ((0 - 300) - 29),

            HBMMENU_MBAR_MINIMIZE = 3,
            HBMMENU_MBAR_CLOSE = 5,

            ILC_COLOR = 0x0000,

            IDM_PRINT = 27,

            LB_ERR = (-1),

            LB_GETSEL = 0x0187,

            LB_GETTEXT = 0x0189,

            LB_FINDSTRING = 0x018F,

            LVSCW_AUTOSIZE = -1,

            LVM_SETBKIMAGEA = (0x1000 + 68),
            LVM_SETBKIMAGEW = (0x1000 + 138),

            LVM_GETITEMA = (0x1000 + 5),
            LVM_GETITEMW = (0x1000 + 75),
            LVM_SETITEMA = (0x1000 + 6),
            LVM_SETITEMW = (0x1000 + 76),

            LVM_INSERTITEMA = (0x1000 + 7),
            LVM_INSERTITEMW = (0x1000 + 77),

            LVM_FINDITEMA = (0x1000 + 13),
            LVM_FINDITEMW = (0x1000 + 83),

            LVM_GETSTRINGWIDTHA = (0x1000 + 17),
            LVM_GETSTRINGWIDTHW = (0x1000 + 87),

            LVHT_ONITEM = (0x0002 | 0x0004 | 0x0008),

            LVM_EDITLABELA = (0x1000 + 23),
            LVM_EDITLABELW = (0x1000 + 118),

            LVM_GETCOLUMNA = (0x1000 + 25),
            LVM_GETCOLUMNW = (0x1000 + 95),
            LVM_SETCOLUMNA = (0x1000 + 26),
            LVM_SETCOLUMNW = (0x1000 + 96),
            LVM_INSERTCOLUMNA = (0x1000 + 27),
            LVM_INSERTCOLUMNW = (0x1000 + 97),

            LVM_SETINSERTMARK = (0x1000 + 166),

            LVM_SETITEMPOSITION = (0x1000 + 15),

            LVM_GETITEMTEXTA = (0x1000 + 45),
            LVM_GETITEMTEXTW = (0x1000 + 115),

            LVM_SETITEMTEXTA = (0x1000 + 46),
            LVM_SETITEMTEXTW = (0x1000 + 116),

            LVM_GETISEARCHSTRINGA = (0x1000 + 52),
            LVM_GETISEARCHSTRINGW = (0x1000 + 117),

            LVM_GETINSERTMARK = (0x1000 + 167),

            LVN_BEGINLABELEDITA = ((0 - 100) - 5),
            LVN_BEGINLABELEDITW = ((0 - 100) - 75),
            LVN_ENDLABELEDITA = ((0 - 100) - 6),
            LVN_ENDLABELEDITW = ((0 - 100) - 76),

            LVN_ODFINDITEMA = ((0 - 100) - 52),
            LVN_ODFINDITEMW = ((0 - 100) - 79),

            LVN_GETDISPINFOA = ((0 - 100) - 50),
            LVN_GETDISPINFOW = ((0 - 100) - 77),

            LVN_SETDISPINFOA = ((0 - 100) - 51),
            LVN_SETDISPINFOW = ((0 - 100) - 78),
            LVN_GETINFOTIPA = ((0 - 100) - 57),
            LVN_GETINFOTIPW = ((0 - 100) - 58),

            LANG_NEUTRAL = 0x00,

            MAX_PATH = 260,
            MA_ACTIVATE = 0x0001,

            MA_NOACTIVATE = 0x0003,

            MCHT_TITLE = 0x00010000,
            MCHT_CALENDAR = 0x00020000,

            MCHT_CALENDARDATE = (0x00020000 | 0x0001),

            MCS_NOTODAY = 0x0010,

            OLEMISC_RECOMPOSEONRESIZE = 0x00000001,
            OLEMISC_INSIDEOUT = 0x00000080,
            OLEMISC_ACTIVATEWHENVISIBLE = 0x0000100,
            OLEMISC_ACTSLIKEBUTTON = 0x00001000,
            OLEMISC_SETCLIENTSITEFIRST = 0x00020000,

            PD_ENABLEPRINTTEMPLATE = 0x00004000,
            PD_ENABLESETUPTEMPLATE = 0x00008000,

            PD_USEDEVMODECOPIES = 0x00040000,

            PBM_SETRANGE = (0x0400 + 1),

            PSM_SETTITLEA = (0x0400 + 111),
            PSM_SETTITLEW = (0x0400 + 120),
            PSM_SETFINISHTEXTA = (0x0400 + 115),
            PSM_SETFINISHTEXTW = (0x0400 + 121),

            QS_KEY = 0x0001,
            QS_MOUSEMOVE = 0x0002,
            QS_MOUSEBUTTON = 0x0004,
            QS_POSTMESSAGE = 0x0008,
            QS_TIMER = 0x0010,
            QS_PAINT = 0x0020,
            QS_SENDMESSAGE = 0x0040,
            QS_HOTKEY = 0x0080,

            QS_MOUSE = QS_MOUSEMOVE | QS_MOUSEBUTTON,
            QS_INPUT = QS_MOUSE | QS_KEY,

            RGN_XOR = 3,

            RDW_INVALIDATE = 0x0001,
            RDW_ERASE = 0x0004,

            RDW_ERASENOW = 0x0200,
            RDW_UPDATENOW = 0x0100,
            RDW_FRAME = 0x0400,
            RB_INSERTBANDA = (0x0400 + 1),
            RB_INSERTBANDW = (0x0400 + 10),
            stc4 = 0x0443,

            STGM_READ = 0x00000000,

            SB_THUMBTRACK = 5,

            SB_ENDSCROLL = 8,

            SORT_DEFAULT = 0x0,
            SUBLANG_DEFAULT = 0x01,

            SW_SHOW = 5,

            SW_MAX = 10,

            SM_CXICON = 11,
            SM_CYICON = 12,

            SM_CYMENU = 15,

            SM_CXMIN = 28,
            SM_CYMIN = 29,
            SM_CXSIZE = 30,
            SM_CYSIZE = 31,
            SM_CXFRAME = 32,
            SM_CYFRAME = 33,

            SPI_GETFONTSMOOTHING = 0x004A,

            SB_SETTEXTA = (0x0400 + 1),
            SB_SETTEXTW = (0x0400 + 11),
            SB_GETTEXTA = (0x0400 + 2),
            SB_GETTEXTW = (0x0400 + 13),
            SB_GETTEXTLENGTHA = (0x0400 + 3),
            SB_GETTEXTLENGTHW = (0x0400 + 12),

            SB_SETTIPTEXTA = (0x0400 + 16),
            SB_SETTIPTEXTW = (0x0400 + 17),
            SB_GETTIPTEXTA = (0x0400 + 18),
            SB_GETTIPTEXTW = (0x0400 + 19),

            SRCCOPY = 0x00CC0020,

            STATFLAG_DEFAULT = 0x0,
            STATFLAG_NONAME = 0x1,

            TRANSPARENT = 1,
            OPAQUE = 2,

            TV_FIRST = 0x1100,

            TB_ADDBUTTONSA = (0x0400 + 20),
            TB_ADDBUTTONSW = (0x0400 + 68),
            TB_INSERTBUTTONA = (0x0400 + 21),
            TB_INSERTBUTTONW = (0x0400 + 67),

            TB_GETBUTTON = (0x0400 + 23),
            TB_SAVERESTOREA = (0x0400 + 26),
            TB_SAVERESTOREW = (0x0400 + 76),
            TB_ADDSTRINGA = (0x0400 + 28),
            TB_ADDSTRINGW = (0x0400 + 77),

            TB_GETBUTTONTEXTA = (0x0400 + 45),
            TB_GETBUTTONTEXTW = (0x0400 + 75),

            TB_GETBUTTONINFOW = (0x0400 + 63),
            TB_SETBUTTONINFOW = (0x0400 + 64),
            TB_GETBUTTONINFOA = (0x0400 + 65),
            TB_SETBUTTONINFOA = (0x0400 + 66),
            TB_MAPACCELERATORA = (0x0400 + 78),

            TB_MAPACCELERATORW = (0x0400 + 90),

            TBN_GETBUTTONINFOA = ((0 - 700) - 0),
            TBN_GETBUTTONINFOW = ((0 - 700) - 20),

            TBN_GETDISPINFOA = ((0 - 700) - 16),
            TBN_GETDISPINFOW = ((0 - 700) - 17),
            TBN_GETINFOTIPA = ((0 - 700) - 18),
            TBN_GETINFOTIPW = ((0 - 700) - 19),

            TTM_SETTITLEA = (WM_USER + 32),
            TTM_SETTITLEW = (WM_USER + 33),
            TTM_ADDTOOLA = (0x0400 + 4),
            TTM_ADDTOOLW = (0x0400 + 50),
            TTM_DELTOOLA = (0x0400 + 5),
            TTM_DELTOOLW = (0x0400 + 51),
            TTM_NEWTOOLRECTA = (0x0400 + 6),
            TTM_NEWTOOLRECTW = (0x0400 + 52),

            TTM_GETTOOLINFOA = (0x0400 + 8),
            TTM_GETTOOLINFOW = (0x0400 + 53),
            TTM_SETTOOLINFOA = (0x0400 + 9),
            TTM_SETTOOLINFOW = (0x0400 + 54),
            TTM_HITTESTA = (0x0400 + 10),
            TTM_HITTESTW = (0x0400 + 55),
            TTM_GETTEXTA = (0x0400 + 11),
            TTM_GETTEXTW = (0x0400 + 56),
            TTM_UPDATE = (0x0400 + 29),
            TTM_UPDATETIPTEXTA = (0x0400 + 12),
            TTM_UPDATETIPTEXTW = (0x0400 + 57),
            TTM_ENUMTOOLSA = (0x0400 + 14),
            TTM_ENUMTOOLSW = (0x0400 + 58),
            TTM_GETCURRENTTOOLA = (0x0400 + 15),
            TTM_GETCURRENTTOOLW = (0x0400 + 59),

            TTN_GETDISPINFOA = ((0 - 520) - 0),
            TTN_GETDISPINFOW = ((0 - 520) - 10),

            TTN_NEEDTEXTA = ((0 - 520) - 0),
            TTN_NEEDTEXTW = ((0 - 520) - 10),

            TBM_SETTIC = (0x0400 + 4),

            TBM_SETRANGE = (0x0400 + 6),

            TVIS_EXPANDED = 0x0020,

            TVM_INSERTITEMA = (0x1100 + 0),
            TVM_INSERTITEMW = (0x1100 + 50),

            TVGN_NEXT = 0x0001,
            TVGN_PREVIOUS = 0x0002,

            TVM_GETITEMA = (0x1100 + 12),
            TVM_GETITEMW = (0x1100 + 62),
            TVM_SETITEMA = (0x1100 + 13),
            TVM_SETITEMW = (0x1100 + 63),
            TVM_EDITLABELA = (0x1100 + 14),
            TVM_EDITLABELW = (0x1100 + 65),

            TVM_GETISEARCHSTRINGA = (0x1100 + 23),
            TVM_GETISEARCHSTRINGW = (0x1100 + 64),

            TVN_SELCHANGINGA = ((0 - 400) - 1),
            TVN_SELCHANGINGW = ((0 - 400) - 50),

            TVN_SELCHANGEDA = ((0 - 400) - 2),
            TVN_SELCHANGEDW = ((0 - 400) - 51),

            TVN_GETDISPINFOA = ((0 - 400) - 3),
            TVN_GETDISPINFOW = ((0 - 400) - 52),
            TVN_SETDISPINFOA = ((0 - 400) - 4),
            TVN_SETDISPINFOW = ((0 - 400) - 53),
            TVN_ITEMEXPANDINGA = ((0 - 400) - 5),
            TVN_ITEMEXPANDINGW = ((0 - 400) - 54),
            TVN_ITEMEXPANDEDA = ((0 - 400) - 6),
            TVN_ITEMEXPANDEDW = ((0 - 400) - 55),
            TVN_BEGINDRAGA = ((0 - 400) - 7),
            TVN_BEGINDRAGW = ((0 - 400) - 56),
            TVN_BEGINRDRAGA = ((0 - 400) - 8),
            TVN_BEGINRDRAGW = ((0 - 400) - 57),
            TVN_BEGINLABELEDITA = ((0 - 400) - 10),
            TVN_BEGINLABELEDITW = ((0 - 400) - 59),
            TVN_ENDLABELEDITA = ((0 - 400) - 11),
            TVN_ENDLABELEDITW = ((0 - 400) - 60),

            TCS_RIGHT = 0x0002,

            TCM_GETITEMA = (0x1300 + 5),
            TCM_GETITEMW = (0x1300 + 60),
            TCM_SETITEMA = (0x1300 + 6),
            TCM_SETITEMW = (0x1300 + 61),
            TCM_INSERTITEMA = (0x1300 + 7),
            TCM_INSERTITEMW = (0x1300 + 62);

        internal const string

            MSH_SCROLL_LINES = "MSH_SCROLL_LINES_MSG";

        internal const string uuid_IAccessible = "{618736E0-3C3D-11CF-810C-00AA00389B71}";

        internal const string uuid_IEnumVariant = "{00020404-0000-0000-C000-000000000046}";

        internal const int
            HH_FTS_DEFAULT_PROXIMITY = -1;

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

        internal static IntPtr InvalidIntPtr = (IntPtr)(-1);

        internal static IntPtr LPSTR_TEXTCALLBACK = (IntPtr)(-1);

        internal static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        internal static HandleRef
            HWND_TOP = new HandleRef(null, (IntPtr)0);

        static Win32Native()
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

        internal delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

        internal delegate int EditStreamCallback(IntPtr dwCookie, IntPtr buf, int cb, out int transferred);

        internal delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        internal delegate int ListViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

        internal delegate int TreeViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

        internal delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

        internal enum tagVT
        {
            VT_EMPTY = 0,

            VT_BSTR = 8,
            VT_DISPATCH = 9,

            VT_UNKNOWN = 13,

            VT_BLOB = 65,
            VT_STREAM = 66,

            VT_BYREF = 16384,

            VT_ILLEGAL = 65535,

            VT_TYPEMASK = 4095
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        internal static int MAKELANGID(int primary, int sub)
        {
            return ((((ushort)(sub)) << 10) | (ushort)(primary));
        }

        internal static int MAKELCID(int lgid)
        {
            return MAKELCID(lgid, SORT_DEFAULT);
        }

        internal static int MAKELCID(int lgid, int sort)
        {
            return ((0xFFFF & lgid) | (((0x000f) & sort) << 16));
        }

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "CreateRectRgn",
            CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int CombineRgn(HandleRef hRgn, HandleRef hRgn1, HandleRef hRgn2, int nCombineMode);

        [ResourceExposure(ResourceScope.Process)]
        [ResourceConsumption(ResourceScope.Process)]
        internal static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
        {
            return HandleCollector.Add(IntCreateRectRgn(x1, y1, x2, y2), CommonHandles.GDI);
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "GetWindowDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern IntPtr IntGetWindowDC(HandleRef hWnd);

        [ResourceExposure(ResourceScope.Process)]
        [ResourceConsumption(ResourceScope.Process)]
        internal static IntPtr GetWindowDC(HandleRef hWnd)
        {
            return HandleCollector.Add(IntGetWindowDC(hWnd), CommonHandles.HDC);
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            HandleCollector.Remove((IntPtr)hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool RedrawWindow(HandleRef hwnd, ref RECT rcUpdate, HandleRef hrgnUpdate, int flags);

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "DeleteObject",
            CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern bool IntDeleteObject(HandleRef hObject);

        internal static bool DeleteObject(HandleRef hObject)
        {
            HandleCollector.Remove((IntPtr)hObject, CommonHandles.GDI);
            return IntDeleteObject(hObject);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTSTRUCT
        {
            internal int x;
            internal int y;

            internal POINTSTRUCT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;

            internal RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            internal RECT(Rectangle r)
            {
                left = r.Left;
                top = r.Top;
                right = r.Right;
                bottom = r.Bottom;
            }

            internal static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            internal Size Size
            {
                get { return new Size(right - left, bottom - top); }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TEXTMETRIC
        {
            internal int tmHeight;
            internal int tmAscent;
            internal int tmDescent;
            internal int tmInternalLeading;
            internal int tmExternalLeading;
            internal int tmAveCharWidth;
            internal int tmMaxCharWidth;
            internal int tmWeight;
            internal int tmOverhang;
            internal int tmDigitizedAspectX;
            internal int tmDigitizedAspectY;
            internal char tmFirstChar;
            internal char tmLastChar;
            internal char tmDefaultChar;
            internal char tmBreakChar;
            internal byte tmItalic;
            internal byte tmUnderlined;
            internal byte tmStruckOut;
            internal byte tmPitchAndFamily;
            internal byte tmCharSet;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class FONTDESC
        {
            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(FONTDESC));
            internal string lpstrName;
            internal long cySize;
            internal short sWeight;
            internal short sCharset;
            internal bool fItalic;
            internal bool fUnderline;
            internal bool fStrikethrough;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PICTDESCbmp
        {
            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(PICTDESCbmp));
            internal int picType = Ole.PICTYPE_BITMAP;
            internal IntPtr hbitmap = IntPtr.Zero;
            internal IntPtr hpalette = IntPtr.Zero;
            internal int unused = 0;

            internal PICTDESCbmp(Bitmap bitmap)
            {
                hbitmap = bitmap.GetHbitmap();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PICTDESCemf
        {
            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(PICTDESCemf));
            internal int picType = Ole.PICTYPE_ENHMETAFILE;
            internal IntPtr hemf = IntPtr.Zero;
            internal int unused1 = 0;
            internal int unused2 = 0;

            internal PICTDESCemf(Metafile metafile)
            {
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class HH_AKLINK
        {
            internal int cbStruct = Marshal.SizeOf(typeof(HH_AKLINK));
            internal bool fReserved = false;
            internal string pszKeywords = null;
            internal string pszUrl = null;
            internal string pszMsgText = null;
            internal string pszMsgTitle = null;
            internal string pszWindow = null;
            internal bool fIndexOnFail = false;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class HH_POPUP
        {
            internal int cbStruct = Marshal.SizeOf(typeof(HH_POPUP));
            internal IntPtr hinst = IntPtr.Zero;
            internal int idString = 0;
            internal IntPtr pszText;
            internal POINT pt;
            internal int clrForeground = -1;
            internal int clrBackground = -1;

            internal RECT rcMargins = RECT.FromXYWH(-1, -1, -1, -1);

            internal string pszFont = null;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class HH_FTS_QUERY
        {
            internal int cbStruct = Marshal.SizeOf(typeof(HH_FTS_QUERY));
            internal bool fUniCodeStrings = false;

            [MarshalAs(UnmanagedType.LPStr)]
            internal string pszSearchQuery = null;

            internal int iProximity = HH_FTS_DEFAULT_PROXIMITY;
            internal bool fStemmedSearch = false;
            internal bool fTitleOnly = false;
            internal bool fExecute = true;

            [MarshalAs(UnmanagedType.LPStr)]
            internal string pszWindow = null;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class MONITORINFOEX
        {
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            internal RECT rcMonitor = new RECT();
            internal RECT rcWork = new RECT();
            internal int dwFlags = 0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class MONITORINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            internal RECT rcMonitor = new RECT();
            internal RECT rcWork = new RECT();
            internal int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class IMAGELISTDRAWPARAMS
        {
            internal int cbSize = Marshal.SizeOf(typeof(IMAGELISTDRAWPARAMS));
            internal IntPtr himl = IntPtr.Zero;
            internal int i = 0;
            internal IntPtr hdcDst = IntPtr.Zero;
            internal int x = 0;
            internal int y = 0;
            internal int cx = 0;
            internal int cy = 0;
            internal int xBitmap = 0;
            internal int yBitmap = 0;
            internal int rgbBk = 0;
            internal int rgbFg = 0;
            internal int fStyle = 0;
            internal int dwRop = 0;
            internal int fState = 0;
            internal int Frame = 0;
            internal int crEffect = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class TRACKMOUSEEVENT
        {
            internal int cbSize = Marshal.SizeOf(typeof(TRACKMOUSEEVENT));
            internal int dwFlags;
            internal IntPtr hwndTrack;
            internal int dwHoverTime = 100;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class POINT
        {
            internal int x;
            internal int y;

            internal POINT()
            {
            }

            internal POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class NONCLIENTMETRICS
        {
            internal int cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));
            internal int iBorderWidth = 0;
            internal int iScrollWidth = 0;
            internal int iScrollHeight = 0;
            internal int iCaptionWidth = 0;
            internal int iCaptionHeight = 0;

            [MarshalAs(UnmanagedType.Struct)]
            internal LOGFONT lfCaptionFont = null;

            internal int iSmCaptionWidth = 0;
            internal int iSmCaptionHeight = 0;

            [MarshalAs(UnmanagedType.Struct)]
            internal LOGFONT lfSmCaptionFont = null;

            internal int iMenuWidth = 0;
            internal int iMenuHeight = 0;

            [MarshalAs(UnmanagedType.Struct)]
            internal LOGFONT lfMenuFont = null;

            [MarshalAs(UnmanagedType.Struct)]
            internal LOGFONT lfStatusFont = null;

            [MarshalAs(UnmanagedType.Struct)]
            internal LOGFONT lfMessageFont = null;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class SCROLLINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
            internal int fMask;
            internal int nMin;
            internal int nMax;
            internal int nPage;
            internal int nPos;
            internal int nTrackPos;

            internal SCROLLINFO()
            {
            }

            internal SCROLLINFO(int mask, int min, int max, int page, int pos)
            {
                fMask = mask;
                nMin = min;
                nMax = max;
                nPage = page;
                nPos = pos;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class TPMPARAMS
        {
            internal int cbSize = Marshal.SizeOf(typeof(TPMPARAMS));

            internal int rcExclude_left;
            internal int rcExclude_top;
            internal int rcExclude_right;
            internal int rcExclude_bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class SIZE
        {
            internal int cx;
            internal int cy;

            internal SIZE()
            {
            }

            internal SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PICTDESC
        {
            internal int cbSizeOfStruct;
            internal int picType;
            internal IntPtr union1;
            internal int union2;
            internal int union3;

            internal static PICTDESC CreateBitmapPICTDESC(IntPtr hbitmap, IntPtr hpal)
            {
                PICTDESC pictdesc = new PICTDESC();
                pictdesc.cbSizeOfStruct = 16;
                pictdesc.picType = Ole.PICTYPE_BITMAP;
                pictdesc.union1 = hbitmap;
                pictdesc.union2 = (int)(((long)hpal) & 0xffffffff);
                pictdesc.union3 = (int)(((long)hpal) >> 32);
                return pictdesc;
            }

            internal static PICTDESC CreateIconPICTDESC(IntPtr hicon)
            {
                PICTDESC pictdesc = new PICTDESC();
                pictdesc.cbSizeOfStruct = 12;
                pictdesc.picType = Ole.PICTYPE_ICON;
                pictdesc.union1 = hicon;
                return pictdesc;
            }

            internal virtual IntPtr GetHandle()
            {
                return union1;
            }

            internal virtual IntPtr GetHPal()
            {
                if (picType == Ole.PICTYPE_BITMAP)
                    return (IntPtr)((uint)union2 | (((long)union3) << 32));

                return IntPtr.Zero;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class tagFONTDESC
        {
            internal int cbSizeofstruct = Marshal.SizeOf(typeof(tagFONTDESC));

            [MarshalAs(UnmanagedType.LPWStr)]
            internal string lpstrName;

            [MarshalAs(UnmanagedType.U8)]
            internal long cySize;

            [MarshalAs(UnmanagedType.U2)]
            internal short sWeight;

            [MarshalAs(UnmanagedType.U2)]
            internal short sCharset;

            [MarshalAs(UnmanagedType.Bool)]
            internal bool fItalic;

            [MarshalAs(UnmanagedType.Bool)]
            internal bool fUnderline;

            [MarshalAs(UnmanagedType.Bool)]
            internal bool fStrikethrough;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class CHOOSECOLOR
        {
            internal int lStructSize = Marshal.SizeOf(typeof(CHOOSECOLOR));
            internal IntPtr hwndOwner;
            internal IntPtr hInstance;
            internal int rgbResult;
            internal IntPtr lpCustColors;
            internal int Flags;
            internal IntPtr lCustData = IntPtr.Zero;
            internal WndProc lpfnHook;
            internal string lpTemplateName = null;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class LOGFONT
        {
            internal LOGFONT()
            {
            }

            internal LOGFONT(LOGFONT lf)
            {
                Debug.Assert(lf != null, "lf is null");
                lfHeight = lf.lfHeight;
                lfWidth = lf.lfWidth;
                lfEscapement = lf.lfEscapement;
                lfOrientation = lf.lfOrientation;
                lfWeight = lf.lfWeight;
                lfItalic = lf.lfItalic;
                lfUnderline = lf.lfUnderline;
                lfStrikeOut = lf.lfStrikeOut;
                lfCharSet = lf.lfCharSet;
                lfOutPrecision = lf.lfOutPrecision;
                lfClipPrecision = lf.lfClipPrecision;
                lfQuality = lf.lfQuality;
                lfPitchAndFamily = lf.lfPitchAndFamily;
                lfFaceName = lf.lfFaceName;
            }

            internal int lfHeight;
            internal int lfWidth;
            internal int lfEscapement;
            internal int lfOrientation;
            internal int lfWeight;
            internal byte lfItalic;
            internal byte lfUnderline;
            internal byte lfStrikeOut;
            internal byte lfCharSet;
            internal byte lfOutPrecision;
            internal byte lfClipPrecision;
            internal byte lfQuality;
            internal byte lfPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            internal string lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class NOTIFYICONDATA
        {
            internal int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            internal IntPtr hWnd;
            internal int uID;
            internal int uFlags;
            internal int uCallbackMessage;
            internal IntPtr hIcon;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szTip;

            internal int dwState = 0;
            internal int dwStateMask = 0;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szInfo;

            internal int uTimeoutOrVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            internal string szInfoTitle;

            internal int dwInfoFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MENUITEMINFO_T
        {
            internal int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO_T));
            internal int fMask;
            internal int fType;
            internal int fState;
            internal int wID;
            internal IntPtr hSubMenu;
            internal IntPtr hbmpChecked;
            internal IntPtr hbmpUnchecked;
            internal IntPtr dwItemData;
            internal string dwTypeData;
            internal int cch;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MENUITEMINFO_T_RW
        {
            internal int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO_T_RW));
            internal int fMask = 0;
            internal int fType = 0;
            internal int fState = 0;
            internal int wID = 0;
            internal IntPtr hSubMenu = IntPtr.Zero;
            internal IntPtr hbmpChecked = IntPtr.Zero;
            internal IntPtr hbmpUnchecked = IntPtr.Zero;
            internal IntPtr dwItemData = IntPtr.Zero;
            internal IntPtr dwTypeData = IntPtr.Zero;
            internal int cch = 0;
            internal IntPtr hbmpItem = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class OPENFILENAME_I
        {
            internal int lStructSize = Marshal.SizeOf(typeof(OPENFILENAME_I));
            internal IntPtr hwndOwner;
            internal IntPtr hInstance;
            internal string lpstrFilter;
            internal IntPtr lpstrCustomFilter = IntPtr.Zero;
            internal int nMaxCustFilter = 0;
            internal int nFilterIndex;
            internal IntPtr lpstrFile;
            internal int nMaxFile = MAX_PATH;
            internal IntPtr lpstrFileTitle = IntPtr.Zero;
            internal int nMaxFileTitle = MAX_PATH;
            internal string lpstrInitialDir;
            internal string lpstrTitle;
            internal int Flags;
            internal short nFileOffset = 0;
            internal short nFileExtension = 0;
            internal string lpstrDefExt;
            internal IntPtr lCustData = IntPtr.Zero;
            internal WndProc lpfnHook;
            internal string lpTemplateName = null;
            internal IntPtr pvReserved = IntPtr.Zero;
            internal int dwReserved = 0;
            internal int FlagsEx;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto), CLSCompliant(false)]
        internal class CHOOSEFONT
        {
            internal int lStructSize = Marshal.SizeOf(typeof(CHOOSEFONT));
            internal IntPtr hwndOwner;
            internal IntPtr hDC;
            internal IntPtr lpLogFont;
            internal int iPointSize = 0;
            internal int Flags;
            internal int rgbColors;
            internal IntPtr lCustData = IntPtr.Zero;
            internal WndProc lpfnHook;
            internal string lpTemplateName = null;
            internal IntPtr hInstance;
            internal string lpszStyle = null;
            internal short nFontType = 0;
            internal short ___MISSING_ALIGNMENT__ = 0;
            internal int nSizeMin;
            internal int nSizeMax;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class BITMAPINFO
        {
            internal int bmiHeader_biSize = 40;
            internal int bmiHeader_biWidth = 0;
            internal int bmiHeader_biHeight = 0;
            internal short bmiHeader_biPlanes = 0;
            internal short bmiHeader_biBitCount = 0;
            internal int bmiHeader_biCompression = 0;
            internal int bmiHeader_biSizeImage = 0;
            internal int bmiHeader_biXPelsPerMeter = 0;
            internal int bmiHeader_biYPelsPerMeter = 0;
            internal int bmiHeader_biClrUsed = 0;
            internal int bmiHeader_biClrImportant = 0;

            internal byte bmiColors_rgbBlue = 0;
            internal byte bmiColors_rgbGreen = 0;
            internal byte bmiColors_rgbRed = 0;
            internal byte bmiColors_rgbReserved = 0;

            private BITMAPINFO()
            {
            }
        }

        internal class Ole
        {
            internal const int PICTYPE_BITMAP = 1;

            internal const int PICTYPE_ICON = 3;
            internal const int PICTYPE_ENHMETAFILE = 4;
            internal const int STATFLAG_DEFAULT = 0;
            internal const int STATFLAG_NONAME = 1;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class COMRECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;

            internal COMRECT()
            {
            }

            internal COMRECT(Rectangle r)
            {
                left = r.X;
                top = r.Y;
                right = r.Right;
                bottom = r.Bottom;
            }

            internal COMRECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /* Unused
            internal RECT ToRECT() {
                return new RECT(left, top, right, bottom);
            }
            */

            internal static COMRECT FromXYWH(int x, int y, int width, int height)
            {
                return new COMRECT(x, y, x + width, y + height);
            }

            public override string ToString()
            {
                return "Left = " + left + " Top " + top + " Right = " + right + " Bottom = " + bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        internal class MSOCRINFOSTRUCT
        {
            internal int cbSize = Marshal.SizeOf(typeof(MSOCRINFOSTRUCT));
            internal int uIdleTimeInterval;

            internal int grfcrf;
            internal int grfcadvf;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class tagCONTROLINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            internal int cb =
                Marshal.SizeOf(typeof(tagCONTROLINFO));

            internal IntPtr hAccel;

            [MarshalAs(UnmanagedType.U2)]
            internal short cAccel;

            [MarshalAs(UnmanagedType.U4)]
            internal int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class VARIANT
        {
            [MarshalAs(UnmanagedType.I2)]
            internal short vt;

            [MarshalAs(UnmanagedType.I2)]
            internal short reserved1 = 0;

            [MarshalAs(UnmanagedType.I2)]
            internal short reserved2 = 0;

            [MarshalAs(UnmanagedType.I2)]
            internal short reserved3 = 0;

            internal IntPtr data1;
            internal IntPtr data2;

            internal bool Byref
            {
                get { return 0 != (vt & (int)tagVT.VT_BYREF); }
            }

            internal void Clear()
            {
                if ((vt == (int)tagVT.VT_UNKNOWN || vt == (int)tagVT.VT_DISPATCH) && data1 != IntPtr.Zero)
                {
                    Marshal.Release(data1);
                }
                if (vt == (int)tagVT.VT_BSTR && data1 != IntPtr.Zero)
                {
                    SysFreeString(data1);
                }
                data1 = data2 = IntPtr.Zero;
                vt = (int)tagVT.VT_EMPTY;
            }

            ~VARIANT()
            {
                Clear();
            }

            [ResourceExposure(ResourceScope.None)]
            internal static extern void SysFreeString(IntPtr pbstr);

            internal void SetLong(long lVal)
            {
                data1 = (IntPtr)(lVal & 0xFFFFFFFF);
                data2 = (IntPtr)((lVal >> 32) & 0xFFFFFFFF);
            }

            internal IntPtr ToCoTaskMemPtr()
            {
                IntPtr mem = Marshal.AllocCoTaskMem(16);
                Marshal.WriteInt16(mem, vt);
                Marshal.WriteInt16(mem, 2, reserved1);
                Marshal.WriteInt16(mem, 4, reserved2);
                Marshal.WriteInt16(mem, 6, reserved3);
                Marshal.WriteInt32(mem, 8, (int)data1);
                Marshal.WriteInt32(mem, 12, (int)data2);
                return mem;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class tagLICINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            internal int cbLicInfo =
                Marshal.SizeOf(typeof(tagLICINFO));

            internal int fRuntimeAvailable = 0;
            internal int fLicVerified = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class TOOLINFO_T
        {
            internal int cbSize = Marshal.SizeOf(typeof(TOOLINFO_T));
            internal int uFlags;
            internal IntPtr hwnd;
            internal IntPtr uId;
            internal RECT rect;
            internal IntPtr hinst = IntPtr.Zero;
            internal string lpszText;
            internal IntPtr lParam = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class TOOLINFO_TOOLTIP
        {
            internal int cbSize = Marshal.SizeOf(typeof(TOOLINFO_TOOLTIP));
            internal int uFlags;
            internal IntPtr hwnd;
            internal IntPtr uId;
            internal RECT rect;
            internal IntPtr hinst = IntPtr.Zero;
            internal IntPtr lpszText;
            internal IntPtr lParam = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class HELPINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(HELPINFO));
            internal int iContextType = 0;
            internal int iCtrlId = 0;
            internal IntPtr hItemHandle = IntPtr.Zero;
            internal IntPtr dwContextId = IntPtr.Zero;
            internal POINT MousePos = null;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MCHITTESTINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(MCHITTESTINFO));
            internal int pt_x = 0;
            internal int pt_y = 0;
            internal int uHit = 0;
            internal short st_wYear = 0;
            internal short st_wMonth = 0;
            internal short st_wDayOfWeek = 0;
            internal short st_wDay = 0;
            internal short st_wHour = 0;
            internal short st_wMinute = 0;
            internal short st_wSecond = 0;
            internal short st_wMilliseconds = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class LVGROUP
        {
            internal uint cbSize = (uint)Marshal.SizeOf(typeof(LVGROUP));
            internal uint mask;
            internal IntPtr pszHeader;
            internal int cchHeader;
            internal IntPtr pszFooter = IntPtr.Zero;
            internal int cchFooter = 0;
            internal int iGroupId;
            internal uint stateMask = 0;
            internal uint state = 0;
            internal uint uAlign;

            public override string ToString()
            {
                return "LVGROUP: header = " + pszHeader.ToString() + ", iGroupId = " +
                       iGroupId.ToString(CultureInfo.InvariantCulture);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class LVINSERTMARK
        {
            internal uint cbSize = (uint)Marshal.SizeOf(typeof(LVINSERTMARK));
            internal int dwFlags;
            internal int iItem;
            internal int dwReserved = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class LVTILEVIEWINFO
        {
            internal uint cbSize = (uint)Marshal.SizeOf(typeof(LVTILEVIEWINFO));
            internal int dwMask;
            internal int dwFlags;
            internal SIZE sizeTile;
            internal int cLines;
            internal RECT rcLabelMargin;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class CLIENTCREATESTRUCT
        {
            internal IntPtr hWindowMenu;
            internal int idFirstChild;

            internal CLIENTCREATESTRUCT(IntPtr hmenu, int idFirst)
            {
                hWindowMenu = hmenu;
                idFirstChild = idFirst;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal class CHARFORMATW
        {
            internal int cbSize = Marshal.SizeOf(typeof(CHARFORMATW));
            internal int dwMask;
            internal int dwEffects;
            internal int yHeight;
            internal int yOffset = 0;
            internal int crTextColor = 0;
            internal byte bCharSet;
            internal byte bPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[] szFaceName = new byte[64];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal class CHARFORMATA
        {
            internal int cbSize = Marshal.SizeOf(typeof(CHARFORMATA));
            internal int dwMask;
            internal int dwEffects;
            internal int yHeight;
            internal int yOffset;
            internal int crTextColor;
            internal byte bCharSet;
            internal byte bPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal byte[] szFaceName = new byte[32];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal class CHARFORMAT2A
        {
            internal int cbSize = Marshal.SizeOf(typeof(CHARFORMAT2A));
            internal int dwMask = 0;
            internal int dwEffects = 0;
            internal int yHeight = 0;
            internal int yOffset = 0;
            internal int crTextColor = 0;
            internal byte bCharSet = 0;
            internal byte bPitchAndFamily = 0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal byte[] szFaceName = new byte[32];

            internal short wWeight = 0;
            internal short sSpacing = 0;
            internal int crBackColor = 0;
            internal int lcid = 0;
            internal int dwReserved = 0;
            internal short sStyle = 0;
            internal short wKerning = 0;
            internal byte bUnderlineType = 0;
            internal byte bAnimation = 0;
            internal byte bRevAuthor = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PARAFORMAT
        {
            internal int cbSize = Marshal.SizeOf(typeof(PARAFORMAT));
            internal int dwMask;
            internal short wNumbering;
            internal short wReserved = 0;
            internal int dxStartIndent;
            internal int dxRightIndent;
            internal int dxOffset;
            internal short wAlignment;
            internal short cTabCount;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal int[] rgxTabs;
        }

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        internal class DOCHOSTUIINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            internal int cbSize = Marshal.SizeOf(typeof(DOCHOSTUIINFO));

            [MarshalAs(UnmanagedType.I4)]
            internal int dwFlags;

            [MarshalAs(UnmanagedType.I4)]
            internal int dwDoubleClick;

            [MarshalAs(UnmanagedType.I4)]
            internal int dwReserved1 = 0;

            [MarshalAs(UnmanagedType.I4)]
            internal int dwReserved2 = 0;
        }

        internal class ActiveX
        {
            internal const int

                DISPID_UNKNOWN = unchecked((int)0xFFFFFFFF),

                DISPID_READYSTATE = unchecked((int)0xFFFFFDF3),

                DVASPECT_CONTENT = 0x1,

                OLEMISC_RECOMPOSEONRESIZE = 0x1,

                OLEMISC_INSIDEOUT = 0x80,
                OLEMISC_ACTIVATEWHENVISIBLE = 0x100,

                OLEMISC_ACTSLIKEBUTTON = 0x1000,

                OLEMISC_SETCLIENTSITEFIRST = 0x20000;

            internal static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

            private ActiveX()
            {
            }
        }

        internal sealed class CommonHandles
        {
            internal static readonly int Accelerator = HandleCollector.RegisterType("Accelerator", 80, 50);
            internal static readonly int Cursor = HandleCollector.RegisterType("Cursor", 20, 500);
            internal static readonly int EMF = HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);
            internal static readonly int Find = HandleCollector.RegisterType("Find", 0, 1000);
            internal static readonly int GDI = HandleCollector.RegisterType("GDI", 50, 500);
            internal static readonly int HDC = HandleCollector.RegisterType("HDC", 100, 2);
            internal static readonly int CompatibleHDC = HandleCollector.RegisterType("ComptibleHDC", 50, 50);
            internal static readonly int Icon = HandleCollector.RegisterType("Icon", 20, 500);
            internal static readonly int Kernel = HandleCollector.RegisterType("Kernel", 0, 1000);
            internal static readonly int Menu = HandleCollector.RegisterType("Menu", 30, 1000);
            internal static readonly int Window = HandleCollector.RegisterType("Window", 5, 1000);
        }
    }
}