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
// ast.py script on Mon Feb 16 22:56:51 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class UnaryExpressionImpl : Expression
	{
		protected UnaryOperatorType _operator;
		protected Expression _operand;
		
		protected UnaryExpressionImpl()
		{
 		}
		
		protected UnaryExpressionImpl(UnaryOperatorType operator_, Expression operand)
		{
 			Operator = operator_;
			Operand = operand;
		}
		
		protected UnaryExpressionImpl(LexicalInfo lexicalInfo, UnaryOperatorType operator_, Expression operand) : base(lexicalInfo)
		{
 			Operator = operator_;				
			Operand = operand;				
		}
		
		protected UnaryExpressionImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.UnaryExpression;
			}
		}
		public UnaryOperatorType Operator
		{
			get
			{
				return _operator;
			}
			
			set
			{
				
				if (_operator != value)
				{
					_operator = value;
				}
			}
		}
		public Expression Operand
		{
			get
			{
				return _operand;
			}
			
			set
			{
				
				if (_operand != value)
				{
					_operand = value;
					if (null != _operand)
					{
						_operand.InitializeParent(this);
					}
				}
			}
		}
		new public UnaryExpression CloneNode()
		{
			return (UnaryExpression)Clone();
		}
		
		override public object Clone()
		{
			UnaryExpression clone = (UnaryExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			clone._operator = _operator;
			if (null != _operand)
			{
				clone._operand = (Expression)_operand.Clone();
			}
			
			return clone;
		}
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			if (_operand == existing)
			{
				this.Operand = (Expression)newNode;
				return true;
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			UnaryExpression thisNode = (UnaryExpression)this;
			Expression resultingTypedNode = thisNode;
			transformer.OnUnaryExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
