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
// ast.py script on Sun Jan 11 23:16:51 2004
//
using System;

namespace Boo.Ast.Impl
{
	[Serializable]
	public abstract class ForStatementImpl : Statement
	{
		protected DeclarationCollection _declarations;
		protected Expression _iterator;
		protected StatementCollection _statements;
		
		protected ForStatementImpl()
		{
			_declarations = new DeclarationCollection(this);
			_statements = new StatementCollection(this);
 		}
		
		protected ForStatementImpl(Expression iterator)
		{
			_declarations = new DeclarationCollection(this);
			_statements = new StatementCollection(this);
 			Iterator = iterator;
		}
		
		protected ForStatementImpl(antlr.Token token, Expression iterator) : base(token)
		{
			_declarations = new DeclarationCollection(this);
			_statements = new StatementCollection(this);
 			Iterator = iterator;
		}
		
		internal ForStatementImpl(antlr.Token token) : base(token)
		{
			_declarations = new DeclarationCollection(this);
			_statements = new StatementCollection(this);
 		}
		
		internal ForStatementImpl(Node lexicalInfoProvider) : base(lexicalInfoProvider)
		{
			_declarations = new DeclarationCollection(this);
			_statements = new StatementCollection(this);
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.ForStatement;
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
		public override void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			ForStatement thisNode = (ForStatement)this;
			Statement resultingTypedNode = thisNode;
			transformer.OnForStatement(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
