﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

// $ANTLR 2.7.4rc1: "src/Boo.AntlrParser/boo.g" -> "BooParserBase.cs"$

namespace Boo.AntlrParser
{
	public class BooTokenTypes
	{
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int TIMESPAN = 4;
		public const int DOUBLE = 5;
		public const int LONG = 6;
		public const int ESEPARATOR = 7;
		public const int INDENT = 8;
		public const int DEDENT = 9;
		public const int COMPILATION_UNIT = 10;
		public const int PARAMETERS = 11;
		public const int PARAMETER = 12;
		public const int ELIST = 13;
		public const int DLIST = 14;
		public const int TYPE = 15;
		public const int CALL = 16;
		public const int STMT = 17;
		public const int BLOCK = 18;
		public const int FIELD = 19;
		public const int MODIFIERS = 20;
		public const int MODULE = 21;
		public const int LITERAL = 22;
		public const int LIST_LITERAL = 23;
		public const int UNPACKING = 24;
		public const int ABSTRACT = 25;
		public const int AND = 26;
		public const int AS = 27;
		public const int BREAK = 28;
		public const int CONTINUE = 29;
		public const int CALLABLE = 30;
		public const int CAST = 31;
		public const int CLASS = 32;
		public const int CONSTRUCTOR = 33;
		public const int DEF = 34;
		public const int ELSE = 35;
		public const int ENSURE = 36;
		public const int ENUM = 37;
		public const int EXCEPT = 38;
		public const int FAILURE = 39;
		public const int FINAL = 40;
		public const int FROM = 41;
		public const int FOR = 42;
		public const int FALSE = 43;
		public const int GET = 44;
		public const int GIVEN = 45;
		public const int IMPORT = 46;
		public const int INTERFACE = 47;
		public const int INTERNAL = 48;
		public const int IS = 49;
		public const int ISA = 50;
		public const int IF = 51;
		public const int IN = 52;
		public const int NOT = 53;
		public const int NULL = 54;
		public const int OR = 55;
		public const int OTHERWISE = 56;
		public const int OVERRIDE = 57;
		public const int PASS = 58;
		public const int NAMESPACE = 59;
		public const int PUBLIC = 60;
		public const int PROTECTED = 61;
		public const int PRIVATE = 62;
		public const int RAISE = 63;
		public const int RETURN = 64;
		public const int RETRY = 65;
		public const int SET = 66;
		public const int SELF = 67;
		public const int SUPER = 68;
		public const int STATIC = 69;
		public const int SUCCESS = 70;
		public const int TRY = 71;
		public const int TRANSIENT = 72;
		public const int TRUE = 73;
		public const int TYPEOF = 74;
		public const int UNLESS = 75;
		public const int VIRTUAL = 76;
		public const int WHEN = 77;
		public const int WHILE = 78;
		public const int YIELD = 79;
		public const int EOS = 80;
		public const int TRIPLE_QUOTED_STRING = 81;
		public const int ID = 82;
		public const int LPAREN = 83;
		public const int RPAREN = 84;
		public const int ASSIGN = 85;
		public const int LBRACK = 86;
		public const int COMMA = 87;
		public const int RBRACK = 88;
		public const int COLON = 89;
		public const int MULTIPLY = 90;
		public const int CMP_OPERATOR = 91;
		public const int ADD = 92;
		public const int SUBTRACT = 93;
		public const int BITWISE_OR = 94;
		public const int DIVISION = 95;
		public const int MODULUS = 96;
		public const int EXPONENTIATION = 97;
		public const int INCREMENT = 98;
		public const int DECREMENT = 99;
		public const int DOT = 100;
		public const int INT = 101;
		public const int DOUBLE_QUOTED_STRING = 102;
		public const int SINGLE_QUOTED_STRING = 103;
		public const int LBRACE = 104;
		public const int RBRACE = 105;
		public const int RE_LITERAL = 106;
		public const int LINE_CONTINUATION = 107;
		public const int SL_COMMENT = 108;
		public const int ML_COMMENT = 109;
		public const int WS = 110;
		public const int NEWLINE = 111;
		public const int ESCAPED_EXPRESSION = 112;
		public const int DQS_ESC = 113;
		public const int SQS_ESC = 114;
		public const int SESC = 115;
		public const int RE_CHAR = 116;
		public const int RE_ESC = 117;
		public const int ID_LETTER = 118;
		public const int DIGIT = 119;
		
	}
}
