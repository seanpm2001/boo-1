#region license
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
// This file was generated automatically by the
// ast.py script on Thu Feb 12 15:51:49 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class StringLiteralExpressionImpl : LiteralExpression
	{
		protected string _value;
		
		protected StringLiteralExpressionImpl()
		{
 		}
		
		protected StringLiteralExpressionImpl(string value)
		{
 			Value = value;
		}
		
		protected StringLiteralExpressionImpl(LexicalInfo lexicalInfo, string value) : base(lexicalInfo)
		{
 			Value = value;				
		}
		
		protected StringLiteralExpressionImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.StringLiteralExpression;
			}
		}
		public string Value
		{
			get
			{
				return _value;
			}
			
			set
			{
				
				_value = value;
			}
		}
		new public StringLiteralExpression CloneNode()
		{
			return (StringLiteralExpression)Clone();
		}
		
		override public object Clone()
		{
			StringLiteralExpression clone = (StringLiteralExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			clone._value = _value;
			
			return clone;
		}
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			StringLiteralExpression thisNode = (StringLiteralExpression)this;
			Expression resultingTypedNode = thisNode;
			transformer.OnStringLiteralExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
