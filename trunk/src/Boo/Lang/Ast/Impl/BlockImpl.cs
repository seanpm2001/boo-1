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
// astgenerator.boo on 2/28/04 9:20:34 a
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public abstract class BlockImpl : Statement
	{

		protected StatementCollection _statements;

		protected BlockImpl()
		{
			InitializeFields();
		}
		
		protected BlockImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		new public Boo.Lang.Ast.Block CloneNode()
		{
			return Clone() as Boo.Lang.Ast.Block;
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.Block;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.Block thisNode = (Boo.Lang.Ast.Block)this;
			Boo.Lang.Ast.Statement resultingTypedNode = thisNode;
			transformer.OnBlock(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_modifier == existing)
			{
				this.Modifier = ((Boo.Lang.Ast.StatementModifier)newNode);
				return true;
			}

			if (_statements != null)
			{
				Boo.Lang.Ast.Statement item = existing as Boo.Lang.Ast.Statement;
				if (null != item)
				{
					if (_statements.Replace(item, (Boo.Lang.Ast.Statement)newNode))
					{
						return true;
					}
				}
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.Block clone = (Boo.Lang.Ast.Block)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.Block));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			if (null != _modifier)
			{
				clone._modifier = ((StatementModifier)_modifier.Clone());
			}

			if (null != _statements)
			{
				clone._statements = ((StatementCollection)_statements.Clone());
			}
			
			return clone;
		}
			
		public StatementCollection Statements
		{
			get
			{
				return _statements;
			}
			

			set
			{
				if (_statements != value)
				{
					_statements = value;
					if (null != _statements)
					{
						_statements.InitializeParent(this);

					}
				}
			}
			

		}
		

		private void InitializeFields()
		{
			_statements = new StatementCollection(this);

		}
	}
}
