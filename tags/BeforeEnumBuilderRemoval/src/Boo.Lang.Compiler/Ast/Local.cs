﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo Barreto de Oliveira
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

using System;
using Boo.Lang.Compiler.Ast.Impl;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Local : LocalImpl
	{		
		protected bool _privateScope;
		
		public Local()
		{
		}
		
		public Local(ReferenceExpression reference, bool privateScope)
		{
			_name = reference.Name;
			LexicalInfo = reference.LexicalInfo;
			_privateScope = privateScope;
 		}
 		
 		public Local(Declaration declaration, bool privateScope)
 		{
 			_name = declaration.Name;
 			LexicalInfo = declaration.LexicalInfo;
 			_privateScope = privateScope;
 		}
 		
 		public bool PrivateScope
 		{
 			get
 			{
 				return _privateScope;
 			}
			
			set
			{
				_privateScope = true;
			}
 		}
		
		override public void Accept(IAstVisitor visitor)
		{
			visitor.OnLocal(this);
		}
	}
}
