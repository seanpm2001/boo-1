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
	public abstract class IteratorExpressionImpl : Expression
	{

		protected Expression _expression;
		protected DeclarationCollection _declarations;
		protected Expression _iterator;
		protected StatementModifier _filter;

		protected IteratorExpressionImpl()
		{
			InitializeFields();
		}
		
		protected IteratorExpressionImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected IteratorExpressionImpl(Expression expression, Expression iterator, StatementModifier filter)
		{
			InitializeFields();
			Expression = expression;
			Iterator = iterator;
			Filter = filter;
		}
			
		protected IteratorExpressionImpl(LexicalInfo lexicalInfo, Expression expression, Expression iterator, StatementModifier filter) : base(lexicalInfo)
		{
			InitializeFields();
			Expression = expression;
			Iterator = iterator;
			Filter = filter;
		}
			
		new public Boo.Lang.Ast.IteratorExpression CloneNode()
		{
			return Clone() as Boo.Lang.Ast.IteratorExpression;
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.IteratorExpression;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.IteratorExpression thisNode = (Boo.Lang.Ast.IteratorExpression)this;
			Boo.Lang.Ast.Expression resultingTypedNode = thisNode;
			transformer.OnIteratorExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_expression == existing)
			{
				this.Expression = ((Boo.Lang.Ast.Expression)newNode);
				return true;
			}

			if (_declarations != null)
			{
				Boo.Lang.Ast.Declaration item = existing as Boo.Lang.Ast.Declaration;
				if (null != item)
				{
					if (_declarations.Replace(item, (Boo.Lang.Ast.Declaration)newNode))
					{
						return true;
					}
				}
			}

			if (_iterator == existing)
			{
				this.Iterator = ((Boo.Lang.Ast.Expression)newNode);
				return true;
			}

			if (_filter == existing)
			{
				this.Filter = ((Boo.Lang.Ast.StatementModifier)newNode);
				return true;
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.IteratorExpression clone = (Boo.Lang.Ast.IteratorExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.IteratorExpression));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			if (null != _expression)
			{
				clone._expression = ((Expression)_expression.Clone());
			}

			if (null != _declarations)
			{
				clone._declarations = ((DeclarationCollection)_declarations.Clone());
			}

			if (null != _iterator)
			{
				clone._iterator = ((Expression)_iterator.Clone());
			}

			if (null != _filter)
			{
				clone._filter = ((StatementModifier)_filter.Clone());
			}
			
			return clone;
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
		

		private void InitializeFields()
		{
			_declarations = new DeclarationCollection(this);

		}
	}
}
