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
// ast.py script on Thu Feb 19 02:30:32 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class IteratorExpressionImpl : Expression
	{
		protected Expression _expression;
		protected DeclarationCollection _declarations;
		protected Expression _iterator;
		protected StatementModifier _filter;
		
		protected IteratorExpressionImpl()
		{
			_declarations = new DeclarationCollection(this);
 		}
		
		protected IteratorExpressionImpl(Expression expression, Expression iterator, StatementModifier filter)
		{
			_declarations = new DeclarationCollection(this);
 			Expression = expression;
			Iterator = iterator;
			Filter = filter;
		}
		
		protected IteratorExpressionImpl(LexicalInfo lexicalInfo, Expression expression, Expression iterator, StatementModifier filter) : base(lexicalInfo)
		{
			_declarations = new DeclarationCollection(this);
 			Expression = expression;				
			Iterator = iterator;				
			Filter = filter;				
		}
		
		protected IteratorExpressionImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_declarations = new DeclarationCollection(this);
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.IteratorExpression;
			}
		}
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			
			set
			{
				
				if (_expression != value)
				{
					_expression = value;
					if (null != _expression)
					{
						_expression.InitializeParent(this);
					}
				}
			}
		}
		public DeclarationCollection Declarations
		{
			get
			{
				return _declarations;
			}
			
			set
			{
				
				if (_declarations != value)
				{
					_declarations = value;
					if (null != _declarations)
					{
						_declarations.InitializeParent(this);
					}
				}
			}
		}
		public Expression Iterator
		{
			get
			{
				return _iterator;
			}
			
			set
			{
				
				if (_iterator != value)
				{
					_iterator = value;
					if (null != _iterator)
					{
						_iterator.InitializeParent(this);
					}
				}
			}
		}
		public StatementModifier Filter
		{
			get
			{
				return _filter;
			}
			
			set
			{
				
				if (_filter != value)
				{
					_filter = value;
					if (null != _filter)
					{
						_filter.InitializeParent(this);
					}
				}
			}
		}
		new public IteratorExpression CloneNode()
		{
			return (IteratorExpression)Clone();
		}
		
		override public object Clone()
		{
			IteratorExpression clone = (IteratorExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			if (null != _expression)
			{
				clone._expression = (Expression)_expression.Clone();
			}
			if (null != _declarations)
			{
				clone._declarations = (DeclarationCollection)_declarations.Clone();
			}
			if (null != _iterator)
			{
				clone._iterator = (Expression)_iterator.Clone();
			}
			if (null != _filter)
			{
				clone._filter = (StatementModifier)_filter.Clone();
			}
			
			return clone;
		}
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			if (_expression == existing)
			{
				this.Expression = (Expression)newNode;
				return true;
			}
			if (_declarations != null)
			{
				Declaration item = existing as Declaration;
				if (null != item)
				{
					if (_declarations.Replace(item, (Declaration)newNode))
					{
						return true;
					}
				}
			}
			if (_iterator == existing)
			{
				this.Iterator = (Expression)newNode;
				return true;
			}
			if (_filter == existing)
			{
				this.Filter = (StatementModifier)newNode;
				return true;
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			IteratorExpression thisNode = (IteratorExpression)this;
			Expression resultingTypedNode = thisNode;
			transformer.OnIteratorExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
