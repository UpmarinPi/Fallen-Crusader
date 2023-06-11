namespace UnityEngine
{
	//
	// äTóv:
	//     Key codes returned by Event.keyCode. These map directly to a physical key on
	//     the keyboard.
	public class KeyCode2Class
	{
		//keycodeÇ∆keycode2ÇÇªÇÍÇºÇÍÇ…ïœä∑Ç∑ÇÈ
		public static KeyCode TransformCode(KeyCode2 _keyCode)
		{
			return (KeyCode)System.Enum.ToObject(typeof(KeyCode), (int)_keyCode);
		}

		public static KeyCode2 TransformCode(KeyCode _keyCode)
		{
			return (KeyCode2) System.Enum.ToObject(typeof(KeyCode2), (int) _keyCode);
		}
	}
	public enum KeyCode2
	{
		//
		// äTóv:
		//     Not assigned (never returned as the result of a keystroke).
		None = 0,
		//
		// äTóv:
		//     The backspace key.
		Backspace = 8,
		//
		// äTóv:
		//     The tab key.
		Tab = 9,
		//
		// äTóv:
		//     The Clear key.
		Clear = 12,
		//
		// äTóv:
		//     Return key.
		Return = 13,
		//
		// äTóv:
		//     Pause on PC machines.
		Pause = 19,
		//
		// äTóv:
		//     Escape key.
		Escape = 27,
		//
		// äTóv:
		//     Space key.
		Space = 32,
		//
		// äTóv:
		//     Exclamation mark key '!'.
		Exclaim = 33,
		//
		// äTóv:
		//     Double quote key '"'.
		DoubleQuote = 34,
		//
		// äTóv:
		//     Hash key '#'.
		Hash = 35,
		//
		// äTóv:
		//     Dollar sign key '$'.
		Dollar = 36,
		//
		// äTóv:
		//     Percent '%' key.
		Percent = 37,
		//
		// äTóv:
		//     Ampersand key '&'.
		Ampersand = 38,
		//
		// äTóv:
		//     Quote key '.
		Quote = 39,
		//
		// äTóv:
		//     Left Parenthesis key '('.
		LeftParen = 40,
		//
		// äTóv:
		//     Right Parenthesis key ')'.
		RightParen = 41,
		//
		// äTóv:
		//     Asterisk key '*'.
		Asterisk = 42,
		//
		// äTóv:
		//     Plus key '+'.
		Plus = 43,
		//
		// äTóv:
		//     Comma ',' key.
		Comma = 44,
		//
		// äTóv:
		//     Minus '-' key.
		Minus = 45,
		//
		// äTóv:
		//     Period '.' key.
		Period = 46,
		//
		// äTóv:
		//     Slash '/' key.
		Slash = 47,
		//
		// äTóv:
		//     The '0' key on the top of the alphanumeric keyboard.
		Alpha0 = 48,
		//
		// äTóv:
		//     The '1' key on the top of the alphanumeric keyboard.
		Alpha1 = 49,
		//
		// äTóv:
		//     The '2' key on the top of the alphanumeric keyboard.
		Alpha2 = 50,
		//
		// äTóv:
		//     The '3' key on the top of the alphanumeric keyboard.
		Alpha3 = 51,
		//
		// äTóv:
		//     The '4' key on the top of the alphanumeric keyboard.
		Alpha4 = 52,
		//
		// äTóv:
		//     The '5' key on the top of the alphanumeric keyboard.
		Alpha5 = 53,
		//
		// äTóv:
		//     The '6' key on the top of the alphanumeric keyboard.
		Alpha6 = 54,
		//
		// äTóv:
		//     The '7' key on the top of the alphanumeric keyboard.
		Alpha7 = 55,
		//
		// äTóv:
		//     The '8' key on the top of the alphanumeric keyboard.
		Alpha8 = 56,
		//
		// äTóv:
		//     The '9' key on the top of the alphanumeric keyboard.
		Alpha9 = 57,
		//
		// äTóv:
		//     Colon ':' key.
		Colon = 58,
		//
		// äTóv:
		//     Semicolon ';' key.
		Semicolon = 59,
		//
		// äTóv:
		//     Less than '<' key.
		Less = 60,
		//
		// äTóv:
		//     Equals '=' key.
		Equals = 61,
		//
		// äTóv:
		//     Greater than '>' key.
		Greater = 62,
		//
		// äTóv:
		//     Question mark '?' key.
		Question = 63,
		//
		// äTóv:
		//     At key '@'.
		At = 64,
		//
		// äTóv:
		//     Left square bracket key '['.
		LeftBracket = 91,
		//
		// äTóv:
		//     Backslash key '\'.
		Backslash = 92,
		//
		// äTóv:
		//     Right square bracket key ']'.
		RightBracket = 93,
		//
		// äTóv:
		//     Caret key '^'.
		Caret = 94,
		//
		// äTóv:
		//     Underscore '_' key.
		Underscore = 95,
		//
		// äTóv:
		//     Back quote key '`'.
		BackQuote = 96,
		//
		// äTóv:
		//     'a' key.
		A = 97,
		//
		// äTóv:
		//     'b' key.
		B = 98,
		//
		// äTóv:
		//     'c' key.
		C = 99,
		//
		// äTóv:
		//     'd' key.
		D = 100,
		//
		// äTóv:
		//     'e' key.
		E = 101,
		//
		// äTóv:
		//     'f' key.
		F = 102,
		//
		// äTóv:
		//     'g' key.
		G = 103,
		//
		// äTóv:
		//     'h' key.
		H = 104,
		//
		// äTóv:
		//     'i' key.
		I = 105,
		//
		// äTóv:
		//     'j' key.
		J = 106,
		//
		// äTóv:
		//     'k' key.
		K = 107,
		//
		// äTóv:
		//     'l' key.
		L = 108,
		//
		// äTóv:
		//     'm' key.
		M = 109,
		//
		// äTóv:
		//     'n' key.
		N = 110,
		//
		// äTóv:
		//     'o' key.
		O = 111,
		//
		// äTóv:
		//     'p' key.
		P = 112,
		//
		// äTóv:
		//     'q' key.
		Q = 113,
		//
		// äTóv:
		//     'r' key.
		R = 114,
		//
		// äTóv:
		//     's' key.
		S = 115,
		//
		// äTóv:
		//     't' key.
		T = 116,
		//
		// äTóv:
		//     'u' key.
		U = 117,
		//
		// äTóv:
		//     'v' key.
		V = 118,
		//
		// äTóv:
		//     'w' key.
		W = 119,
		//
		// äTóv:
		//     'x' key.
		X = 120,
		//
		// äTóv:
		//     'y' key.
		Y = 121,
		//
		// äTóv:
		//     'z' key.
		Z = 122,
		//
		// äTóv:
		//     Left curly bracket key '{'.
		LeftCurlyBracket = 123,
		//
		// äTóv:
		//     Pipe '|' key.
		Pipe = 124,
		//
		// äTóv:
		//     Right curly bracket key '}'.
		RightCurlyBracket = 125,
		//
		// äTóv:
		//     Tilde '~' key.
		Tilde = 126,
		//
		// äTóv:
		//     The forward delete key.
		Delete = 127,
		//
		// äTóv:
		//     Numeric keypad 0.
		Keypad0 = 256,
		//
		// äTóv:
		//     Numeric keypad 1.
		Keypad1 = 257,
		//
		// äTóv:
		//     Numeric keypad 2.
		Keypad2 = 258,
		//
		// äTóv:
		//     Numeric keypad 3.
		Keypad3 = 259,
		//
		// äTóv:
		//     Numeric keypad 4.
		Keypad4 = 260,
		//
		// äTóv:
		//     Numeric keypad 5.
		Keypad5 = 261,
		//
		// äTóv:
		//     Numeric keypad 6.
		Keypad6 = 262,
		//
		// äTóv:
		//     Numeric keypad 7.
		Keypad7 = 263,
		//
		// äTóv:
		//     Numeric keypad 8.
		Keypad8 = 264,
		//
		// äTóv:
		//     Numeric keypad 9.
		Keypad9 = 265,
		//
		// äTóv:
		//     Numeric keypad '.'.
		KeypadPeriod = 266,
		//
		// äTóv:
		//     Numeric keypad '/'.
		KeypadDivide = 267,
		//
		// äTóv:
		//     Numeric keypad '*'.
		KeypadMultiply = 268,
		//
		// äTóv:
		//     Numeric keypad '-'.
		KeypadMinus = 269,
		//
		// äTóv:
		//     Numeric keypad '+'.
		KeypadPlus = 270,
		//
		// äTóv:
		//     Numeric keypad Enter.
		KeypadEnter = 271,
		//
		// äTóv:
		//     Numeric keypad '='.
		KeypadEquals = 272,
		//
		// äTóv:
		//     Up arrow key.
		UpArrow = 273,
		//
		// äTóv:
		//     Down arrow key.
		DownArrow = 274,
		//
		// äTóv:
		//     Right arrow key.
		RightArrow = 275,
		//
		// äTóv:
		//     Left arrow key.
		LeftArrow = 276,
		//
		// äTóv:
		//     Insert key key.
		Insert = 277,
		//
		// äTóv:
		//     Home key.
		Home = 278,
		//
		// äTóv:
		//     End key.
		End = 279,
		//
		// äTóv:
		//     Page up.
		PageUp = 280,
		//
		// äTóv:
		//     Page down.
		PageDown = 281,
		//
		// äTóv:
		//     F1 function key.
		F1 = 282,
		//
		// äTóv:
		//     F2 function key.
		F2 = 283,
		//
		// äTóv:
		//     F3 function key.
		F3 = 284,
		//
		// äTóv:
		//     F4 function key.
		F4 = 285,
		//
		// äTóv:
		//     F5 function key.
		F5 = 286,
		//
		// äTóv:
		//     F6 function key.
		F6 = 287,
		//
		// äTóv:
		//     F7 function key.
		F7 = 288,
		//
		// äTóv:
		//     F8 function key.
		F8 = 289,
		//
		// äTóv:
		//     F9 function key.
		F9 = 290,
		//
		// äTóv:
		//     F10 function key.
		F10 = 291,
		//
		// äTóv:
		//     F11 function key.
		F11 = 292,
		//
		// äTóv:
		//     F12 function key.
		F12 = 293,
		//
		// äTóv:
		//     F13 function key.
		F13 = 294,
		//
		// äTóv:
		//     F14 function key.
		F14 = 295,
		//
		// äTóv:
		//     F15 function key.
		F15 = 296,
		//
		// äTóv:
		//     Numlock key.
		Numlock = 300,
		//
		// äTóv:
		//     Capslock key.
		CapsLock = 301,
		//
		// äTóv:
		//     Scroll lock key.
		ScrollLock = 302,
		//
		// äTóv:
		//     Right shift key.
		RightShift = 303,
		//
		// äTóv:
		//     Left shift key.
		LeftShift = 304,
		//
		// äTóv:
		//     Right Control key.
		RightControl = 305,
		//
		// äTóv:
		//     Left Control key.
		LeftControl = 306,
		//
		// äTóv:
		//     Right Alt key.
		RightAlt = 307,
		//
		// äTóv:
		//     Left Alt key.
		LeftAlt = 308,
		//
		// äTóv:
		//     Right Command key.
		RightCommand = 309,
		//
		// äTóv:
		//     Right Command key.
		RightApple = 309,
		//
		// äTóv:
		//     Left Command key.
		LeftCommand = 310,
		//
		// äTóv:
		//     Left Command key.
		LeftApple = 310,
		//
		// äTóv:
		//     Left Windows key.
		LeftWindows = 311,
		//
		// äTóv:
		//     Right Windows key.
		RightWindows = 312,
		//
		// äTóv:
		//     Alt Gr key.
		AltGr = 313,
		//
		// äTóv:
		//     Help key.
		Help = 315,
		//
		// äTóv:
		//     Print key.
		Print = 316,
		//
		// äTóv:
		//     Sys Req key.
		SysReq = 317,
		//
		// äTóv:
		//     Break key.
		Break = 318,
		//
		// äTóv:
		//     Menu key.
		Menu = 319,
		//
		// äTóv:
		//     The Left (or primary) mouse button.
		Mouse0 = 323,
		//
		// äTóv:
		//     Right mouse button (or secondary mouse button).
		Mouse1 = 324,
		//
		// äTóv:
		//     Middle mouse button (or third button).
		Mouse2 = 325,
		//
		// äTóv:
		//     Additional (fourth) mouse button.
		Mouse3 = 326,
		//
		// äTóv:
		//     Additional (fifth) mouse button.
		Mouse4 = 327,
		//
		// äTóv:
		//     Additional (or sixth) mouse button.
		Mouse5 = 328,
		//
		// äTóv:
		//     Additional (or seventh) mouse button.
		Mouse6 = 329,
		//
		// äTóv:
		//     Button 0 on any joystick.
		AÉ{É^Éì = 330,
		//
		// äTóv:
		//     Button 1 on any joystick.
		BÉ{É^Éì = 331,
		//
		// äTóv:
		//     Button 2 on any joystick.
		XÉ{É^Éì = 332,
		//
		// äTóv:
		//     Button 3 on any joystick.
		YÉ{É^Éì = 333,
		//
		// äTóv:
		//     Button 4 on any joystick.
		L1 = 334,
		//
		// äTóv:
		//     Button 5 on any joystick.
		R1 = 335,
		//
		// äTóv:
		//     Button 6 on any joystick.
		BackÉ{É^Éì = 336,
		//
		// äTóv:
		//     Button 7 on any joystick.
		StartÉ{É^Éì  = 337,
		//
		// äTóv:
		//     Button 8 on any joystick.
		L3 = 338,
		//
		// äTóv:
		//     Button 9 on any joystick.
		R3 = 339,
		//
		// äTóv:
		//     Button 10 on any joystick.
		JoystickButton10 = 340,
		//
		// äTóv:
		//     Button 11 on any joystick.
		JoystickButton11 = 341,
		//
		// äTóv:
		//     Button 12 on any joystick.
		LÉXÉeÉBÉbÉNè„ = 342,
		//
		// äTóv:
		//     Button 13 on any joystick.
		LÉXÉeÉBÉbÉNâ∫ = 343,
		//
		// äTóv:
		//     Button 14 on any joystick.
		LÉXÉeÉBÉbÉN = 344,
		//
		// äTóv:
		//     Button 15 on any joystick.
		RÉXÉeÉBÉbÉN = 345,
		//
		// äTóv:
		//     Button 16 on any joystick.
		è\éöÉLÅ[ = 346,
		//
		// äTóv:
		//     Button 17 on any joystick.
		L2 = 347,
		//
		// äTóv:
		//     Button 18 on any joystick.
		R2 = 348,
		//
		// äTóv:
		//     Button 19 on any joystick.
		JoystickButton19 = 349,
		//
		// äTóv:
		//     Button 0 on first joystick.
		Joystick1Button0 = 350,
		//
		// äTóv:
		//     Button 1 on first joystick.
		Joystick1Button1 = 351,
		//
		// äTóv:
		//     Button 2 on first joystick.
		Joystick1Button2 = 352,
		//
		// äTóv:
		//     Button 3 on first joystick.
		Joystick1Button3 = 353,
		//
		// äTóv:
		//     Button 4 on first joystick.
		Joystick1Button4 = 354,
		//
		// äTóv:
		//     Button 5 on first joystick.
		Joystick1Button5 = 355,
		//
		// äTóv:
		//     Button 6 on first joystick.
		Joystick1Button6 = 356,
		//
		// äTóv:
		//     Button 7 on first joystick.
		Joystick1Button7 = 357,
		//
		// äTóv:
		//     Button 8 on first joystick.
		Joystick1Button8 = 358,
		//
		// äTóv:
		//     Button 9 on first joystick.
		Joystick1Button9 = 359,
		//
		// äTóv:
		//     Button 10 on first joystick.
		Joystick1Button10 = 360,
		//
		// äTóv:
		//     Button 11 on first joystick.
		Joystick1Button11 = 361,
		//
		// äTóv:
		//     Button 12 on first joystick.
		Joystick1Button12 = 362,
		//
		// äTóv:
		//     Button 13 on first joystick.
		Joystick1Button13 = 363,
		//
		// äTóv:
		//     Button 14 on first joystick.
		Joystick1Button14 = 364,
		//
		// äTóv:
		//     Button 15 on first joystick.
		Joystick1Button15 = 365,
		//
		// äTóv:
		//     Button 16 on first joystick.
		Joystick1Button16 = 366,
		//
		// äTóv:
		//     Button 17 on first joystick.
		Joystick1Button17 = 367,
		//
		// äTóv:
		//     Button 18 on first joystick.
		Joystick1Button18 = 368,
		//
		// äTóv:
		//     Button 19 on first joystick.
		Joystick1Button19 = 369,
		//
		// äTóv:
		//     Button 0 on second joystick.
		Joystick2Button0 = 370,
		//
		// äTóv:
		//     Button 1 on second joystick.
		Joystick2Button1 = 371,
		//
		// äTóv:
		//     Button 2 on second joystick.
		Joystick2Button2 = 372,
		//
		// äTóv:
		//     Button 3 on second joystick.
		Joystick2Button3 = 373,
		//
		// äTóv:
		//     Button 4 on second joystick.
		Joystick2Button4 = 374,
		//
		// äTóv:
		//     Button 5 on second joystick.
		Joystick2Button5 = 375,
		//
		// äTóv:
		//     Button 6 on second joystick.
		Joystick2Button6 = 376,
		//
		// äTóv:
		//     Button 7 on second joystick.
		Joystick2Button7 = 377,
		//
		// äTóv:
		//     Button 8 on second joystick.
		Joystick2Button8 = 378,
		//
		// äTóv:
		//     Button 9 on second joystick.
		Joystick2Button9 = 379,
		//
		// äTóv:
		//     Button 10 on second joystick.
		Joystick2Button10 = 380,
		//
		// äTóv:
		//     Button 11 on second joystick.
		Joystick2Button11 = 381,
		//
		// äTóv:
		//     Button 12 on second joystick.
		Joystick2Button12 = 382,
		//
		// äTóv:
		//     Button 13 on second joystick.
		Joystick2Button13 = 383,
		//
		// äTóv:
		//     Button 14 on second joystick.
		Joystick2Button14 = 384,
		//
		// äTóv:
		//     Button 15 on second joystick.
		Joystick2Button15 = 385,
		//
		// äTóv:
		//     Button 16 on second joystick.
		Joystick2Button16 = 386,
		//
		// äTóv:
		//     Button 17 on second joystick.
		Joystick2Button17 = 387,
		//
		// äTóv:
		//     Button 18 on second joystick.
		Joystick2Button18 = 388,
		//
		// äTóv:
		//     Button 19 on second joystick.
		Joystick2Button19 = 389,
		//
		// äTóv:
		//     Button 0 on third joystick.
		Joystick3Button0 = 390,
		//
		// äTóv:
		//     Button 1 on third joystick.
		Joystick3Button1 = 391,
		//
		// äTóv:
		//     Button 2 on third joystick.
		Joystick3Button2 = 392,
		//
		// äTóv:
		//     Button 3 on third joystick.
		Joystick3Button3 = 393,
		//
		// äTóv:
		//     Button 4 on third joystick.
		Joystick3Button4 = 394,
		//
		// äTóv:
		//     Button 5 on third joystick.
		Joystick3Button5 = 395,
		//
		// äTóv:
		//     Button 6 on third joystick.
		Joystick3Button6 = 396,
		//
		// äTóv:
		//     Button 7 on third joystick.
		Joystick3Button7 = 397,
		//
		// äTóv:
		//     Button 8 on third joystick.
		Joystick3Button8 = 398,
		//
		// äTóv:
		//     Button 9 on third joystick.
		Joystick3Button9 = 399,
		//
		// äTóv:
		//     Button 10 on third joystick.
		Joystick3Button10 = 400,
		//
		// äTóv:
		//     Button 11 on third joystick.
		Joystick3Button11 = 401,
		//
		// äTóv:
		//     Button 12 on third joystick.
		Joystick3Button12 = 402,
		//
		// äTóv:
		//     Button 13 on third joystick.
		Joystick3Button13 = 403,
		//
		// äTóv:
		//     Button 14 on third joystick.
		Joystick3Button14 = 404,
		//
		// äTóv:
		//     Button 15 on third joystick.
		Joystick3Button15 = 405,
		//
		// äTóv:
		//     Button 16 on third joystick.
		Joystick3Button16 = 406,
		//
		// äTóv:
		//     Button 17 on third joystick.
		Joystick3Button17 = 407,
		//
		// äTóv:
		//     Button 18 on third joystick.
		Joystick3Button18 = 408,
		//
		// äTóv:
		//     Button 19 on third joystick.
		Joystick3Button19 = 409,
		//
		// äTóv:
		//     Button 0 on forth joystick.
		Joystick4Button0 = 410,
		//
		// äTóv:
		//     Button 1 on forth joystick.
		Joystick4Button1 = 411,
		//
		// äTóv:
		//     Button 2 on forth joystick.
		Joystick4Button2 = 412,
		//
		// äTóv:
		//     Button 3 on forth joystick.
		Joystick4Button3 = 413,
		//
		// äTóv:
		//     Button 4 on forth joystick.
		Joystick4Button4 = 414,
		//
		// äTóv:
		//     Button 5 on forth joystick.
		Joystick4Button5 = 415,
		//
		// äTóv:
		//     Button 6 on forth joystick.
		Joystick4Button6 = 416,
		//
		// äTóv:
		//     Button 7 on forth joystick.
		Joystick4Button7 = 417,
		//
		// äTóv:
		//     Button 8 on forth joystick.
		Joystick4Button8 = 418,
		//
		// äTóv:
		//     Button 9 on forth joystick.
		Joystick4Button9 = 419,
		//
		// äTóv:
		//     Button 10 on forth joystick.
		Joystick4Button10 = 420,
		//
		// äTóv:
		//     Button 11 on forth joystick.
		Joystick4Button11 = 421,
		//
		// äTóv:
		//     Button 12 on forth joystick.
		Joystick4Button12 = 422,
		//
		// äTóv:
		//     Button 13 on forth joystick.
		Joystick4Button13 = 423,
		//
		// äTóv:
		//     Button 14 on forth joystick.
		Joystick4Button14 = 424,
		//
		// äTóv:
		//     Button 15 on forth joystick.
		Joystick4Button15 = 425,
		//
		// äTóv:
		//     Button 16 on forth joystick.
		Joystick4Button16 = 426,
		//
		// äTóv:
		//     Button 17 on forth joystick.
		Joystick4Button17 = 427,
		//
		// äTóv:
		//     Button 18 on forth joystick.
		Joystick4Button18 = 428,
		//
		// äTóv:
		//     Button 19 on forth joystick.
		Joystick4Button19 = 429,
		//
		// äTóv:
		//     Button 0 on fifth joystick.
		Joystick5Button0 = 430,
		//
		// äTóv:
		//     Button 1 on fifth joystick.
		Joystick5Button1 = 431,
		//
		// äTóv:
		//     Button 2 on fifth joystick.
		Joystick5Button2 = 432,
		//
		// äTóv:
		//     Button 3 on fifth joystick.
		Joystick5Button3 = 433,
		//
		// äTóv:
		//     Button 4 on fifth joystick.
		Joystick5Button4 = 434,
		//
		// äTóv:
		//     Button 5 on fifth joystick.
		Joystick5Button5 = 435,
		//
		// äTóv:
		//     Button 6 on fifth joystick.
		Joystick5Button6 = 436,
		//
		// äTóv:
		//     Button 7 on fifth joystick.
		Joystick5Button7 = 437,
		//
		// äTóv:
		//     Button 8 on fifth joystick.
		Joystick5Button8 = 438,
		//
		// äTóv:
		//     Button 9 on fifth joystick.
		Joystick5Button9 = 439,
		//
		// äTóv:
		//     Button 10 on fifth joystick.
		Joystick5Button10 = 440,
		//
		// äTóv:
		//     Button 11 on fifth joystick.
		Joystick5Button11 = 441,
		//
		// äTóv:
		//     Button 12 on fifth joystick.
		Joystick5Button12 = 442,
		//
		// äTóv:
		//     Button 13 on fifth joystick.
		Joystick5Button13 = 443,
		//
		// äTóv:
		//     Button 14 on fifth joystick.
		Joystick5Button14 = 444,
		//
		// äTóv:
		//     Button 15 on fifth joystick.
		Joystick5Button15 = 445,
		//
		// äTóv:
		//     Button 16 on fifth joystick.
		Joystick5Button16 = 446,
		//
		// äTóv:
		//     Button 17 on fifth joystick.
		Joystick5Button17 = 447,
		//
		// äTóv:
		//     Button 18 on fifth joystick.
		Joystick5Button18 = 448,
		//
		// äTóv:
		//     Button 19 on fifth joystick.
		Joystick5Button19 = 449,
		//
		// äTóv:
		//     Button 0 on sixth joystick.
		Joystick6Button0 = 450,
		//
		// äTóv:
		//     Button 1 on sixth joystick.
		Joystick6Button1 = 451,
		//
		// äTóv:
		//     Button 2 on sixth joystick.
		Joystick6Button2 = 452,
		//
		// äTóv:
		//     Button 3 on sixth joystick.
		Joystick6Button3 = 453,
		//
		// äTóv:
		//     Button 4 on sixth joystick.
		Joystick6Button4 = 454,
		//
		// äTóv:
		//     Button 5 on sixth joystick.
		Joystick6Button5 = 455,
		//
		// äTóv:
		//     Button 6 on sixth joystick.
		Joystick6Button6 = 456,
		//
		// äTóv:
		//     Button 7 on sixth joystick.
		Joystick6Button7 = 457,
		//
		// äTóv:
		//     Button 8 on sixth joystick.
		Joystick6Button8 = 458,
		//
		// äTóv:
		//     Button 9 on sixth joystick.
		Joystick6Button9 = 459,
		//
		// äTóv:
		//     Button 10 on sixth joystick.
		Joystick6Button10 = 460,
		//
		// äTóv:
		//     Button 11 on sixth joystick.
		Joystick6Button11 = 461,
		//
		// äTóv:
		//     Button 12 on sixth joystick.
		Joystick6Button12 = 462,
		//
		// äTóv:
		//     Button 13 on sixth joystick.
		Joystick6Button13 = 463,
		//
		// äTóv:
		//     Button 14 on sixth joystick.
		Joystick6Button14 = 464,
		//
		// äTóv:
		//     Button 15 on sixth joystick.
		Joystick6Button15 = 465,
		//
		// äTóv:
		//     Button 16 on sixth joystick.
		Joystick6Button16 = 466,
		//
		// äTóv:
		//     Button 17 on sixth joystick.
		Joystick6Button17 = 467,
		//
		// äTóv:
		//     Button 18 on sixth joystick.
		Joystick6Button18 = 468,
		//
		// äTóv:
		//     Button 19 on sixth joystick.
		Joystick6Button19 = 469,
		//
		// äTóv:
		//     Button 0 on seventh joystick.
		Joystick7Button0 = 470,
		//
		// äTóv:
		//     Button 1 on seventh joystick.
		Joystick7Button1 = 471,
		//
		// äTóv:
		//     Button 2 on seventh joystick.
		Joystick7Button2 = 472,
		//
		// äTóv:
		//     Button 3 on seventh joystick.
		Joystick7Button3 = 473,
		//
		// äTóv:
		//     Button 4 on seventh joystick.
		Joystick7Button4 = 474,
		//
		// äTóv:
		//     Button 5 on seventh joystick.
		Joystick7Button5 = 475,
		//
		// äTóv:
		//     Button 6 on seventh joystick.
		Joystick7Button6 = 476,
		//
		// äTóv:
		//     Button 7 on seventh joystick.
		Joystick7Button7 = 477,
		//
		// äTóv:
		//     Button 8 on seventh joystick.
		Joystick7Button8 = 478,
		//
		// äTóv:
		//     Button 9 on seventh joystick.
		Joystick7Button9 = 479,
		//
		// äTóv:
		//     Button 10 on seventh joystick.
		Joystick7Button10 = 480,
		//
		// äTóv:
		//     Button 11 on seventh joystick.
		Joystick7Button11 = 481,
		//
		// äTóv:
		//     Button 12 on seventh joystick.
		Joystick7Button12 = 482,
		//
		// äTóv:
		//     Button 13 on seventh joystick.
		Joystick7Button13 = 483,
		//
		// äTóv:
		//     Button 14 on seventh joystick.
		Joystick7Button14 = 484,
		//
		// äTóv:
		//     Button 15 on seventh joystick.
		Joystick7Button15 = 485,
		//
		// äTóv:
		//     Button 16 on seventh joystick.
		Joystick7Button16 = 486,
		//
		// äTóv:
		//     Button 17 on seventh joystick.
		Joystick7Button17 = 487,
		//
		// äTóv:
		//     Button 18 on seventh joystick.
		Joystick7Button18 = 488,
		//
		// äTóv:
		//     Button 19 on seventh joystick.
		Joystick7Button19 = 489,
		//
		// äTóv:
		//     Button 0 on eighth joystick.
		Joystick8Button0 = 490,
		//
		// äTóv:
		//     Button 1 on eighth joystick.
		Joystick8Button1 = 491,
		//
		// äTóv:
		//     Button 2 on eighth joystick.
		Joystick8Button2 = 492,
		//
		// äTóv:
		//     Button 3 on eighth joystick.
		Joystick8Button3 = 493,
		//
		// äTóv:
		//     Button 4 on eighth joystick.
		Joystick8Button4 = 494,
		//
		// äTóv:
		//     Button 5 on eighth joystick.
		Joystick8Button5 = 495,
		//
		// äTóv:
		//     Button 6 on eighth joystick.
		Joystick8Button6 = 496,
		//
		// äTóv:
		//     Button 7 on eighth joystick.
		Joystick8Button7 = 497,
		//
		// äTóv:
		//     Button 8 on eighth joystick.
		Joystick8Button8 = 498,
		//
		// äTóv:
		//     Button 9 on eighth joystick.
		Joystick8Button9 = 499,
		//
		// äTóv:
		//     Button 10 on eighth joystick.
		Joystick8Button10 = 500,
		//
		// äTóv:
		//     Button 11 on eighth joystick.
		Joystick8Button11 = 501,
		//
		// äTóv:
		//     Button 12 on eighth joystick.
		Joystick8Button12 = 502,
		//
		// äTóv:
		//     Button 13 on eighth joystick.
		Joystick8Button13 = 503,
		//
		// äTóv:
		//     Button 14 on eighth joystick.
		Joystick8Button14 = 504,
		//
		// äTóv:
		//     Button 15 on eighth joystick.
		Joystick8Button15 = 505,
		//
		// äTóv:
		//     Button 16 on eighth joystick.
		Joystick8Button16 = 506,
		//
		// äTóv:
		//     Button 17 on eighth joystick.
		Joystick8Button17 = 507,
		//
		// äTóv:
		//     Button 18 on eighth joystick.
		Joystick8Button18 = 508,
		//
		// äTóv:
		//     Button 19 on eighth joystick.
		Joystick8Button19 = 509
	}
}
