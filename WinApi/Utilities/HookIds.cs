﻿// ReSharper disable All

namespace WinApi.Utilities;

public enum KeyboardEventId
{
    WM_KEYDOWN = 0x100,
    WM_SYSKEYDOWN = 0x104,
}

public enum MouseEventId
{
    WM_LBUTTONDOWN = 0x0201,
    WM_XBUTTONDOWN = 0x020B,
    WM_RBUTTONDOWN = 0x0204,
    WM_MOUSEHWHEEL = 0x020E,
    WM_MOUSEMOVE = 0x0200,
}

public enum ScreenshotEventId
{
    SRCCOPY = 0x00CC0020,
    SM_XVIRTUALSCREEN = 76,
    SM_YVIRTUALSCREEN = 77,
    SM_CXVIRTUALSCREEN = 78,
    SM_CYVIRTUALSCREEN = 79,
}

public enum HookId
{
    WH_KEYBOARD_LL = 13,
    WH_MOUSE_LL = 14,
}