namespace UnityEngine
{
	//
	// 概要:
	//     Key codes returned by Event.keyCode. These map directly to a physical key on
	//     the keyboard.
	public class KeyCode2Class
	{
		//keycodeとkeycode2をそれぞれに変換する
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
		// 概要:
		//     Not assigned (never returned as the result of a keystroke).
		None = 0,
		//
		// 概要:
		//     The backspace key.
		Backspace = 8,
		//
		// 概要:
		//     The tab key.
		Tab = 9,
		//
		// 概要:
		//     The Clear key.
		Clear = 12,
		//
		// 概要:
		//     Return key.
		Return = 13,
		//
		// 概要:
		//     Pause on PC machines.
		Pause = 19,
		//
		// 概要:
		//     Escape key.
		Escape = 27,
		//
		// 概要:
		//     Space key.
		Space = 32,
		//
		// 概要:
		//     Exclamation mark key '!'.
		Exclaim = 33,
		//
		// 概要:
		//     Double quote key '"'.
		DoubleQuote = 34,
		//
		// 概要:
		//     Hash key '#'.
		Hash = 35,
		//
		// 概要:
		//     Dollar sign key '$'.
		Dollar = 36,
		//
		// 概要:
		//     Percent '%' key.
		Percent = 37,
		//
		// 概要:
		//     Ampersand key '&'.
		Ampersand = 38,
		//
		// 概要:
		//     Quote key '.
		Quote = 39,
		//
		// 概要:
		//     Left Parenthesis key '('.
		LeftParen = 40,
		//
		// 概要:
		//     Right Parenthesis key ')'.
		RightParen = 41,
		//
		// 概要:
		//     Asterisk key '*'.
		Asterisk = 42,
		//
		// 概要:
		//     Plus key '+'.
		Plus = 43,
		//
		// 概要:
		//     Comma ',' key.
		Comma = 44,
		//
		// 概要:
		//     Minus '-' key.
		Minus = 45,
		//
		// 概要:
		//     Period '.' key.
		Period = 46,
		//
		// 概要:
		//     Slash '/' key.
		Slash = 47,
		//
		// 概要:
		//     The '0' key on the top of the alphanumeric keyboard.
		Alpha0 = 48,
		//
		// 概要:
		//     The '1' key on the top of the alphanumeric keyboard.
		Alpha1 = 49,
		//
		// 概要:
		//     The '2' key on the top of the alphanumeric keyboard.
		Alpha2 = 50,
		//
		// 概要:
		//     The '3' key on the top of the alphanumeric keyboard.
		Alpha3 = 51,
		//
		// 概要:
		//     The '4' key on the top of the alphanumeric keyboard.
		Alpha4 = 52,
		//
		// 概要:
		//     The '5' key on the top of the alphanumeric keyboard.
		Alpha5 = 53,
		//
		// 概要:
		//     The '6' key on the top of the alphanumeric keyboard.
		Alpha6 = 54,
		//
		// 概要:
		//     The '7' key on the top of the alphanumeric keyboard.
		Alpha7 = 55,
		//
		// 概要:
		//     The '8' key on the top of the alphanumeric keyboard.
		Alpha8 = 56,
		//
		// 概要:
		//     The '9' key on the top of the alphanumeric keyboard.
		Alpha9 = 57,
		//
		// 概要:
		//     Colon ':' key.
		Colon = 58,
		//
		// 概要:
		//     Semicolon ';' key.
		Semicolon = 59,
		//
		// 概要:
		//     Less than '<' key.
		Less = 60,
		//
		// 概要:
		//     Equals '=' key.
		Equals = 61,
		//
		// 概要:
		//     Greater than '>' key.
		Greater = 62,
		//
		// 概要:
		//     Question mark '?' key.
		Question = 63,
		//
		// 概要:
		//     At key '@'.
		At = 64,
		//
		// 概要:
		//     Left square bracket key '['.
		LeftBracket = 91,
		//
		// 概要:
		//     Backslash key '\'.
		Backslash = 92,
		//
		// 概要:
		//     Right square bracket key ']'.
		RightBracket = 93,
		//
		// 概要:
		//     Caret key '^'.
		Caret = 94,
		//
		// 概要:
		//     Underscore '_' key.
		Underscore = 95,
		//
		// 概要:
		//     Back quote key '`'.
		BackQuote = 96,
		//
		// 概要:
		//     'a' key.
		A = 97,
		//
		// 概要:
		//     'b' key.
		B = 98,
		//
		// 概要:
		//     'c' key.
		C = 99,
		//
		// 概要:
		//     'd' key.
		D = 100,
		//
		// 概要:
		//     'e' key.
		E = 101,
		//
		// 概要:
		//     'f' key.
		F = 102,
		//
		// 概要:
		//     'g' key.
		G = 103,
		//
		// 概要:
		//     'h' key.
		H = 104,
		//
		// 概要:
		//     'i' key.
		I = 105,
		//
		// 概要:
		//     'j' key.
		J = 106,
		//
		// 概要:
		//     'k' key.
		K = 107,
		//
		// 概要:
		//     'l' key.
		L = 108,
		//
		// 概要:
		//     'm' key.
		M = 109,
		//
		// 概要:
		//     'n' key.
		N = 110,
		//
		// 概要:
		//     'o' key.
		O = 111,
		//
		// 概要:
		//     'p' key.
		P = 112,
		//
		// 概要:
		//     'q' key.
		Q = 113,
		//
		// 概要:
		//     'r' key.
		R = 114,
		//
		// 概要:
		//     's' key.
		S = 115,
		//
		// 概要:
		//     't' key.
		T = 116,
		//
		// 概要:
		//     'u' key.
		U = 117,
		//
		// 概要:
		//     'v' key.
		V = 118,
		//
		// 概要:
		//     'w' key.
		W = 119,
		//
		// 概要:
		//     'x' key.
		X = 120,
		//
		// 概要:
		//     'y' key.
		Y = 121,
		//
		// 概要:
		//     'z' key.
		Z = 122,
		//
		// 概要:
		//     Left curly bracket key '{'.
		LeftCurlyBracket = 123,
		//
		// 概要:
		//     Pipe '|' key.
		Pipe = 124,
		//
		// 概要:
		//     Right curly bracket key '}'.
		RightCurlyBracket = 125,
		//
		// 概要:
		//     Tilde '~' key.
		Tilde = 126,
		//
		// 概要:
		//     The forward delete key.
		Delete = 127,
		//
		// 概要:
		//     Numeric keypad 0.
		Keypad0 = 256,
		//
		// 概要:
		//     Numeric keypad 1.
		Keypad1 = 257,
		//
		// 概要:
		//     Numeric keypad 2.
		Keypad2 = 258,
		//
		// 概要:
		//     Numeric keypad 3.
		Keypad3 = 259,
		//
		// 概要:
		//     Numeric keypad 4.
		Keypad4 = 260,
		//
		// 概要:
		//     Numeric keypad 5.
		Keypad5 = 261,
		//
		// 概要:
		//     Numeric keypad 6.
		Keypad6 = 262,
		//
		// 概要:
		//     Numeric keypad 7.
		Keypad7 = 263,
		//
		// 概要:
		//     Numeric keypad 8.
		Keypad8 = 264,
		//
		// 概要:
		//     Numeric keypad 9.
		Keypad9 = 265,
		//
		// 概要:
		//     Numeric keypad '.'.
		KeypadPeriod = 266,
		//
		// 概要:
		//     Numeric keypad '/'.
		KeypadDivide = 267,
		//
		// 概要:
		//     Numeric keypad '*'.
		KeypadMultiply = 268,
		//
		// 概要:
		//     Numeric keypad '-'.
		KeypadMinus = 269,
		//
		// 概要:
		//     Numeric keypad '+'.
		KeypadPlus = 270,
		//
		// 概要:
		//     Numeric keypad Enter.
		KeypadEnter = 271,
		//
		// 概要:
		//     Numeric keypad '='.
		KeypadEquals = 272,
		//
		// 概要:
		//     Up arrow key.
		UpArrow = 273,
		//
		// 概要:
		//     Down arrow key.
		DownArrow = 274,
		//
		// 概要:
		//     Right arrow key.
		RightArrow = 275,
		//
		// 概要:
		//     Left arrow key.
		LeftArrow = 276,
		//
		// 概要:
		//     Insert key key.
		Insert = 277,
		//
		// 概要:
		//     Home key.
		Home = 278,
		//
		// 概要:
		//     End key.
		End = 279,
		//
		// 概要:
		//     Page up.
		PageUp = 280,
		//
		// 概要:
		//     Page down.
		PageDown = 281,
		//
		// 概要:
		//     F1 function key.
		F1 = 282,
		//
		// 概要:
		//     F2 function key.
		F2 = 283,
		//
		// 概要:
		//     F3 function key.
		F3 = 284,
		//
		// 概要:
		//     F4 function key.
		F4 = 285,
		//
		// 概要:
		//     F5 function key.
		F5 = 286,
		//
		// 概要:
		//     F6 function key.
		F6 = 287,
		//
		// 概要:
		//     F7 function key.
		F7 = 288,
		//
		// 概要:
		//     F8 function key.
		F8 = 289,
		//
		// 概要:
		//     F9 function key.
		F9 = 290,
		//
		// 概要:
		//     F10 function key.
		F10 = 291,
		//
		// 概要:
		//     F11 function key.
		F11 = 292,
		//
		// 概要:
		//     F12 function key.
		F12 = 293,
		//
		// 概要:
		//     F13 function key.
		F13 = 294,
		//
		// 概要:
		//     F14 function key.
		F14 = 295,
		//
		// 概要:
		//     F15 function key.
		F15 = 296,
		//
		// 概要:
		//     Numlock key.
		Numlock = 300,
		//
		// 概要:
		//     Capslock key.
		CapsLock = 301,
		//
		// 概要:
		//     Scroll lock key.
		ScrollLock = 302,
		//
		// 概要:
		//     Right shift key.
		RightShift = 303,
		//
		// 概要:
		//     Left shift key.
		LeftShift = 304,
		//
		// 概要:
		//     Right Control key.
		RightControl = 305,
		//
		// 概要:
		//     Left Control key.
		LeftControl = 306,
		//
		// 概要:
		//     Right Alt key.
		RightAlt = 307,
		//
		// 概要:
		//     Left Alt key.
		LeftAlt = 308,
		//
		// 概要:
		//     Right Command key.
		RightCommand = 309,
		//
		// 概要:
		//     Right Command key.
		RightApple = 309,
		//
		// 概要:
		//     Left Command key.
		LeftCommand = 310,
		//
		// 概要:
		//     Left Command key.
		LeftApple = 310,
		//
		// 概要:
		//     Left Windows key.
		LeftWindows = 311,
		//
		// 概要:
		//     Right Windows key.
		RightWindows = 312,
		//
		// 概要:
		//     Alt Gr key.
		AltGr = 313,
		//
		// 概要:
		//     Help key.
		Help = 315,
		//
		// 概要:
		//     Print key.
		Print = 316,
		//
		// 概要:
		//     Sys Req key.
		SysReq = 317,
		//
		// 概要:
		//     Break key.
		Break = 318,
		//
		// 概要:
		//     Menu key.
		Menu = 319,
		//
		// 概要:
		//     The Left (or primary) mouse button.
		Mouse0 = 323,
		//
		// 概要:
		//     Right mouse button (or secondary mouse button).
		Mouse1 = 324,
		//
		// 概要:
		//     Middle mouse button (or third button).
		Mouse2 = 325,
		//
		// 概要:
		//     Additional (fourth) mouse button.
		Mouse3 = 326,
		//
		// 概要:
		//     Additional (fifth) mouse button.
		Mouse4 = 327,
		//
		// 概要:
		//     Additional (or sixth) mouse button.
		Mouse5 = 328,
		//
		// 概要:
		//     Additional (or seventh) mouse button.
		Mouse6 = 329,
		//
		// 概要:
		//     Button 0 on any joystick.
		Aボタン = 330,
		//
		// 概要:
		//     Button 1 on any joystick.
		Bボタン = 331,
		//
		// 概要:
		//     Button 2 on any joystick.
		Xボタン = 332,
		//
		// 概要:
		//     Button 3 on any joystick.
		Yボタン = 333,
		//
		// 概要:
		//     Button 4 on any joystick.
		L1 = 334,
		//
		// 概要:
		//     Button 5 on any joystick.
		R1 = 335,
		//
		// 概要:
		//     Button 6 on any joystick.
		Backボタン = 336,
		//
		// 概要:
		//     Button 7 on any joystick.
		Startボタン  = 337,
		//
		// 概要:
		//     Button 8 on any joystick.
		L3 = 338,
		//
		// 概要:
		//     Button 9 on any joystick.
		R3 = 339,
		//
		// 概要:
		//     Button 10 on any joystick.
		JoystickButton10 = 340,
		//
		// 概要:
		//     Button 11 on any joystick.
		JoystickButton11 = 341,
		//
		// 概要:
		//     Button 12 on any joystick.
		Lスティック上 = 342,
		//
		// 概要:
		//     Button 13 on any joystick.
		Lスティック下 = 343,
		//
		// 概要:
		//     Button 14 on any joystick.
		Lスティック = 344,
		//
		// 概要:
		//     Button 15 on any joystick.
		Rスティック = 345,
		//
		// 概要:
		//     Button 16 on any joystick.
		十字キー = 346,
		//
		// 概要:
		//     Button 17 on any joystick.
		L2 = 347,
		//
		// 概要:
		//     Button 18 on any joystick.
		R2 = 348,
		//
		// 概要:
		//     Button 19 on any joystick.
		JoystickButton19 = 349,
		//
		// 概要:
		//     Button 0 on first joystick.
		Joystick1Button0 = 350,
		//
		// 概要:
		//     Button 1 on first joystick.
		Joystick1Button1 = 351,
		//
		// 概要:
		//     Button 2 on first joystick.
		Joystick1Button2 = 352,
		//
		// 概要:
		//     Button 3 on first joystick.
		Joystick1Button3 = 353,
		//
		// 概要:
		//     Button 4 on first joystick.
		Joystick1Button4 = 354,
		//
		// 概要:
		//     Button 5 on first joystick.
		Joystick1Button5 = 355,
		//
		// 概要:
		//     Button 6 on first joystick.
		Joystick1Button6 = 356,
		//
		// 概要:
		//     Button 7 on first joystick.
		Joystick1Button7 = 357,
		//
		// 概要:
		//     Button 8 on first joystick.
		Joystick1Button8 = 358,
		//
		// 概要:
		//     Button 9 on first joystick.
		Joystick1Button9 = 359,
		//
		// 概要:
		//     Button 10 on first joystick.
		Joystick1Button10 = 360,
		//
		// 概要:
		//     Button 11 on first joystick.
		Joystick1Button11 = 361,
		//
		// 概要:
		//     Button 12 on first joystick.
		Joystick1Button12 = 362,
		//
		// 概要:
		//     Button 13 on first joystick.
		Joystick1Button13 = 363,
		//
		// 概要:
		//     Button 14 on first joystick.
		Joystick1Button14 = 364,
		//
		// 概要:
		//     Button 15 on first joystick.
		Joystick1Button15 = 365,
		//
		// 概要:
		//     Button 16 on first joystick.
		Joystick1Button16 = 366,
		//
		// 概要:
		//     Button 17 on first joystick.
		Joystick1Button17 = 367,
		//
		// 概要:
		//     Button 18 on first joystick.
		Joystick1Button18 = 368,
		//
		// 概要:
		//     Button 19 on first joystick.
		Joystick1Button19 = 369,
		//
		// 概要:
		//     Button 0 on second joystick.
		Joystick2Button0 = 370,
		//
		// 概要:
		//     Button 1 on second joystick.
		Joystick2Button1 = 371,
		//
		// 概要:
		//     Button 2 on second joystick.
		Joystick2Button2 = 372,
		//
		// 概要:
		//     Button 3 on second joystick.
		Joystick2Button3 = 373,
		//
		// 概要:
		//     Button 4 on second joystick.
		Joystick2Button4 = 374,
		//
		// 概要:
		//     Button 5 on second joystick.
		Joystick2Button5 = 375,
		//
		// 概要:
		//     Button 6 on second joystick.
		Joystick2Button6 = 376,
		//
		// 概要:
		//     Button 7 on second joystick.
		Joystick2Button7 = 377,
		//
		// 概要:
		//     Button 8 on second joystick.
		Joystick2Button8 = 378,
		//
		// 概要:
		//     Button 9 on second joystick.
		Joystick2Button9 = 379,
		//
		// 概要:
		//     Button 10 on second joystick.
		Joystick2Button10 = 380,
		//
		// 概要:
		//     Button 11 on second joystick.
		Joystick2Button11 = 381,
		//
		// 概要:
		//     Button 12 on second joystick.
		Joystick2Button12 = 382,
		//
		// 概要:
		//     Button 13 on second joystick.
		Joystick2Button13 = 383,
		//
		// 概要:
		//     Button 14 on second joystick.
		Joystick2Button14 = 384,
		//
		// 概要:
		//     Button 15 on second joystick.
		Joystick2Button15 = 385,
		//
		// 概要:
		//     Button 16 on second joystick.
		Joystick2Button16 = 386,
		//
		// 概要:
		//     Button 17 on second joystick.
		Joystick2Button17 = 387,
		//
		// 概要:
		//     Button 18 on second joystick.
		Joystick2Button18 = 388,
		//
		// 概要:
		//     Button 19 on second joystick.
		Joystick2Button19 = 389,
		//
		// 概要:
		//     Button 0 on third joystick.
		Joystick3Button0 = 390,
		//
		// 概要:
		//     Button 1 on third joystick.
		Joystick3Button1 = 391,
		//
		// 概要:
		//     Button 2 on third joystick.
		Joystick3Button2 = 392,
		//
		// 概要:
		//     Button 3 on third joystick.
		Joystick3Button3 = 393,
		//
		// 概要:
		//     Button 4 on third joystick.
		Joystick3Button4 = 394,
		//
		// 概要:
		//     Button 5 on third joystick.
		Joystick3Button5 = 395,
		//
		// 概要:
		//     Button 6 on third joystick.
		Joystick3Button6 = 396,
		//
		// 概要:
		//     Button 7 on third joystick.
		Joystick3Button7 = 397,
		//
		// 概要:
		//     Button 8 on third joystick.
		Joystick3Button8 = 398,
		//
		// 概要:
		//     Button 9 on third joystick.
		Joystick3Button9 = 399,
		//
		// 概要:
		//     Button 10 on third joystick.
		Joystick3Button10 = 400,
		//
		// 概要:
		//     Button 11 on third joystick.
		Joystick3Button11 = 401,
		//
		// 概要:
		//     Button 12 on third joystick.
		Joystick3Button12 = 402,
		//
		// 概要:
		//     Button 13 on third joystick.
		Joystick3Button13 = 403,
		//
		// 概要:
		//     Button 14 on third joystick.
		Joystick3Button14 = 404,
		//
		// 概要:
		//     Button 15 on third joystick.
		Joystick3Button15 = 405,
		//
		// 概要:
		//     Button 16 on third joystick.
		Joystick3Button16 = 406,
		//
		// 概要:
		//     Button 17 on third joystick.
		Joystick3Button17 = 407,
		//
		// 概要:
		//     Button 18 on third joystick.
		Joystick3Button18 = 408,
		//
		// 概要:
		//     Button 19 on third joystick.
		Joystick3Button19 = 409,
		//
		// 概要:
		//     Button 0 on forth joystick.
		Joystick4Button0 = 410,
		//
		// 概要:
		//     Button 1 on forth joystick.
		Joystick4Button1 = 411,
		//
		// 概要:
		//     Button 2 on forth joystick.
		Joystick4Button2 = 412,
		//
		// 概要:
		//     Button 3 on forth joystick.
		Joystick4Button3 = 413,
		//
		// 概要:
		//     Button 4 on forth joystick.
		Joystick4Button4 = 414,
		//
		// 概要:
		//     Button 5 on forth joystick.
		Joystick4Button5 = 415,
		//
		// 概要:
		//     Button 6 on forth joystick.
		Joystick4Button6 = 416,
		//
		// 概要:
		//     Button 7 on forth joystick.
		Joystick4Button7 = 417,
		//
		// 概要:
		//     Button 8 on forth joystick.
		Joystick4Button8 = 418,
		//
		// 概要:
		//     Button 9 on forth joystick.
		Joystick4Button9 = 419,
		//
		// 概要:
		//     Button 10 on forth joystick.
		Joystick4Button10 = 420,
		//
		// 概要:
		//     Button 11 on forth joystick.
		Joystick4Button11 = 421,
		//
		// 概要:
		//     Button 12 on forth joystick.
		Joystick4Button12 = 422,
		//
		// 概要:
		//     Button 13 on forth joystick.
		Joystick4Button13 = 423,
		//
		// 概要:
		//     Button 14 on forth joystick.
		Joystick4Button14 = 424,
		//
		// 概要:
		//     Button 15 on forth joystick.
		Joystick4Button15 = 425,
		//
		// 概要:
		//     Button 16 on forth joystick.
		Joystick4Button16 = 426,
		//
		// 概要:
		//     Button 17 on forth joystick.
		Joystick4Button17 = 427,
		//
		// 概要:
		//     Button 18 on forth joystick.
		Joystick4Button18 = 428,
		//
		// 概要:
		//     Button 19 on forth joystick.
		Joystick4Button19 = 429,
		//
		// 概要:
		//     Button 0 on fifth joystick.
		Joystick5Button0 = 430,
		//
		// 概要:
		//     Button 1 on fifth joystick.
		Joystick5Button1 = 431,
		//
		// 概要:
		//     Button 2 on fifth joystick.
		Joystick5Button2 = 432,
		//
		// 概要:
		//     Button 3 on fifth joystick.
		Joystick5Button3 = 433,
		//
		// 概要:
		//     Button 4 on fifth joystick.
		Joystick5Button4 = 434,
		//
		// 概要:
		//     Button 5 on fifth joystick.
		Joystick5Button5 = 435,
		//
		// 概要:
		//     Button 6 on fifth joystick.
		Joystick5Button6 = 436,
		//
		// 概要:
		//     Button 7 on fifth joystick.
		Joystick5Button7 = 437,
		//
		// 概要:
		//     Button 8 on fifth joystick.
		Joystick5Button8 = 438,
		//
		// 概要:
		//     Button 9 on fifth joystick.
		Joystick5Button9 = 439,
		//
		// 概要:
		//     Button 10 on fifth joystick.
		Joystick5Button10 = 440,
		//
		// 概要:
		//     Button 11 on fifth joystick.
		Joystick5Button11 = 441,
		//
		// 概要:
		//     Button 12 on fifth joystick.
		Joystick5Button12 = 442,
		//
		// 概要:
		//     Button 13 on fifth joystick.
		Joystick5Button13 = 443,
		//
		// 概要:
		//     Button 14 on fifth joystick.
		Joystick5Button14 = 444,
		//
		// 概要:
		//     Button 15 on fifth joystick.
		Joystick5Button15 = 445,
		//
		// 概要:
		//     Button 16 on fifth joystick.
		Joystick5Button16 = 446,
		//
		// 概要:
		//     Button 17 on fifth joystick.
		Joystick5Button17 = 447,
		//
		// 概要:
		//     Button 18 on fifth joystick.
		Joystick5Button18 = 448,
		//
		// 概要:
		//     Button 19 on fifth joystick.
		Joystick5Button19 = 449,
		//
		// 概要:
		//     Button 0 on sixth joystick.
		Joystick6Button0 = 450,
		//
		// 概要:
		//     Button 1 on sixth joystick.
		Joystick6Button1 = 451,
		//
		// 概要:
		//     Button 2 on sixth joystick.
		Joystick6Button2 = 452,
		//
		// 概要:
		//     Button 3 on sixth joystick.
		Joystick6Button3 = 453,
		//
		// 概要:
		//     Button 4 on sixth joystick.
		Joystick6Button4 = 454,
		//
		// 概要:
		//     Button 5 on sixth joystick.
		Joystick6Button5 = 455,
		//
		// 概要:
		//     Button 6 on sixth joystick.
		Joystick6Button6 = 456,
		//
		// 概要:
		//     Button 7 on sixth joystick.
		Joystick6Button7 = 457,
		//
		// 概要:
		//     Button 8 on sixth joystick.
		Joystick6Button8 = 458,
		//
		// 概要:
		//     Button 9 on sixth joystick.
		Joystick6Button9 = 459,
		//
		// 概要:
		//     Button 10 on sixth joystick.
		Joystick6Button10 = 460,
		//
		// 概要:
		//     Button 11 on sixth joystick.
		Joystick6Button11 = 461,
		//
		// 概要:
		//     Button 12 on sixth joystick.
		Joystick6Button12 = 462,
		//
		// 概要:
		//     Button 13 on sixth joystick.
		Joystick6Button13 = 463,
		//
		// 概要:
		//     Button 14 on sixth joystick.
		Joystick6Button14 = 464,
		//
		// 概要:
		//     Button 15 on sixth joystick.
		Joystick6Button15 = 465,
		//
		// 概要:
		//     Button 16 on sixth joystick.
		Joystick6Button16 = 466,
		//
		// 概要:
		//     Button 17 on sixth joystick.
		Joystick6Button17 = 467,
		//
		// 概要:
		//     Button 18 on sixth joystick.
		Joystick6Button18 = 468,
		//
		// 概要:
		//     Button 19 on sixth joystick.
		Joystick6Button19 = 469,
		//
		// 概要:
		//     Button 0 on seventh joystick.
		Joystick7Button0 = 470,
		//
		// 概要:
		//     Button 1 on seventh joystick.
		Joystick7Button1 = 471,
		//
		// 概要:
		//     Button 2 on seventh joystick.
		Joystick7Button2 = 472,
		//
		// 概要:
		//     Button 3 on seventh joystick.
		Joystick7Button3 = 473,
		//
		// 概要:
		//     Button 4 on seventh joystick.
		Joystick7Button4 = 474,
		//
		// 概要:
		//     Button 5 on seventh joystick.
		Joystick7Button5 = 475,
		//
		// 概要:
		//     Button 6 on seventh joystick.
		Joystick7Button6 = 476,
		//
		// 概要:
		//     Button 7 on seventh joystick.
		Joystick7Button7 = 477,
		//
		// 概要:
		//     Button 8 on seventh joystick.
		Joystick7Button8 = 478,
		//
		// 概要:
		//     Button 9 on seventh joystick.
		Joystick7Button9 = 479,
		//
		// 概要:
		//     Button 10 on seventh joystick.
		Joystick7Button10 = 480,
		//
		// 概要:
		//     Button 11 on seventh joystick.
		Joystick7Button11 = 481,
		//
		// 概要:
		//     Button 12 on seventh joystick.
		Joystick7Button12 = 482,
		//
		// 概要:
		//     Button 13 on seventh joystick.
		Joystick7Button13 = 483,
		//
		// 概要:
		//     Button 14 on seventh joystick.
		Joystick7Button14 = 484,
		//
		// 概要:
		//     Button 15 on seventh joystick.
		Joystick7Button15 = 485,
		//
		// 概要:
		//     Button 16 on seventh joystick.
		Joystick7Button16 = 486,
		//
		// 概要:
		//     Button 17 on seventh joystick.
		Joystick7Button17 = 487,
		//
		// 概要:
		//     Button 18 on seventh joystick.
		Joystick7Button18 = 488,
		//
		// 概要:
		//     Button 19 on seventh joystick.
		Joystick7Button19 = 489,
		//
		// 概要:
		//     Button 0 on eighth joystick.
		Joystick8Button0 = 490,
		//
		// 概要:
		//     Button 1 on eighth joystick.
		Joystick8Button1 = 491,
		//
		// 概要:
		//     Button 2 on eighth joystick.
		Joystick8Button2 = 492,
		//
		// 概要:
		//     Button 3 on eighth joystick.
		Joystick8Button3 = 493,
		//
		// 概要:
		//     Button 4 on eighth joystick.
		Joystick8Button4 = 494,
		//
		// 概要:
		//     Button 5 on eighth joystick.
		Joystick8Button5 = 495,
		//
		// 概要:
		//     Button 6 on eighth joystick.
		Joystick8Button6 = 496,
		//
		// 概要:
		//     Button 7 on eighth joystick.
		Joystick8Button7 = 497,
		//
		// 概要:
		//     Button 8 on eighth joystick.
		Joystick8Button8 = 498,
		//
		// 概要:
		//     Button 9 on eighth joystick.
		Joystick8Button9 = 499,
		//
		// 概要:
		//     Button 10 on eighth joystick.
		Joystick8Button10 = 500,
		//
		// 概要:
		//     Button 11 on eighth joystick.
		Joystick8Button11 = 501,
		//
		// 概要:
		//     Button 12 on eighth joystick.
		Joystick8Button12 = 502,
		//
		// 概要:
		//     Button 13 on eighth joystick.
		Joystick8Button13 = 503,
		//
		// 概要:
		//     Button 14 on eighth joystick.
		Joystick8Button14 = 504,
		//
		// 概要:
		//     Button 15 on eighth joystick.
		Joystick8Button15 = 505,
		//
		// 概要:
		//     Button 16 on eighth joystick.
		Joystick8Button16 = 506,
		//
		// 概要:
		//     Button 17 on eighth joystick.
		Joystick8Button17 = 507,
		//
		// 概要:
		//     Button 18 on eighth joystick.
		Joystick8Button18 = 508,
		//
		// 概要:
		//     Button 19 on eighth joystick.
		Joystick8Button19 = 509
	}
}
