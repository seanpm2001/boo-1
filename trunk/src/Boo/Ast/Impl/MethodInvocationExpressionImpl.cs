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
// ast.py script on Tue Jan 13 23:45:02 2004
//
using System;

namespace Boo.Ast.Impl
{
	[Serializable]
	public abstract class MethodInvocationExpressionImpl : Expression, INodeWithArguments
	{
		protected Expression _target;
		protected ExpressionCollection _arguments;
		protected ExpressionPairCollection _namedArguments;
		
		protected MethodInvocationExpressionImpl()
		{
			_arguments = new ExpressionCollection(this);
			_namedArguments = new ExpressionPairCollection(this);
 		}
		
		protected MethodInvocationExpressionImpl(Expression target)
		{
			_arguments = new ExpressionCollection(this);
			_namedArguments = new ExpressionPairCollection(this);
 			Target = target;
		}
		
		protected MethodInvocationExpressionImpl(LexicalInfo lexicalInfo, Expression target) : base(lexicalInfo)
		{
			_arguments = new ExpressionCollection(this);
			_namedArguments = new ExpressionPairCollection(this);
 			Target = target;				
		}
		
		protected MethodInvocationExpressionImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_arguments = new ExpressionCollection(this);
			_namedArguments = new ExpressionPairCollection(this);
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.MethodInvocationExpression;
			}
		}
		public Expression Target
		{
			get
			{
				return _target;
			}
			
			set
			{
				
				if (_target != value)
				{
					_target = value;
					if (null != _target)
					{
						_target.InitializeParent(this);
					}
				}
			}
		}
		public ExpressionCollection Arguments
		{
			get
			{
				return _arguments;
			}
			
			set
			{
				
				if (_arguments != value)
				{
					_arguments = value;
					if (null != _arguments)
					{
						_arguments.InitializeParent(this);
					}
				}
			}
		}
		public ExpressionPairCollection NamedArguments
		{
			get
			{
				return _namedArguments;
			}
			
			set
			{
				
				if (_namedArguments != value)
				{
					_namedArguments = value;
					if (null != _namedArguments)
					{
						_namedArguments.InitializeParent(this);
					}
				}
			}
		}
		public override void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			MethodInvocationExpression thisNode = (MethodInvocationExpression)this;
			Expression resultingTypedNode = thisNode;
			transformer.OnMethodInvocationExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
