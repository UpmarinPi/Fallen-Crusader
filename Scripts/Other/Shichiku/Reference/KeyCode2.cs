namespace UnityEngine
{
	//
	// �T�v:
	//     Key codes returned by Event.keyCode. These map directly to a physical key on
	//     the keyboard.
	public class KeyCode2Class
	{
		//keycode��keycode2�����ꂼ��ɕϊ�����
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
		// �T�v:
		//     Not assigned (never returned as the result of a keystroke).
		None = 0,
		//
		// �T�v:
		//     The backspace key.
		Backspace = 8,
		//
		// �T�v:
		//     The tab key.
		Tab = 9,
		//
		// �T�v:
		//     The Clear key.
		Clear = 12,
		//
		// �T�v:
		//     Return key.
		Return = 13,
		//
		// �T�v:
		//     Pause on PC machines.
		Pause = 19,
		//
		// �T�v:
		//     Escape key.
		Escape = 27,
		//
		// �T�v:
		//     Space key.
		Space = 32,
		//
		// �T�v:
		//     Exclamation mark key '!'.
		Exclaim = 33,
		//
		// �T�v:
		//     Double quote key '"'.
		DoubleQuote = 34,
		//
		// �T�v:
		//     Hash key '#'.
		Hash = 35,
		//
		// �T�v:
		//     Dollar sign key '$'.
		Dollar = 36,
		//
		// �T�v:
		//     Percent '%' key.
		Percent = 37,
		//
		// �T�v:
		//     Ampersand key '&'.
		Ampersand = 38,
		//
		// �T�v:
		//     Quote key '.
		Quote = 39,
		//
		// �T�v:
		//     Left Parenthesis key '('.
		LeftParen = 40,
		//
		// �T�v:
		//     Right Parenthesis key ')'.
		RightParen = 41,
		//
		// �T�v:
		//     Asterisk key '*'.
		Asterisk = 42,
		//
		// �T�v:
		//     Plus key '+'.
		Plus = 43,
		//
		// �T�v:
		//     Comma ',' key.
		Comma = 44,
		//
		// �T�v:
		//     Minus '-' key.
		Minus = 45,
		//
		// �T�v:
		//     Period '.' key.
		Period = 46,
		//
		// �T�v:
		//     Slash '/' key.
		Slash = 47,
		//
		// �T�v:
		//     The '0' key on the top of the alphanumeric keyboard.
		Alpha0 = 48,
		//
		// �T�v:
		//     The '1' key on the top of the alphanumeric keyboard.
		Alpha1 = 49,
		//
		// �T�v:
		//     The '2' key on the top of the alphanumeric keyboard.
		Alpha2 = 50,
		//
		// �T�v:
		//     The '3' key on the top of the alphanumeric keyboard.
		Alpha3 = 51,
		//
		// �T�v:
		//     The '4' key on the top of the alphanumeric keyboard.
		Alpha4 = 52,
		//
		// �T�v:
		//     The '5' key on the top of the alphanumeric keyboard.
		Alpha5 = 53,
		//
		// �T�v:
		//     The '6' key on the top of the alphanumeric keyboard.
		Alpha6 = 54,
		//
		// �T�v:
		//     The '7' key on the top of the alphanumeric keyboard.
		Alpha7 = 55,
		//
		// �T�v:
		//     The '8' key on the top of the alphanumeric keyboard.
		Alpha8 = 56,
		//
		// �T�v:
		//     The '9' key on the top of the alphanumeric keyboard.
		Alpha9 = 57,
		//
		// �T�v:
		//     Colon ':' key.
		Colon = 58,
		//
		// �T�v:
		//     Semicolon ';' key.
		Semicolon = 59,
		//
		// �T�v:
		//     Less than '<' key.
		Less = 60,
		//
		// �T�v:
		//     Equals '=' key.
		Equals = 61,
		//
		// �T�v:
		//     Greater than '>' key.
		Greater = 62,
		//
		// �T�v:
		//     Question mark '?' key.
		Question = 63,
		//
		// �T�v:
		//     At key '@'.
		At = 64,
		//
		// �T�v:
		//     Left square bracket key '['.
		LeftBracket = 91,
		//
		// �T�v:
		//     Backslash key '\'.
		Backslash = 92,
		//
		// �T�v:
		//     Right square bracket key ']'.
		RightBracket = 93,
		//
		// �T�v:
		//     Caret key '^'.
		Caret = 94,
		//
		// �T�v:
		//     Underscore '_' key.
		Underscore = 95,
		//
		// �T�v:
		//     Back quote key '`'.
		BackQuote = 96,
		//
		// �T�v:
		//     'a' key.
		A = 97,
		//
		// �T�v:
		//     'b' key.
		B = 98,
		//
		// �T�v:
		//     'c' key.
		C = 99,
		//
		// �T�v:
		//     'd' key.
		D = 100,
		//
		// �T�v:
		//     'e' key.
		E = 101,
		//
		// �T�v:
		//     'f' key.
		F = 102,
		//
		// �T�v:
		//     'g' key.
		G = 103,
		//
		// �T�v:
		//     'h' key.
		H = 104,
		//
		// �T�v:
		//     'i' key.
		I = 105,
		//
		// �T�v:
		//     'j' key.
		J = 106,
		//
		// �T�v:
		//     'k' key.
		K = 107,
		//
		// �T�v:
		//     'l' key.
		L = 108,
		//
		// �T�v:
		//     'm' key.
		M = 109,
		//
		// �T�v:
		//     'n' key.
		N = 110,
		//
		// �T�v:
		//     'o' key.
		O = 111,
		//
		// �T�v:
		//     'p' key.
		P = 112,
		//
		// �T�v:
		//     'q' key.
		Q = 113,
		//
		// �T�v:
		//     'r' key.
		R = 114,
		//
		// �T�v:
		//     's' key.
		S = 115,
		//
		// �T�v:
		//     't' key.
		T = 116,
		//
		// �T�v:
		//     'u' key.
		U = 117,
		//
		// �T�v:
		//     'v' key.
		V = 118,
		//
		// �T�v:
		//     'w' key.
		W = 119,
		//
		// �T�v:
		//     'x' key.
		X = 120,
		//
		// �T�v:
		//     'y' key.
		Y = 121,
		//
		// �T�v:
		//     'z' key.
		Z = 122,
		//
		// �T�v:
		//     Left curly bracket key '{'.
		LeftCurlyBracket = 123,
		//
		// �T�v:
		//     Pipe '|' key.
		Pipe = 124,
		//
		// �T�v:
		//     Right curly bracket key '}'.
		RightCurlyBracket = 125,
		//
		// �T�v:
		//     Tilde '~' key.
		Tilde = 126,
		//
		// �T�v:
		//     The forward delete key.
		Delete = 127,
		//
		// �T�v:
		//     Numeric keypad 0.
		Keypad0 = 256,
		//
		// �T�v:
		//     Numeric keypad 1.
		Keypad1 = 257,
		//
		// �T�v:
		//     Numeric keypad 2.
		Keypad2 = 258,
		//
		// �T�v:
		//     Numeric keypad 3.
		Keypad3 = 259,
		//
		// �T�v:
		//     Numeric keypad 4.
		Keypad4 = 260,
		//
		// �T�v:
		//     Numeric keypad 5.
		Keypad5 = 261,
		//
		// �T�v:
		//     Numeric keypad 6.
		Keypad6 = 262,
		//
		// �T�v:
		//     Numeric keypad 7.
		Keypad7 = 263,
		//
		// �T�v:
		//     Numeric keypad 8.
		Keypad8 = 264,
		//
		// �T�v:
		//     Numeric keypad 9.
		Keypad9 = 265,
		//
		// �T�v:
		//     Numeric keypad '.'.
		KeypadPeriod = 266,
		//
		// �T�v:
		//     Numeric keypad '/'.
		KeypadDivide = 267,
		//
		// �T�v:
		//     Numeric keypad '*'.
		KeypadMultiply = 268,
		//
		// �T�v:
		//     Numeric keypad '-'.
		KeypadMinus = 269,
		//
		// �T�v:
		//     Numeric keypad '+'.
		KeypadPlus = 270,
		//
		// �T�v:
		//     Numeric keypad Enter.
		KeypadEnter = 271,
		//
		// �T�v:
		//     Numeric keypad '='.
		KeypadEquals = 272,
		//
		// �T�v:
		//     Up arrow key.
		UpArrow = 273,
		//
		// �T�v:
		//     Down arrow key.
		DownArrow = 274,
		//
		// �T�v:
		//     Right arrow key.
		RightArrow = 275,
		//
		// �T�v:
		//     Left arrow key.
		LeftArrow = 276,
		//
		// �T�v:
		//     Insert key key.
		Insert = 277,
		//
		// �T�v:
		//     Home key.
		Home = 278,
		//
		// �T�v:
		//     End key.
		End = 279,
		//
		// �T�v:
		//     Page up.
		PageUp = 280,
		//
		// �T�v:
		//     Page down.
		PageDown = 281,
		//
		// �T�v:
		//     F1 function key.
		F1 = 282,
		//
		// �T�v:
		//     F2 function key.
		F2 = 283,
		//
		// �T�v:
		//     F3 function key.
		F3 = 284,
		//
		// �T�v:
		//     F4 function key.
		F4 = 285,
		//
		// �T�v:
		//     F5 function key.
		F5 = 286,
		//
		// �T�v:
		//     F6 function key.
		F6 = 287,
		//
		// �T�v:
		//     F7 function key.
		F7 = 288,
		//
		// �T�v:
		//     F8 function key.
		F8 = 289,
		//
		// �T�v:
		//     F9 function key.
		F9 = 290,
		//
		// �T�v:
		//     F10 function key.
		F10 = 291,
		//
		// �T�v:
		//     F11 function key.
		F11 = 292,
		//
		// �T�v:
		//     F12 function key.
		F12 = 293,
		//
		// �T�v:
		//     F13 function key.
		F13 = 294,
		//
		// �T�v:
		//     F14 function key.
		F14 = 295,
		//
		// �T�v:
		//     F15 function key.
		F15 = 296,
		//
		// �T�v:
		//     Numlock key.
		Numlock = 300,
		//
		// �T�v:
		//     Capslock key.
		CapsLock = 301,
		//
		// �T�v:
		//     Scroll lock key.
		ScrollLock = 302,
		//
		// �T�v:
		//     Right shift key.
		RightShift = 303,
		//
		// �T�v:
		//     Left shift key.
		LeftShift = 304,
		//
		// �T�v:
		//     Right Control key.
		RightControl = 305,
		//
		// �T�v:
		//     Left Control key.
		LeftControl = 306,
		//
		// �T�v:
		//     Right Alt key.
		RightAlt = 307,
		//
		// �T�v:
		//     Left Alt key.
		LeftAlt = 308,
		//
		// �T�v:
		//     Right Command key.
		RightCommand = 309,
		//
		// �T�v:
		//     Right Command key.
		RightApple = 309,
		//
		// �T�v:
		//     Left Command key.
		LeftCommand = 310,
		//
		// �T�v:
		//     Left Command key.
		LeftApple = 310,
		//
		// �T�v:
		//     Left Windows key.
		LeftWindows = 311,
		//
		// �T�v:
		//     Right Windows key.
		RightWindows = 312,
		//
		// �T�v:
		//     Alt Gr key.
		AltGr = 313,
		//
		// �T�v:
		//     Help key.
		Help = 315,
		//
		// �T�v:
		//     Print key.
		Print = 316,
		//
		// �T�v:
		//     Sys Req key.
		SysReq = 317,
		//
		// �T�v:
		//     Break key.
		Break = 318,
		//
		// �T�v:
		//     Menu key.
		Menu = 319,
		//
		// �T�v:
		//     The Left (or primary) mouse button.
		Mouse0 = 323,
		//
		// �T�v:
		//     Right mouse button (or secondary mouse button).
		Mouse1 = 324,
		//
		// �T�v:
		//     Middle mouse button (or third button).
		Mouse2 = 325,
		//
		// �T�v:
		//     Additional (fourth) mouse button.
		Mouse3 = 326,
		//
		// �T�v:
		//     Additional (fifth) mouse button.
		Mouse4 = 327,
		//
		// �T�v:
		//     Additional (or sixth) mouse button.
		Mouse5 = 328,
		//
		// �T�v:
		//     Additional (or seventh) mouse button.
		Mouse6 = 329,
		//
		// �T�v:
		//     Button 0 on any joystick.
		A�{�^�� = 330,
		//
		// �T�v:
		//     Button 1 on any joystick.
		B�{�^�� = 331,
		//
		// �T�v:
		//     Button 2 on any joystick.
		X�{�^�� = 332,
		//
		// �T�v:
		//     Button 3 on any joystick.
		Y�{�^�� = 333,
		//
		// �T�v:
		//     Button 4 on any joystick.
		L1 = 334,
		//
		// �T�v:
		//     Button 5 on any joystick.
		R1 = 335,
		//
		// �T�v:
		//     Button 6 on any joystick.
		Back�{�^�� = 336,
		//
		// �T�v:
		//     Button 7 on any joystick.
		Start�{�^��  = 337,
		//
		// �T�v:
		//     Button 8 on any joystick.
		L3 = 338,
		//
		// �T�v:
		//     Button 9 on any joystick.
		R3 = 339,
		//
		// �T�v:
		//     Button 10 on any joystick.
		JoystickButton10 = 340,
		//
		// �T�v:
		//     Button 11 on any joystick.
		JoystickButton11 = 341,
		//
		// �T�v:
		//     Button 12 on any joystick.
		L�X�e�B�b�N�� = 342,
		//
		// �T�v:
		//     Button 13 on any joystick.
		L�X�e�B�b�N�� = 343,
		//
		// �T�v:
		//     Button 14 on any joystick.
		L�X�e�B�b�N = 344,
		//
		// �T�v:
		//     Button 15 on any joystick.
		R�X�e�B�b�N = 345,
		//
		// �T�v:
		//     Button 16 on any joystick.
		�\���L�[ = 346,
		//
		// �T�v:
		//     Button 17 on any joystick.
		L2 = 347,
		//
		// �T�v:
		//     Button 18 on any joystick.
		R2 = 348,
		//
		// �T�v:
		//     Button 19 on any joystick.
		JoystickButton19 = 349,
		//
		// �T�v:
		//     Button 0 on first joystick.
		Joystick1Button0 = 350,
		//
		// �T�v:
		//     Button 1 on first joystick.
		Joystick1Button1 = 351,
		//
		// �T�v:
		//     Button 2 on first joystick.
		Joystick1Button2 = 352,
		//
		// �T�v:
		//     Button 3 on first joystick.
		Joystick1Button3 = 353,
		//
		// �T�v:
		//     Button 4 on first joystick.
		Joystick1Button4 = 354,
		//
		// �T�v:
		//     Button 5 on first joystick.
		Joystick1Button5 = 355,
		//
		// �T�v:
		//     Button 6 on first joystick.
		Joystick1Button6 = 356,
		//
		// �T�v:
		//     Button 7 on first joystick.
		Joystick1Button7 = 357,
		//
		// �T�v:
		//     Button 8 on first joystick.
		Joystick1Button8 = 358,
		//
		// �T�v:
		//     Button 9 on first joystick.
		Joystick1Button9 = 359,
		//
		// �T�v:
		//     Button 10 on first joystick.
		Joystick1Button10 = 360,
		//
		// �T�v:
		//     Button 11 on first joystick.
		Joystick1Button11 = 361,
		//
		// �T�v:
		//     Button 12 on first joystick.
		Joystick1Button12 = 362,
		//
		// �T�v:
		//     Button 13 on first joystick.
		Joystick1Button13 = 363,
		//
		// �T�v:
		//     Button 14 on first joystick.
		Joystick1Button14 = 364,
		//
		// �T�v:
		//     Button 15 on first joystick.
		Joystick1Button15 = 365,
		//
		// �T�v:
		//     Button 16 on first joystick.
		Joystick1Button16 = 366,
		//
		// �T�v:
		//     Button 17 on first joystick.
		Joystick1Button17 = 367,
		//
		// �T�v:
		//     Button 18 on first joystick.
		Joystick1Button18 = 368,
		//
		// �T�v:
		//     Button 19 on first joystick.
		Joystick1Button19 = 369,
		//
		// �T�v:
		//     Button 0 on second joystick.
		Joystick2Button0 = 370,
		//
		// �T�v:
		//     Button 1 on second joystick.
		Joystick2Button1 = 371,
		//
		// �T�v:
		//     Button 2 on second joystick.
		Joystick2Button2 = 372,
		//
		// �T�v:
		//     Button 3 on second joystick.
		Joystick2Button3 = 373,
		//
		// �T�v:
		//     Button 4 on second joystick.
		Joystick2Button4 = 374,
		//
		// �T�v:
		//     Button 5 on second joystick.
		Joystick2Button5 = 375,
		//
		// �T�v:
		//     Button 6 on second joystick.
		Joystick2Button6 = 376,
		//
		// �T�v:
		//     Button 7 on second joystick.
		Joystick2Button7 = 377,
		//
		// �T�v:
		//     Button 8 on second joystick.
		Joystick2Button8 = 378,
		//
		// �T�v:
		//     Button 9 on second joystick.
		Joystick2Button9 = 379,
		//
		// �T�v:
		//     Button 10 on second joystick.
		Joystick2Button10 = 380,
		//
		// �T�v:
		//     Button 11 on second joystick.
		Joystick2Button11 = 381,
		//
		// �T�v:
		//     Button 12 on second joystick.
		Joystick2Button12 = 382,
		//
		// �T�v:
		//     Button 13 on second joystick.
		Joystick2Button13 = 383,
		//
		// �T�v:
		//     Button 14 on second joystick.
		Joystick2Button14 = 384,
		//
		// �T�v:
		//     Button 15 on second joystick.
		Joystick2Button15 = 385,
		//
		// �T�v:
		//     Button 16 on second joystick.
		Joystick2Button16 = 386,
		//
		// �T�v:
		//     Button 17 on second joystick.
		Joystick2Button17 = 387,
		//
		// �T�v:
		//     Button 18 on second joystick.
		Joystick2Button18 = 388,
		//
		// �T�v:
		//     Button 19 on second joystick.
		Joystick2Button19 = 389,
		//
		// �T�v:
		//     Button 0 on third joystick.
		Joystick3Button0 = 390,
		//
		// �T�v:
		//     Button 1 on third joystick.
		Joystick3Button1 = 391,
		//
		// �T�v:
		//     Button 2 on third joystick.
		Joystick3Button2 = 392,
		//
		// �T�v:
		//     Button 3 on third joystick.
		Joystick3Button3 = 393,
		//
		// �T�v:
		//     Button 4 on third joystick.
		Joystick3Button4 = 394,
		//
		// �T�v:
		//     Button 5 on third joystick.
		Joystick3Button5 = 395,
		//
		// �T�v:
		//     Button 6 on third joystick.
		Joystick3Button6 = 396,
		//
		// �T�v:
		//     Button 7 on third joystick.
		Joystick3Button7 = 397,
		//
		// �T�v:
		//     Button 8 on third joystick.
		Joystick3Button8 = 398,
		//
		// �T�v:
		//     Button 9 on third joystick.
		Joystick3Button9 = 399,
		//
		// �T�v:
		//     Button 10 on third joystick.
		Joystick3Button10 = 400,
		//
		// �T�v:
		//     Button 11 on third joystick.
		Joystick3Button11 = 401,
		//
		// �T�v:
		//     Button 12 on third joystick.
		Joystick3Button12 = 402,
		//
		// �T�v:
		//     Button 13 on third joystick.
		Joystick3Button13 = 403,
		//
		// �T�v:
		//     Button 14 on third joystick.
		Joystick3Button14 = 404,
		//
		// �T�v:
		//     Button 15 on third joystick.
		Joystick3Button15 = 405,
		//
		// �T�v:
		//     Button 16 on third joystick.
		Joystick3Button16 = 406,
		//
		// �T�v:
		//     Button 17 on third joystick.
		Joystick3Button17 = 407,
		//
		// �T�v:
		//     Button 18 on third joystick.
		Joystick3Button18 = 408,
		//
		// �T�v:
		//     Button 19 on third joystick.
		Joystick3Button19 = 409,
		//
		// �T�v:
		//     Button 0 on forth joystick.
		Joystick4Button0 = 410,
		//
		// �T�v:
		//     Button 1 on forth joystick.
		Joystick4Button1 = 411,
		//
		// �T�v:
		//     Button 2 on forth joystick.
		Joystick4Button2 = 412,
		//
		// �T�v:
		//     Button 3 on forth joystick.
		Joystick4Button3 = 413,
		//
		// �T�v:
		//     Button 4 on forth joystick.
		Joystick4Button4 = 414,
		//
		// �T�v:
		//     Button 5 on forth joystick.
		Joystick4Button5 = 415,
		//
		// �T�v:
		//     Button 6 on forth joystick.
		Joystick4Button6 = 416,
		//
		// �T�v:
		//     Button 7 on forth joystick.
		Joystick4Button7 = 417,
		//
		// �T�v:
		//     Button 8 on forth joystick.
		Joystick4Button8 = 418,
		//
		// �T�v:
		//     Button 9 on forth joystick.
		Joystick4Button9 = 419,
		//
		// �T�v:
		//     Button 10 on forth joystick.
		Joystick4Button10 = 420,
		//
		// �T�v:
		//     Button 11 on forth joystick.
		Joystick4Button11 = 421,
		//
		// �T�v:
		//     Button 12 on forth joystick.
		Joystick4Button12 = 422,
		//
		// �T�v:
		//     Button 13 on forth joystick.
		Joystick4Button13 = 423,
		//
		// �T�v:
		//     Button 14 on forth joystick.
		Joystick4Button14 = 424,
		//
		// �T�v:
		//     Button 15 on forth joystick.
		Joystick4Button15 = 425,
		//
		// �T�v:
		//     Button 16 on forth joystick.
		Joystick4Button16 = 426,
		//
		// �T�v:
		//     Button 17 on forth joystick.
		Joystick4Button17 = 427,
		//
		// �T�v:
		//     Button 18 on forth joystick.
		Joystick4Button18 = 428,
		//
		// �T�v:
		//     Button 19 on forth joystick.
		Joystick4Button19 = 429,
		//
		// �T�v:
		//     Button 0 on fifth joystick.
		Joystick5Button0 = 430,
		//
		// �T�v:
		//     Button 1 on fifth joystick.
		Joystick5Button1 = 431,
		//
		// �T�v:
		//     Button 2 on fifth joystick.
		Joystick5Button2 = 432,
		//
		// �T�v:
		//     Button 3 on fifth joystick.
		Joystick5Button3 = 433,
		//
		// �T�v:
		//     Button 4 on fifth joystick.
		Joystick5Button4 = 434,
		//
		// �T�v:
		//     Button 5 on fifth joystick.
		Joystick5Button5 = 435,
		//
		// �T�v:
		//     Button 6 on fifth joystick.
		Joystick5Button6 = 436,
		//
		// �T�v:
		//     Button 7 on fifth joystick.
		Joystick5Button7 = 437,
		//
		// �T�v:
		//     Button 8 on fifth joystick.
		Joystick5Button8 = 438,
		//
		// �T�v:
		//     Button 9 on fifth joystick.
		Joystick5Button9 = 439,
		//
		// �T�v:
		//     Button 10 on fifth joystick.
		Joystick5Button10 = 440,
		//
		// �T�v:
		//     Button 11 on fifth joystick.
		Joystick5Button11 = 441,
		//
		// �T�v:
		//     Button 12 on fifth joystick.
		Joystick5Button12 = 442,
		//
		// �T�v:
		//     Button 13 on fifth joystick.
		Joystick5Button13 = 443,
		//
		// �T�v:
		//     Button 14 on fifth joystick.
		Joystick5Button14 = 444,
		//
		// �T�v:
		//     Button 15 on fifth joystick.
		Joystick5Button15 = 445,
		//
		// �T�v:
		//     Button 16 on fifth joystick.
		Joystick5Button16 = 446,
		//
		// �T�v:
		//     Button 17 on fifth joystick.
		Joystick5Button17 = 447,
		//
		// �T�v:
		//     Button 18 on fifth joystick.
		Joystick5Button18 = 448,
		//
		// �T�v:
		//     Button 19 on fifth joystick.
		Joystick5Button19 = 449,
		//
		// �T�v:
		//     Button 0 on sixth joystick.
		Joystick6Button0 = 450,
		//
		// �T�v:
		//     Button 1 on sixth joystick.
		Joystick6Button1 = 451,
		//
		// �T�v:
		//     Button 2 on sixth joystick.
		Joystick6Button2 = 452,
		//
		// �T�v:
		//     Button 3 on sixth joystick.
		Joystick6Button3 = 453,
		//
		// �T�v:
		//     Button 4 on sixth joystick.
		Joystick6Button4 = 454,
		//
		// �T�v:
		//     Button 5 on sixth joystick.
		Joystick6Button5 = 455,
		//
		// �T�v:
		//     Button 6 on sixth joystick.
		Joystick6Button6 = 456,
		//
		// �T�v:
		//     Button 7 on sixth joystick.
		Joystick6Button7 = 457,
		//
		// �T�v:
		//     Button 8 on sixth joystick.
		Joystick6Button8 = 458,
		//
		// �T�v:
		//     Button 9 on sixth joystick.
		Joystick6Button9 = 459,
		//
		// �T�v:
		//     Button 10 on sixth joystick.
		Joystick6Button10 = 460,
		//
		// �T�v:
		//     Button 11 on sixth joystick.
		Joystick6Button11 = 461,
		//
		// �T�v:
		//     Button 12 on sixth joystick.
		Joystick6Button12 = 462,
		//
		// �T�v:
		//     Button 13 on sixth joystick.
		Joystick6Button13 = 463,
		//
		// �T�v:
		//     Button 14 on sixth joystick.
		Joystick6Button14 = 464,
		//
		// �T�v:
		//     Button 15 on sixth joystick.
		Joystick6Button15 = 465,
		//
		// �T�v:
		//     Button 16 on sixth joystick.
		Joystick6Button16 = 466,
		//
		// �T�v:
		//     Button 17 on sixth joystick.
		Joystick6Button17 = 467,
		//
		// �T�v:
		//     Button 18 on sixth joystick.
		Joystick6Button18 = 468,
		//
		// �T�v:
		//     Button 19 on sixth joystick.
		Joystick6Button19 = 469,
		//
		// �T�v:
		//     Button 0 on seventh joystick.
		Joystick7Button0 = 470,
		//
		// �T�v:
		//     Button 1 on seventh joystick.
		Joystick7Button1 = 471,
		//
		// �T�v:
		//     Button 2 on seventh joystick.
		Joystick7Button2 = 472,
		//
		// �T�v:
		//     Button 3 on seventh joystick.
		Joystick7Button3 = 473,
		//
		// �T�v:
		//     Button 4 on seventh joystick.
		Joystick7Button4 = 474,
		//
		// �T�v:
		//     Button 5 on seventh joystick.
		Joystick7Button5 = 475,
		//
		// �T�v:
		//     Button 6 on seventh joystick.
		Joystick7Button6 = 476,
		//
		// �T�v:
		//     Button 7 on seventh joystick.
		Joystick7Button7 = 477,
		//
		// �T�v:
		//     Button 8 on seventh joystick.
		Joystick7Button8 = 478,
		//
		// �T�v:
		//     Button 9 on seventh joystick.
		Joystick7Button9 = 479,
		//
		// �T�v:
		//     Button 10 on seventh joystick.
		Joystick7Button10 = 480,
		//
		// �T�v:
		//     Button 11 on seventh joystick.
		Joystick7Button11 = 481,
		//
		// �T�v:
		//     Button 12 on seventh joystick.
		Joystick7Button12 = 482,
		//
		// �T�v:
		//     Button 13 on seventh joystick.
		Joystick7Button13 = 483,
		//
		// �T�v:
		//     Button 14 on seventh joystick.
		Joystick7Button14 = 484,
		//
		// �T�v:
		//     Button 15 on seventh joystick.
		Joystick7Button15 = 485,
		//
		// �T�v:
		//     Button 16 on seventh joystick.
		Joystick7Button16 = 486,
		//
		// �T�v:
		//     Button 17 on seventh joystick.
		Joystick7Button17 = 487,
		//
		// �T�v:
		//     Button 18 on seventh joystick.
		Joystick7Button18 = 488,
		//
		// �T�v:
		//     Button 19 on seventh joystick.
		Joystick7Button19 = 489,
		//
		// �T�v:
		//     Button 0 on eighth joystick.
		Joystick8Button0 = 490,
		//
		// �T�v:
		//     Button 1 on eighth joystick.
		Joystick8Button1 = 491,
		//
		// �T�v:
		//     Button 2 on eighth joystick.
		Joystick8Button2 = 492,
		//
		// �T�v:
		//     Button 3 on eighth joystick.
		Joystick8Button3 = 493,
		//
		// �T�v:
		//     Button 4 on eighth joystick.
		Joystick8Button4 = 494,
		//
		// �T�v:
		//     Button 5 on eighth joystick.
		Joystick8Button5 = 495,
		//
		// �T�v:
		//     Button 6 on eighth joystick.
		Joystick8Button6 = 496,
		//
		// �T�v:
		//     Button 7 on eighth joystick.
		Joystick8Button7 = 497,
		//
		// �T�v:
		//     Button 8 on eighth joystick.
		Joystick8Button8 = 498,
		//
		// �T�v:
		//     Button 9 on eighth joystick.
		Joystick8Button9 = 499,
		//
		// �T�v:
		//     Button 10 on eighth joystick.
		Joystick8Button10 = 500,
		//
		// �T�v:
		//     Button 11 on eighth joystick.
		Joystick8Button11 = 501,
		//
		// �T�v:
		//     Button 12 on eighth joystick.
		Joystick8Button12 = 502,
		//
		// �T�v:
		//     Button 13 on eighth joystick.
		Joystick8Button13 = 503,
		//
		// �T�v:
		//     Button 14 on eighth joystick.
		Joystick8Button14 = 504,
		//
		// �T�v:
		//     Button 15 on eighth joystick.
		Joystick8Button15 = 505,
		//
		// �T�v:
		//     Button 16 on eighth joystick.
		Joystick8Button16 = 506,
		//
		// �T�v:
		//     Button 17 on eighth joystick.
		Joystick8Button17 = 507,
		//
		// �T�v:
		//     Button 18 on eighth joystick.
		Joystick8Button18 = 508,
		//
		// �T�v:
		//     Button 19 on eighth joystick.
		Joystick8Button19 = 509
	}
}
