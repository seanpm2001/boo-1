﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// mailto:rbo@acm.org
#endregion

//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by
// astgenerator.boo on 2/24/2004 7:16:37 PM
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public class ExceptionHandlerCollectionImpl : NodeCollection
	{
		protected ExceptionHandlerCollectionImpl()
		{
		}
		
		protected ExceptionHandlerCollectionImpl(Node parent) : base(parent)
		{
		}
		
		public Boo.Lang.Ast.ExceptionHandler this[int index]
		{
			get
			{
				return (Boo.Lang.Ast.ExceptionHandler)InnerList[index];
			}
		}

		public void Add(Boo.Lang.Ast.ExceptionHandler item)
		{
			base.Add(item);			
		}
		
		public void Add(params Boo.Lang.Ast.ExceptionHandler[] items)
		{
			base.Add(items);			
		}
		
		public void Add(System.Collections.ICollection items)
		{
			foreach (Boo.Lang.Ast.ExceptionHandler item in items)
			{
				base.Add(item);
			}
		}
		
		public void AddClones(System.Collections.ICollection items)
		{
			foreach (Boo.Lang.Ast.ExceptionHandler item in items)
			{
				base.Add(item.CloneNode());
			}
		}
		
		public void Insert(int index, Boo.Lang.Ast.ExceptionHandler item)
		{
			base.Insert(index, item);
		}
		
		public bool Replace(Boo.Lang.Ast.ExceptionHandler existing, Boo.Lang.Ast.ExceptionHandler newItem)
		{
			return base.Replace(existing, newItem);
		}
		
		public new Boo.Lang.Ast.ExceptionHandler[] ToArray()
		{
			return (Boo.Lang.Ast.ExceptionHandler[])InnerList.ToArray(typeof(Boo.Lang.Ast.ExceptionHandler));
		}
	}
}
